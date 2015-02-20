using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class LoadLevelEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the world
        /// </summary>
        public World World { get; internal set; }
    }
}
