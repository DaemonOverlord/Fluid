using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when the player gets the gold crown
    /// </summary>
    public class CrownEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who touched the crown
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
