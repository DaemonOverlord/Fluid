using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class InfoHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "info", "write" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            string title = message.GetString(0);
            string text = message.GetString(1);
            ChatMessage systemMessage = new ChatMessage(null, string.Format("{0} {1}", title, text));

            if (connectionBase is LobbyConnection)
            {
                LobbyConnection lobbyCon = (LobbyConnection)connectionBase;

                if (!handled)
                {
                    lobbyCon.MessageOfTheDay = message.GetString(1);
                }
            }
            else if (connectionBase is WorldConnection)
            {
                if (!handled)
                {
                    WorldConnection worldCon = (WorldConnection)connectionBase;
                    worldCon.Chat.Add(systemMessage);
                }
            }

            InfoEvent infoEvent = new InfoEvent()
            {
                Raw = message,
                SystemMessage = systemMessage
            };

            connectionBase.RaiseServerEvent<InfoEvent>(infoEvent);
        }
    }
}
