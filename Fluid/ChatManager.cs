﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fluid
{
    [DebuggerDisplay("Count = {Count}")]
    public class ChatManager
    {
        private char[] m_Ext;
        private Random m_Random;
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
                lock (m_ChatHistory)
                {
                    if (m_ChatHistory.Count > 0)
                    {
                        return m_ChatHistory[m_ChatHistory.Count - 1];
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the amount of times this message has already been chatted
        /// </summary>
        /// <param name="message">The message to count</param>
        /// <returns>The amount of recurrances</returns>
        private int GetCount(string message)
        {
            int count = 0;
            lock (m_ChatHistory)
            {
                for (int i = 0; i < m_ChatHistory.Count; i++)
                {
                    if (!m_ChatHistory[i].IsOld)
                    {
                        if (string.Compare(m_ChatHistory[i].Text, message, false) == 0)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
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
        /// <param name="offset">Offset length used for adding unique characters</param>
        /// <returns>The message segments</returns>
        internal List<string> SplitMessage(string message, int length, int offset)
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

            string overlapped = null;
            for (int j = 0; j < messages.Count; j++)
            {
                string current = (overlapped == null) ? messages[j].Trim() : messages[j].Insert(0, overlapped).Trim();

                int overlap = current.Length - (length - offset);
                if (overlap <= 0)
                {
                    overlapped = null;
                    messages[j] = current;
                    continue;
                }

                while (overlap > 0)
                {
                    string[] words = current.Split(' ');
                    if (words.Length != 0)
                    {
                        int index = current.LastIndexOf(words[words.Length - 1]);

                        overlapped = current.Substring(index);
                        current = current.Substring(0, index);
                    }
                    else
                    {
                        //Remove exact amount of characters
                        overlapped = current.Substring(current.Length - overlap);
                        current = current.Substring(0, current.Length - overlap);
                    }

                    overlap = current.Length - (length - offset);
                }

                messages[j] = current.Trim();
            }

            if (overlapped != null)
            {
                messages.Add(overlapped);
            }

            for (int i = 0; i < messages.Count; i++)
            {
                int c = 0;
                while (GetCount(messages[i]) >= 4)
                {
                    char rnd = m_Ext[m_Random.Next(m_Ext.Length)];
                    messages[i] += rnd;
                    c++;
                }
            }


            return messages;
        }

        /// <summary>
        /// Says a message
        /// </summary>
        /// <param name="message">The message to say</param>
        public void Say(string message, string prefix = "")
        {
            List<string> messageSegments = SplitMessage(message, 80 - prefix.Length, 5);
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
        /// Creates a new chat manager
        /// </summary>
        public ChatManager(WorldConnection m_Connection)
        {
            this.m_Connection = m_Connection;
            m_ChatHistory = new List<ChatMessage>();
            m_Random = new Random();
            m_Ext = new char[] { '·', '.', ',', '¸' };
        }
    }
}
