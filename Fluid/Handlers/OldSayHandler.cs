using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class OldSayHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "say_old" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            string username = message.GetString(0);
            string text = message.GetString(1);

            WorldConnection worldCon = (WorldConnection)connectionBase;
            Player player = worldCon.Players.GetPlayer(username);

            if (player == null)
            {
                player = worldCon.Client.GetPlayerByUsername(username);
            }

            ChatMessage chatMessage = new ChatMessage(player, text);
            if (!handled)
            {
                worldCon.Chat.Add(chatMessage);
            }

            OldSayEvent oldSayEvent = new OldSayEvent()
            {
                Raw = message,
                ChatMessage = chatMessage
            };

            connectionBase.RaiseServerEvent<OldSayEvent>(oldSayEvent);
        }
    }
}
