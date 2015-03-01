using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a player is killed
    /// </summary>
    public class KillEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who was killed
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
