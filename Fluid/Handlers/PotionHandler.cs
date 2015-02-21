using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class PotionHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "p" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection baes</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            int userId = message.GetInt(0);
            int potionId = message.GetInt(1);
            bool isActive = message.GetBoolean(2);

            Potion potion = (Potion)potionId;

            WorldConnection worldCon = (WorldConnection)connectionBase;
            WorldPlayer player = worldCon.Players.GetPlayer(userId);

            if (!handled && player != null)
            {
                player.SetPotion(potion, isActive ? PotionState.Active : PotionState.Inactive);
            }

            PotionEvent potionEvent = new PotionEvent()
            {
                Raw = message,
                Player = player,
                Potion = potion,
                IsActive = isActive
            };

            connectionBase.RaiseServerEvent<PotionEvent>(potionEvent);
        }
    }
}
