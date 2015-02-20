using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.ServerEvents
{
    public class HideEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the list of key triggers
        /// </summary>
        public List<KeyTrigger> Triggers { get; internal set; }
    }
}
