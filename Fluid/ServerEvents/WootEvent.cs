using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class WootEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
