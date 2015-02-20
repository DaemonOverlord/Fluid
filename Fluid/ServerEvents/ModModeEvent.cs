using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class ModModeEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who toggled moderator mode
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
