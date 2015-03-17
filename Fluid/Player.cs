using System.Collections.Generic;
using System.Diagnostics;
namespace Fluid
{
    [DebuggerDisplay("Name = {Username}")]
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
            if (m_Client != null)
            {
                if (Username.Contains("-"))
                {
                    ConnectionId = "simpleguest";
                }
                else
                {
                    ConnectionId = (m_Client.PlayerDatabase.Connected) ? m_Client.PlayerDatabase.GetConnectionId(Username) : null;
                    if (ConnectionId == null)
                    {
                        Profile p = GetProfile();

                        if (p != null)
                        {
                            ConnectionId = p.ConnectionId;

                            if (!string.IsNullOrEmpty(ConnectionId) && m_Client.Config.AddProfilesToDatabase)
                            {
                                m_Client.PlayerDatabase.Set(ConnectionId, Username);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the player's profile
        /// </summary>
        public Profile GetProfile()
        {
            return m_Client.LoadProfile(Username);
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
