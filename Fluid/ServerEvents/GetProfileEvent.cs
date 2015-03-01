using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server callback event for getting a player's profile
    /// </summary>
    public class GetProfileEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the profile
        /// </summary>
        public Profile Profile { get; internal set; }
    }
}
