using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a purple block is placed
    /// </summary>
    public class PurpleBlockEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the purple block
        /// </summary>
        public PurpleBlock PurpleBlock { get; internal set; }
    }
}
