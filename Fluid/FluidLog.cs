using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Fluid
{
    public class FluidLog
    {
        private TextWriter m_TextWriter;
        private List<FluidLogMessage> m_Log;

        /// <summary>
        /// Gets the log count
        /// </summary>
        public int Count
        {
            get
            {
                return m_Log.Count;
            }
        }

        /// <summary>
        /// Gets the last log message; null if no messages have been logged
        /// </summary>
        public FluidLogMessage Last
        {
            get
            {
                if (m_Log.Count == 0)
                {
                    return null;
                }

                return m_Log[m_Log.Count - 1];
            }
        }

        /// <summary>
        /// Gets or sets the log output
        /// </summary>
        public TextWriter Output
        {
            get
            {
                return m_TextWriter;
            }
            set
            {
                this.m_TextWriter = value;
            }
        }

        /// <summary>
        /// Get's all log messages
        /// </summary>
        public List<FluidLogMessage> GetAllMessages()
        {
            List<FluidLogMessage> allMessages = new List<FluidLogMessage>(m_Log.Count);
            foreach (FluidLogMessage logEntry in m_Log)
            {
                allMessages.Add(logEntry);
            }

            return allMessages;
        }

        /// <summary>
        /// Get's all messages from a category
        /// </summary>
        /// <param name="category">The category</param>
        public List<FluidLogMessage> GetAllMessages(FluidLogCategory category)
        {
            List<FluidLogMessage> categoryMessages = new List<FluidLogMessage>();
            foreach (FluidLogMessage logEntry in m_Log)
            {
                if (logEntry.Category == category)
                {
                    categoryMessages.Add(logEntry);
                }
            }

            return categoryMessages;
        }

        /// <summary>
        /// Log's a message
        /// </summary>
        /// <param name="category">The message category</param>
        /// <param name="text">The text</param>
        internal void Add(FluidLogCategory category, string text)
        {
            StackTrace currentTrace = new StackTrace(1);
            m_Log.Add(new FluidLogMessage(text, currentTrace, category));

            if (m_TextWriter != null)
            {
                string cat = Enum.GetName(typeof(FluidLogCategory), category);
                m_TextWriter.WriteLine(string.Format("[Log] ({0}): {1}", cat, text));
            }
        }

        /// <summary>
        /// Clears the log
        /// </summary>
        public void Clear()
        {
            m_Log.Clear();
        }

        /// <summary>
        /// Shows hte count of messages
        /// </summary>
        public override string ToString()
        {
            return string.Format("Messages: {0}", Count);
        }

        /// <summary>
        /// Creates a new Fluid log
        /// </summary>
        public FluidLog()
        {
            m_Log = new List<FluidLogMessage>();
        }

        /// <summary>
        /// Creates a new Fluid log
        /// </summary>
        /// <param name="output">The output writer</param>
        public FluidLog(TextWriter output) : this()
        {
            this.m_TextWriter = output;
        }
    }
}
