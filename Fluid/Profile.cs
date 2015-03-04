using System.Diagnostics;
namespace Fluid
{
    [DebuggerDisplay("Username = {Username}")]
    public class Profile
    {
        /// <summary>
        /// Gets the connection id
        /// </summary>
        public string ConnectionId { get; internal set; }

        /// <summary>
        /// The user's name
        /// </summary>
        public string Username { get; internal set; }

        /// <summary>
        /// The user's last smiley choosen
        /// </summary>
        public FaceID Smiley { get; internal set; }

        /// <summary>
        /// Gets if the player is beta
        /// </summary>
        public bool IsBeta { get; internal set; }

        /// <summary>
        /// Gets if the player is a moderator
        /// </summary>
        public bool IsModerator { get; internal set; }

        /// <summary>
        /// Gets if the player is in builders club
        /// </summary>
        public bool IsInBuildersClub { get; internal set; }

        /// <summary>
        /// Gets the amount of time left the player is subscribed to builders club
        /// </summary>
        public double BuildersClubMembershipTimeRemaining { get; internal set; }

        /// <summary>
        /// Gets the amount of time the player has been subscribed to builders club
        /// </summary>
        public double BuildersClubMembershipTime { get; internal set; }

        /// <summary>
        /// Gets the player's builders club id
        /// </summary>
        public string BuildersClubMembershipID { get; internal set; }

        /// <summary>
        /// Gets the builder's club welcome
        /// </summary>
        public bool BuildersClubWelcome { get; internal set; }

        /// <summary>
        /// Gets room 0
        /// </summary>
        public WorldReference Room0 { get; internal set; }

        /// <summary>
        /// Gets the player's beta only world
        /// </summary>
        public WorldReference BetaOnlyWorld { get; internal set; }

        /// <summary>
        /// Gets the list of worlds the player owns
        /// </summary>
        public WorldReference[] OwnedWorlds { get; internal set; }

        /// <summary>
        /// Gets if the player's profile is visible
        /// </summary>
        public bool ProfileVisible { get; internal set; }
    }
}
