namespace Fluid.Room
{
    public class WorldPortal : Block
    {
        /// <summary>
        /// Creates a world portal block
        /// </summary>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="targetId">The target world id</param>
        public WorldPortal(int x, int y, string targetId)
            : base(BlockIDs.Action.Portals.World, Layer.Foreground, x, y)
        {
            TargetID = targetId;
        }

        /// <summary>
        /// Creates a world portal block
        /// </summary>
        /// <param name="worldCon">The world connection</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="targetId">The target world id</param>
        public WorldPortal(WorldConnection worldCon, int x, int y, string targetId)
            : base(worldCon, BlockIDs.Action.Portals.World, Layer.Foreground, x, y)
        {
            TargetID = targetId;
        }

        /// <summary>
        /// Gets or sets the target ID
        /// </summary>
        public string TargetID { get; set; }

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
                TargetID
            );
        }

        /// <summary>
        /// Creates a clone of this world portal
        /// </summary>
        /// <returns>A clone of this world portal</returns>
        public override Block Clone()
        {
            return new WorldPortal(X, Y, TargetID);
        }

        /// <summary>
        /// Tests if a wolrd portal block is equal to a block
        /// </summary>
        /// <param name="b">The block</param>
        /// <returns>True if equal in value</returns>
        public override bool EqualsBlock(Block b)
        {
            if (b is WorldPortal)
            {
                WorldPortal wpB = (WorldPortal)b;
                return wpB.X == X && wpB.Y == Y && wpB.Layer == Layer && wpB.ID == ID && wpB.TargetID == TargetID;
            }

            return false;
        }
    }
}
