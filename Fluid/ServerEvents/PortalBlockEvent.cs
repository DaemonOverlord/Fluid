using Fluid.Room;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a portal block is placed
    /// </summary>
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
