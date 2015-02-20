using PlayerIOClient;

namespace Fluid.ServerEvents
{
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
