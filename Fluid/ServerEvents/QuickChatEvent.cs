using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a player sends a quick chat message
    /// </summary>
    public class QuickChatEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the quick chat message
        /// </summary>
        public ChatMessage ChatMessage { get; internal set; }

        /// <summary>
        /// Gets the quick chat message
        /// </summary>
        public QuickChatMessage QuickChatMessage { get; internal set; }
    }
}
