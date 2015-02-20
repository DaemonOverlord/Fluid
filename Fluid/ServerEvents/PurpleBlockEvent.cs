using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
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
