using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a text block is placed
    /// </summary>
    public class TextBlockEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the text/sign block
        /// </summary>
        public TextBlock TextBlock { get; internal set; }
    }
}
