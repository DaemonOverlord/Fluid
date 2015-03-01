using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when the level is reloaded
    /// </summary>
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
