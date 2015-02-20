using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class PlayerObjectEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player object
        /// </summary>
        public PlayerObject PlayerObject { get; internal set; }
    }
}
