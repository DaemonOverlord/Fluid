using Fluid.Blocks;
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
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            int x = message.GetInt(0);
            int y = message.GetInt(1);
            BlockID blockId = (BlockID)message.GetInt(2);
            string text = message.GetString(3);
            string hexColor = message.GetString(4);
            int userId = message.GetInt(5);

            WorldConnection worldCon = (WorldConnection)connectionBase;

            World world = worldCon.World;
            WorldPlayer player = worldCon.Players.GetPlayer(userId);

            LabelBlock labelBlock = new LabelBlock(worldCon, blockId, x, y, text, hexColor)
            {
                Placer = player
            };

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
