using Fluid.ServerEvents;
using PlayerIOClient;
using System;

namespace Fluid.Handlers
{
    public class InitHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "init" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            WorldConnection worldCon = (WorldConnection)connectionBase;

            World world = new World(connectionBase.Client, message);

            string username = message.GetString(9);
            int userId = message.GetInt(6);

            WorldPlayer connected = new WorldPlayer(worldCon, username, userId);

            FluidClient client = connectionBase.Client;

            if (client.GetConnectionType(client.ConnectionUserId) != PlayerType.Guest)
            {
                PlayerObject playerObject = connectionBase.Client.LoadMyPlayerObject();

                connected.Face = playerObject.Smiley;
                connected.HasBuildersClub = playerObject.IsInBuildersClub;
                connected.HasChat = playerObject.CanChat;
                connected.IsFriendsWithYou = false;
                connected.IsGuardian = playerObject.IsGuardian;
                connected.IsModerator = playerObject.IsModerator;
            }
            else
            {
                connected.Face = FaceID.Smile;
                connected.HasBuildersClub = false;
                connected.HasChat = false;
                connected.IsFriendsWithYou = false;
                connected.IsGuardian = false;
                connected.IsModerator = false;
            }

            connected.X = message.GetInt(7);
            connected.Y = message.GetInt(8);

            bool decodingPotions = true;
            for (uint i = message.Count - 1; i >= 0; i--)
            {
                if (message[i] is string)
                {
                    if (string.Compare(message.GetString(i), "pe", false) == 0)
                    {
                        decodingPotions = true;
                        continue;
                    }
                    else if (string.Compare(message.GetString(i), "ps", false) == 0)
                    {
                        break;
                    }
                }

                if (decodingPotions)
                {
                    int potionCount = message.GetInt(i);
                    int potionType = message.GetInt(i--);
                    worldCon.Potions.SetPotionCount((Potion)potionType, potionCount);
                }
            }

            if (!handled)
            {
                worldCon.World = world;
                worldCon.Players.Add(connected);
                worldCon.Me = connected;
            }

            InitEvent initEvent = new InitEvent();
            initEvent.Raw = message;
            initEvent.World = world;
            initEvent.ConnectedPlayer = connected;

            connectionBase.RaiseServerEvent<InitEvent>(initEvent);            
        }
    }
}
  