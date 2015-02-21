using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class LeftHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "left" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            WorldConnection worldCon = (WorldConnection)connectionBase;

            int userId = message.GetInt(0);

            WorldPlayer player = worldCon.Players.GetPlayer(userId);
            LeftEvent leftEvent = new LeftEvent()
            {
                Raw = message,
                Player = player
            };

            if (!handled)
            {
                worldCon.Players.Remove(userId);
            }

            connectionBase.RaiseServerEvent<LeftEvent>(leftEvent);
        }
    }
}
