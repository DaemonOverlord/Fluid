using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class QuickChatHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "autotext" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            int userId = message.GetInt(0);
            string text = message.GetString(1);

            WorldConnection worldCon = (WorldConnection)connectionBase;
            WorldPlayer player = worldCon.Players.GetPlayer(userId);

            ChatMessage chatMessage = new ChatMessage(player, text);
            QuickChatEvent quickChatEvent = new QuickChatEvent()
            {
                Raw = message,
                ChatMessage = chatMessage
            };

            connectionBase.RaiseServerEvent<QuickChatEvent>(quickChatEvent);
        }
    }
}
