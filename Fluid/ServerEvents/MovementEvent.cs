using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class MovementEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player
        /// </summary>
        public WorldPlayer Player { get; internal set; }

        /// <summary>
        /// Gets the input of the player
        /// </summary>
        public Input Input { get; internal set; }
    }
}
