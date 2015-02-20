using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class KillEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who was killed
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
