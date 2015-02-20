using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.ServerEvents
{
    public class GetPendingEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the list of pending friends
        /// </summary>
        public List<Friend> Pending { get; internal set; }
    }
}
