using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class ArmorgamesAuthHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "auth" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            string userId = message.GetString(0);
            string auth = message.GetString(1);

            AuthEvent authEvent = new AuthEvent(userId, auth);
            connectionBase.RaiseServerEvent<AuthEvent>(authEvent);
        }
    }
}
