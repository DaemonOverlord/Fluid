using PlayerIOClient;

namespace Fluid.Auth
{
    public class SimpleAuth : IAuth
    {
        /// <summary>
        /// The email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Log's In to everybody edits
        /// </summary>
        /// <param name="config">The game configuration</param>
        public Client LogIn(Config config)
        {
            return PlayerIO.QuickConnect.SimpleConnect(config.GameID, Email, Password, null);
        }

        /// <summary>
        /// Simple authentication
        /// </summary>
        /// <param name="email">The email</param>
        /// <param name="password">The password</param>
        public SimpleAuth(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
