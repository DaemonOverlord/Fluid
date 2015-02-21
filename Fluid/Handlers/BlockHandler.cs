using Fluid.Blocks;
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
            Layer layer = (Layer)message.GetInt(0);
            int x = message.GetInt(1);
            int y = message.GetInt(2);
            BlockID blockId = (BlockID)message.GetInt(3);
            int userId = message.GetInt(4);

            WorldConnection worldCon = (WorldConnection)connectionBase;

            World world = worldCon.World;
            WorldPlayer player = worldCon.Players.GetPlayer(userId);
            Block block = new Block(worldCon, blockId, layer, x, y)
            {
                Placer = player
            };

            if (!handled)
            {
                if (blockId == 0)
                {
                    Block old = world[x, y, layer];
                    if (old.ID == BlockID.CoinGold ||
                        old.ID == BlockID.CoinBlue)
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
