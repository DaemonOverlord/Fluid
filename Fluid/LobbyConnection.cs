using Fluid.ServerEvents;
using Fluid.Handlers;
using PlayerIOClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Fluid
{
    public class LobbyConnection : ConnectionBase
    {
        /// <summary>
        /// Gets the lobby connection event timeout
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets the message of the day
        /// </summary>
        public string MessageOfTheDay { get; internal set; }

        /// <summary>
        /// Requests lobby properties from the server
        /// </summary>
        public void RequestLobbyProperties()
        {
            this.SendMessage("getLobbyProperties");
        }

        /// <summary>
        /// Gets the lobby's properties
        /// </summary>
        public LobbyProperties GetLobbyProperties()
        {
            RequestLobbyProperties();
            GetLobbyPropertiesEvent lobbyPropertiesEvent = base.WaitForServerEvent<GetLobbyPropertiesEvent>(Timeout);
            if (lobbyPropertiesEvent == null)
            {
                return null;
            }

            return lobbyPropertiesEvent.LobbyProperties;
        }

        /// <summary>
        /// Requests the players simple object
        /// </summary>
        public void RequestPlayerObject()
        {
            this.SendMessage("getMySimplePlayerObject");
        }

        /// <summary>
        /// Gets player object
        /// </summary>
        public PlayerObject GetPlayerObject()
        {
            RequestPlayerObject();
            PlayerObjectEvent playerObjectEvent = base.WaitForServerEvent<PlayerObjectEvent>(Timeout);
            if (playerObjectEvent == null)
            {
                return null;
            }

            return playerObjectEvent.PlayerObject;
        }

        /// <summary>
        /// Requests a player's profile
        /// </summary>
        /// <param name="username">The username</param>
        public void RequestProfile(string username)
        {
            this.SendMessage("getProfileObject", username);
        }

        /// <summary>
        /// Gets a player's profile
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>the player profile if found; otherwise null</returns>
        public Profile GetProfile(string username)
        {
            RequestProfile(username);
            GetProfileEvent getProfileEvent = WaitForServerEvent<GetProfileEvent>(Timeout);
            if (getProfileEvent == null)
            {
                return null;
            }

            return getProfileEvent.Profile;
        }

        /// <summary>
        /// Requests the shop
        /// </summary>
        public void RequestShop()
        {
            this.SendMessage("getShop");
        }

        /// <summary>
        /// Gets the shop
        /// </summary>
        public Shop GetShop()
        {
            RequestShop();
            GetShopEvent shopMessage = base.WaitForServerEvent<GetShopEvent>();
            if (shopMessage != null)
            {
                return shopMessage.Shop;
            }

            return null;
        }

        /// <summary>
        /// Requests to use energy on a shop item
        /// </summary>
        /// <param name="item">The item</param>
        public void RequestUseEnergy(VaultShopItem item)
        {
            this.RequestUseEnergy(item.ID);
        }

        /// <summary>
        /// Use's energy on a shop item
        /// </summary>
        /// <param name="shopItemId">The shop item id</param>       
        public void RequestUseEnergy(string shopItemId)
        {
            this.SendMessage("useEnergy", shopItemId);
        }

        /// <summary>
        /// Requests to use energy on a 
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>Whether the energy was spent</returns>
        public bool UseEnergy(VaultShopItem item)
        {
            return this.UseEnergy(item.ID);
        }

        /// <summary>
        /// Use's energy on a shop item
        /// </summary>
        /// <param name="shopItemId"></param>
        /// <returns>Whether the energy was spent</returns>
        public bool UseEnergy(string shopItemId)
        {
            RequestUseEnergy(shopItemId);
            GetShopEvent shopUpdate = WaitForServerEvent<GetShopEvent>(Timeout);
            if (shopUpdate == null)
            {
                return false;
            }

            return shopUpdate.Success;
        }

        /// <summary>
        /// Use's gems on a shop item
        /// </summary>
        /// <param name="item">The item</param>
        public void RequestUseGems(VaultShopItem item)
        {
            this.RequestUseGems(item.ID);
        }

        /// <summary>
        /// Use's gems on a shop item
        /// </summary>
        /// <param name="shopItemId">The shop item id</param>
        public void RequestUseGems(string shopItemId)
        {
            this.SendMessage("useGems", shopItemId);
        }

        /// <summary>
        /// Use's gems on a shop item
        /// </summary>
        /// <param name="shopItemId">The item</param>
        /// <returns>Whether gems were spent</returns>
        public bool UseGems(VaultShopItem item)
        {
            return this.UseGems(item.ID);
        }

        /// <summary>
        /// Use's gems on a shop item
        /// </summary>
        /// <param name="shopItemId">The item</param>
        /// <returns>Whether gems were spent</returns>
        public bool UseGems(string shopItemId)
        {
            RequestUseGems(shopItemId);
            GetShopEvent shopUpdate = WaitForServerEvent<GetShopEvent>(Timeout);
            if (shopUpdate == null)
            {
                return false;
            }

            return shopUpdate.Success;
        }

        /// <summary>
        /// Requests the player's blocked users
        /// </summary>
        public void RequestBlocked()
        {
            this.SendMessage("getBlockedUsers");
        }

        /// <summary>
        /// Gets the player's blocked users
        /// </summary>
        /// <returns>The list of blocked friends</returns>
        public List<Friend> GetBlocked()
        {
            RequestBlocked();
            GetBlockedEvent blockedEvent = WaitForServerEvent<GetBlockedEvent>(Timeout);
            if (blockedEvent == null)
            {
                return null;
            }

            return blockedEvent.Blocked;
        }

        /// <summary>
        /// Requests the list of pending friend requests
        /// </summary>
        public void RequestPending()
        {
            this.SendMessage("getPending");
        }

        /// <summary>
        /// Gets the player's pending friends
        /// </summary>
        /// <returns>The list of pending friends</returns>
        public List<Friend> GetPending()
        {
            RequestPending();
            GetPendingEvent pendingEvent = WaitForServerEvent<GetPendingEvent>(Timeout);
            if (pendingEvent == null)
            {
                return null;
            }

            return pendingEvent.Pending;
        }

        /// <summary>
        /// Requests the list of friends
        /// </summary>
        public void RequestFriends()
        {
            this.SendMessage("getFriends");
        }

        /// <summary>
        /// Gets the player's friends
        /// </summary>
        /// <returns>The list of friends</returns>
        public List<Friend> GetFriends()
        {
            RequestFriends();
            GetFriendsEvent friendsEvent = WaitForServerEvent<GetFriendsEvent>(Timeout);
            if (friendsEvent == null)
            {
                return null;
            }

            return friendsEvent.Friends;
        }

        /// <summary>
        /// Requests to get all invites to the player
        /// </summary>
        public void RequestAllInvites()
        {
            this.SendMessage("getInvitesToMe");
        }

        /// <summary>
        /// Gets all friend invites
        /// </summary>
        /// <returns>The list of friend invites</returns>
        public List<Friend> GetAllInvites()
        {
            RequestAllInvites();
            GetInvitesEvent getInvitesEvent = WaitForServerEvent<GetInvitesEvent>(Timeout);
            if (getInvitesEvent == null)
            {
                return null;
            }

            return getInvitesEvent.Invites;
        }

        /// <summary>
        /// Deletes a friend
        /// </summary>
        /// <param name="friend">The friend</param>
        public void DeleteFriend(Friend friend)
        {
            this.DeleteFriend(friend.Username);
        }

        /// <summary>
        /// Deletes a friend
        /// </summary>
        /// <param name="username">The friend's username</param>
        public void DeleteFriend(string username)
        {
            this.SendMessage("deleteFriend", username);
        }

        /// <summary>
        /// Deletes a pending friend
        /// </summary>
        /// <param name="friend">The pending friend</param>
        public void DeletePending(Friend friend)
        {
            this.DeletePending(friend.Username);
        }

        /// <summary>
        /// Deletes a pending friend
        /// </summary>
        /// <param name="username">The pending friend's username</param>
        public void DeletePending(string username)
        {
            this.SendMessage("deletePending", username);
        }

        /// <summary>
        /// Creates a new friend invite
        /// </summary>
        /// <param name="emailAddress">The email address</param>
        public void CreateInvite(string emailAddress)
        {
            if (Regex.IsMatch(emailAddress, "([a-z0-9._-]+)@([a-z0-9.-]+)\\.([a-z]{2,4})"))
            {
                this.SendMessage("createInvite", emailAddress);
            }
            else if (m_Client != null)
            {
                m_Client.Log.Add(FluidLogCategory.Message, "Invalid invite email address.");
            }
        }

        /// <summary>
        /// Accepts a friend invite
        /// </summary>
        /// <param name="username">The username</param>
        public void AcceptInvite(string username)
        {
            this.SendMessage("answerInvite", username, true);
        }

        /// <summary>
        /// Denies a friend invite
        /// </summary>
        /// <param name="username">The username</param>
        public void DenyInvite(string username)
        {
            this.SendMessage("answerInvite", username, false);
        }

        /// <summary>
        /// Blocks a friend invite
        /// </summary>
        /// <param name="username">The username</param>
        public void BlockInvite(string username)
        {
            this.SendMessage("blockInvite", null, username);
        }

        /// <summary>
        /// Un-Blocks a blocked friend invite
        /// </summary>
        /// <param name="username">The username</param>
        public void UnBlockInvite(string username)
        {
            this.SendMessage("unblockInvite", null, username);
        }

        /// <summary>
        /// Requests to get the profile visibility
        /// </summary>
        public void RequestProfileVisibility()
        {
            this.SendMessage("getProfile");
        }

        /// <summary>
        /// Gets the player's visibility of their profile
        /// </summary>
        /// <returns>True if public; false if private</returns>
        public bool GetProfileVisibility()
        {
            RequestProfileVisibility();
            GetProfileVisibilityEvent getProfileEvent = WaitForServerEvent<GetProfileVisibilityEvent>(Timeout);
            if (getProfileEvent == null)
            {
                return true;
            }

            return getProfileEvent.IsVisible;
        }

        /// <summary>
        /// Requests to toggle the profile visibility
        /// </summary>
        public void RequestToggleProfileVisibility()
        {
            this.SendMessage("toggleProfile");
        }

        /// <summary>
        /// Toggles the profile's visibility
        /// </summary>
        /// <returns>True if public; false if private</returns>
        public bool ToggleProfileVisibility()
        {
            RequestToggleProfileVisibility();
            GetProfileVisibilityEvent getProfileEvent = WaitForServerEvent<GetProfileVisibilityEvent>(Timeout);
            if (getProfileEvent == null)
            {
                return true;
            }

            return getProfileEvent.IsVisible;
        }

        /// <summary>
        /// Sets the player's profile visibility
        /// </summary>
        /// <param name="profileVisibility">The profile visibility</param>
        public void SetProfileVisibility(ProfileVisibility profileVisibility)
        {
            bool requestedVisibility = profileVisibility == ProfileVisibility.Public;
            this.SetProfileVisibility(requestedVisibility);
        }

        /// <summary>
        /// Sets the player's profile visibility
        /// </summary>
        /// <param name="requestedVisibility">The profile visibility</param>
        public void SetProfileVisibility(bool requestedVisibility)
        {
            bool currentVisibility = GetProfileVisibility();
            if (currentVisibility != requestedVisibility)
            {
                ToggleProfileVisibility();
            }
        }

        /// <summary>
        /// Trys to reestablish the lobby connection
        /// </summary>
        internal override void Reconnect()
        {
            Connection connection = m_Client.GetLobbyConnection();

            if (connection != null)
            {
                SetConnection(connection);
            }

            base.Reconnect();
        }

        /// <summary>
        /// Creates a new Fluid lobby connection
        /// </summary>
        /// <param name="client">The Fluid client</param>
        public LobbyConnection(FluidClient client) : base(client)
        {
            Timeout = 2500;

            base.AddMessageHandler(new ConnectionCompleteHandler());
            base.AddMessageHandler(new LobbyPropertiesHandler());
            base.AddMessageHandler(new GetProfileHandler());
            base.AddMessageHandler(new GetPlayerObjectHandler());
            base.AddMessageHandler(new GetShopHandler());
            base.AddMessageHandler(new GetFriendsHandler());
            base.AddMessageHandler(new GetPendingHandler());
            base.AddMessageHandler(new GetBlockedHandler());
            base.AddMessageHandler(new GetInvitesHandler());
            base.AddMessageHandler(new InfoHandler()); 
        }
    }
}
