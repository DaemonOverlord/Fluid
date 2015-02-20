using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class PortalBlockEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the portal
        /// </summary>
        public Portal Portal { get; internal set; }
    }
}
