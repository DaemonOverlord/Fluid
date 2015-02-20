using PlayerIOClient;

namespace Fluid.ServerEvents
{
    public class FaceEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who changed their face
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
