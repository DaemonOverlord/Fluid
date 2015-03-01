using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when potions enabled has changed
    /// </summary>
    public class AllowPotionEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the potions changed
        /// </summary>
        public List<Potion> Potions { get; internal set; }

        /// <summary>
        /// Gets whether the potions are enabled
        /// </summary>
        public bool AreEnabled { get; internal set; }
    }
}
