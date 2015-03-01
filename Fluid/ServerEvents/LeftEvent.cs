using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a player leaves the world
    /// </summary>
    public class LeftEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who left
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
