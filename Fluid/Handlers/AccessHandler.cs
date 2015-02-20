using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class AccessHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "access" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            WorldConnection worldCon = (WorldConnection)connectionBase;
            worldCon.HasAccess = true;

            AccessEvent accessEvent = new AccessEvent()
            {
                Raw = message
            };

            connectionBase.RaiseServerEvent<AccessEvent>(accessEvent);
        } 
    }
}
