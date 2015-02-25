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
        public string Text { get; private set; }

        /// <summary>
        /// Gets the chat message debug message
        /// </summary>
        public override string ToString()
        {
            if (Player == null)
            {
                return Text;
            }

            return string.Format("{0}: {1}", Player.Username, Text);
        }

        /// <summary>
        /// Creates a new chat message
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="text">The text</param>
        public ChatMessage(Player player, string text)
        {
            Player = player;
            Text = text;
        }
    }
}
