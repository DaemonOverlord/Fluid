using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when the world's plays, woots, title, etc. is updated
    /// </summary>
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
