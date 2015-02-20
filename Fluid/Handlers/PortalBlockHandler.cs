﻿using Fluid.Blocks;
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
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            int x = message.GetInt(0);
            int y = message.GetInt(1);
            BlockID blockId = (BlockID)message.GetInt(2);
            Rotation rotation = (Rotation)message.GetUInt(3);

            uint portalId = message.GetUInt(4);
            uint portalTargetId = message.GetUInt(5);

            int userId = message.GetInt(6);

            WorldConnection worldCon = (WorldConnection)connectionBase;

            World world = worldCon.World;
            WorldPlayer player = worldCon.Players.GetPlayer(userId);

            Portal portal = new Portal(worldCon, blockId, x, y, rotation, portalId, portalTargetId)
            {
                Placer = player
            };

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
