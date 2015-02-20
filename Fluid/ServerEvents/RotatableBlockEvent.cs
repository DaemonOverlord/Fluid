using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class RotatableBlockEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the rotatable block
        /// </summary>
        public RotatableBlock RotatableBlock { get; internal set; }
    }
}
