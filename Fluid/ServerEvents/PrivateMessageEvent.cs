using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a private message is received
    /// </summary>
    public class PrivateMessageEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the chat message
        /// </summary>
        public ChatMessage ChatMessage { get; set; }
    }
}
