namespace Fluid.Blocks
{
    public class Portal : RotatableBlock
    {
        /// <summary>
        /// Creates a portal block
        /// </summary>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="rotation">The rotation</param>
        /// <param name="sourceId">The portal's source id</param>
        /// <param name="target">The portal's target id</param>
        public Portal(BlockID blockId, int x, int y, Rotation rotation, uint sourceId, uint target)
            : base(blockId, x, y, rotation)
        {
            SourceID = sourceId;
            Target = target;
        }

        /// <summary>
        /// Creates a portal block
        /// </summary>
        /// <param name="worldCon">The world connection</param>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="rotation">The rotation</param>
        /// <param name="sourceId">The portal's source id</param>
        /// <param name="target">The portal's target id</param>
        public Portal(WorldConnection worldCon, BlockID blockId, int x, int y, Rotation rotation, uint sourceId, uint target)
            : base(worldCon, blockId, x, y, rotation)
        {
            SourceID = sourceId;
            Target = target;
        }

        /// <summary>
        /// Gets the portal's id
        /// </summary>
        public uint SourceID { get; private set; }

        /// <summary>
        /// Gets the portal's target
        /// </summary>
        public uint Target { get; private set; }

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
                (int)Rotation,
                (int)SourceID,
                (int)Target
            );
        }

        /// <summary>
        /// Tests if a portal block is equal to a block
        /// </summary>
        /// <param name="b">The block</param>
        /// <returns>True if equal in value</returns>
        public bool EqualsBlock(Block b)
        {
            if (b is Portal)
            {
                Portal p = (Portal)b;
                return p.X == X && p.Y == Y && p.Layer == Layer && p.ID == ID && p.Rotation == Rotation && p.SourceID == SourceID && p.Target == Target;
            }

            return false;
        }
    }
}
