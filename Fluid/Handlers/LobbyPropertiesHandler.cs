using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class LobbyPropertiesHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the lobby properties handle types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "getLobbyProperties" }; }
        }

        /// <summary>
        /// Processes the lobby properties
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            bool firstDailyLogin = message.GetBoolean(0);

            LobbyProperties lobbyProps = new LobbyProperties(firstDailyLogin);

            GetLobbyPropertiesEvent lobbyPropertiesMessage = new GetLobbyPropertiesEvent()
            {
                LobbyProperties = lobbyProps,
                Raw = message
            };

            connectionBase.RaiseServerEvent<GetLobbyPropertiesEvent>(lobbyPropertiesMessage);
        }
    }
}
