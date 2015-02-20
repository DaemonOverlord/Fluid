namespace Fluid.Auth
{
    internal class KongregatePlayerObject
    {
        /// <summary>
        /// Gets the user identification
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Gets the game authentication token
        /// </summary>
        public string GameAuthToken { get; set; }

        /// <summary>
        /// Creates a new kongregate player object
        /// </summary>
        public KongregatePlayerObject(string userId, string gameAuthToken)
	    {
            UserID = userId;
            GameAuthToken = gameAuthToken;
	    }
    }
}
