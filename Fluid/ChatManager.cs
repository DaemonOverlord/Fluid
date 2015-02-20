using System.Collections.Generic;

namespace Fluid
{
    public class ChatManager
    {
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
        /// Gets the chat debug message
        /// </summary>
        public override string ToString()
        {
            return string.Format("Count: {0}", Count);
        }

        /// <summary>
        /// Creates a new chat manager
        /// </summary>
        public ChatManager()
        {
            m_ChatHistory = new List<ChatMessage>();
        }
    }
}
