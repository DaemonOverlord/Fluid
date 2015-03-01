using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a player sends a chat message
    /// </summary>
    public class SayEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Get the chat message
        /// </summary>
        public ChatMessage ChatMessage { get; internal set; }
    }
}
