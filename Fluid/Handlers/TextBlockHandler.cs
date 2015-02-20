using Fluid.Blocks;
using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class TextBlockHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "ts" }; }
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
            int userId = message.GetInt(4);

            WorldConnection worldCon = (WorldConnection)connectionBase;

            World world = worldCon.World;
            WorldPlayer player = worldCon.Players.GetPlayer(userId);

            TextBlock textBlock = new TextBlock(worldCon, blockId, x, y, text)
            {
                Placer = player
            };

            if (!handled)
            {
                world.SetBlock(textBlock);
            }

            worldCon.CheckBlock(textBlock);
            TextBlockEvent textBlockEvent = new TextBlockEvent()
            {
                Raw = message,
                TextBlock = textBlock
            };

            connectionBase.RaiseServerEvent<TextBlockEvent>(textBlockEvent);
        }
    }
}
