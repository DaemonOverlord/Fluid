using Fluid.ServerEvents;
using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.Handlers
{
    public class GetInvitesHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "getInvitesToMe" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            List<Friend> invites = new List<Friend>();
            for (uint i = 0; i < message.Count; i += 3)
            {
                Friend friendInvite = new Friend((LobbyConnection)connectionBase, message.GetString(i));
                invites.Add(friendInvite);
            }

            GetInvitesEvent getInvitesEvent = new GetInvitesEvent()
            {
                Invites = invites,
                Raw = message
            };

            connectionBase.RaiseServerEvent<GetInvitesEvent>(getInvitesEvent);
        }
    }
}
