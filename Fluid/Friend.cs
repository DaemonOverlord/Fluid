namespace Fluid
{
    public class Friend
    {
        private LobbyConnection m_LobbyConnection;

        /// <summary>
        /// Gets the friend's username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets whether the friend is online
        /// </summary>
        public bool Online { get; set; }

        /// <summary>
        /// Gets the player's default smiley
        /// </summary>
        public FaceID Smiley { get; set; }

        /// <summary>
        /// Gets the player the world is in; otherwise null
        /// </summary>
        public WorldReference In { get; set; }

        /// <summary>
        /// Gets the players profile
        /// </summary>
        /// <returns>The profile if successful; otherwise null</returns>
        public Profile GetProfile()
        {
            if (m_LobbyConnection == null)
            {
                return null;
            }

            return m_LobbyConnection.GetProfile(Username);
        }

        /// <summary>
        /// Deletes the friend
        /// </summary>
        public void Delete()
        {
            if (m_LobbyConnection == null)
            {
                return;
            }

            m_LobbyConnection.DeleteFriend(this);
        }

        /// <summary>
        /// Deletes the pending friend
        /// </summary>
        public void DeletePending()
        {
            if (m_LobbyConnection == null)
            {
                return;
            }

            m_LobbyConnection.DeletePending(this);
        }

        /// <summary>
        /// Accepts the friend
        /// </summary>
        public void Accept()
        {
            if (m_LobbyConnection == null)
            {
                return;
            }

            m_LobbyConnection.AcceptInvite(Username);
        }

        /// <summary>
        /// Denies the friend
        /// </summary>
        public void Deny()
        {
            if (m_LobbyConnection == null)
            {
                return;
            }

            m_LobbyConnection.DenyInvite(Username);
        }

        /// <summary>
        /// Blocks the friend
        /// </summary>
        public void Block()
        {
            if (m_LobbyConnection == null)
            {
                return;
            }

            m_LobbyConnection.BlockInvite(Username);
        }

        /// <summary>
        /// UnBlocks the friend
        /// </summary>
        public void UnBlock()
        {
            if (m_LobbyConnection == null)
            {
                return;
            }

            m_LobbyConnection.UnBlockInvite(Username);
        }

        /// <summary>
        /// Sets the friend's status
        /// </summary>
        /// <param name="online">Is online</param>
        /// <param name="smiley">The selected smiley</param>
        /// <param name="worldId">The World ID</param>
        /// <param name="worldName">The world name</param>
        internal void SetOnlineStatus(bool online, FaceID smiley, string worldId, string worldName)
        {
            Online = online;
            Smiley = smiley;

            if (!string.IsNullOrEmpty(worldId))
            {
                In = new WorldReference(m_LobbyConnection.Client, worldId)
                {
                    WorldName = worldName
                };
            }
            else
            {
                In = null;
            }
        }

        /// <summary>
        /// Gets the debug string for the friend
        /// </summary>
        public override string ToString()
        {
            return string.Format("Name: {0}", Username);
        }

        /// <summary>
        /// Creates a new friend
        /// </summary>
        /// <param name="lobbyConnection">The lobby connection</param>
        /// <param name="username">The username</param>
        public Friend(LobbyConnection lobbyConnection, string username)
        {  
            this.m_LobbyConnection = lobbyConnection;

            Username = username;
        }
    }
}
