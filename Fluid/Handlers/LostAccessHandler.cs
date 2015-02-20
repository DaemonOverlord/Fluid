using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class LostAccessHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "lostaccess" }; }
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
            worldCon.HasAccess = false;

            LostAccessEvent lostAccessEvent = new LostAccessEvent()
            {
                Raw = message
            };

            connectionBase.RaiseServerEvent<LostAccessEvent>(lostAccessEvent);
        } 
    }
}
