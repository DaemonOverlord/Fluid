using Fluid.Blocks;
using System;

namespace Fluid
{
    internal class BlockRequest
    {
        private bool m_NullTimestamp = false;

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
        /// Sets the block as missed
        /// </summary>
        public void SetMissed()
        {
            Missed = true;
            m_NullTimestamp = true;
            Timestamp = null;
        }

        /// <summary>
        /// Gets the last time a request was sent
        /// </summary>
        public int GetTimePassed()
        {
            if (!Timestamp.HasValue || m_NullTimestamp)
            {
                return 0;
            }

            return (int)DateTime.Now.Subtract(Timestamp.Value).TotalMilliseconds;
        }

        /// <summary>
        /// Sets the last request time to now
        /// </summary>
        public void Request()
        {
            m_NullTimestamp = false;
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
        }
    }
}
