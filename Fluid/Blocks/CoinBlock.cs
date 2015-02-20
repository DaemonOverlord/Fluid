namespace Fluid.Blocks
{
    public class CoinBlock : Block
    {
        /// <summary>
        /// Creates a coin block
        /// </summary>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="goal">The coin door coin goal</param>
        public CoinBlock(BlockID blockId, int x, int y, uint goal)
            : base(blockId, Layer.Foreground, x, y)
        {
            Goal = goal;
        }

        /// <summary>
        /// Creates a coin block
        /// </summary>
        /// <param name="worldCon">The world connection</param>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="goal">The coin door coin goal</param>
        public CoinBlock(WorldConnection worldCon, BlockID blockId, int x, int y, uint goal)
            : base(worldCon, blockId, Layer.Background, x, y)
        {
            Goal = goal;
        }

        /// <summary>
        /// Gets the coin door's coin goal
        /// </summary>
        public uint Goal { get; private set; }

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
                (int)Goal
            );
        }

        /// <summary>
        /// Tests if a coin block is equal to an object
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>True if equal in value</returns>
        public override bool Equals(object obj)
        {
            if (obj is CoinBlock)
            {
                CoinBlock cB = (CoinBlock)obj;
                return cB.X == X && cB.Y == Y && cB.Layer == Layer && cB.ID == ID && cB.Goal == Goal;
            }

            return false;
        }
    }
}
