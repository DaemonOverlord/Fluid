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
        /// Gets whether the chat message is old
        /// </summary>
        public bool IsOld { get; private set; }

        /// <summary>
        /// Creates a new chat message
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="text">The text</param>
        /// <param name="isOld">Is the chatmessage old</param>
        public ChatMessage(Player player, string text, bool isOld)
        {
            Player = player;
            Text = text;
            IsOld = isOld;
        }
    }
}
