using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a level is cleared
    /// </summary>
    public class ClearEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the world
        /// </summary>
        public World World { get; internal set; }

        /// <summary>
        /// Gets the border block id
        /// </summary>
        public BlockID BorderBlockID { get; internal set; }

        /// <summary>
        /// Gets the workarea block id
        /// </summary>
        public BlockID WorkareaBlockID { get; internal set; }
    }
}
