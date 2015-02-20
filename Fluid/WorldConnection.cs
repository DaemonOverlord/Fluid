using Fluid.Blocks;
using Fluid.ServerEvents;
using Fluid.Handlers;
using Fluid.Physics;
using PlayerIOClient;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fluid
{
    public class WorldConnection : FluidConnectionBase
    {
        private long? m_blockThrottle = null;
        private DateTime m_throttleTimestamp;
        private UploadManager m_UploadManager;

        /// <summary>
        /// Gets whether the connection has building access to the world
        /// </summary>
        public bool HasAccess { get; internal set; }

        /// <summary>
        /// Gets the world
        /// </summary>
        public World World { get; internal set; }

        /// <summary>
        /// Gets the chat
        /// </summary>
        public ChatManager Chat { get; internal set; }

        /// <summary>
        /// Gets the players
        /// </summary>
        public PlayerManager Players { get; internal set; }

        /// <summary>
        /// Gets the keys
        /// </summary>
        public KeyManager Keys { get; internal set; }

        /// <summary>
        /// Gets the potion manager
        /// </summary>
        public PotionManager Potions { get; internal set; }

        /// <summary>
        /// Gets the physics engine
        /// </summary>
        public PhysicsEngine PhysicsEngine { get; internal set; }

        /// <summary>
        /// Gets the currently connected player
        /// </summary>
        public WorldPlayer Me { get; internal set; }

        /// <summary>
        /// Gets the player with the crown
        /// </summary>
        public WorldPlayer CrownHolder { get; internal set; }

        /// <summary>
        /// Joins the world
        /// </summary>
        internal void Join()
        {
            this.SendMessage("init");
            PhysicsEngine.Start();

            WaitForServerEvent<InitEvent>();           
            this.SendMessage("init2");
        }

        /// <summary>
        /// Checks the current block throttle
        /// </summary>
        internal void CheckThrottle()
        {
            if (m_blockThrottle.HasValue)
            {
                double secondsPassed = DateTime.Now.Subtract(m_throttleTimestamp).TotalSeconds;
                if (secondsPassed > 10)
                {
                    m_blockThrottle = m_Client.ConnectionValue.GetBlockThrottle();
                    m_throttleTimestamp = DateTime.Now;
                }
            }
            else
            {
                m_blockThrottle = m_Client.ConnectionValue.GetBlockThrottle();
                m_throttleTimestamp = DateTime.Now;
            }
        }

        /// <summary>
        /// Activates a key
        /// </summary>
        /// <param name="key">The key to activate</param>
        public void ActivateKey(Key key)
        {
            switch (key)
            {
                case Key.Red:
                    this.SendMessage(World.WorldKey + "r");
                    break;
                case Key.Green:
                    this.SendMessage(World.WorldKey + "g");
                    break;
                case Key.Blue:
                    this.SendMessage(World.WorldKey + "b");
                    break;
                case Key.Cyan:
                    this.SendMessage(World.WorldKey + "c");
                    break;
                case Key.Magenta:
                    this.SendMessage(World.WorldKey + "m");
                    break;
                case Key.Yellow:
                    this.SendMessage(World.WorldKey + "y");
                    break;
                default:
                    string keyName = Enum.GetName(typeof(Key), key);
                    m_Client.Log.Add(FluidLogCategory.Message, string.Format("You cannot activate a '{0}'", keyName));
                    break;
            }
        }

        /// <summary>
        /// Sends a block to the world
        /// </summary>
        /// <param name="id">The block id</param>
        /// <param name="layer">The layer</param>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        public void SendBlock(int id, Layer layer, int x, int y)
        {
            this.SendBlock((BlockID)id, layer, x, y);
        }

        /// <summary>
        /// Sends a block to the world
        /// </summary>
        /// <param name="id">The block id</param>
        /// <param name="layer">The layer</param>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        public void SendBlock(BlockID id, Layer layer, int x, int y)
        {
            this.SendBlock(new Block(this, id, layer, x, y));
        }

        /// <summary>
        /// Sends a block to the world
        /// </summary>
        /// <param name="block">The block to send</param>
        public void SendBlock(Block block)
        {
            this.SendBlock(block, true);
        }

        /// <summary>
        /// Sends a block to the world
        /// </summary>
        /// <param name="block">The block to send</param>
        internal void SendBlock(Block block, bool waitForManager)
        {
            if (!block.IsBinded)
            {
                block.Bind(this);
            }

            CheckThrottle();
            long blockThrottle = m_blockThrottle.Value;

            if (waitForManager)
            {
                m_UploadManager.QueueBlock(block);
            }
            else
            {
                block.Upload();
                Thread.Sleep((int)blockThrottle);
            }
        }

        /// <summary>
        /// Checks the block to see 
        /// </summary>
        /// <param name="block"></param>
        internal void CheckBlock(Block block)
        {
            m_UploadManager.Confirm(block);
        }

        /// <summary>
        /// Requests access to edit
        /// </summary>
        /// <param name="code">The code</param>
        public void RequestAccess(string code)
        {
            this.SendMessage("access", code);
        }

        /// <summary>
        /// Sets the code to the level
        /// </summary>
        /// <param name="code">The code</param>
        public void SetCode(string code)
        {
            this.SendMessage("key", code);
        }

        /// <summary>
        /// Gives the crown to the connected player
        /// </summary>
        public void GetCrown()
        {
            this.SendMessage(World.WorldKey + "k");
        }

        /// <summary>
        /// Sets the title to the level
        /// </summary>
        /// <param name="title">The title</param>
        public void SetTitle(string title)
        {
            this.SendMessage("name", title);
        }

        /// <summary>
        /// Changes the connected user's face
        /// </summary>
        /// <param name="face">The face id</param>
        public void ChangeFace(FaceID face)
        {
            this.SendMessage(World.WorldKey + "f", (int)face);
        }

        /// <summary>
        /// Saves the world to the server database
        /// </summary>
        public void SaveWorld()
        {
            this.SendMessage("save");
        }

        /// <summary>
        /// Sends a woot
        /// </summary>
        public void WootUp()
        {
            this.SendMessage("wootup");
        }

        /// <summary>
        /// Sets the connected players god mode value
        /// </summary>
        /// <param name="value">True for god mode; False for no god mode</param>
        public void SetGodMode(bool value)
        {
            this.SendMessage("god", value);
        }

        /// <summary>
        /// Sets the connected players guardian mode value
        /// </summary>
        /// <param name="value">True for guardian mode; False for no guardian mode</param>
        public void SetGuardianMode(bool value)
        {
            this.SendMessage("guardian", value);
        }

        /// <summary>
        /// Sets the connected players moderator mode value
        /// </summary>
        /// <param name="value">True for moderator mode; False for no moderator mode</param>
        public void SetModeratorMode(bool value)
        {
            this.SendMessage("mod", value);
        }

        /// <summary>
        /// Sets if the world should allow potions
        /// </summary>
        public void SetAllowPotions(bool allow)
        {
            this.SendMessage("allowpotions", allow);
        }

        /// <summary>
        /// Sets the world's background color
        /// </summary>
        /// <param name="color">The color</param>
        public void SetBackgroundColor(FluidColor color)
        {
            this.SendMessage("say", string.Format("/bgcolor {0}", color.ToHtml()));
        }

        /// <summary>
        /// Loads the level to its latest saved state
        /// </summary>
        public void LoadLevel()
        {
            this.SendMessage("say", "/loadlevel");
        }

        /// <summary>
        /// Resets all players to the set spawn
        /// </summary>
        public void Reset()
        {
            this.SendMessage("say", "/reset");
        }

        /// <summary>
        /// Kicks the player from the world
        /// </summary>
        /// <param name="player">The player</param>
        public void KickPlayer(WorldPlayer player, string reason)
        {
            if (player == null)
            {
                m_Client.Log.Add(FluidLogCategory.Suggestion, "Check if your player is null before attempting to use it.");
                return;
            }

            this.SendMessage("say", string.Format("/kick {0} {1}", player.Username, reason));
        }

        /// <summary>
        /// Touches the player with a potion
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="potion">The potion</param>
        public void TouchPlayer(WorldPlayer player, Potion potion)
        {
            if (player == null)
            {
                m_Client.Log.Add(FluidLogCategory.Suggestion, "Check if your player is null before attempting to use it.");
                return;
            }

            this.SendMessage("touch", player.Id, potion);
        }

        /// <summary>
        /// Touches cake
        /// </summary>
        public void TouchCake()
        {
            this.SendMessage("caketouch", Me.BlockX, Me.BlockY);
        }

        /// <summary>
        /// Touchs diamond
        /// </summary>
        public void TouchDiamond()
        {
            this.SendMessage("diamondtouch", Me.BlockX, Me.BlockY);
        }

        /// <summary>
        /// Touchs a checkpoint
        /// </summary>
        public void TouchCheckpoint()
        {
            this.SendMessage("checkpoint", Me.BlockX, Me.BlockY);
        }

        /// <summary>
        /// Activates a potion
        /// </summary>
        /// <param name="potion">The potion</param>
        public void ActivatePotion(Potion potion)
        {
            this.SendMessage(World.WorldKey + "p", (int)potion);
        }

        /// <summary>
        /// Says a message in the chat
        /// </summary>
        /// <param name="message">The message</param>
        public void Say(string message)
        {
            this.SendMessage("say", message);
        }

        /// <summary>
        /// Sends a private message to a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="message">The message</param>
        public void PrivateMessage(WorldPlayer player, string message)
        {
            if (player == null)
            {
                m_Client.Log.Add(FluidLogCategory.Suggestion, "Check if your player is null before attempting to use it.");
                return;
            }

            this.SendMessage("say", string.Format("/pm {0} {1}", player.Username, message));
        }

        /// <summary>
        /// Kicks all guests
        /// </summary>
        public void KickGuests()
        {
            this.SendMessage("say", "/kickguests");
        }

        /// <summary>
        /// Respawns all players
        /// </summary>
        public void RespawnAll()
        {
            this.SendMessage("say", "/respawnall");
        }

        /// <summary>
        /// Gives edit to a player
        /// </summary>
        /// <param name="player">The player</param>
        public void GiveEdit(WorldPlayer player)
        {
            this.SendMessage("say", string.Format("/giveedit {0}", player.Username));
        }

        /// <summary>
        /// Removes edit from a player
        /// </summary>
        /// <param name="player">The player</param>
        public void RemoveEdit(WorldPlayer player)
        {
            this.SendMessage("say", string.Format("/removeedit {0}", player.Username));
        }

        /// <summary>
        /// Kills a player
        /// </summary>
        /// <param name="player">The player</param>
        public void KillPlayer(WorldPlayer player)
        {
            if (player == null)
            {
                m_Client.Log.Add(FluidLogCategory.Suggestion, "Check if your player is null before attempting to use it.");
                return;
            }

            this.SendMessage("say", string.Format("/kill {0}", player.Username));
        }

        /// <summary>
        /// Teleports a player to a location
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="location">The location</param>
        public void TeleportPlayer(WorldPlayer player, Vector location)
        {
            if (player == null)
            {
                m_Client.Log.Add(FluidLogCategory.Suggestion, "Check if your player is null before attempting to use it.");
                return;
            }

            this.SendMessage("say", string.Format("/teleport {0} {1} {2}", player.Username, location.X, location.Y));
        }

        /// <summary>
        /// Teleports a player to another player
        /// </summary>
        /// <param name="player">The source player</param>
        /// <param name="target">The target player</param>
        public void TeleportPlayer(WorldPlayer player, WorldPlayer target)
        {
            if (player == null)
            {
                m_Client.Log.Add(FluidLogCategory.Suggestion, "Check if your player is null before attempting to use it.");
                return;
            }

            this.TeleportPlayer(player, target.GetLocation());
        }

        /// <summary>
        /// Shutdown immediate active resources
        /// </summary>
        internal override void Shutdown()
        {
            if (PhysicsEngine != null)
            {
                PhysicsEngine.Stop();
            }

            base.Shutdown();
        }

        /// <summary>
        /// Creates a new Fluid connection
        /// </summary>
        /// <param name="client">The Fluid client</param>
        /// <param name="connection">The playerio connection</param>
        public WorldConnection(FluidClient client) : base(client)
        {
            Chat = new ChatManager();
            Players = new PlayerManager();
            Keys = new KeyManager();
            Potions = new PotionManager();
            PhysicsEngine = new PhysicsEngine(this);
            m_UploadManager = new UploadManager(this);

            base.AddMessageHandler(new InitHandler());
            base.AddMessageHandler(new AddHandler());
            base.AddMessageHandler(new LeftHandler());
            base.AddMessageHandler(new CoinHandler());
            base.AddMessageHandler(new CrownHandler());
            base.AddMessageHandler(new SilverCrownHandler());
            base.AddMessageHandler(new FaceHandler());
            base.AddMessageHandler(new GodHandler());
            base.AddMessageHandler(new ModHandler());
            base.AddMessageHandler(new GuardianHandler());
            base.AddMessageHandler(new BackgroundColorHandler());
            base.AddMessageHandler(new SayHandler());
            base.AddMessageHandler(new OldSayHandler());
            base.AddMessageHandler(new LevelupHandler());
            base.AddMessageHandler(new UpdateMetaHandler());
            base.AddMessageHandler(new QuickChatHandler());
            base.AddMessageHandler(new ClearHandler());
            base.AddMessageHandler(new LoadLevelHandler());
            base.AddMessageHandler(new PotionHandler());
            base.AddMessageHandler(new BlockHandler());
            base.AddMessageHandler(new DoorGateBlockHandler());
            base.AddMessageHandler(new MusicBlockHandler());
            base.AddMessageHandler(new PortalBlockHandler());
            base.AddMessageHandler(new RotatableBlockHandler());
            base.AddMessageHandler(new LabelBlockHandler());
            base.AddMessageHandler(new TextBlockHandler());
            base.AddMessageHandler(new WorldPortalBlockHandler());
            base.AddMessageHandler(new AllowPotionsHandler());
            base.AddMessageHandler(new WootHandler());
            base.AddMessageHandler(new WootUpHandler());
            base.AddMessageHandler(new SavedHandler());
            base.AddMessageHandler(new UpgradeHandler());
            base.AddMessageHandler(new MovementHandler());
            base.AddMessageHandler(new HideHandler());
            base.AddMessageHandler(new ShowHandler());
            base.AddMessageHandler(new ConnectionCompleteHandler());
            base.AddMessageHandler(new InfoHandler());
            base.AddMessageHandler(new KillHandler());
            base.AddMessageHandler(new TeleportHandler());
            base.AddMessageHandler(new TeleHandler());
            base.AddMessageHandler(new AccessHandler());
            base.AddMessageHandler(new LostAccessHandler());
        }
    }
}
