using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class SayHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "say" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            int userId = message.GetInt(0);
            string text = message.GetString(1);

            WorldConnection worldCon = (WorldConnection)connectionBase;
            WorldPlayer player = worldCon.Players.GetPlayer(userId);
            ChatMessage chatMessage = new ChatMessage(player, text);

            if (!handled)
            {
                worldCon.Chat.Add(chatMessage);
            }

            SayEvent sayEvent = new SayEvent()
            {
                Raw = message,
                ChatMessage = chatMessage
            };

            connectionBase.RaiseServerEvent<SayEvent>(sayEvent);
        }
    }
}
