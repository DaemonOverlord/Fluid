using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class CrownHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "k" }; }
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
            WorldPlayer player = worldCon.Players.GetPlayer(userId);

            if (!handled && player != null)
            {
                if (worldCon.CrownHolder != null)
                {
                    worldCon.CrownHolder.HasCrown = false;
                }

                player.HasCrown = true;
                worldCon.CrownHolder = player;
            }

            CrownEvent crownEvent = new CrownEvent()
            {
                Raw = message,
                Player = player
            };

            connectionBase.RaiseServerEvent<CrownEvent>(crownEvent);
        }
    }
}
