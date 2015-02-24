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
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            int userId = message.GetInt(0);
            string text = message.GetString(1);

            QuickChatMessage quickChatMesssage = QuickChatMessage.Unknown;
            switch (text)
            {
                case "Hi.":
                    quickChatMesssage = QuickChatMessage.Hi;
                    break;
                case "Goodbye.":
                    quickChatMesssage = QuickChatMessage.Bye;
                    break;
                case "Help me!":
                    quickChatMesssage = QuickChatMessage.Help;
                    break;
                case "Thank you.":
                    quickChatMesssage = QuickChatMessage.Thanks;
                    break;
                case "Follow me.":
                    quickChatMesssage = QuickChatMessage.Come;
                    break;
                case "Stop!":
                    quickChatMesssage = QuickChatMessage.Stop;
                    break;
                case "Yes.":
                    quickChatMesssage = QuickChatMessage.Yes;
                    break;
                case "No.":
                    quickChatMesssage = QuickChatMessage.No;
                    break;
                case "Right.":
                    quickChatMesssage = QuickChatMessage.Right;
                    break;
                case "Left.":
                    quickChatMesssage = QuickChatMessage.Left;
                    break;
            }

            WorldConnection worldCon = (WorldConnection)connectionBase;
            WorldPlayer player = worldCon.Players.GetPlayer(userId);

            ChatMessage chatMessage = new ChatMessage(player, text);
            QuickChatEvent quickChatEvent = new QuickChatEvent()
            {
                Raw = message,
                ChatMessage = chatMessage,
                QuickChatMessage = quickChatMesssage
            };

            connectionBase.RaiseServerEvent<QuickChatEvent>(quickChatEvent);
        }
    }
}
