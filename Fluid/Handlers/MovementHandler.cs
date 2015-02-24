using Fluid.ServerEvents;
using PlayerIOClient;
using System;

namespace Fluid.Handlers
{
    public class MovementHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "m" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(ConnectionBase connectionBase, Message message, bool handled)
        {
            int userId = message.GetInt(0);
            double x = message.GetDouble(1);
            double y = message.GetDouble(2);
            double speedX = message.GetDouble(3);
            double speedY = message.GetDouble(4);
            int modifierX = message.GetInt(5);
            int modifierY = message.GetInt(6);
            int horizontal = message.GetInt(7);
            int vertical = message.GetInt(8);
            bool spacedown = message.GetBoolean(10);

            WorldConnection worldCon = (WorldConnection)connectionBase;
            WorldPlayer player = worldCon.Players.GetPlayer(userId);

            if (!handled && player != null)
            {
                Input playerInput = 0;

                if (horizontal != player.Horizontal)
                {
                    if (horizontal < 0)
                    {
                        playerInput |= Input.HoldLeft;
                    }
                    else if (horizontal > 0)
                    {
                        playerInput |= Input.HoldRight;
                    }
                    else if (horizontal == 0)
                    {
                        if ((player.LastInput & Input.HoldLeft) != 0)
                        {
                            playerInput |= Input.ReleaseLeft;
                        }
                        else if ((player.LastInput & Input.HoldRight) != 0)
                        {
                            playerInput |= Input.ReleaseRight;
                        }
                    }
                }

                if (vertical != player.Vertical)
                {
                    if (vertical < 0)
                    {
                        playerInput |= Input.HoldUp;
                    }
                    else if (vertical > 0)
                    {
                        playerInput |= Input.HoldDown;
                    }
                    else if (vertical == 0)
                    {
                        if ((player.LastInput & Input.HoldUp) != 0)
                        {
                            playerInput |= Input.ReleaseUp;
                        }
                        else if ((player.LastInput & Input.HoldDown) != 0)
                        {
                            playerInput |= Input.ReleaseDown;
                        }
                    }
                }

                player.X = x;
                player.Y = y;
                player.SpeedX = speedX;
                player.SpeedY = speedY; 
                player.ModifierX = modifierX;
                player.ModifierY = modifierY;
                player.Horizontal = horizontal;
                player.Vertical = vertical;
                player.SpaceDown = spacedown;
                player.LastInput = playerInput;

                worldCon.Physics.ServerUpdate(player);
            }

            MovementEvent movementEvent = new MovementEvent()
            {
                Raw = message,
                Player = player,
                Input = player.LastInput
            };

            connectionBase.RaiseServerEvent<MovementEvent>(movementEvent);
        }
    }
}
