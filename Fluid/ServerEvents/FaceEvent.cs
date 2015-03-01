using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a player's face changes
    /// </summary>
    public class FaceEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who changed their face
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
