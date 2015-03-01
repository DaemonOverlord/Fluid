using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server callback event for when a player object is received
    /// </summary>
    public class PlayerObjectEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player object
        /// </summary>
        public PlayerObject PlayerObject { get; internal set; }
    }
}
