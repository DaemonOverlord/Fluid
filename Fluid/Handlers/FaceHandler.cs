using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class FaceHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "face" }; }
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
            int face = message.GetInt(1);

            WorldConnection worldCon = (WorldConnection)connectionBase;
            WorldPlayer player = worldCon.Players.GetPlayer(userId);

            if (!handled && player != null)
            {
                player.Face = (FaceID)face;
            }

            FaceEvent faceEvent = new FaceEvent()
            {
                Raw = message,
                Player = player
            };

            connectionBase.RaiseServerEvent<FaceEvent>(faceEvent);
        }
    }
}
