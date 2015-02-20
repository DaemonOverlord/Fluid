using Fluid.ServerEvents;
using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.Handlers
{
    public class GetPendingHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handle types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "getPending" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            List<Friend> pending = new List<Friend>();
            for (uint i = 0; i < message.Count; i++)
            {
                Friend pendingFriend = new Friend((LobbyConnection)connectionBase, message.GetString(i));
                pending.Add(pendingFriend);
            }

            GetPendingEvent getPendingEvent = new GetPendingEvent()
            {
                Pending = pending,
                Raw = message
            };

            connectionBase.RaiseServerEvent<GetPendingEvent>(getPendingEvent);
        }
    }
}
