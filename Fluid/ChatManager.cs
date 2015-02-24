using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fluid
{
    public class ChatManager
    {
        private WorldConnection m_Connection;
        private List<ChatMessage> m_ChatHistory;

        /// <summary>
        /// Gets whether the chat is empty
        /// </summary>
        public bool IsEmpty { get { return m_ChatHistory.Count == 0; } }

        /// <summary>
        /// Gets the amount of chat messages in the chat
        /// </summary>
        public int Count { get { return m_ChatHistory.Count; } }

        /// <summary>
        /// Gets the last chat message
        /// </summary>
        public ChatMessage Last
        {
            get
            {
                if (m_ChatHistory.Count > 0)
                {
                    return m_ChatHistory[m_ChatHistory.Count - 1];
                }

                return null;
            }
        }

        /// <summary>
        /// Adds a chat message to the chat
        /// </summary>
        /// <param name="chatMessage"></param>
        public void Add(ChatMessage chatMessage)
        {
            m_ChatHistory.Add(chatMessage);
        }
        
        /// <summary>
        /// Splits a message up into segments by length
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="length">Segment length</param>
        /// <returns>The message segments</returns>
        internal List<string> SplitMessage(string message, int length)
        {
            List<string> messages = new List<string>();

            StringBuilder currentMessage = new StringBuilder();
            int currentMessageIndex = 0;

            char[] array = message.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (currentMessageIndex < length)
                {
                    currentMessage.Append(array[i]);
                    currentMessageIndex++;
                }
                else
                {
                    messages.Add(currentMessage.ToString());
                    currentMessage.Clear();
                    currentMessageIndex = 0;
                }
            }

            if (currentMessage.Length > 0)
            {
                messages.Add(currentMessage.ToString());
            }

            return messages;
        }

        /// <summary>
        /// Says a message
        /// </summary>
        /// <param name="message">The message to say</param>
        public void Say(string message, string prefix = "")
        {
            List<string> messageSegments = SplitMessage(message, 80 - prefix.Length);
            Task chatTask = Task.Run(delegate()
            {
                for (int i = 0; i < messageSegments.Count; i++)
                {
                    m_Connection.SayInternal(prefix + messageSegments[i]);
                    Thread.Sleep(700);
                }
            });

            Task.WaitAll(chatTask);
        }

        /// <summary>
        /// Gets the chat debug message
        /// </summary>
        public override string ToString()
        {
            return string.Format("Count: {0}", Count);
        }

        /// <summary>
        /// Creates a new chat manager
        /// </summary>
        public ChatManager(WorldConnection m_Connection)
        {
            this.m_Connection = m_Connection;
            m_ChatHistory = new List<ChatMessage>();
        }
    }
}
