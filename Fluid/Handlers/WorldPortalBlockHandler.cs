using Fluid.Blocks;
using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class WorldPortalBlockHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "wp" }; }
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
            string target = message.GetString(3);

            WorldPortal worldPortal = null;
            if (message.Count > 4)
            {
                int userId = message.GetInt(4);
                WorldPlayer player = worldCon.Players.Get(userId);
                worldPortal = new WorldPortal(worldCon, x, y, target)
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
                worldPortal = new WorldPortal(worldCon, x, y, target);
            }

            if (!handled)
            {
                world.SetBlock(worldPortal);
            }

            worldCon.CheckBlock(worldPortal);
            WorldPortalBlockEvent worldPortalBlockEvent = new WorldPortalBlockEvent()
            {
                Raw = message,
                WorldPortal = worldPortal
            };

            connectionBase.RaiseServerEvent<WorldPortalBlockEvent>(worldPortalBlockEvent);
        }
    }
}
