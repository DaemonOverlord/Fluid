using Fluid.ServerEvents;
using PlayerIOClient;
using System.Collections.Generic;

namespace Fluid.Handlers
{
    public class GetShopHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "getShop", "useEnergy" }; }
        }

        /// <summary>
        /// Processes the shop message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            bool success = false;

            Shop shop = new Shop();
            if (message[0] is bool)
            {
                success = message.GetBoolean(0);
            }
            else if (message[0] is string)
            {
                success = string.Compare(message.GetString(0), "error", true) != 0;
            }

            shop.Energy = message.GetInt(1);
            shop.TimeToEnergy = message.GetInt(2);
            shop.TotalEnergy = message.GetInt(3);
            shop.SecondsBetweenEnergy = message.GetInt(4);

            List<ShopItem> shopItemsData = new List<ShopItem>();

            uint index = 7u;
            while (index < message.Count)
            {
                ShopItem shopItemData = new ShopItem(
                    message.GetString(index),
                    message.GetString(index + 1),
                    message.GetInt(index + 2),
                    message.GetInt(index + 3),
                    message.GetInt(index + 4),
                    message.GetInt(index + 5),
                    message.GetInt(index + 6),
                    message.GetInt(index + 7),
                    message.GetString(index + 8),
                    message.GetString(index + 9),
                    message.GetString(index + 10),
                    message.GetInt(index + 11),
                    message.GetBoolean(index + 12),
                    message.GetBoolean(index + 13),
                    message.GetBoolean(index + 14),
                    message.GetBoolean(index + 15),
                    message.GetBoolean(index + 16),
                    message.GetBoolean(index + 17),
                    message.GetBoolean(index + 18),
                    message.GetInt(index + 19),
                    message.GetInt(index + 20),
                    message.GetBoolean(index + 21),
                    message.GetBoolean(index + 22));

                shopItemsData.Add(shopItemData);
                index += 23u;
            }

            shop.ShopItems = shopItemsData;

            GetShopEvent getShopMessage = new GetShopEvent(shop) { Success = success };
            connectionBase.RaiseServerEvent<GetShopEvent>(getShopMessage);
        }
    }
}
