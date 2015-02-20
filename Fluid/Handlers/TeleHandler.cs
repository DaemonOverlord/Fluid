using Fluid.ServerEvents;
using Fluid.Physics;
using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.Handlers
{
    public class TeleHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "tele" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            WorldConnection worldCon = (WorldConnection)connectionBase;

            List<WorldPlayer> players = new List<WorldPlayer>();

            bool isWorldReset = message.GetBoolean(0);

            uint index = 1;
            while (index + 2 < message.Count)
            {
                WorldPlayer player = worldCon.Players.GetPlayer(message.GetInt(index));
                if (player != null)
                {
                    if (!handled)
                    {
                        Vector loc = new Vector(message.GetInt(index + 1), message.GetInt(index + 2));
                        player.SetLocationInternal(loc);

                        if (!isWorldReset)
                        {
                            player.Respawn();
                        }
                    }

                    players.Add(player);
                }

                index += 3;
            }

            if (isWorldReset)
            {
                ResetEvent resetEvent = new ResetEvent()
                {
                    Raw = message
                };

                connectionBase.RaiseServerEvent<ResetEvent>(resetEvent);
            }
            else
            {
                for (int i = 0; i < players.Count; i++)
                {
                    KillEvent killEvent = new KillEvent()
                    {
                        Raw = message,
                        Player = players[i]
                    };

                    connectionBase.RaiseServerEvent<KillEvent>(killEvent);
                }
            }
        }
    }
}
