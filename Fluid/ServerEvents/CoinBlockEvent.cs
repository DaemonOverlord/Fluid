using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class CoinBlockEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the coin block
        /// </summary>
        public CoinBlock CoinBlock { get; internal set; }
    }
}
