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
        /// Gets the location of the block
        /// </summary>
        /// <returns>The location of the block</returns>
        public FluidPoint Location()
        {
            return new FluidPoint(X, Y);
        }

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
    }
}
