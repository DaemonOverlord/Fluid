using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.ServerEvents
{
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
