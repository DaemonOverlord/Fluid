using Fluid.Blocks;
using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a music block is placed
    /// </summary>
    public class MusicBlockEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the music block
        /// </summary>
        public MusicBlock MusicBlock { get; set; }
    }
}
