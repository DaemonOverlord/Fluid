using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class AuthEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// The user id
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// The auth
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// Checks if the message is authenticated
        /// </summary>
        public bool IsAuthenitcated()
        {
            return UserID.Length != 0 || UserID.Length != 0;
        }

        /// <summary>
        /// Creates a new armor games auth message
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="auth">The auth</param>
        public AuthEvent(string userId, string authToken)
        {
            UserID = userId;
            AuthToken = authToken;
        }
    }
}
