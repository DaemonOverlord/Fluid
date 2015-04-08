using System.Diagnostics;
namespace Fluid.Room
{
    [DebuggerDisplay("ID = {ID}, X = {X}, Y = {Y}, Layer = {Layer}")]
    public class Block
    {
        protected WorldConnection m_WorldConnection;

        /// <summary>
        /// Gets or sets the block's id
        /// </summary>
        public BlockID ID { get; set; }

        /// <summary>
        /// Gets or sets the block's layer
        /// </summary>
        public Layer Layer { get; set; }

        /// <summary>
        /// Gets or sets the block's X Coordinate
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the block's Y Coordinate
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets the block's placer
        /// </summary>
        public WorldPlayer Placer { get; internal set; }

        /// <summary>
        /// Checks if the block is binded to a connection
        /// </summary>
        internal bool IsBinded { get { return m_WorldConnection != null; } }

        /// <summary>
        /// Gets the location of the block
        /// </summary>
        /// <returns>The location of the block</returns>
        public FluidPoint Location()
        {
            return new FluidPoint(X, Y);
        }

        /// <summary>
        /// Gets the minimap color
        /// </summary>
        /// <returns>The minimap color of the block if known; otherwise null</returns>
        public FluidColor GetMinimapColor()
        {
            return ItemInfo.GetMinimapColor(ID);
        }

        /// <summary>
        /// Gets the block image
        /// </summary>
        /// <returns>The image of the block if known; otherwise null</returns>
        public BlockImage GetImage()
        {
            return ItemInfo.GetBlockImage(ID);
        }

        /// <summary>
        /// Binds the block to a connection
        /// </summary>
        /// <param name="worldCon">The world connection</param>
        internal void Bind(WorldConnection worldCon)
        {
            this.m_WorldConnection = worldCon;
        }

        /// <summary>
        /// Uploads the block to the server
        /// </summary>
        internal virtual void Upload()
        {
            if (m_WorldConnection != null)
            {
                if (m_WorldConnection.World != null)
                {
                    string key = m_WorldConnection.World.WorldKey;
                    m_WorldConnection.SendMessage(key,
                        (int)Layer,
                        X,
                        Y,
                        (int)ID
                    );
                }
            }
        }

        /// <summary>
        /// Create's a cloned copy of this block
        /// </summary>
        /// <returns>A clone of this block</returns>
        public virtual Block Clone()
        {
            return new Block(ID, Layer, X, Y);
        }

        /// <summary>
        /// Checks if two blocks are the same
        /// </summary>
        /// <param name="bl">The block</param>
        public virtual bool EqualsBlock(Block bl)
        {
            return X == bl.X && Y == bl.Y && ID == bl.ID && Layer == bl.Layer;
        }

        /// <summary>
        /// Creates a new world block
        /// </summary>
        /// <param name="id">the block ID</param>
        /// <param name="layer">The layer</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        public Block(BlockID id, Layer layer, int x, int y)
        {
            ID = id;
            Layer = layer;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Creates a new world block
        /// </summary>
        /// <param name="worldCon">The world connection</param>
        /// <param name="id">the block ID</param>
        /// <param name="layer">The layer</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        public Block(WorldConnection worldCon, BlockID id, Layer layer, int x, int y)
            : this(id, layer, x, y)
        {
            this.m_WorldConnection = worldCon;
        }

        /// <summary>
        /// Gets the layer of a block id
        /// </summary>
        /// <param name="blockId">The block id</param>
        /// <returns>The layer of the block</returns>
        public static Layer GetBlockLayer(BlockID blockId)
        {
            int id = (int)blockId;
            if (500 <= id && id <= 700)
            {
                return Layer.Background;
            }

            return Layer.Foreground;
        }

        /// <summary>
        /// Creates a default block
        /// </summary>
        /// <param name="id">The id of the block</param>
        /// <param name="x">The block x coordinate</param>
        /// <param name="y">The block y coordinate</param>
        /// <returns>The created block</returns>
        public static Block Create(BlockID id, int x, int y)
        {
            Layer l = GetBlockLayer(id);
            switch (id)
            {
                case BlockIDs.Decor.Scifi2013.Blue:
                case BlockIDs.Decor.Scifi2013.BlueDiagonal:
                case BlockIDs.Decor.Scifi2013.Gold:
                case BlockIDs.Decor.Scifi2013.GoldDiagonal:
                case BlockIDs.Decor.Scifi2013.Green:
                case BlockIDs.Decor.Scifi2013.GreenDiagonal:
                case BlockIDs.Action.Hazards.Spike:
                case BlockIDs.Blocks.OneWay.Cyan:
                case BlockIDs.Blocks.OneWay.Pink:
                case BlockIDs.Blocks.OneWay.Orange:
                case BlockIDs.Blocks.OneWay.Gold:
                    return new RotatableBlock(id, x, y, 0);
                case BlockIDs.Action.Doors.GoldCoin:
                case BlockIDs.Action.Gates.GoldCoin:
                case BlockIDs.Action.Doors.BlueCoin:
                case BlockIDs.Action.Gates.BlueCoin:
                    return new CoinBlock(id, x, y, 0);
                case BlockIDs.Action.Music.Drum:
                case BlockIDs.Action.Music.Piano:
                    return new MusicBlock(id, x, y, 0);
                case BlockIDs.Action.Portals.Portal:
                case BlockIDs.Action.Portals.InvisPortal:           
                    return new Portal(id, x, y, 0, 0, 0);
                case BlockIDs.Action.Switches.Switch:
                case BlockIDs.Action.Doors.Switch:
                case BlockIDs.Action.Gates.Switch:
                    return new PurpleBlock(id, x, y, 0);
                case BlockIDs.Action.Doors.Death:
                case BlockIDs.Action.Gates.Death:
                    return new DeathBlock(id, x, y, 0);
                case BlockIDs.Action.Portals.World:
                    return new WorldPortal(x, y, string.Empty);
                case BlockIDs.Action.Sign.Block:
                    return new TextBlock(id, x, y, string.Empty);
                case BlockIDs.Action.Admin.Text:
                    return new LabelBlock(x, y, string.Empty);
                default:
                    return new Block(id, l, x, y);
            }
        }

        /// <summary>
        /// Creates a default block
        /// </summary>
        /// <param name="con">The attached world connection</param>
        /// <param name="id">The id of the block</param>
        /// <param name="x">The block x coordinate</param>
        /// <param name="y">The block y coordinate</param>
        /// <returns>The created block</returns>
        public static Block Create(WorldConnection con, BlockID id, int x, int y)
        {
            Layer l = GetBlockLayer(id);
            switch (id)
            {
                case BlockIDs.Decor.Scifi2013.Blue:
                case BlockIDs.Decor.Scifi2013.BlueDiagonal:
                case BlockIDs.Decor.Scifi2013.Gold:
                case BlockIDs.Decor.Scifi2013.GoldDiagonal:
                case BlockIDs.Decor.Scifi2013.Green:
                case BlockIDs.Decor.Scifi2013.GreenDiagonal:
                case BlockIDs.Action.Hazards.Spike:
                case BlockIDs.Blocks.OneWay.Cyan:
                case BlockIDs.Blocks.OneWay.Pink:
                case BlockIDs.Blocks.OneWay.Orange:
                case BlockIDs.Blocks.OneWay.Gold:
                    return new RotatableBlock(con, id, x, y, 0);
                case BlockIDs.Action.Doors.GoldCoin:
                case BlockIDs.Action.Gates.GoldCoin:
                case BlockIDs.Action.Doors.BlueCoin:
                case BlockIDs.Action.Gates.BlueCoin:
                    return new CoinBlock(con, id, x, y, 0);
                case BlockIDs.Action.Music.Drum:
                case BlockIDs.Action.Music.Piano:
                    return new MusicBlock(con, id, x, y, 0);
                case BlockIDs.Action.Portals.Portal:
                case BlockIDs.Action.Portals.InvisPortal:
                    return new Portal(con, id, x, y, 0, 0, 0);
                case BlockIDs.Action.Switches.Switch:
                case BlockIDs.Action.Doors.Switch:
                case BlockIDs.Action.Gates.Switch:
                    return new PurpleBlock(con, id, x, y, 0);
                case BlockIDs.Action.Doors.Death:
                case BlockIDs.Action.Gates.Death:
                    return new DeathBlock(con, id, x, y, 0);
                case BlockIDs.Action.Portals.World:
                    return new WorldPortal(con, x, y, string.Empty);
                case BlockIDs.Action.Sign.Block:
                    return new TextBlock(con, id, x, y, string.Empty);
                case BlockIDs.Action.Admin.Text:
                    return new LabelBlock(con, x, y, string.Empty);
                default:
                    return new Block(con, id, l, x, y);
            }
        }
    }
}
