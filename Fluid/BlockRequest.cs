using Fluid.Blocks;
using System;

namespace Fluid
{
    internal class BlockRequest
    {
        /// <summary>
        /// Gets the block
        /// </summary>
        public Block Block { get; set; }

        /// <summary>
        /// Gets the request timestamp
        /// </summary>
        public DateTime? Timestamp { get; set; }

        /// <summary>
        /// Gets the last time a request was sent
        /// </summary>
        public int GetTimePassed()
        {
            if (!Timestamp.HasValue)
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
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Creates a new block request
        /// </summary>
        /// <param name="block">The block to be uploaded</param>
        public BlockRequest(Block block)
        {
            Block = block;
        }
    }
}
