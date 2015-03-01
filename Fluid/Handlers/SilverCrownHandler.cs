using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class SilverCrownHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "ks" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            int userId = message.GetInt(0);

            WorldConnection worldCon = (WorldConnection)connectionBase;
            WorldPlayer player = worldCon.Players.Get(userId);

            if (!handled && player != null)
            {
                player.HasSilverCrown = true;
            }

            SilverCrownEvent silverCrownEvent = new SilverCrownEvent()
            {
                Raw = message,
                Player = player
            };

            connectionBase.RaiseServerEvent<SilverCrownEvent>(silverCrownEvent);
        }
    }
}
