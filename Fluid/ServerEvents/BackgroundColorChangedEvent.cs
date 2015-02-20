using PlayerIOClient;

namespace Fluid.ServerEvents
{
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
