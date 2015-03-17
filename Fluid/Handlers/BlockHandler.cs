using Fluid.Room;
using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class BlockHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "b" }; }
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

            Layer layer = (Layer)message.GetInt(0);
            int x = message.GetInt(1);
            int y = message.GetInt(2);
            BlockID blockId = (BlockID)message.GetInt(3);

            Block block = null;
            if (message.Count > 4)
            {
                int userId = message.GetInt(4);

                WorldPlayer player = worldCon.Players.Get(userId);
                block = new Block(worldCon, blockId, layer, x, y)
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
                block = new Block(worldCon, blockId, layer, x, y);
            }

            if (!handled)
            {
                Block old = world[x, y, layer];
                if (old.ID == BlockIDS.Action.Coins.Gold ||
                    old.ID == BlockIDS.Action.Coins.Blue)
                {
                    if (blockId != old.ID)
                    {
                        worldCon.Physics.RemoveCoin(old);
                    }
                }

                world.SetBlock(block);               
            }

            worldCon.CheckBlock(block);
            BlockEvent blockEvent = new BlockEvent()
            {
                Raw = message,
                Block = block
            };

            connectionBase.RaiseServerEvent<BlockEvent>(blockEvent);
        }
    }
}
