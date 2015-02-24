namespace Fluid.Blocks
{
    public class TextBlock : Block
    {
        /// <summary>
        /// Creates a text block
        /// </summary>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="text">The text of the block</param>
        public TextBlock(BlockID blockId, int x, int y, string text)
            : base(blockId, Layer.Foreground, x, y)
        {
            Text = text;
        }

        /// <summary>
        /// Creates a text block
        /// </summary>
        /// <param name="worldCon">The world connection</param>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="text">The text of the block</param>
        public TextBlock(WorldConnection worldCon, BlockID blockId, int x, int y, string text)
            : base(worldCon, blockId, Layer.Foreground, x, y)
        {
            Text = text;
        }

        /// <summary>
        /// Gets the text
        /// </summary>
        public string Text { get; private set; }

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
                Text
            );
        }

        /// <summary>
        /// Tests if a text block is equal to a block
        /// </summary>
        /// <param name="b">The block</param>
        /// <returns>True if equal in value</returns>
        public bool EqualsBlock(Block b)
        {
            if (b is TextBlock)
            {
                TextBlock tB = (TextBlock)b;
                return tB.X == X && tB.Y == Y && tB.Layer == Layer && tB.ID == ID && tB.Text == Text;
            }

            return false;
        }
    }
}
