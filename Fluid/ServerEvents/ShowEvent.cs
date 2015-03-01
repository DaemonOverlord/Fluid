using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when the world's keys are shown
    /// </summary>
    public class ShowEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the key triggers
        /// </summary>
        public List<KeyTrigger> Triggers { get; internal set; }
    }
}
