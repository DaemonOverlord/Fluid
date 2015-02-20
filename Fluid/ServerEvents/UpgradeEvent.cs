using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class UpgradeEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }
    }
}
