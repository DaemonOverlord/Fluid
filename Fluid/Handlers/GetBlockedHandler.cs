using Fluid.ServerEvents;
using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.Handlers
{
    public class GetBlockedHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handle types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "getBlockedUsers" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            List<Friend> blocked = new List<Friend>();
            for (uint i = 0; i < message.Count; i += 2)
            {
                Friend pendingFriend = new Friend((LobbyConnection)connectionBase, message.GetString(i));
                blocked.Add(pendingFriend);
            }

            GetBlockedEvent getBlockedEvent = new GetBlockedEvent()
            {
                Blocked = blocked,
                Raw = message
            };

            connectionBase.RaiseServerEvent<GetBlockedEvent>(getBlockedEvent);
        }
    }
}
