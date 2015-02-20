using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class OldSayEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the old chat message
        /// </summary>
        public ChatMessage ChatMessage { get; internal set; }
    }
}
