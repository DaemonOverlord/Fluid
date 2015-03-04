
using System.Diagnostics;
namespace Fluid
{
    [DebuggerDisplay("Name = {Name}, OnlineUsers = {OnlineUsers}")]
    public class LobbyWorldReference : WorldReference
    {
        /// <summary>
        /// Gets the amount of online users
        /// </summary>
        public int OnlineUsers { get; internal set; }

        /// <summary>
        /// Gets whether the world is owned
        /// </summary>
        public string Owned { get; internal set; }

        /// <summary>
        /// Gets whether the world needs a key
        /// </summary>
        public string NeedsKey { get; internal set; }

        /// <summary>
        /// Gets the amount of plays the world has
        /// </summary>
        public string Plays { get; internal set; }

        /// <summary>
        /// Gets the rating of the world
        /// </summary>
        public string Rating { get; internal set; }

        /// <summary>
        /// Gets the name of the world
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the amount of woots the world has
        /// </summary>
        public string Woots { get; internal set; }

        /// <summary>
        /// Gets whether the world is featured
        /// </summary>
        public string IsFeatured { get; internal set; }

        /// <summary>
        /// Creates a new lobby world reference
        /// </summary>
        /// <param name="client">The client</param>
        /// <param name="worldID">The world id</param>
        /// <param name="onlineUsers">The amount of online users</param>
        public LobbyWorldReference(FluidClient client, string worldID, int onlineUsers) : base(client, worldID)
        {
            OnlineUsers = onlineUsers;
        }
    }
}
