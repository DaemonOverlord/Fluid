using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class ModHandler  : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "mod" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            int userId = message.GetInt(0);

            WorldConnection worldCon = (WorldConnection)connectionBase;
            WorldPlayer player = worldCon.Players.Get(userId);

            if (!handled && player != null)
            {
                player.InModMode = !player.InModMode;
            }

            ModModeEvent modModeEvent = new ModModeEvent()
            {
                Raw = message,
                Player = player
            };

            connectionBase.RaiseServerEvent<ModModeEvent>(modModeEvent);
        }
    }
}
