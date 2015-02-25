using Fluid.ServerEvents;
using PlayerIOClient;
using System.Globalization;

namespace Fluid.Handlers
{
    public class GetProfileHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "getProfileObject" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            Profile profile = new Profile();
            profile.ProfileVisible = string.Compare(message.GetString(0), "public", true) == 0;
            if (profile.ProfileVisible)
            {
                profile.ConnectionId = message.GetString(1);
                profile.Username = message.GetString(2);
                profile.Smiley = (FaceID)message.GetInt(3);
                profile.IsBeta = message.GetBoolean(4);
                profile.IsModerator = message.GetBoolean(5);
                profile.IsInBuildersClub = message.GetBoolean(6);
                profile.BuildersClubMembershipTimeRemaining = message.GetDouble(7);
                profile.BuildersClubMembershipTime = message.GetDouble(8);
                profile.BuildersClubMembershipID = message.GetString(9);

                string room0 = message.GetString(10);
                profile.Room0 = room0 == string.Empty ? null : new WorldReference(connectionBase.Client, room0);

                string betaOnlyWorld = message.GetString(11);
                profile.BetaOnlyWorld = betaOnlyWorld == string.Empty ? null : new WorldReference(connectionBase.Client, betaOnlyWorld);

                string[] worldList = message.GetString(13).Split(',');
                profile.OwnedWorlds = new WorldReference[worldList.Length];
                for (int i = 0; i < worldList.Length; i++)
                {
                    profile.OwnedWorlds[i] = new WorldReference(connectionBase.Client, worldList[i]);
                }
            }

            GetProfileEvent getProfileEvent = new GetProfileEvent()
            {
                Profile = profile,
                Raw = message
            };

            connectionBase.RaiseServerEvent<GetProfileEvent>(getProfileEvent);
        }
    }
}
