namespace Fluid.Blocks
{
    public class Block
    {
        protected WorldConnection m_WorldConnection;
        private BlockID m_ID;

        /// <summary>
        /// Gets the block's id
        /// </summary>
        public BlockID ID 
        { 
            get 
            { 
                return m_ID; 
            } 
            set 
            {
                m_ID = value;
                m_WorldConnection.SendBlock(this);
            }
        }

        /// <summary>
        /// Gets the block's layer
        /// </summary>
        public Layer Layer { get; internal set; }

        /// <summary>
        /// Gets the block's X Coordinate
        /// </summary>
        public int X { get; internal set; }

        /// <summary>
        /// Gets the block's Y Coordinate
        /// </summary>
        public int Y { get; internal set; }

        /// <summary>
        /// Gets the block's placer
        /// </summary>
        public WorldPlayer Placer { get; internal set; }

        /// <summary>
        /// Checks if the block is binded to a connection
        /// </summary>
        internal bool IsBinded { get { return m_WorldConnection != null; } }

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
            string key = m_WorldConnection.World.WorldKey;
            m_WorldConnection.SendMessage(key,
                (int)Layer,
                X,
                Y,
                (int)ID
            );
        }

        /// <summary>
        /// Checks if two blocks are the same
        /// </summary>
        /// <param name="bl">The block</param>
        public bool EqualsBlock(Block bl)
        {
            return X == bl.X && Y == bl.Y && ID == bl.ID && Layer == bl.Layer;
        }

        /// <summary>
        /// Gets the block debug message
        /// </summary>
        public override string ToString()
        {
            return string.Format("ID: {0}, X: {1}, Y: {2}, L: {3}", ID, X, Y, Layer);
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
            m_ID = id;
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
    }
}
