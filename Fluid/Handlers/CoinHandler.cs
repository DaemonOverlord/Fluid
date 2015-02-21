using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class CoinHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "c" }; }
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

            if (player != null)
            {
                player.GoldCoins = message.GetInt(1);
                player.BlueCoins = message.GetInt(2);
            }

            CoinEvent coinEvent = new CoinEvent()
            {
                Raw = message,
                Player = player
            };

            connectionBase.RaiseServerEvent<CoinEvent>(coinEvent);
        }
    }
}
