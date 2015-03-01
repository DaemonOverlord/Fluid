using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a player is added
    /// </summary>
    public class AddEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who joined
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
