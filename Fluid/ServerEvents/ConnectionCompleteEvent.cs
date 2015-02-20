using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class ConnectionCompleteEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }
    }
}
