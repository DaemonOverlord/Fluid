namespace Fluid.Auth
{
    internal class ArmorgamesFlashvars
    {
        /// <summary>
        /// The auth token
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// The user id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Creates a new armorgames flashvars
        /// </summary>
        /// <param name="authToken">The auth token</param>
        /// <param name="userId">The user id</param>
        public ArmorgamesFlashvars(string authToken, string userId)
        {
            AuthToken = authToken;
            UserId = userId;
        }
    }
}
