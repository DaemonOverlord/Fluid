using Fluid.Physics;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a player is teleported
    /// </summary>
    public class TeleportEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the teleported player
        /// </summary>
        public WorldPlayer Player { get; internal set; }

        /// <summary>
        /// Gets the location
        /// </summary>
        public Vector Location { get; internal set; }
    }
}
