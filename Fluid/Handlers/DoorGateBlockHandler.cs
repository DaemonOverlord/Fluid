using Fluid.Room;
using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class DoorGateBlockHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "bc" }; }
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
            World world = worldCon.World;

            int x = message.GetInt(0);
            int y = message.GetInt(1);
            BlockID blockId = (BlockID)message.GetInt(2);

            uint arg = message.GetUInt(3);

            WorldPlayer player = null;
            if (message.Count > 4)
            {
                int userId = message.GetInt(4);
                player = worldCon.Players.Get(userId);
            }

            if (player != null)
            {
                if ((player.AccessLevel & AccessLevel.Edit) == 0)
                {
                    player.AccessLevel |= AccessLevel.Edit;
                }
            }

            switch (blockId)
            {
                case BlockIDs.Action.Gates.BlueCoin:
                case BlockIDs.Action.Doors.BlueCoin:
                case BlockIDs.Action.Gates.GoldCoin:
                case BlockIDs.Action.Doors.GoldCoin:
                    {
                        CoinBlock block = new CoinBlock(worldCon, blockId, x, y, arg)
                        {
                            Placer = player
                        };

                        if (!handled)
                        {
                            world.SetBlock(block);
                            worldCon.CheckBlock(block);
                        }

                        CoinBlockEvent coinBlockEvent = new CoinBlockEvent()
                        {
                            Raw = message,
                            CoinBlock = block
                        };

                        connectionBase.RaiseServerEvent<CoinBlockEvent>(coinBlockEvent);
                    }
                    break;
                case BlockIDs.Action.Doors.Death:
                case BlockIDs.Action.Gates.Death:
                    {
                        DeathBlock block = new DeathBlock(worldCon, blockId, x, y, arg)
                        {
                            Placer = player
                        };

                        if (!handled)
                        {
                            world.SetBlock(block);
                            worldCon.CheckBlock(block);
                        }

                        DeathBlockEvent deathBlockEvent = new DeathBlockEvent()
                        {
                            Raw = message,
                            DeathBlock = block
                        };

                        connectionBase.RaiseServerEvent<DeathBlockEvent>(deathBlockEvent);
                    }
                    break;
                case BlockIDs.Action.Switches.Switch:
                case BlockIDs.Action.Doors.Switch:
                case BlockIDs.Action.Gates.Switch:
                    {
                        PurpleBlock block = new PurpleBlock(worldCon, blockId, x, y, arg)
                        {
                            Placer = player
                        };

                        if (!handled)
                        {
                            world.SetBlock(block);
                            worldCon.CheckBlock(block);
                        }

                        PurpleBlockEvent purpleBlockEvent = new PurpleBlockEvent()
                        {
                            Raw = message,
                            PurpleBlock = block
                        };

                        connectionBase.RaiseServerEvent<PurpleBlockEvent>(purpleBlockEvent);
                    }
                    break;
            }
        }
    }
}
