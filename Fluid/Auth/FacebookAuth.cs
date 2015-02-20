using PlayerIOClient;

namespace Fluid.Auth
{
    public class FacebookAuth : IAuth
    {
        /// <summary>
        /// Gets or Sets the facebook access token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Log's in through facebook
        /// </summary>
        /// <param name="config">The config</param>
        public Client LogIn(Config config)
        {
            return PlayerIO.QuickConnect.FacebookOAuthConnect(config.GameID, Token, null, null);
        }

        /// <summary>
        /// Creates a new facebook authentication
        /// </summary>
        /// <param name="token">The access token</param>
        public FacebookAuth(string token)
        {
            Token = token;
        }
    }
}
