using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a player receives information from the server
    /// </summary>
    public class InfoEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the system message
        /// </summary>
        public ChatMessage SystemMessage { get; internal set; }
    }
}
