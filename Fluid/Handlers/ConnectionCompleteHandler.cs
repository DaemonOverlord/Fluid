using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class ConnectionCompleteHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the list of handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "connectioncomplete" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            ConnectionCompleteEvent connectionComplete = new ConnectionCompleteEvent();
            connectionComplete.Raw = message;
            connectionBase.RaiseServerEvent<ConnectionCompleteEvent>(connectionComplete);
        }
    }
}
