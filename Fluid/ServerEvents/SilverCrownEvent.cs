using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class SilverCrownEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who touched the silver crown
        /// </summary>
        public Player Player { get; internal set; }
    }
}
