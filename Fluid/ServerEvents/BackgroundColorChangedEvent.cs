using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a world's background color is changed
    /// </summary>
    public class BackgroundColorChangedEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the color the background was changed to
        /// </summary>
        public FluidColor Color { get; internal set; }
    }
}
