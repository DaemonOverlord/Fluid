namespace Fluid
{
    public class ChatMessage
    {
        /// <summary>
        /// Gets the player who chatted
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// Gets the chat message
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the chat message debug message
        /// </summary>
        public override string ToString()
        {
            if (Player == null)
            {
                return Message;
            }

            return string.Format("{0}: {1}", Player.Username, Message);
        }

        /// <summary>
        /// Creates a new chat message
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="message">The message</param>
        public ChatMessage(Player player, string message)
        {
            Player = player;
            Message = message;
        }
    }
}
