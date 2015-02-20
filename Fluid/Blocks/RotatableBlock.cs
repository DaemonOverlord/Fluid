namespace Fluid.Blocks
{
    public class RotatableBlock : Block
    {
        /// <summary>
        /// Creates a rotatable block
        /// </summary>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="rotation">The rotation of the block</param>
        public RotatableBlock(BlockID blockId, int x, int y, Rotation rotation)
            : base(blockId, Layer.Foreground, x, y)
        {
            Rotation = rotation;
        }

        /// <summary>
        /// Creates a rotatable block
        /// </summary>
        /// <param name="worldCon">The world connection</param>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="rotation">The rotation of the block</param>
        public RotatableBlock(WorldConnection worldCon, BlockID blockId, int x, int y, Rotation rotation)
            : base(worldCon, blockId, Layer.Foreground, x, y)
        {
            Rotation = rotation;
        }

        /// <summary>
        /// Gets the rotation of the block
        /// </summary>
        public Rotation Rotation { get; private set; }

        /// <summary>
        /// Uploads the block to the server
        /// </summary>
        internal override void Upload()
        {
            string key = m_WorldConnection.World.WorldKey;
            m_WorldConnection.SendMessage(key,
                (int)Layer,
                X,
                Y,
                (int)ID,
                (int)Rotation
            );
        }

        /// <summary>
        /// Tests if a rotatatable block is equal to an object
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>True if equal in value</returns>
        public override bool Equals(object obj)
        {
            if (obj is RotatableBlock)
            {
                RotatableBlock rB = (RotatableBlock)obj;
                return rB.X == X && rB.Y == Y && rB.Layer == Layer && rB.ID == ID && rB.Rotation == Rotation;
            }

            return false;
        }
    }
}
