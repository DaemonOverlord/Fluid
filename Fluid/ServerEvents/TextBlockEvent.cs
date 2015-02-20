using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
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
