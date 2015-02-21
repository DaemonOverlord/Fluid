namespace Fluid.Events
{
    public class DisconnectEvent : IEvent
    {
        /// <summary>
        /// Gets the world connection
        /// </summary>
        public ConnectionBase Connection { get; internal set; }

        /// <summary>
        /// Gets the disconnection message from the server
        /// </summary>
        public string Message { get; internal set; }

        /// <summary>
        /// Gets if the connection should try and reconnect
        /// </summary>
        internal bool TryReconnect { get; set; }

        /// <summary>
        /// Attempts to reestablish the connection
        /// </summary>
        public void Reconnect()
        {
            TryReconnect = true;
        }
    }
}
