using Fluid.Room;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a coin block is placed
    /// </summary>
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
