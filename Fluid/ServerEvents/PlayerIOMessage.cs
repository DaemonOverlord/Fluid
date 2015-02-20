using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class PlayerIOMessage
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; private set; }

        /// <summary>
        /// Gets or Sets whether the message has been handled
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Creates a new player io message
        /// </summary>
        /// <param name="raw">The raw message</param>
        public PlayerIOMessage(Message raw)
        {
            Raw = raw;
        }
    }
}
