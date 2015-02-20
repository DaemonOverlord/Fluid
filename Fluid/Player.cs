namespace Fluid
{
    public class Player
    {
        private FluidClient m_Client;

        /// <summary>
        /// Gets the player's username
        /// </summary>
        public string Username { get; internal set; }

        /// <summary>
        /// Gets the player's connection id
        /// </summary>
        public string ConnectionId { get; internal set; }

        /// <summary>
        /// Gets the player's connection type
        /// </summary>
        public PlayerType Type { get; internal set; }

        /// <summary>
        /// Trys to fetch the player's connection id from the player database
        /// </summary>
        private void FetchConnectionId()
        {
            if (m_Client != null && m_Client.PlayerDatabase.Connected)
            {
                ConnectionId = m_Client.PlayerDatabase.GetConnectionId(Username);
            }
        }

        /// <summary>
        /// Gets the player's profile
        /// </summary>
        private Profile GetProfile()
        {
            return m_Client.LoadProfile(Username);
        }

        /// <summary>
        /// Gets the player debug message
        /// </summary>
        public override string ToString()
        {
            return string.Format("Name: {0}", Username);
        }

        /// <summary>
        /// Creates a new player
        /// </summary>
        /// <param name="client">The Fluid client</param>
        /// <param name="username">The username</param>
        internal Player(FluidClient client, string username)
        {
            Username = username;
            this.m_Client = client;
            this.FetchConnectionId();

            Type = m_Client.GetConnectionType(ConnectionId);
        }

        /// <summary>
        /// Creates a new player
        /// </summary>
        /// <param name="client">The Fluid client</param>
        /// <param name="username">The username</param>
        /// <param name="connectionId">The connection id</param>
        internal Player(FluidClient client, string username, string connectionId)
        {
            Username = username;
            ConnectionId = connectionId;
            this.m_Client = client;

            Type = m_Client.GetConnectionType(ConnectionId);
        }
    }
}
