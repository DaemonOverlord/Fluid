namespace Fluid.Blocks
{
    public class LabelBlock : TextBlock
    {
        /// <summary>
        /// Creates a label block
        /// </summary>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="text">The text of the block</param>
        /// <param name="hexColor">The html color of the label</param>
        public LabelBlock(BlockID blockId, int x, int y, string text, string hexColor)
            : base(blockId, x, y, text)
        {
            Color = new FluidColor(hexColor);
        }

        /// <summary>
        /// Creates a label block
        /// </summary>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="color">The color of the label</param>
        public LabelBlock(BlockID blockId, int x, int y, string text, FluidColor color)
            : base(blockId, x, y, text)
        {
            Color = color;
        }

        /// <summary>
        /// Creates a label block
        /// </summary>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="text">The text of the block</param>
        public LabelBlock(BlockID blockId, int x, int y, string text)
            : base(blockId, x, y, text)
        {
            Color = new FluidColor(255, 255, 255);
        }

        /// <summary>
        /// Creates a label block
        /// </summary>
        /// <param name="worldCon">The world connection</param>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="text">The text of the block</param>
        /// <param name="hexColor">The hexadecimal color string</param>
        public LabelBlock(WorldConnection worldCon, BlockID blockId, int x, int y, string text, string hexColor)
            : base(worldCon, blockId, x, y, text)
        {
            Color = new FluidColor(hexColor);
        }

        /// <summary>
        /// Creates a label block
        /// </summary>
        /// <param name="worldCon">The world connection</param>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="text">The text of the block</param>
        public LabelBlock(WorldConnection worldCon, BlockID blockId, int x, int y, string text)
            : base(worldCon, blockId, x, y, text)
        {
            Color = new FluidColor(255, 255, 255);
        }

        /// <summary>
        /// Gets the label color
        /// </summary>
        public FluidColor Color { get; set; }

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
                Text,
                Color.ToHtml()
            );
        }

        /// <summary>
        /// Creates a clone of this label block
        /// </summary>
        /// <returns>A clone of this label block</returns>
        public override Block Clone()
        {
            return new LabelBlock(ID, X, Y, Text, Color);
        }

        /// <summary>
        /// Tests if a label block is equal to a block
        /// </summary>
        /// <param name="b">The block</param>
        /// <returns>True if equal in value</returns>
        public override bool EqualsBlock(Block b)
        {
            if (b is LabelBlock)
            {
                LabelBlock lB = (LabelBlock)b;
                return lB.X == X && lB.Y == Y && lB.Layer == Layer && lB.ID == ID && lB.Color == Color;
            }

            return false;
        }
    }
}
