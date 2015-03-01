using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a player inputs movement
    /// </summary>
    public class MovementEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player
        /// </summary>
        public WorldPlayer Player { get; internal set; }

        /// <summary>
        /// Gets the input of the player
        /// </summary>
        public Input Input { get; internal set; }
    }
}
