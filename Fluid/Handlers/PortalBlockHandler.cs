using Fluid.Room;
using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class PortalBlockHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "pt" }; }
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

            int x = message.GetInt(0);
            int y = message.GetInt(1);
            BlockID blockId = (BlockID)message.GetInt(2);
            Rotation rotation = (Rotation)message.GetUInt(3);

            uint portalId = message.GetUInt(4);
            uint portalTargetId = message.GetUInt(5);

            Portal portal = null;

            if (message.Count > 6)
            {
                int userId = message.GetInt(6);
                WorldPlayer player = worldCon.Players.Get(userId);

                portal = new Portal(worldCon, blockId, x, y, rotation, portalId, portalTargetId)
                {
                    Placer = player
                };

                if (player != null)
                {
                    if ((player.AccessLevel & AccessLevel.Edit) == 0)
                    {
                        player.AccessLevel |= AccessLevel.Edit;
                    }
                }
            }
            else
            {
                portal = new Portal(worldCon, blockId, x, y, rotation, portalId, portalTargetId);
            }

            if (!handled)
            {
                world.SetBlock(portal);
            }

            worldCon.CheckBlock(portal);
            PortalBlockEvent portalBlockEvent = new PortalBlockEvent()
            {
                Raw = message,
                Portal = portal
            };

            connectionBase.RaiseServerEvent<PortalBlockEvent>(portalBlockEvent);
        }
    }
}
