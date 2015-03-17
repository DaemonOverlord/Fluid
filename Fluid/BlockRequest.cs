using Fluid.Room;
using Fluid.Room;
using System;
using System.Diagnostics;

namespace Fluid
{
    [DebuggerDisplay("Block = {Block}")]
    internal class BlockRequest
    {
        //private bool m_NullTimestamp = false;

        /// <summary>
        /// Gets the block
        /// </summary>
        public Block Block { get; set; }

        /// <summary>
        /// Gets the request timestamp
        /// </summary>
        public DateTime? Timestamp { get; set; }

        /// <summary>
        /// Gets the block throttle
        /// </summary>
        public int BlockThrottle { get; set; }

        /// <summary>
        /// Gets or sets whether the request was missed
        /// </summary>
        public bool Missed { get; private set; }

        /// <summary>
        /// Gets or sets whether the block has been sent
        /// </summary>
        public bool HasBeenSent { get; internal set; }

        /// <summary>
        /// Sets the block as missed
        /// </summary>
        public void SetMissed()
        {
            Missed = true;
        }

        /// <summary>
        /// Gets the time since the last request
        /// </summary>
        public int GetTimePassed()
        {
            if (!Timestamp.HasValue)
            {
                return 0;
            }

            try
            {
                return (int)DateTime.Now.Subtract(Timestamp.Value).TotalMilliseconds;
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
        }

        /// <summary>
        /// Sets the last request time to now
        /// </summary>
        public void Request()
        {
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Creates a new block request
        /// </summary>
        /// <param name="block">The block to be uploaded</param>
        /// <param name="blockThrottle">The speed of the block to be uploaded at</param>
        public BlockRequest(Block block, int blockThrottle)
        {
            Block = block;
            BlockThrottle = blockThrottle;
            HasBeenSent = false;
        }
    }
}
