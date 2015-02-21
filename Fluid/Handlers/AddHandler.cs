using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class AddHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "add" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            WorldConnection worldCon = (WorldConnection)connectionBase;

            WorldPlayer player = new WorldPlayer(worldCon, message.GetString(1), message.GetInt(0));

            player.Face = (FaceID)message.GetInt(2);
            player.X = message.GetInt(3);
            player.Y = message.GetInt(4);
            player.InGodMode = message.GetBoolean(5);
            player.IsModerator = message.GetBoolean(6);
            player.HasChat = message.GetBoolean(7);
            player.GoldCoins = message.GetInt(8);
            player.BlueCoins = message.GetInt(9);

            //Message 10 will eventually be removed or changed
            //No need to get purple information

            player.IsFriendsWithYou = message.GetBoolean(11);
            player.MagicLevel = message.GetInt(12);
            player.HasBuildersClub = message.GetBoolean(13);
            player.IsGuardian = message.GetBoolean(14);

            if (!handled)
            {
                worldCon.Players.Add(player);
            }

            AddEvent addEvent = new AddEvent()
            {
                Raw = message,
                Player = player
            };

            connectionBase.RaiseServerEvent<AddEvent>(addEvent);
        }
    }
}
