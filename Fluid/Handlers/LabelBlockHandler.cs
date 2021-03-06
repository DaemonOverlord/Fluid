﻿using Fluid.Room;
using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class LabelBlockHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "lb" }; }
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
            string text = message.GetString(3);
            string hexColor = message.GetString(4);

            LabelBlock labelBlock = null;
            if (message.Count > 5)
            {
                int userId = message.GetInt(5);

                WorldPlayer player = worldCon.Players.Get(userId);
                labelBlock = new LabelBlock(worldCon, x, y, text, hexColor)
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
                labelBlock = new LabelBlock(worldCon, x, y, text, hexColor);
            }

            if (!handled)
            {
                world.SetBlock(labelBlock);           
            }

            worldCon.CheckBlock(labelBlock);
            LabelBlockEvent labelBlockEvent = new LabelBlockEvent()
            {
                Raw = message,
                LabelBlock = labelBlock
            };

            connectionBase.RaiseServerEvent<LabelBlockEvent>(labelBlockEvent);
        }
    }
}
