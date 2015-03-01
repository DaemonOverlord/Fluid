using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server callback event for getting the lobby properties
    /// </summary>
    public class GetLobbyPropertiesEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the lobby properties
        /// </summary>
        public LobbyProperties LobbyProperties { get; internal set; }
    }
}
