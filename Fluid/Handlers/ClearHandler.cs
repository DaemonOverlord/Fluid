using Fluid.Room;
using Fluid.ServerEvents;
using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.Handlers
{
    public class ClearHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "clear" }; }
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
            World world = worldCon.World;

            int width = message.GetInt(0);
            int height = message.GetInt(1);
            int borderId = message.GetInt(2);
            int workareaId = message.GetInt(3);

            if (!handled)
            {
                world.Clear(width, height, (BlockID)borderId, (BlockID)workareaId);
                foreach (KeyValuePair<int, WorldPlayer> player in worldCon.Players.GetList())
                {
                    player.Value.Reset();
                }
            }

            BlockID borderID = (BlockID)borderId;
            BlockID workareaID = (BlockID)workareaId;

            ClearEvent clearEvent = new ClearEvent()
            {
                Raw = message,
                World = world,
                BorderBlockID = borderID,
                WorkareaBlockID = workareaID
            };

            connectionBase.RaiseServerEvent<ClearEvent>(clearEvent);
        }
    }
}
