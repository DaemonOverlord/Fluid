namespace Fluid.Auth
{
    internal class KongregateSession
    {
        /// <summary>
        /// Gets the authenticity token
        /// </summary>
        public string AuthenticityToken { get; private set; }

        /// <summary>
        /// Gets or Sets the kong svid
        /// </summary>
        public string KongSvid { get; set; }

        /// <summary>
        /// Gets or Sets the session token
        /// </summary>
        public string SessionToken { get; set; }

        /// <summary>
        /// Gets or Sets the WWW Password
        /// </summary>
        public string WWWPass { get; set; }

        /// <summary>
        /// Checks if the session is authenticated
        /// </summary>
        public bool IsAuthenticated()
        {
            return WWWPass != null;
        }

        /// <summary>
        /// Creates a new kongregate session
        /// </summary>
        /// <param name="authenticityToken">The authenticity token</param>
        /// <param name="kongSvid">The kongregate svid</param>
        /// <param name="sessionToken">The session token</param>
        public KongregateSession(string authenticityToken, string kongSvid, string sessionToken)
        {
            AuthenticityToken = authenticityToken;
            KongSvid = kongSvid;
            SessionToken = sessionToken;
        }
    }
}
