﻿using Fluid.Auth;
using Fluid.Room;
using PlayerIOClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Fluid
{
    /// <summary>
    /// The client used for logging in to the game
    /// </summary>
    public sealed class FluidClient
    {
        private bool m_LoggedIn = false;
        private Config m_Config;
        private FluidLog m_Log;
        private FluidParser m_Parser;
        private FluidToolbelt m_Toolbelt;
        private FluidPlayerDatabase m_PlayerDatabase;

        private Player m_Player;
        private Client m_Client;
        private ManualResetEvent m_LoginHandle;

        /// <summary>
        /// Gets the authentication used to log in.
        /// </summary>
        public IAuth Auth { get; private set; }

        /// <summary>
        /// Gets or sets the configuration
        /// </summary>
        public Config Config { get { return m_Config; } }

        /// <summary>
        /// Gets the log of messages
        /// </summary>
        public FluidLog Log { get { return m_Log; } }

        /// <summary>
        /// Gets the current logged in player
        /// </summary>
        public Player ConnectedPlayer { get { return m_Player; } }

        /// <summary>
        /// Gets the player database
        /// </summary>
        public FluidPlayerDatabase PlayerDatabase { get { return m_PlayerDatabase; } }

        /// <summary>
        /// Gets the connection value
        /// </summary>
        public ConnectionValue ConnectionValue { get; set; }

        /// <summary>
        /// Gets the connection user id
        /// </summary>
        public string ConnectionUserId
        {
            get
            {
                if (m_Client != null)
                {
                    return m_Client.ConnectUserId;
                }

                return null;
            }
        }

        /// <summary>
        /// Loads the game's version from the database if availible
        /// </summary>
        public int GetGameVersion()
        {
            if (m_Client != null)
            {
                DatabaseObject configObject = m_Toolbelt.RunSafe<DatabaseObject>(() => m_Client.BigDB.Load("config", "config"));

                if (configObject != null)
                {
                    if (configObject.Contains("version"))
                    {
                        if (configObject["version"] is int)
                        {
                            return (int)configObject["version"];
                        }
                        else
                        {
                            m_Log.Add(FluidLogCategory.Message, "Database has been updated. Config version is no longer a integer.");
                        }
                    }
                    else
                    {
                        m_Log.Add(FluidLogCategory.Message, "Database has been updated. Config version no longer exists.");
                    }
                }
                else
                {
                    m_Log.Add(FluidLogCategory.Message, "Failed to load everybodyedits configuration from database.");
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the connection type
        /// </summary>
        public PlayerType GetConnectionType(string connectionId)
        {
            if (connectionId != null)
            {
                if (string.Compare(connectionId, "simpleguest", true) == 0)
                {
                    return PlayerType.Guest;
                }
                else if (connectionId.StartsWith("simple"))
                {
                    return PlayerType.Everybodyedits;
                }
                else if (connectionId.StartsWith("kong"))
                {
                    return PlayerType.Kongregate;
                }
                else if (connectionId.StartsWith("armor"))
                {
                    return PlayerType.Armorgames;
                }
                else if (connectionId.StartsWith("fb"))
                {
                    return PlayerType.Facebook;
                }
                else if (connectionId.StartsWith("mouse"))
                {
                    return PlayerType.Mousebreaker;
                }
            }

            return PlayerType.Unknown; ;
        }

        /// <summary>
        /// Gets a list of rooms in the lobby
        /// </summary>
        /// <returns>The list of rooms in the lobby</returns>
        public List<LobbyWorldReference> GetLobbyRooms()
        {
            List<LobbyWorldReference> worlds = new List<LobbyWorldReference>();

            int gameVersion = GetGameVersion();
            if (gameVersion == -1)
            {
                return worlds;
            }

            string eeRoom = string.Format(m_Config.NormalRoom, gameVersion);
            string betaRoom = string.Format(m_Config.BetaRoom, gameVersion);

            RoomInfo[] rooms = m_Client.Multiplayer.ListRooms(null, null, 0, 0);
            for (int i = 0; i < rooms.Length; i++)
            {
                if (string.Compare(rooms[i].RoomType, eeRoom) == 0 ||
                    string.Compare(rooms[i].RoomType, betaRoom) == 0)
                {
                    worlds.Add(new LobbyWorldReference(this, rooms[i].Id, rooms[i].OnlineUsers)
                        {
                            Owned = m_Toolbelt.GetValueIfExists(rooms[i].RoomData, "owned"),
                            NeedsKey = m_Toolbelt.GetValueIfExists(rooms[i].RoomData, "needskey"),
                            Plays = m_Toolbelt.GetValueIfExists(rooms[i].RoomData, "plays"),
                            Rating = m_Toolbelt.GetValueIfExists(rooms[i].RoomData, "rating"),
                            Name = m_Toolbelt.GetValueIfExists(rooms[i].RoomData, "name"),
                            Woots = m_Toolbelt.GetValueIfExists(rooms[i].RoomData, "woots"),
                            IsFeatured = m_Toolbelt.GetValueIfExists(rooms[i].RoomData, "IsFeatured")
                        });
                }
            }

            return worlds;
        }

        /// <summary>
        /// Gets a list of rooms in the lobby asynchronously
        /// </summary>
        /// <returns>The async task</returns>
        public async Task<List<LobbyWorldReference>> GetLobbyRoomsAsync()
        {
            return await Task.Run<List<LobbyWorldReference>>(() => GetLobbyRooms());
        }

        /// <summary>
        /// Gets the list of players online
        /// </summary>
        /// <returns>The list of players online</returns>
        public List<Player> GetPlayersOnline()
        {
            List<Player> online = new List<Player>();

            int currentVersion = GetGameVersion();
            if (currentVersion == -1)
            {
                //Logged message will be from .GetGameVersion() if failed.
                return online;
            }

            string lobbyType = string.Format(m_Config.LobbyRoom, currentVersion);
            RoomInfo[] roomInfo = m_Client.Multiplayer.ListRooms(lobbyType, null, 0, 0);

            for (int i = 0; i < roomInfo.Length; i++)
            {
                Player player = GetPlayerByConnectionId(roomInfo[i].Id);
                if (player != null)
                {
                    online.Add(player);
                }
            }

            return online;
        }

        /// <summary>
        /// Gets the players online asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<List<Player>> GetPlayersOnlineAsync()
        {
            return await Task.Run<List<Player>>(() => GetPlayersOnline());
        }

        /// <summary>
        /// Gets a player from everybody edits
        /// </summary>
        /// <param name="username">Their username</param>
        /// <returns>The player if successful; otherwise null</returns>
        public Player GetPlayerByUsername(string username)
        {
            if (username == null)
            {   
                //Don't log anything, this is an okay call
                return null;
            }

            return new Player(this, username);
        }

        /// <summary>
        /// Gets a player from everybody edits
        /// </summary>
        /// <param name="connectionId">Their connection id</param>
        /// <returns>The player if successful; otherwise null</returns>
        public Player GetPlayerByConnectionId(string connectionId)
        {
            if (m_PlayerDatabase.Connected)
            {
                string username = m_PlayerDatabase.GetUsername(connectionId);
                if (username != null)
                {
                    return new Player(this, username, connectionId);
                }
            }

            return null;
        }

        /// <summary>
        /// Loads a world from the database; the world will be the last time 
        /// the user saved his/her world
        /// </summary>
        /// <param name="worldIdOrUrl">World Id or Url</param>
        public World LoadWorld(string worldIdOrUrl)
        {
            string worldId = null;
            if (m_Parser.Parse(worldIdOrUrl, out worldId))
            {
                if (m_Client != null)
                {
                    DatabaseObject worldObject = m_Toolbelt.RunSafe<DatabaseObject>(() => m_Client.BigDB.Load("worlds", worldId));

                    if (worldObject != null)
                    {
                        return new World(this, worldObject);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Loads a world from the database; the world will be the last time 
        /// the user saved his/her world asynchronously
        /// </summary>
        /// <param name="worldIdOrUrl">World Id or Url</param>
        public async Task<World> LoadWorldAsync(string worldIdOrUrl)
        {
            return await Task.Run<World>(() => LoadWorld(worldIdOrUrl));
        }

        /// <summary>
        /// Loads a profile
        /// </summary>
        /// <param name="username">The username</param>
        public Profile LoadProfile(string username)
        {
            LobbyConnection lobbyConnection = GetLobbyConnection().Join();
            if (lobbyConnection != null)
            {
                Profile profile = lobbyConnection.GetProfile(username);
                //lobbyConnection.Disconnect();

                return profile;
            }

            return null;
        }

        /// <summary>
        /// Loads a profile asynchronously
        /// </summary>
        /// <param name="username">The username</param>
        public async Task<Profile> LoadProfileAsync(string username)
        {
            return await Task.Run<Profile>(() => LoadProfile(username));
        }

        /// <summary>
        /// Loads the player object
        /// </summary>
        public PlayerObject LoadMyPlayerObject()
        {
            LobbyConnection lobbyConnection = GetLobbyConnection().Join();
            if (lobbyConnection != null)
            {
                PlayerObject playerObject = lobbyConnection.GetPlayerObject();

                //lobbyConnection.Disconnect();
                return playerObject;     
            }

            return null;
        }

        /// <summary>
        /// Loads the player object asynchronously
        /// </summary>
        public async Task<PlayerObject> LoadMyPlayerObjectAsync()
        {
            return await Task.Run<PlayerObject>(() => LoadMyPlayerObject());
        }

        /// <summary>
        /// Loads all shop items owned
        /// </summary>
        public VaultShopItem[] LoadPlayerItems()
        {
            m_Client.PayVault.Refresh();
            VaultItem[] vaultItems = m_Client.PayVault.Items;
            VaultShopItem[] shopItems = new VaultShopItem[vaultItems.Length];
            for (int i = 0; i < vaultItems.Length; i++)
            {
                VaultItem cur = vaultItems[i];
                VaultShopItem shopItem = new VaultShopItem()
                {
                    ID = cur.Id,
                    Type = cur.ItemKey,
                    PriceCoins = m_Toolbelt.GetValueIfExists<int>(cur, "PriceCoins"),
                    Enabled = m_Toolbelt.GetValueIfExists<bool>(cur, "Enabled"),
                    IsFeatured = m_Toolbelt.GetValueIfExists<bool>(cur, "IsFeatured"),
                    Span = m_Toolbelt.GetValueIfExists<int>(cur, "Span"),
                    BitmapSheetOffset = m_Toolbelt.GetValueIfExists<int>(cur, "BitmapSheetOffset"),
                    HeaderText = m_Toolbelt.GetValueIfExists<string>(cur, "Header"),
                    BodyText = m_Toolbelt.GetValueIfExists<string>(cur, "Body"),
                    IsGridFeatured = m_Toolbelt.GetValueIfExists<bool>(cur, "IsGridFeatured"),
                    PriceUSD = m_Toolbelt.GetValueIfExists<int>(cur, "PriceUSD"),
                    PriceEnergy = m_Toolbelt.GetValueIfExists<int>(cur, "PriceEnergy"),
                    EnergyPerClick = m_Toolbelt.GetValueIfExists<int>(cur, "EnergyPerClick"),
                    InPlayerWorldsOnly = m_Toolbelt.GetValueIfExists<bool>(cur, "PWOnly"),
                    MinimumClass = m_Toolbelt.GetValueIfExists<int>(cur, "MinClass"),
                    IsClassic = m_Toolbelt.GetValueIfExists<bool>(cur, "IsClassic"),
                    OnSale = m_Toolbelt.GetValueIfExists<bool>(cur, "OnSale"),
                    BetaOnly = m_Toolbelt.GetValueIfExists<bool>(cur, "BetaOnly"),
                    BitmapSheetID = m_Toolbelt.GetValueIfExists<string>(cur, "BitmapSheetId"),
                    Reusable = m_Toolbelt.GetValueIfExists<bool>(cur, "Reusable"),
                    IsNew = m_Toolbelt.GetValueIfExists<bool>(cur, "IsNew"),
                    IsDevOnly = m_Toolbelt.GetValueIfExists<bool>(cur, "DevOnly")
                };

                shopItems[i] = shopItem;
            }

            return shopItems;
        }

        /// <summary>
        /// Loads all shop items owned asynchronously
        /// </summary>
        public async Task<VaultShopItem[]> LoadPlayerItemsAsync()
        {
            return await Task.Run<VaultShopItem[]>(() => LoadPlayerItems());
        }

        /// <summary>
        /// Event handler for when a client is created
        /// </summary>
        /// <param name="client">The created client</param>
        private void OnClientCreated(Client client)
        {
            this.m_Client = client;
            m_LoginHandle.Set();
        }

        /// <summary>
        /// Event handler for when a playerio error is received
        /// </summary>
        /// <param name="error">The playerio error</param>
        private void OnPlayerIOErrorReceived(PlayerIOError error)
        {
            m_Log.Add(FluidLogCategory.Fail, error.Message);
            m_LoginHandle.Set();
        }

        /// <summary>
        /// Log's in to the game
        /// </summary>
        /// <returns>True if logged in successfully; otherwise false</returns>
        public bool LogIn()
        {
            return this.LogIn(true);
        }

        /// <summary>
        /// Log's in the the game
        /// </summary>
        /// <param name="useSecureConnection">
        /// If enabled, the connection will be secure, but if false
        /// the connection will be faster
        /// </param>
        /// <returns>True if logged in successfully; otherwise false</returns>
        public bool LogIn(bool useSecureConnection)
        {
            if (m_Client == null)
            {
                m_LoginHandle = new ManualResetEvent(false);
                Auth.LogIn(m_Config, new Callback<Client>(OnClientCreated), new Callback<PlayerIOError>(OnPlayerIOErrorReceived));

                m_LoginHandle.WaitOne(10000);
                if (m_Client == null)
                {
                    //Error should be logged from PlayerIOErrorReceived
                    return false;
                }

                m_LoggedIn = true;
                m_Client.Multiplayer.UseSecureConnections = useSecureConnection;
                m_Client.PayVault.Refresh();

                if (!m_PlayerDatabase.Connected)
                {
                    string databaseFile = ExternalResources.FindFile("gDat.db");
                    if (databaseFile != null)
                    {
                        m_Toolbelt.RunSafe(() => m_PlayerDatabase.Connect(m_Toolbelt, databaseFile));
                    }
                    else
                    {
                        m_Log.Add(FluidLogCategory.Message, "Could not file the player database file. Please make the file is in the nuget package's \"build\" folder or in the same directory as the program.");
                    }
                }

                m_Player = GetPlayerByConnectionId(ConnectionUserId);
                if (m_Player == null && Config.AddProfilesToDatabase)
                {
                    PlayerObject pObject = LoadMyPlayerObject();
                    if (pObject != null)
                    {
                        if (!string.IsNullOrEmpty(pObject.Username) && m_PlayerDatabase.Connected)
                        {
                            m_PlayerDatabase.Add(pObject.Username, ConnectionUserId);
                            m_Player = new Player(this, pObject.Username, ConnectionUserId);
                        }
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Log's in to the game asynchronously
        /// </summary>
        /// <returns>True if logged in successfully; otherwise false</returns>
        public async Task<bool> LogInAsync()
        {
            return await LogInAsync(true);
        }

        /// <summary>
        /// Log's in the the game asynchronously
        /// </summary>
        /// <param name="useSecureConnection">
        /// If enabled, the connection will be secure, but if false
        /// the connection will be faster
        /// </param>
        /// <returns>True if logged in successfully; otherwise false</returns>
        public async Task<bool> LogInAsync(bool useSecureConnection)
        {
            return await Task.Run<bool>(() => LogIn(useSecureConnection));
        }

        /// <summary>
        /// Logs out of the game
        /// </summary>
        public void LogOut()
        {
            if (m_Client != null)
            {
                m_Client.Logout();
            }
        }

        /// <summary>
        /// Logs out of the game asynchronously
        /// </summary>
        public async void LogOutAsync()
        {
            await Task.Run(() => LogOut());
        }

        /// <summary>
        /// Checks to see if the player has a block
        /// </summary>
        /// <param name="blockId">The block id</param>
        /// <param name="refreshPayvault">Whether to reload the payvault</param>
        /// <returns>True is the block is owned; otherwise false</returns>
        public bool HasBlock(BlockID blockId, bool refreshPayvault = false)
        {
            int block = (int)blockId;

            int[] defaultBlocks = ItemInfo.GetDefaultBlocks();
            for (int i = 0; i < defaultBlocks.Length; i++)
            {
                if (defaultBlocks[i] == block)
                {
                    return true;
                }
            }

            string blockPack = ItemInfo.GetBlockPack(blockId);
            if (blockPack == null)
            {
                return false;
            }

            if (refreshPayvault)
            {
                m_Client.PayVault.Refresh();
            }

            return m_Client.PayVault.Has(blockPack);
        }

        /// <summary>
        /// Counts the amount of blocks owned
        /// </summary>
        /// <param name="blockId">The block id</param>
        /// <param name="refreshPayvault">Whether to reload the payvault</param>
        /// <returns>The number of blocks owned; -1 if no limit</returns>
        public int CountBlock(BlockID blockId, bool refreshPayvault = false)
        {
            int block = (int)blockId;

            int[] defaultBlocks = ItemInfo.GetDefaultBlocks();
            for (int i = 0; i < defaultBlocks.Length; i++)
            {
                if (defaultBlocks[i] == block)
                {
                    return -1;
                }
            }

            string package = ItemInfo.GetBlockPack(blockId);
            if (package == null)
            {
                return -1;
            }

            if (refreshPayvault)
            {
                m_Client.PayVault.Refresh();
            }

            int multiplier = 1;
            switch (package)
            {
                case "brickcoindoor":
                case "brickbluecoindoor":
                case "brickswitchpurple":
                case "brickdeathdoor":
                case "bricktimeddoor":
                case "brickzombiedoor":
                case "brickcoingate":
                case "brickbluecoingate":
                case "brickspike":
                case "brickfire":
                    multiplier = 10;
                    break;
                case "brickportal":
                case "brickinvisibleportal":
                    multiplier = 5;
                    break;
            }

            int count = m_Client.PayVault.Count(package);

            return count * multiplier;
        }

        /// <summary>
        /// Gets the corresponding block pack of a block
        /// </summary>
        /// <param name="blockId">The block id</param>
        /// <returns>The block pack name</returns>
        public string GetBlockPack(BlockID blockId)
        {
            return ItemInfo.GetBlockPack(blockId);
        }

        /// <summary>
        /// Checks to see if the player has a smiley
        /// </summary>
        /// <param name="smiley">The smiley</param>
        /// <returns></returns>
        public bool HasSmiley(FaceID smiley)
        {
            string smileyPack = ItemInfo.GetSmileyId(smiley);
            if (smileyPack == null)
            {
                return false;
            }
            else if (smileyPack == string.Empty)
            {
                return true;
            }

            m_Client.PayVault.Refresh();
            return m_Client.PayVault.Has(smileyPack);
        }

        /// <summary>
        /// Gets the smiley's shop name
        /// </summary>
        /// <param name="smiley">The smiley</param>
        public string GetSmileyPackName(FaceID smiley)
        {
            return ItemInfo.GetSmileyId(smiley);
        }

        /// <summary>
        /// Gets if the user has a potion
        /// </summary>
        /// <param name="potion">The potion type</param>
        public bool HasPotion(Potion potion)
        {
            string packName = GetPotionShopId(potion);

            m_Client.PayVault.Refresh();
            return m_Client.PayVault.Has(packName);
        }

        /// <summary>
        /// Gets the amount of potions owned
        /// </summary>
        /// <param name="potion">The potion type</param>
        public int GetPotionCount(Potion potion)
        {
            string packName = GetPotionShopId(potion);

            m_Client.PayVault.Refresh();
            return m_Client.PayVault.Count(packName);
        }

        /// <summary>
        /// Gets the potion shop id
        /// </summary>
        /// <param name="potion">The potion type</param>
        public string GetPotionShopId(Potion potion)
        {
            string potionName = Enum.GetName(typeof(Potion), potion);
            return string.Format("potion{0}", potionName.ToLowerInvariant());
        }

        /// <summary>
        /// Creates a lobby connection
        /// </summary>
        /// <returns>The lobby connection</returns>
        internal Connection JoinLobby()
        {
            int currentVersion = GetGameVersion();
            if (currentVersion == -1)
            {
                //Logged message will be from .GetGameVersion() if failed.
                return null;
            }

            PlayerType currentPlayerType = GetConnectionType(ConnectionUserId);
            string lobbyRoomFormat = (currentPlayerType == PlayerType.Guest) ? m_Config.LobbyGuestRoom : m_Config.LobbyRoom;
            string lobbyRoom = string.Format(lobbyRoomFormat, currentVersion);

            LobbyConnection lobbyCon = new LobbyConnection(this);
            return m_Client.Multiplayer.CreateJoinRoom(ConnectionUserId, lobbyRoom, true, null, null);
        }

        /// <summary>
        /// Joins the lobby
        /// </summary>
        /// <returns></returns>
        public LobbyConnection GetLobbyConnection()
        {
            return new LobbyConnection(this);
        }

        /// <summary>
        /// Establishes a world connection
        /// </summary>
        /// <param name="worldId">The world id</param>
        /// <returns>The world connection</returns>
        internal Connection CreateWorldConnection(string worldId)
        {
            int currentVersion = GetGameVersion();
            if (currentVersion == -1)
            {
                //Logged message will be from .GetGameVersion() if failed.
                return null;
            }

            bool isBetaRoom = m_Parser.IsBeta(worldId);
            string roomType = (isBetaRoom) ? string.Format(m_Config.BetaRoom, currentVersion) : string.Format(m_Config.NormalRoom, currentVersion);

            return m_Toolbelt.RunSafe<Connection>(() => m_Client.Multiplayer.CreateJoinRoom(worldId, roomType, true, null, null));
        }

        /// <summary>
        /// Creates a world connection
        /// </summary>
        /// <param name="worldUrlOrId">A full room url or just a world id</param>
        /// <returns>The connection established; otherwise null</returns>
        public WorldConnection GetWorldConnection(string worldUrlOrId)
        {
            string worldId = null;
            if (m_Parser.Parse(worldUrlOrId, out worldId))
            {
                return new WorldConnection(this, worldId);
            }

            return null;
        }

        /// <summary>
        /// Gets a secure connection used for authentication
        /// </summary>
        /// <returns>A secure connection if established; otherwise null</returns>
        public SecureConnection GetSecureConnection()
        {
            PlayerType playerType = GetConnectionType(ConnectionUserId);
            if (playerType != PlayerType.Guest)
            {
                m_Log.Add(FluidLogCategory.Fail, "Secure connections can only be established through guest connections.");
                return null;
            }

            int currentVersion = GetGameVersion();
            if (currentVersion == -1)
            {
                //Logged message will be from .GetGameVersion() if failed.
                return null;
            }

            string authRoomType = string.Format(m_Config.AuthRoom, currentVersion);
            Connection connection = m_Toolbelt.RunSafe<Connection>(() => m_Client.Multiplayer.CreateJoinRoom(string.Empty, authRoomType, false, null, null));
            if (connection != null)
            {
                return new SecureConnection(this, connection);
            }

            return null;
        }

        /// <summary>
        /// Creates a new game connection
        /// </summary>
        /// <param name="auth">The authentication</param>
        public FluidClient(IAuth auth)
        {
            m_Log = new FluidLog();

            if (auth == null)
            {
                auth = new GuestAuth();
                m_Log.Add(FluidLogCategory.Suggestion, "Auto-Logged in as guest, for clarity specify your authentication instead of passing in null.");
            }

            Auth = auth;
            ConnectionValue = new ConnectionValue();
     
            m_Config = new Config();        
            m_Parser = new FluidParser();
            m_Toolbelt = new FluidToolbelt();
            m_PlayerDatabase = new FluidPlayerDatabase(this, m_Log);
        }
    }
}
