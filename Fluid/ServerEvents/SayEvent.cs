using PlayerIOClient;

namespace Fluid.ServerEvents
{
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
