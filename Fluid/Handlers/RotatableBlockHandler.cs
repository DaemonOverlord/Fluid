using Fluid.Blocks;
using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class RotatableBlockHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "br" }; }
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

            RotatableBlock rotatableBlock = null;
            if (message.Count > 4)
            {
                int userId = message.GetInt(4);
                WorldPlayer player = worldCon.Players.GetPlayer(userId);

                rotatableBlock = new RotatableBlock(worldCon, blockId, x, y, rotation)
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
                rotatableBlock = new RotatableBlock(worldCon, blockId, x, y, rotation);
            }

            if (!handled)
            {
                world.SetBlock(rotatableBlock);
            }

            worldCon.CheckBlock(rotatableBlock);
            RotatableBlockEvent rotatableBlockEvent = new RotatableBlockEvent()
            {
                Raw = message,
                RotatableBlock = rotatableBlock
            };

            connectionBase.RaiseServerEvent<RotatableBlockEvent>(rotatableBlockEvent);
        }
    }
}
