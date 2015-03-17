using Fluid.Room;
using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class MusicBlockHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "bs" }; }
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
            uint musicId = message.GetUInt(3);

            MusicBlock musicBlock = null;
            if (message.Count > 4)
            {
                int userId = message.GetInt(4);
                WorldPlayer player = worldCon.Players.Get(userId);

                musicBlock = new MusicBlock(worldCon, blockId, x, y, musicId)
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
                musicBlock = new MusicBlock(worldCon, blockId, x, y, musicId);
            }

            if (!handled)
            {
                world.SetBlock(musicBlock);
            }

            worldCon.CheckBlock(musicBlock);
            MusicBlockEvent musicBlockEvent = new MusicBlockEvent()
            {
                Raw = message,
                MusicBlock = musicBlock
            };

            connectionBase.RaiseServerEvent<MusicBlockEvent>(musicBlockEvent);
        }
    }
}
