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
        /// <param name="clientCallback">The client success callback</param>
        /// <param name="errorCallback">The playerio error callback</param>
        public void LogIn(Config config, Callback<Client> clientCallback, Callback<PlayerIOError> errorCallback)
        {
            PlayerIO.QuickConnect.SimpleConnect(config.GameID, Email, Password, null, clientCallback, errorCallback);
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
