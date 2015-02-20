using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class LeftEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who left
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
