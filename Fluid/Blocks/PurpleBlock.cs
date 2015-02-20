namespace Fluid.Blocks
{
    public class PurpleBlock : Block
    {
        /// <summary>
        /// Creates a world portal block
        /// </summary>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="switchId">The switch id</param>
        public PurpleBlock(BlockID blockId, int x, int y, uint switchId)
            : base(blockId, Layer.Foreground, x, y)
        {
            SwitchID = switchId;
        }

        /// <summary>
        /// Creates a world portal block
        /// </summary>
        /// <param name="worldCon">The world connection</param>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="switchId">The switch id</param>
        public PurpleBlock(WorldConnection worldCon, BlockID blockId, int x, int y, uint switchId)
            : base(worldCon, blockId, Layer.Foreground, x, y)
        {
            SwitchID = switchId;
        }

        /// <summary>
        /// Gets or sets the switch ID
        /// </summary>
        public uint SwitchID { get; set; }

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
                (int)SwitchID
            );
        }

        /// <summary>
        /// Tests if a purple block is equal to an object
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>True if equal in value</returns>
        public override bool Equals(object obj)
        {
            if (obj is PurpleBlock)
            {
                PurpleBlock pB = (PurpleBlock)obj;
                return pB.X == X && pB.Y == Y && pB.Layer == Layer && pB.ID == ID && pB.SwitchID == SwitchID;
            }

            return false;
        }
    }
}
