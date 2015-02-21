using Fluid.ServerEvents;
using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.Handlers
{
    public class LoadLevelHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "reset" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection bae</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            WorldConnection worldCon = (WorldConnection)connectionBase;
            World world = worldCon.World;

            if (!handled)
            {
                world.Deserialize(message);
                foreach (KeyValuePair<int, WorldPlayer> player in worldCon.Players.GetDictionary())
                {
                    player.Value.Reset();
                }
            }

            LoadLevelEvent loadLevelEvent = new LoadLevelEvent()
            {
                Raw = message,
                World = world
            };

            connectionBase.RaiseServerEvent<LoadLevelEvent>(loadLevelEvent);
        }
    }
}
