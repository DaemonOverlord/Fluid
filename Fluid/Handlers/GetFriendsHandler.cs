using Fluid.ServerEvents;
using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.Handlers
{
    public class GetFriendsHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handle types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "getFriends" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base </param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            List<Friend> friends = new List<Friend>();

            uint index = 0;
            while (index < message.Count)
            {
                Friend friend = new Friend((LobbyConnection)connectionBase, message.GetString(index));

                friend.SetOnlineStatus(message.GetBoolean(index + 1), (FaceID)message.GetInt(index + 4), message.GetString(index + 2), message.GetString(index + 3));
                friends.Add(friend);
                index += 5;
            }

            GetFriendsEvent getFriendsEvent = new GetFriendsEvent()
            {
                Friends = friends,
                Raw = message
            };

            connectionBase.RaiseServerEvent<GetFriendsEvent>(getFriendsEvent);
        }
    }
}
