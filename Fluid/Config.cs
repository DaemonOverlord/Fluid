namespace Fluid
{
    public class Config
    {
        /// <summary>
        /// Gets or Sets the PlayerIO Id attached to the game
        /// </summary>
        public string GameID { get; set; }

        /// <summary>
        /// Gets or Sets the mousebreaker game url
        /// </summary>
        public string MouseBreakerGameUrl { get; set; }

        /// <summary>
        /// Gets or Sets the normal room format
        /// </summary>
        public string NormalRoom { get; set; }

        /// <summary>
        /// Gets or Sets the beta room format
        /// </summary>
        public string BetaRoom { get; set; }

        /// <summary>
        /// Gets or Sets the lobby room format
        /// </summary>
        public string LobbyRoom { get; set; }

        /// <summary>
        /// Gets or Sets the auth room format
        /// </summary>
        public string AuthRoom { get; set; }

        /// <summary>
        /// Gets or Sets the lobby guest room format
        /// </summary>
        public string LobbyGuestRoom { get; set; }

        /// <summary>
        /// Gets or Sets whether if a player could not be found in the database, to look up the player and add the profile
        /// </summary>
        public bool AddProfilesToDatabase { get; set; }

        /// <summary>
        /// Creates the default config
        /// </summary>
        public Config()
        {
            GameID = "everybody-edits-su9rn58o40itdbnw69plyw";
            MouseBreakerGameUrl = "http://www.mousebreaker.com/games/everybodyedits/playgame";
            NormalRoom = "Everybodyedits{0}";
            BetaRoom = "Beta{0}";
            LobbyRoom = "Lobby{0}";
            LobbyGuestRoom = "LobbyGuest{0}";
            AuthRoom = "Auth{0}";
            AddProfilesToDatabase = false;
        }
    }
}
