using Fluid.Blocks;
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
                case BlockID.BlueCoinDoor:
                case BlockID.BlueCoinGate:
                case BlockID.CoinDoor:
                case BlockID.CoinGate:
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
                case BlockID.DeathDoor:
                case BlockID.DeathGate:
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
                case BlockID.SwitchPurple:
                case BlockID.PurpleSwitchDoor:
                case BlockID.PurpleSwitchGate:
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
