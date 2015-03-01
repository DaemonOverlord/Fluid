using Fluid.Blocks;

using PlayerIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluid
{
    public sealed class World
    {
        private FluidClient m_Client;
        private Block[, ,] m_WorldData;
        private string m_WorldKey;

        /// <summary>
        /// Gets or Sets the block at a location
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="layer">The layer</param>
        /// <returns>The block at the location; null if the location is invalid</returns>
        public Block this[int x, int y, Layer layer]
        {
            get { return this.GetBlockAt(x, y, layer); }
        }

        /// <summary>
        /// Gets the world's owner
        /// </summary>
        public Player Owner { get; internal set; }

        /// <summary>
        /// Gets the world's title
        /// </summary>
        public string Title { get; internal set; }

        /// <summary>
        /// Gets the world's width
        /// </summary>
        public int Width { get; internal set; }

        /// <summary>
        /// Gets the world's height
        /// </summary>
        public int Height { get; internal set; }

        /// <summary>
        /// Gets the world's plays
        /// </summary>
        public int Plays { get; internal set; }

        /// <summary>
        /// Gets the world's encryption key, does not exists in database world
        /// </summary>
        public string WorldKey
        {
            get
            {
                if (IsDatabaseWorld)
                {
                    if (m_Client != null)
                    {
                        m_Client.Log.Add(FluidLogCategory.Suggestion, "The encryption key property is null in a database world.");
                    }

                    return null;
                }

                return m_WorldKey;
            }
        }

        /// <summary>
        /// Gets the world's type
        /// </summary>
        public WorldType WorldType { get; internal set; }

        /// <summary>
        /// Gets the world's background color
        /// </summary>
        public FluidColor BackgroundColor { get; internal set; }

        /// <summary>
        /// Gets whether the world allows potions
        /// </summary>
        public bool AllowPotions { get; internal set; }

        /// <summary>
        /// Gets the world's woots
        /// </summary>
        public int Woots { get; internal set; }

        /// <summary>
        /// Gets the world's total woots
        /// </summary>
        public int TotalWoots { get; internal set; }

        /// <summary>
        /// Gets whether the world is visible
        /// </summary>
        public bool Visible { get; internal set; }

        /// <summary>
        /// Gets whether the world is loaded
        /// </summary>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// Gets whether the world is a database world
        /// </summary>
        public bool IsDatabaseWorld { get; private set; }

        /// <summary>
        /// Gets whether the world is a tutorial world
        /// </summary>
        public bool IsTutorialWorld { get; private set; }

        /// <summary>
        /// Gets the world's gravity
        /// </summary>
        public double Gravity { get; private set; }

        /// <summary>
        /// Tests if a location is within the world
        /// </summary>
        /// <param name="block">The block</param>
        /// <returns>If the location is within the world's bounds</returns>
        public bool IsInBounds(Block block)
        {
            return this.IsInBounds(block.X, block.Y, block.Layer);
        }

        /// <summary>
        /// Tests if a location is within the world
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="layer">The layer</param>
        /// <returns>If the location is within the world's bounds</returns>
        public bool IsInBounds(int x, int y, Layer layer)
        {
            return x >= 0 && Width > x && y >= 0 && Height > y;
        }

        /// <summary>
        /// Gets the block at a location
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="layer">The layer</param>
        /// <returns>The block at the location; null if the location is invalid</returns>
        public Block GetBlockAt(int x, int y, Layer layer)
        {
            if (IsInBounds(x, y, layer))
            {
                return m_WorldData[x, y, (int)layer].Clone();
            }

            return null;
        }

        /// <summary>
        /// Gets all the worlds blocks
        /// </summary>
        /// <returns>The world's array of blocks</returns>
        public Block[, ,] GetBlocks()
        {
            return this.m_WorldData;
        }

        /// <summary>
        /// Sets the block without sending the block to the server
        /// </summary>
        /// <param name="block">Block to set in the world</param>
        internal void SetBlock(Block block)
        {
            if (IsInBounds(block))
            {
                m_WorldData[block.X, block.Y, (int)block.Layer] = block;
            }
        }

        /// <summary>
        /// Gets the world type from the world's width and height
        /// </summary>
        internal WorldType GetWorldType(int width, int height)
        {
            if (width == 25 && height == 25)
            {
                return WorldType.Small;
            }
            else if (width == 50 && height == 50)
            {
                return WorldType.Medium;
            }
            else if (width == 100 && height == 100)
            {
                return WorldType.Large;
            }
            else if (width == 200 && height == 200)
            {
                return WorldType.Massive;
            }
            else if (width == 400 && height == 50)
            {
                return WorldType.Wide;
            }
            else if (width == 400 && height == 200)
            {
                return WorldType.Great;
            }
            else if (width == 100 && height == 400)
            {
                return WorldType.Tall;
            }
            else if (width == 635 && height == 50)
            {
                return WorldType.UltraWide;
            }
            else if (width == 110 && height == 110)
            {
                return WorldType.LowGravity;
            }
            else if (width == 40 && width == 30)
            {
                return WorldType.HomeWorld;
            }
            else if (width == 300 && height == 300)
            {
                return WorldType.HugeWorld;
            }
            else
            {
                return WorldType.Unknown;
            }
        }

        /// <summary>
        /// Gets a portal in the world by id
        /// </summary>
        /// <param name="id">The id of the portal</param>
        internal Portal GetPortalByID(uint id)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Block current = m_WorldData[x, y, 0];
                    if (current.ID == BlockID.InvisiblePortal ||
                        current.ID == BlockID.Portal)
                    {
                        Portal portal = (Portal)current;
                        if (portal.SourceID == id)
                        {
                            return portal;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets if the block is on the border of the world
        /// </summary>
        /// <param name="block">The block</param>
        /// <returns>True if on the border; otherwise false</returns>
        public bool IsBorderBlock(Block block)
        {
            return block.X == 0 || block.X == Width - 1 || block.Y == 0 || block.Y == Height - 1;
        }

        /// <summary>
        /// Clears the world
        /// </summary>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="borderId">The border id</param>
        /// <param name="workareaId">The workarea id</param>
        internal void Clear(int width, int height, BlockID borderId, BlockID workareaId)
        {
            Width = width;
            Height = height;

            m_WorldData = new Block[Width, Height, 2];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                    {
                        m_WorldData[x, y, 0] = new Block(borderId, Layer.Foreground, x, y);
                    }
                    else
                    {
                        m_WorldData[x, y, 0] = new Block(workareaId, Layer.Foreground, x, y);
                    }

                    m_WorldData[x, y, 1] = new Block(BlockID.GravityNothing, Layer.Background, x, y);
                }
            }
        }

        /// <summary>
        /// Trys to get a property value from a database object
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="databaseObject">The database object</param>
        /// <param name="propertyName">The property name</param>
        private T GetValue<T>(DatabaseObject databaseObject, string propertyName)
        {
            return this.GetValue<T>(databaseObject, propertyName, default(T));
        }

        /// <summary>
        /// Trys to get a property value from a database object
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="databaseObject">The database object</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="defaultValue">The default value for the property</param>
        private T GetValue<T>(DatabaseObject databaseObject, string propertyName, T defaultValue)
        {
            if (databaseObject.Contains(propertyName))
            {
                return (T)databaseObject[propertyName];
            }

            return defaultValue;
        }

        /// <summary>
        /// Creates an empty world from the current width and height
        /// </summary>
        private void CreateEmptyWorld()
        {
            m_WorldData = new Block[Width, Height, 2];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int layer = 0; layer < 2; layer++)
                    {
                        m_WorldData[x, y, layer] = new Block(BlockID.GravityNothing, (Layer)layer, x, y);
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts the raw key
        /// </summary>
        /// <param name="keyRaw">The raw key</param>
        private string DecryptKey(string keyRaw)
        {
            char[] array = keyRaw.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                int number = array[i];
                if (number >= 'a' && number <= 'z')
                {
                    if (number > 'm')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                else if (number >= 'A' && number <= 'Z')
                {
                    if (number > 'M')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }

                array[i] = (char)number;
            }

            return new string(array);
        }

        /// <summary>
        /// Gets the block locations
        /// </summary>
        /// <param name="bytesX">The x bytes</param>
        /// <param name="bytesY">The y bytes</param>
        private List<FluidPoint> GetLocations(byte[] bytesX, byte[] bytesY)
        {
            List<FluidPoint> locations = new List<FluidPoint>();
            for (int i = 0; i < bytesX.Length; i += 2)
            {
                int x = bytesX[i] << 8 | bytesX[i + 1];
                int y = bytesY[i] << 8 | bytesY[i + 1];

                locations.Add(new FluidPoint(x, y));
            }

            return locations;
        }

        /// <summary>
        /// Deserialize's the world data
        /// </summary>
        /// <param name="worldObject">The world data as a database array</param>
        private void Deserialize(DatabaseObject worldObject)
        {
            Owner = m_Client.GetPlayerByConnectionId(GetValue<string>(worldObject, "owner"));
            Width = GetValue<int>(worldObject, "width", 200);
            Height = GetValue<int>(worldObject, "height", 200);
            Title = GetValue<string>(worldObject, "name");
            Plays = GetValue<int>(worldObject, "plays");
            WorldType = (WorldType)GetValue<int>(worldObject, "type", 3);
            AllowPotions = GetValue<bool>(worldObject, "allowpotions", true);
            Woots = GetValue<int>(worldObject, "woots", 0);
            TotalWoots = GetValue<int>(worldObject, "totalwoots", 0);
            Visible = GetValue<bool>(worldObject, "visible", true);
            BackgroundColor = new FluidColor(GetValue<uint>(worldObject, "backgroundColor", 0));

            //Check is worlddata is present
            if (!worldObject.Contains("worlddata"))
            {
                return;
            }

            CreateEmptyWorld();

            DatabaseArray databaseArray = (DatabaseArray)worldObject["worlddata"];
            IEnumerable<object> databaseEnum = (IEnumerable<object>)databaseArray;

            using (IEnumerator<object> enumerator = databaseEnum.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    DatabaseObject blockData = (DatabaseObject)enumerator.Current;

                    byte[] xBytes = blockData.GetBytes("x");
                    byte[] yBytes = blockData.GetBytes("y");
                    BlockID blockId = (BlockID)blockData.GetUInt("type");

                    for (int i = 0; i < xBytes.Length; i += 2)
                    {
                        int x = xBytes[i] << 8 | xBytes[i + 1];
                        int y = yBytes[i] << 8 | yBytes[i + 1];
                        if (blockData.Contains("layer"))
                        {
                            Layer layer = (Layer)blockData.GetInt("layer");

                            switch (blockId)
                            {
                                case BlockID.HazardSpike:
                                case BlockID.DecorSciFi2013BlueSlope:
                                case BlockID.DecorSciFi2013BlueStraight:
                                case BlockID.DecorSciFi2013YellowSlope:
                                case BlockID.DecorSciFi2013YellowStraight:
                                case BlockID.DecorSciFi2013GreenSlope:
                                case BlockID.DecorSciFi2013GreenStraight:
                                case BlockID.OneWayCyan:
                                case BlockID.OneWayPink:
                                case BlockID.OneWayRed:
                                case BlockID.OneWayYellow:
                                    {
                                        Rotation rotation = (Rotation)blockData.GetUInt("rotation");

                                        RotatableBlock rotatableBlock = new RotatableBlock(blockId, x, y, rotation);
                                        SetBlock(rotatableBlock);
                                    }
                                    break;
                                case BlockID.CoinDoor:
                                case BlockID.BlueCoinDoor:
                                case BlockID.CoinGate:
                                case BlockID.BlueCoinGate:
                                    {
                                        uint goal = blockData.GetUInt("goal");

                                        CoinBlock door = new CoinBlock(blockId, x, y, goal);
                                        SetBlock(door);
                                    }
                                    break;
                                case BlockID.MusicDrum:
                                case BlockID.MusicPiano:
                                    {
                                        uint musicId = blockData.GetUInt("id");

                                        MusicBlock musicBlock = new MusicBlock(blockId, x, y, musicId);
                                        SetBlock(musicBlock);
                                    }
                                    break;
                                case BlockID.Portal:
                                case BlockID.InvisiblePortal:
                                    {
                                        Rotation rotation = (Rotation)blockData.GetUInt("rotation");
                                        uint portalid = blockData.GetUInt("id");
                                        uint portaltarget = blockData.GetUInt("target");

                                        Portal portal = new Portal(blockId, x, y, rotation, portalid, portaltarget);
                                        SetBlock(portal);
                                    }
                                    break;
                                case BlockID.SwitchPurple:
                                case BlockID.PurpleSwitchDoor:
                                case BlockID.PurpleSwitchGate:
                                    {
                                        uint goal = 0;
                                        if (blockData.Contains("goal"))
                                        {
                                            goal = blockData.GetUInt("goal");
                                        }

                                        PurpleBlock purpleBlock = new PurpleBlock(blockId, x, y, goal);
                                        SetBlock(purpleBlock);
                                    }
                                    break;
                                case BlockID.DeathDoor:
                                case BlockID.DeathGate:
                                    {
                                        uint goal = blockData.GetUInt("goal");

                                        DeathBlock deathBlock = new DeathBlock(blockId, x, y, goal);
                                        SetBlock(deathBlock);
                                    }
                                    break;
                                case BlockID.WorldPortal:
                                    {
                                        string targetId = blockData.GetString("target");

                                        WorldPortal worldPortal = new WorldPortal(blockId, x, y, targetId);
                                        SetBlock(worldPortal);
                                    }
                                    break;
                                case BlockID.DecorSign:
                                    {
                                        string text = blockData.GetString("text");

                                        TextBlock textBlock = new TextBlock(blockId, x, y, text);
                                        SetBlock(textBlock);
                                    }
                                    break;
                                case BlockID.DecorLabel:
                                    {
                                        string text = blockData.GetString("text");
                                        if (blockData.Contains("text_color"))
                                        {
                                            string hexColor = blockData.GetString("text_color");

                                            LabelBlock labelBlock = new LabelBlock(blockId, x, y, text, hexColor);
                                            SetBlock(labelBlock);
                                        }
                                        else
                                        {
                                            LabelBlock labelBlock = new LabelBlock(blockId, x, y, text);
                                            SetBlock(labelBlock);
                                        }
                                    }
                                    break;
                                default:
                                    Block block = new Block(blockId, layer, x, y);
                                    SetBlock(block);
                                    break;
                            }
                        }
                    }
                }
            }

            IsLoaded = true;
        }

        /// <summary>
        /// Deserialize's the world data
        /// </summary>
        /// <param name="message">The message</param>
        internal void Deserialize(Message message)
        {
            // Find the start of the init message's world data
            uint start = 0;
            for (uint i = 0; i < message.Count; i++)
            {
                if (message[i] is string)
                {
                    if (string.Compare(message.GetString(i), "ws", false) == 0)
                    {
                        start = i + 1;
                        break;
                    }
                }
            }

            uint index = start;
            try
            {           
                while (index < message.Count)
                {
                    if (message[index] is string)
                    {
                        if (string.Compare(message.GetString(index), "we", false) == 0)
                        {
                            break;
                        }
                    }

                    int blockInt = message.GetInt(index++);
                    int layerInt = message.GetInt(index++);

                    byte[] bytesX = message.GetByteArray(index++);
                    byte[] bytesY = message.GetByteArray(index++);

                    List<FluidPoint> locations = GetLocations(bytesX, bytesY);

                    BlockID blockId = (BlockID)blockInt;
                    Layer layer = (Layer)layerInt;

                    switch (blockId)
                    {
                        case BlockID.HazardSpike:
                        case BlockID.DecorSciFi2013BlueSlope:
                        case BlockID.DecorSciFi2013BlueStraight:
                        case BlockID.DecorSciFi2013YellowSlope:
                        case BlockID.DecorSciFi2013YellowStraight:
                        case BlockID.DecorSciFi2013GreenSlope:
                        case BlockID.DecorSciFi2013GreenStraight:
                        case BlockID.OneWayCyan:
                        case BlockID.OneWayPink:
                        case BlockID.OneWayRed:
                        case BlockID.OneWayYellow:
                            {
                                Rotation rotation = (Rotation)message.GetUInt(index++);

                                foreach (FluidPoint p in locations)
                                {
                                    RotatableBlock rotatableBlock = new RotatableBlock(blockId, p.X, p.Y, rotation);
                                    SetBlock(rotatableBlock);
                                }
                            }
                            break;
                        case BlockID.CoinDoor:
                        case BlockID.BlueCoinDoor:
                        case BlockID.CoinGate:
                        case BlockID.BlueCoinGate:
                            {
                                uint goal = message.GetUInt(index++);

                                foreach (FluidPoint p in locations)
                                {
                                    CoinBlock door = new CoinBlock(blockId, p.X, p.Y, goal);
                                    SetBlock(door);
                                }
                            }
                            break;
                        case BlockID.MusicDrum:
                        case BlockID.MusicPiano:
                            {
                                uint musicId = message.GetUInt(index++);

                                foreach (FluidPoint p in locations)
                                {
                                    MusicBlock musicBlock = new MusicBlock(blockId, p.X, p.Y, musicId);
                                    SetBlock(musicBlock);
                                }
                            }
                            break;
                        case BlockID.Portal:
                        case BlockID.InvisiblePortal:
                            {
                                Rotation rotation = (Rotation)message.GetUInt(index++);
                                uint portalid = message.GetUInt(index++);
                                uint portaltarget = message.GetUInt(index++);

                                foreach (FluidPoint p in locations)
                                {
                                    Portal portal = new Portal(blockId, p.X, p.Y, rotation, portalid, portaltarget);
                                    SetBlock(portal);
                                }
                            }
                            break;
                        case BlockID.SwitchPurple:
                        case BlockID.PurpleSwitchDoor:
                        case BlockID.PurpleSwitchGate:
                            {
                                uint goal = message.GetUInt(index++);

                                foreach (FluidPoint p in locations)
                                {
                                    PurpleBlock purpleBlock = new PurpleBlock(blockId, p.X, p.Y, goal);
                                    SetBlock(purpleBlock);
                                }
                            }
                            break;
                        case BlockID.DeathDoor:
                        case BlockID.DeathGate:
                            {
                                uint goal = message.GetUInt(index++);

                                foreach (FluidPoint p in locations)
                                {
                                    DeathBlock deathBlock = new DeathBlock(blockId, p.X, p.Y, goal);
                                    SetBlock(deathBlock);
                                }
                            }
                            break;
                        case BlockID.WorldPortal:
                            {
                                string targetId = message.GetString(index++);

                                foreach (FluidPoint p in locations)
                                {
                                    WorldPortal worldPortal = new WorldPortal(blockId, p.X, p.Y, targetId);
                                    SetBlock(worldPortal);
                                }
                            }
                            break;
                        case BlockID.DecorSign:
                            {
                                string text = message.GetString(index++);

                                foreach (FluidPoint p in locations)
                                {
                                    TextBlock textBlock = new TextBlock(blockId, p.X, p.Y, text);
                                    SetBlock(textBlock);
                                }
                            }
                            break;
                        case BlockID.DecorLabel:
                            {
                                string text = message.GetString(index++);
                                string hexColor = message.GetString(index++);

                                foreach (FluidPoint p in locations)
                                {
                                    LabelBlock labelBlock = new LabelBlock(blockId, p.X, p.Y, text, hexColor);
                                    SetBlock(labelBlock);
                                }
                            }
                            break;
                        default:
                            foreach (FluidPoint p in locations)
                            {
                                Block block = new Block(blockId, layer, p.X, p.Y);
                                SetBlock(block);
                            }
                            break;
                    }
                }
            }
            catch
            {
                m_Client.Log.Add(FluidLogCategory.Message, "World init deserializer is out of date. Check for an update.");
            }
        }

        /// <summary>
        /// Deserialize's the init message
        /// </summary>
        /// <param name="initMessage">The init message</param>
        private void DeserializeInit(Message initMessage)
        {
            Owner = m_Client.GetPlayerByUsername(initMessage.GetString(0));
            Title = initMessage.GetString(1);
            Plays = initMessage.GetInt(2);
            Woots = initMessage.GetInt(3);
            TotalWoots = initMessage.GetInt(4);
            m_WorldKey = DecryptKey(initMessage.GetString(5));

            Width = initMessage.GetInt(12u);
            Height = initMessage.GetInt(13u);
            WorldType = GetWorldType(Width, Height);         
            IsTutorialWorld = initMessage.GetBoolean(14);
            Gravity = initMessage.GetDouble(15);
            AllowPotions = initMessage.GetBoolean(16);
            BackgroundColor = new FluidColor(initMessage.GetUInt(17));
            Visible = initMessage.GetBoolean(18);

            CreateEmptyWorld();
            Deserialize(initMessage);

            IsLoaded = true;
        }

        /// <summary>
        /// Gets the world debug message
        /// </summary>
        public override string ToString()
        {
            return string.Format("Title: {0}", Title);
        }

        /// <summary>
        /// Creates a new world from database data
        /// </summary>
        /// <param name="client">The client</param>
        /// <param name="worldObject">The world data</param>
        internal World(FluidClient client, DatabaseObject worldObject)
        {
            IsDatabaseWorld = true;

            this.m_Client = client;
            this.Deserialize(worldObject);     
        }

        /// <summary>
        /// Creates a new world from a init message
        /// </summary>
        /// <param name="client">The client</param>
        /// <param name="initMessage">The init message</param>
        internal World(FluidClient client, Message initMessage)
        {
            IsDatabaseWorld = false;

            this.m_Client = client;
            this.DeserializeInit(initMessage);
        }
    }
}
