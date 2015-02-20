using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
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
