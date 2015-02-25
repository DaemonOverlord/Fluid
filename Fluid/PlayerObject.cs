namespace Fluid
{
    public class PlayerObject
    {
        /// <summary>
        /// The user's name
        /// </summary>
        public string Username { get; internal set; }

        /// <summary>
        /// The user's last smiley choosen
        /// </summary>
        public FaceID Smiley { get; internal set; }

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
        /// Gets whether the player's chat is banned
        /// </summary>
        public bool ChatBanned { get; internal set; }

        /// <summary>
        /// Gets whether the player can chat
        /// </summary>
        public bool CanChat { get; internal set; }

        /// <summary>
        /// Gets whether the player has the smiley package
        /// </summary>
        public bool HasSmileyPackage { get; internal set; }

        /// <summary>
        /// Gets if the player is a guardian
        /// </summary>
        public bool IsGuardian { get; internal set; }

        /// <summary>
        /// Gets the player's home world
        /// </summary>
        public WorldReference HomeWorld { get; internal set; }

        /// <summary>
        /// Gets if the player's profile is visible
        /// </summary>
        public bool ProfileVisible { get; internal set; }

        /// <summary>
        /// Gets if the player is banned
        /// </summary>
        public bool Banned { get; internal set; }

        /// <summary>
        /// Gets if the player has accepted the terms of service
        /// </summary>
        public bool HasAcceptedTerms { get; internal set; }

        /// <summary>
        /// Gets WootUp
        /// </summary>
        public int WootUp { get; internal set; }

        /// <summary>
        /// Gets the player's maximum energy
        /// </summary>
        public int MaxEnergy { get; internal set; }
    }
}
