using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a regular block was placed, or erased
    /// </summary>
    public class BlockEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the changed block
        /// </summary>
        public Block Block { get; internal set; }
    }
}
