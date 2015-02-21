using Fluid.ServerEvents;
using PlayerIOClient;
using System;
using System.Globalization;

namespace Fluid.Handlers
{
    public class GetPlayerObjectHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the player object handle types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "getMySimplePlayerObject" }; }
        }

        /// <summary>
        /// Processes the player object message
        /// </summary>
        /// <param name="connectionBase">The Fluid connection base</param>
        /// <param name="message">The message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            PlayerObject pom = new PlayerObject();

            pom.Username = message.GetString(0);
            pom.Smiley = (FaceID)message.GetInt(1);
            pom.ChatBanned = message.GetBoolean(2);
            pom.CanChat = message.GetBoolean(3);
            pom.HasSmileyPackage = message.GetBoolean(4);
            pom.IsModerator = message.GetBoolean(5);
            pom.IsGuardian = message.GetBoolean(6);
            pom.IsInBuildersClub = message.GetBoolean(7);
            pom.BuildersClubMembershipTimeRemaining = message.GetDouble(8);
            pom.BuildersClubMembershipTime = message.GetDouble(9);
            pom.BuildersClubMembershipID = message.GetString(10);
            pom.BuildersClubWelcome = message.GetBoolean(11);

            string room0 = message.GetString(12);
            pom.Room0 = room0 == string.Empty ? null : new WorldReference(connectionBase.Client, room0);

            string betaOnlyWorld = message.GetString(13);
            pom.BetaOnlyWorld = betaOnlyWorld == string.Empty ? null : new WorldReference(connectionBase.Client, betaOnlyWorld);

            pom.HomeWorld = new WorldReference(connectionBase.Client, message.GetString(14));

            string[] worldList = message.GetString(16).Split(',');
            pom.OwnedWorlds = new WorldReference[worldList.Length];
            for (int i = 0; i < worldList.Length; i++)
            {
                pom.OwnedWorlds[i] = new WorldReference(connectionBase.Client, worldList[i]);
            }

            pom.ProfileVisible = message.GetBoolean(18);
            pom.Banned = message.GetBoolean(19);
            pom.HasAcceptedTerms = Convert.ToBoolean(message.GetInt(20));

            pom.WootUp = message.GetInt(21);
            pom.Woot = message.GetInt(22);
            pom.MagicLevel = message.GetInt(23);
            pom.LevelCapPrevious = message.GetInt(24);
            pom.LevelCapNext = message.GetInt(25);                
            pom.LevelTitle = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(message.GetString(26).ToLowerInvariant());
            pom.WootTotal = message.GetInt(27);
            pom.WootDaily = message.GetInt(28);
            pom.WootDecayTime = message.GetInt(29);
            pom.WootDecay = message.GetInt(30);
            pom.MaxEnergy = message.GetInt(31);

            PlayerObjectEvent playerObjectEvent = new PlayerObjectEvent()
            {
                PlayerObject = pom,
                Raw = message
            };

            connectionBase.RaiseServerEvent<PlayerObjectEvent>(playerObjectEvent);
        }
    }
}
