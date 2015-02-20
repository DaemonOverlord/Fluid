using System.Diagnostics;

namespace Fluid
{
    public class FluidLogMessage
    {
        /// <summary>
        /// Gets the log message's text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or Sets the Fluid log category
        /// </summary>
        public FluidLogCategory Category { get; set; }

        /// <summary>
        /// Gets the log message's stack trace
        /// </summary>
        public StackTrace Trace { get; set; }

        /// <summary>
        /// Converts the message to a string
        /// </summary>
        public override string ToString()
        {
            return string.Format("[{0}, {1}]", Category, Text ?? "null");
        }

        /// <summary>
        /// Creates new a new Fluid log message
        /// </summary>
        /// <param name="text">The message text</param>
        /// <param name="trace">The stack trace</param>
        /// <param name="category">The category</param>
        public FluidLogMessage(string text, StackTrace trace, FluidLogCategory category)
        {
            Text = text;
            Trace = trace;
            Category = category;
        }
    }
}
