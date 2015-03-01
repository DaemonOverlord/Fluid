using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a player completes the level, or gets the silver crown
    /// </summary>
    public class SilverCrownEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who touched the silver crown
        /// </summary>
        public Player Player { get; internal set; }
    }
}
