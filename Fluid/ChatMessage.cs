using System.Diagnostics;

namespace Fluid
{
    [DebuggerDisplay("{Player.Username}: {Text}")]
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
