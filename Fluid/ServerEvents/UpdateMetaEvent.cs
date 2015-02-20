using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class UpdateMetaEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the updated world
        /// </summary>
        public World World { get; internal set; }
    }
}
