using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class CrownEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who touched the crown
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
