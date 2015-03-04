using System.Diagnostics;
namespace Fluid
{
    [DebuggerDisplay("ID = {ID}")]
    public class ShopItem : VaultShopItem
    {

        /// <summary>
        /// Gets the amount of energy used
        /// </summary>
        public int EnergyUsed { get; internal set; }

        /// <summary>
        /// Gets the amount of items owned
        /// </summary>
        public int AmountOwned { get; internal set; }

        /// <summary>
        /// Gets if the item is owned in payvault
        /// </summary>
        public bool OwnedInPayvault { get; internal set; }

        internal ShopItem(string id, string type, int energyPrice, int priceOfEnergyPerClick, int energyUsed, int gemsPrice, int amountOwned, int span, string textHeader, string textBody, string bitmapSheet, int bitmapSheetOffset, bool isOnSale, bool isFeatured, bool isClassic, bool isPlayerWorldOnly, bool isNew, bool isDevOnly, bool isGridFeautured, int priceUsd, int minClass, bool reusable, bool ownedInPayvault)
        {
            ID = id;
            Type = type;
            PriceEnergy = energyPrice;
            EnergyPerClick = priceOfEnergyPerClick;
            EnergyUsed = energyUsed;
            PriceCoins = gemsPrice;
            AmountOwned = amountOwned;
            Span = span;
            HeaderText = textHeader;
            BodyText = textBody;
            BitmapSheetID = bitmapSheet;
            BitmapSheetOffset = bitmapSheetOffset;
            OnSale = isOnSale;
            IsFeatured = isFeatured;
            IsClassic = isClassic;
            InPlayerWorldsOnly = isPlayerWorldOnly;
            IsNew = isNew;
            IsDevOnly = isDevOnly;
            IsGridFeatured = isGridFeautured;
            PriceUSD = priceUsd;
            MinimumClass = minClass;
            Reusable = reusable;
            OwnedInPayvault = ownedInPayvault;
        }
    }
}
