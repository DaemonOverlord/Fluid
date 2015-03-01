using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The event for when the connection gains edit access
    /// </summary>
    public class AccessEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }
    }
}
