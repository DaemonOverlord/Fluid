using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class DeathBlockEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the death block
        /// </summary>
        public DeathBlock DeathBlock { get; internal set; }
    }
}
