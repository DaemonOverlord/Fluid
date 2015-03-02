using Fluid.Blocks;
using Fluid.ServerEvents;
using Fluid.Handlers;
using Fluid.Physics;
using PlayerIOClient;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace Fluid
{
    public class WorldConnection : ConnectionBase
    {
        private long? m_blockThrottle = null;
        private DateTime m_throttleTimestamp;
        private BlockUploadManager m_UploadManager;

        /// <summary>
        /// Gets whether the connection has building access to the world
        /// </summary>
        public bool HasAccess { get; internal set; }

        /// <summary>
        /// Gets the world id
        /// </summary>
        public string WorldID { get; internal set; }

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
        public PhysicsEngine Physics { get; internal set; }

        /// <summary>
        /// Gets the block uploader
        /// </summary>
        public BlockUploadManager Uploader { get { return m_UploadManager; } }

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
        /// <returns>The connection for chaining your code if necessary</returns>
        public WorldConnection Join()
        {
            Connection connection = m_Client.CreateWorldConnection(WorldID);
            this.SetConnection(connection);

            this.SendMessage("init");
            Physics.Start();

            if (WaitForServerEvent<InitEvent>(2000) != null)
            {
                this.SendMessage("init2");
            }
            else
            {
                Physics.Stop();
            }

            return this;
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
        /// Gets the layer of a block id
        /// </summary>
        /// <param name="blockId">The block id</param>
        /// <returns>The layer of the block</returns>
        public Layer GetBlockLayer(BlockID blockId)
        {
            int id = (int)blockId;
            if (500 <= id && id <= 700)
            {
                return Layer.Background;
            }

            return Layer.Foreground;
        }

        /// <summary>
        /// Sends a block to the world
        /// </summary>
        /// <param name="id">The block id</param>
        /// <param name="layer">The layer</param>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        public void SendBlock(BlockID id, int x, int y)
        {
            this.SendBlock(new Block(this, id, GetBlockLayer(id), x, y));
        }

        /// <summary>
        /// Sends a block to the world
        /// </summary>
        /// <param name="id">The block id</param>
        /// <param name="layer">The layer</param>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="blockThrottle">The speed at which to upload the block in milliseconds</param>
        public void SendBlock(BlockID id, int x, int y, int blockThrottle)
        {

            this.SendBlock(new Block(this, id, GetBlockLayer(id), x, y), blockThrottle);
        }

        /// <summary>
        /// Sends a block to the world
        /// </summary>
        /// <param name="block">The block to send</param>
        public void SendBlock(Block block)
        {
            CheckThrottle();
            this.QueueBlock(block, (int)m_blockThrottle.Value);           
        }

        /// <summary>
        /// Sends the block at a specific speed
        /// </summary>
        /// <param name="block">The block to send</param>
        /// <param name="blockThrottle">The speed at which to upload the block in milliseconds</param>
        public void SendBlock(Block block, int blockThrottle)
        {
            this.QueueBlock(block, blockThrottle);
        }

        /// <summary>
        /// Queues the block to be uploaded
        /// </summary>
        /// <param name="block">The block</param>
        /// <param name="blockThrottle">The speed to upload the block</param>
        internal void QueueBlock(Block block, int blockThrottle)
        {
            if (!block.IsBinded)
            {
                block.Bind(this);
            }

            m_UploadManager.QueueBlock(block, (int)blockThrottle);
        }

        /// <summary>
        /// Sends a block to the world
        /// </summary>
        /// <param name="blockRequest">The block to send</param>
        internal void UploadBlockRequest(BlockRequest blockRequest)
        {
            Block block = blockRequest.Block;

            if (block != null)
            {
                block.Upload();
                blockRequest.HasBeenSent = true;
                Thread.Sleep(blockRequest.BlockThrottle);
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
        /// Gets the silver crown
        /// </summary>
        public void GetSilverCrown()
        {
            this.SendMessage("levelcomplete");
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
        /// Toggles the connected players moderator mode
        /// </summary>
        public void ToggleModeratorMode()
        {
            this.SendMessage("mod");
        }

        /// <summary>
        /// Sets if the world should allow potions
        /// </summary>
        public void SetAllowPotions(bool allow)
        {
            this.SendMessage("allowpotions", allow);
        }

        /// <summary>
        /// Sets the visibility of the world
        /// </summary>
        /// <param name="visibility">The visibility</param>
        public void SetVisibility(bool visibility)
        {
            this.SendMessage("say", string.Format("/visible {0}", visibility));
        }

        /// <summary>
        /// Sets the background color
        /// </summary>
        /// <param name="r">The red color value</param>
        /// <param name="g">The green color value</param>
        /// <param name="b">The blue color value</param>
        public void SetBackgroundColor(byte r, byte g, byte b)
        {
            this.SetBackgroundColor(new FluidColor(r, b, b));
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
        public void KickPlayer(WorldPlayer player)
        {
            if (player == null)
            {
                m_Client.Log.Add(FluidLogCategory.Suggestion, "Check if your player is null before attempting to use it.");
                return;
            }

            this.SendMessage("say", string.Format("/kick {0}", player.Username));
        }

        /// <summary>
        /// Kicks the player from the world
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="reason">The reason shown the player why he/she was kicked</param>
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
        /// <param name="blockX">The block's x coordinate</param>
        /// <param name="blockY">The block's y coordinate</param>
        public void TouchCake(int blockX, int blockY)
        {
            this.SendMessage("caketouch", blockX, blockY);
        }

        /// <summary>
        /// Touchs a diamond
        /// </summary>
        /// <param name="blockX">The block's x coordinate</param>
        /// <param name="blockY">The block's y coordinate</param>
        public void TouchDiamond(int blockX, int blockY)
        {
            this.SendMessage("diamondtouch", blockX, blockY);
        }

        /// <summary>
        /// Touchs a hologram
        /// </summary>
        /// <param name="blockX">The block's x coordinate</param>
        /// <param name="blockY">The block's y coordinate</param>
        public void TouchHologram(int blockX, int blockY)
        {
            this.SendMessage("hologramtouch", blockX, blockY);
        }

        /// <summary>
        /// Touchs a checkpoint
        /// </summary>
        /// <param name="blockX">The block's x coordinate</param>
        /// <param name="blockY">The block's y coordinate</param>
        public void TouchCheckpoint(int blockX, int blockY)
        {
            this.SendMessage("checkpoint", blockX, blockY);
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
        /// Sets a unique list of potions on, if no potions are passed in, all potions will be turned on
        /// </summary>
        /// <param name="potions">The unique list of potions</param>
        public void SetPotionsOn(params Potion[] potions)
        {
            if (potions.Length == 0)
            {
                this.SetPotionsOn((Potion[])Enum.GetValues(typeof(Potion)));
            }
            else
            {
                int[] potionIds = new int[potions.Length];
                for (int i = 0; i < potions.Length; i++)
                {
                    potionIds[i] = (int)potions[i];
                }

                this.SendMessage("say", string.Format("/potionson {0}", string.Join(" ", potionIds)));
            }
        }

        /// <summary>
        /// Sets a unique list of potions off, if no potions are passed in, all potions will be turned off
        /// </summary>
        /// <param name="potions">The unique list of potions</param>
        public void SetPotionsOff(params Potion[] potions)
        {
            if (potions.Length == 0)
            {
                this.SetPotionsOff((Potion[])Enum.GetValues(typeof(Potion)));
            }
            else
            {
                int[] potionIds = new int[potions.Length];
                for (int i = 0; i < potions.Length; i++)
                {
                    potionIds[i] = (int)potions[i];
                }

                this.SendMessage("say", string.Format("/potionsoff {0}", string.Join(" ", potionIds)));
            }
        }

        /// <summary>
        /// Sends a quick chat message
        /// </summary>
        /// <param name="quickChatMessage">The quick chat message</param>
        public void QuickChat(QuickChatMessage quickChatMessage)
        {
            this.SendMessage("autosay", (int)quickChatMessage);
        }

        /// <summary>
        /// Says a message in the chat
        /// </summary>
        /// <param name="message">The message</param>
        public void Say(string message)
        {
            this.Chat.Say(message);
        }

        /// <summary>
        /// Sends the chat message to the server
        /// </summary>
        /// <param name="message">The message</param>
        internal void SayInternal(string message)
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
                //Player could be null from getting player from Players
                return;
            }

            this.PrivateMessage(player.Username, message);
        }

        /// <summary>
        /// Sends a private message to a player
        /// </summary>
        /// <param name="username">The player's username</param>
        /// <param name="message">The message</param>
        public void PrivateMessage(string username, string message)
        {
            //Include space at end
            this.Chat.Say(message, string.Format("/pm {0} ", username));
        }

        /// <summary>
        /// Kills all players
        /// </summary>
        public void KillAll()
        {
            this.SendMessage("say", "/killemall");
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
        public void TeleportPlayer(WorldPlayer player, FluidPoint location)
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

            this.TeleportPlayer(player, target.GetBlockLocation());
        }

        /// <summary>
        /// Teleports to a location
        /// </summary>
        /// <param name="location">The world location</param>
        public void MoveToLocation(FluidPoint location)
        {
            if (location == null)
            {
                return;
            }

            this.MoveToLocation(location.X, location.Y);
        }

        /// <summary>
        /// Teleports to a location
        /// </summary>
        /// <param name="x">The world pixel x</param>
        /// <param name="y">The world pixel y</param>
        public void MoveToLocation(double x, double y)
        {
            this.SendMessage("m", x, y, 0, 0, 0, 0, 0, 0, World.Gravity, false);
        }

        /// <summary>
        /// Sends movement
        /// </summary>
        /// <param name="x">The pixel location x</param>
        /// <param name="y">The pixel location y</param>
        /// <param name="speedX">The speed in the x direction</param>
        /// <param name="speedY">The speed in the y direction</param>
        /// <param name="modifierX">The current player modifier in the x direction</param>
        /// <param name="modifierY">The current player modifier in the y direction</param>
        /// <param name="horizontal">The player input in the horizontal direction</param>
        /// <param name="vertical">The player input in the vertical direction</param>
        /// <param name="holdingSpace">Whether the player is holding space</param>
        public void SendMovement(double x, double y, double speedX, double speedY, double modifierX, double modifierY, int horizontal, int vertical, bool holdingSpace)
        {
            this.SendMessage("m",
                x,
                y,
                speedX,
                speedY,
                modifierX,
                modifierY,
                horizontal,
                vertical,
                World.Gravity,
                holdingSpace
            );
        }

        /// <summary>
        /// Sends player input
        /// </summary>
        /// <param name="input">The input combination to send</param>
        public void SendMovementInput(Input input)
        {
            double speedX = 0, speedY = 0;
            int horizontal = 0, vertical = 0;

            WorldPlayer me = Me;
            if ((input & Input.HoldSpace) != 0)
            {
                if (me.SpeedX == 0 && !Physics.ApproachingZero(me.m_morx) && !Physics.ApproachingZero(me.m_mox) && me.X % 16 == 0)
                {
                    speedX = (me.SpeedX - (me.m_morx * PhysicsEngine.JumpHeight));
                }

                if (me.SpeedY == 0 && !Physics.ApproachingZero(me.m_mory) && !Physics.ApproachingZero(me.m_moy) && me.Y % 16 == 0)
                {
                    speedY = (me.SpeedY - (me.m_mory * PhysicsEngine.JumpHeight));
                }
            }

            horizontal = ((input & Input.HoldLeft) != 0 ? -1 : 1) + ((input & Input.HoldRight) != 0 ? 1 : -1);
            vertical = ((input & Input.HoldUp) != 0 ? -1 : 1) + ((input & Input.HoldDown) != 0 ? 1 : -1);

            SendMovement(me.X, me.Y, speedX, speedY, me.ModifierX, me.ModifierY, horizontal, vertical, false);
        }

        /// <summary>
        /// Reestablishes a connection to the world
        /// </summary>
        internal override void Reconnect()
        {
            Connection connection = m_Client.CreateWorldConnection(WorldID);
            if (connection != null)
            {
                base.SetConnection(connection);
                this.Join();
            }

            base.Reconnect();
        }

        /// <summary>
        /// Shutdown immediate active resources
        /// </summary>
        internal override void Shutdown()
        {
            if (Physics != null)
            {
                Physics.Stop();
            }

            base.Shutdown();
        }

        /// <summary>
        /// Creates a new Fluid connection
        /// </summary>
        /// <param name="client">The Fluid client</param>
        /// <param name="worldId">The world id</param>
        public WorldConnection(FluidClient client, string worldId) : base(client)
        {
            WorldID = worldId;

            Chat = new ChatManager(this);
            Players = new PlayerManager();
            Keys = new KeyManager();
            Potions = new PotionManager();
            Physics = new PhysicsEngine(this);
            m_UploadManager = new BlockUploadManager(this);

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
