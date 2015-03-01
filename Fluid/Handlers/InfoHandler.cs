using Fluid.ServerEvents;
using PlayerIOClient;
using System.Text;

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

        public bool IsPrivateMessage(string title, out string username)
        {
            char[] arr = title.ToCharArray();
            if (arr[0] != '*')
            {
                username = null;
                return false;
            }

            StringBuilder usernameBuilder = new StringBuilder();
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] == ' ')
                {
                    continue;
                }
                else if (arr[i] == '>')
                {
                    break;
                }

                usernameBuilder.Append(arr[i]);
            }

            username = usernameBuilder.ToString();
            return true;
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            string title = message.GetString(0);
            string text = message.GetString(1);

            string pmUsername = null;
            if (IsPrivateMessage(title, out pmUsername) && connectionBase is WorldConnection)
            {
                WorldConnection worldCon = (WorldConnection)connectionBase;
                WorldPlayer player = worldCon.Players.Get(pmUsername, true);
                ChatMessage pm = new ChatMessage(player, text.TrimEnd(' '));
                PrivateMessageEvent pmEvent = new PrivateMessageEvent()
                {
                    Raw = message,
                    ChatMessage = pm
                };

                connectionBase.RaiseServerEvent<PrivateMessageEvent>(pmEvent);
                return;
            }

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
