using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server callback event for getting changing the player's profile visibility
    /// </summary>
    public class GetProfileVisibilityEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets if the profile is visible
        /// </summary>
        public bool IsVisible { get; internal set; }
    }
}
