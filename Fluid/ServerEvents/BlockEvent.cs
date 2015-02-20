using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class BlockEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the changed block
        /// </summary>
        public Block Block { get; internal set; }
    }
}
