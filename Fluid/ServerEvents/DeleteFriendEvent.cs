using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a friend was deleted successfully
    /// </summary>
    public class DeleteFriendEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets if the deletion was successful
        /// </summary>
        public bool Success { get; internal set; }
    }
}
