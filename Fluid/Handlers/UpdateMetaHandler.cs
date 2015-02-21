using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class UpdateMetaHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "updatemeta" }; }
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

            if (!handled)
            {
                string username = message.GetString(0);
                if (string.Compare(world.Owner.Username, username, true) != 0)
                {
                    world.Owner = worldCon.Client.GetPlayerByUsername(username);
                }

                world.Title = message.GetString(1);
                world.Plays = message.GetInt(2);
                world.Woots = message.GetInt(3);
                world.TotalWoots = message.GetInt(4);
            }

            UpdateMetaEvent updateMetaEvent = new UpdateMetaEvent()
            {
                Raw = message,
                World = world
            };

            connectionBase.RaiseServerEvent<UpdateMetaEvent>(updateMetaEvent);
        }
    }
}
