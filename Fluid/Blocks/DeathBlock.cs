namespace Fluid.Blocks
{
    public class DeathBlock : Block
    {
        /// <summary>
        /// Creates a death block
        /// </summary>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="requiredDeaths">The deaths required to activate</param>
        public DeathBlock(BlockID blockId, int x, int y, uint requiredDeaths)
            : base(blockId, Layer.Foreground, x, y)
        {
            RequiredDeaths = requiredDeaths;
        }

        /// <summary>
        /// Creates a death block
        /// </summary>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="requiredDeaths">The deaths required to activate</param>
        public DeathBlock(WorldConnection worldCon, BlockID blockId, int x, int y, uint requiredDeaths)
            : base(worldCon, blockId, Layer.Foreground, x, y)
        {
            RequiredDeaths = requiredDeaths;
        }

        /// <summary>
        /// Gets or sets the required deaths
        /// </summary>
        public uint RequiredDeaths { get; set; }

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
                (int)RequiredDeaths
            );
        }

        /// <summary>
        /// Tests if a death block is equal to an object
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>True if equal in value</returns>
        public override bool Equals(object obj)
        {
            if (obj is DeathBlock)
            {
                DeathBlock dB = (DeathBlock)obj;
                return dB.X == X && dB.Y == Y && dB.Layer == Layer && dB.ID == ID && dB.RequiredDeaths == RequiredDeaths;
            }

            return false;
        }
    }
}
