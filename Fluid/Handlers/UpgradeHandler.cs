using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class UpgradeHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "upgrade" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            UpgradeEvent upgradeEvent = new UpgradeEvent()
            {
                Raw = message
            };

            connectionBase.RaiseServerEvent<UpgradeEvent>(upgradeEvent);
        }
    }
}
