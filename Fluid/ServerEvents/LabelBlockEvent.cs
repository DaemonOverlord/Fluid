using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a label block is placed
    /// </summary>
    public class LabelBlockEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the label block
        /// </summary>
        public LabelBlock LabelBlock { get; internal set; }
    }
}
