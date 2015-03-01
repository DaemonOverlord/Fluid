using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a player changes their guardian mode
    /// </summary>
    public class GuardianModeEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who toggled guardian mode
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
