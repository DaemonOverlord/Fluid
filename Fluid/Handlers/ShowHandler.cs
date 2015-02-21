using Fluid.Blocks;
using Fluid.ServerEvents;
using PlayerIOClient;
using System;
using System.Collections.Generic;

namespace Fluid.Handlers
{
    public class ShowHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "show" }; }
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
                    triggers.Add(new KeyTrigger(key));

                    if (!handled)
                    {
                        worldCon.Keys.SetKeyActive(key);
                    }
                }
            }

            ShowEvent showEvent = new ShowEvent()
            {
                Raw = message,
                Triggers = triggers
            };

            connectionBase.RaiseServerEvent<ShowEvent>(showEvent);
        }
    }
}
