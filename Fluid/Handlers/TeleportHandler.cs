using Fluid.ServerEvents;
using Fluid.Physics;
using PlayerIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluid.Handlers
{
    public class TeleportHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "teleport" }; }
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

            Vector location = new Vector(message.GetInt(1), message.GetInt(2));

            if (!handled && player != null)
            {
                player.SetLocationInternal(location);
            }

            TeleportEvent teleportEvent = new TeleportEvent()
            {
                Raw = message,
                Player = player,
                Location = location
            };

            connectionBase.RaiseServerEvent<TeleportEvent>(teleportEvent);
        }
    }
}
