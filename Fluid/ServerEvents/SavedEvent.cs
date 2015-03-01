using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when the world is saved
    /// </summary>
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
