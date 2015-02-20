using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class LevelupEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who leveled up
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
