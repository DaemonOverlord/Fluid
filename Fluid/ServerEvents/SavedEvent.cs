using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class SavedEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the saved world
        /// </summary>
        public World World { get; internal set; }
    }
}
