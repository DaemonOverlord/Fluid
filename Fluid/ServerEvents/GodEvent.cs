﻿using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The server event for when a player changed god mode
    /// </summary>
    public class GodEvent : IServerEvent
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the player who changed god mode
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
