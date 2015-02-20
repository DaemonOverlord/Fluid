using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class PotionEvent : IServerEvent
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
        /// Gets the potion
        /// </summary>
        public Potion Potion { get; internal set; }

        /// <summary>
        /// Gets whether the potion is active
        /// </summary>
        public bool IsActive { get; internal set; }
    }
}
