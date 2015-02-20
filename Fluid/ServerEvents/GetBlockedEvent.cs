using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.ServerEvents
{
    public class GetBlockedEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the list of blocked friends
        /// </summary>
        public List<Friend> Blocked { get; internal set; }
    }
}
