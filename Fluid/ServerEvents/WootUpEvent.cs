using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a player gives a woot
    /// </summary>
    public class WootUpEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
