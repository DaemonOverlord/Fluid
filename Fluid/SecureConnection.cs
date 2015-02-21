using Fluid.Handlers;
using PlayerIOClient;

namespace Fluid
{
    public class SecureConnection : ConnectionBase
    {
        /// <summary>
        /// Sends a authentication request
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="authToken">The authentication token</param>
        public void SendAuth(string userId, string authToken)
        {
            base.SendMessage("auth", userId, authToken);
        }

        /// <summary>
        /// Creates a new Fluid lobby connection
        /// </summary>
        /// <param name="client">The Fluid client</param>
        /// <param name="connection">The playerio connection</param>
        public SecureConnection(FluidClient client, Connection connection)
             : base(client, connection)
        {
            base.AddMessageHandler(new ArmorgamesAuthHandler());
        }
    }
}
