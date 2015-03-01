namespace Fluid.Blocks
{
    public class MusicBlock : Block
    {
        /// <summary>
        /// Creates a music block
        /// </summary>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="musicId">The id of the music; Drum, Piano</param>
        public MusicBlock(BlockID blockId, int x, int y, uint musicId)
            : base(blockId, Layer.Foreground, x, y)
        {
            MusicID = musicId;
        }

        /// <summary>
        /// Creates a music block
        /// </summary>
        /// <param name="worldCon">The world connection</param>
        /// <param name="blockId">the block ID</param>
        /// <param name="x">The x coorindate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="musicId">The id of the music; Drum, Piano</param>
        public MusicBlock(WorldConnection worldCon, BlockID blockId, int x, int y, uint musicId)
            : base(worldCon, blockId, Layer.Foreground, x, y)
        {
            MusicID = musicId;
        }

        /// <summary>
        /// <para>Gets the music id</para>                               
        /// <para>Use PianoNote and PercussionSound for more information</para>
        /// </summary>
        public uint MusicID { get; set; }

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
                (int)MusicID
            );
        }

        /// <summary>
        /// Creates a clone of this music block
        /// </summary>
        /// <returns>A clone of this music block</returns>
        public override Block Clone()
        {
            return new MusicBlock(ID, X, Y, MusicID);
        }

        /// <summary>
        /// Tests if a music block is equal to a block
        /// </summary>
        /// <param name="b">The block</param>
        /// <returns>True if equal in value</returns>
        public override bool EqualsBlock(Block b)
        {
            if (b is MusicBlock)
            {
                MusicBlock mB = (MusicBlock)b;
                return mB.X == X && mB.Y == Y && mB.Layer == Layer && mB.ID == ID && mB.MusicID == MusicID;
            }

            return false;
        }
    }
}
