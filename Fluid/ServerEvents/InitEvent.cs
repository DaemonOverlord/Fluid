using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class InitEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the loaded world
        /// </summary>
        public World World { get; internal set; }

        /// <summary>
        /// Gets the connected player
        /// </summary>
        public WorldPlayer ConnectedPlayer { get; internal set; }
    }
}
