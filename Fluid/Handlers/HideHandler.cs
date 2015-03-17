using Fluid.Room;
using Fluid.ServerEvents;
using PlayerIOClient;
using System;
using System.Collections.Generic;

namespace Fluid.Handlers
{
    public class HideHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "hide" }; }
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

            List<KeyTrigger> triggers = new List<KeyTrigger>();
            for (uint i = 0; i < message.Count; i++)
            {
                if (message[i] is string)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), message.GetString(i), true);
                    if (key != Key.TimeDoor && key != Key.TimeGate)
                    {
                        i++;
                        int userId = message.GetInt(i);
                        triggers.Add(new KeyPress(key, userId));
                    }
                    else
                    {
                        triggers.Add(new KeyTrigger(key));
                    }

                    if (!handled)
                    {
                        worldCon.Keys.SetKeyHidden(key);
                    }
                }
            }

            HideEvent hideEvent = new HideEvent()
            {
                Raw = message,
                Triggers = triggers
            };

            connectionBase.RaiseServerEvent<HideEvent>(hideEvent);
        }
    }
}
