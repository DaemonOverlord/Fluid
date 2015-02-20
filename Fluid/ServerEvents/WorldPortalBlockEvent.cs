using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class WorldPortalBlockEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the world portal
        /// </summary>
        public WorldPortal WorldPortal { get; internal set; }
    }
}
