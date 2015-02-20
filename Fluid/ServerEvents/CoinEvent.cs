using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class CoinEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who hit a coin
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
