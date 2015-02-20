namespace Fluid.Events
{
    public class DisconnectEvent : IEvent
    {
        /// <summary>
        /// Gets the world connection
        /// </summary>
        public FluidConnectionBase Connection { get; internal set; }

        /// <summary>
        /// Gets the disconnection message from the server
        /// </summary>
        public string Message { get; internal set; }
    }
}
