using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.ServerEvents
{
    public class GetShopEvent : IServerEvent
    {
        /// <summary>
        /// Gets if the request was successful
        /// </summary>
        public bool Success { get; internal set; }

        /// <summary>
        /// Gets the shop;
        /// </summary>
        public Shop Shop { get; internal set; }

        /// <summary>
        /// Gets the raw message
        /// </summary>
        public Message Raw { get; set; }

        /// <summary>
        /// Gets the shop
        /// </summary>
        /// <param name="shop">The shop</param>
        internal GetShopEvent(Shop shop)
        {
            Shop = shop;
        }
    }
}
