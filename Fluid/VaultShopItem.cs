namespace Fluid
{
    public class VaultShopItem
    {
        /// <summary>
        /// Gets the item id
        /// </summary>
        public string ID { get; internal set; }

        /// <summary>
        /// Gets the item type
        /// </summary>
        public string Type { get; internal set; }

        /// <summary>
        /// Gets the price in coins of the item
        /// </summary>
        public int PriceCoins { get; internal set; }

        /// <summary>
        /// Gets whether the item is enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets whether the item is featured
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// Gets the span
        /// </summary>
        public int Span { get; set; }

        /// <summary>
        /// Gets the bitmap sheet offset
        /// </summary>
        public int BitmapSheetOffset { get; set; }

        /// <summary>
        /// Gets the header text
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets the body text
        /// </summary>
        public string BodyText { get; set; }

        /// <summary>
        /// Gets if the grid is featured
        /// </summary>
        public bool IsGridFeatured { get; set; }

        /// <summary>
        /// Gets the price of the item in United States Dollars
        /// </summary>
        public int PriceUSD { get; set; }

        /// <summary>
        /// Gets the price of the item in energy
        /// </summary>
        public int PriceEnergy { get; set; }

        /// <summary>
        /// Gets the amount of energy per click
        /// </summary>
        public int EnergyPerClick { get; set; }

        /// <summary>
        /// Gets if the item is only in player worlds
        /// </summary>
        public bool InPlayerWorldsOnly { get; set; }

        /// <summary>
        /// Gets the minimum magic class
        /// </summary>
        public int MinimumClass { get; set; }

        /// <summary>
        /// Gets if the item is a classic item
        /// </summary>
        public bool IsClassic { get; set; }

        /// <summary>
        /// Gets if the item is on sale
        /// </summary>
        public bool OnSale { get; set; }

        /// <summary>
        /// Gets if the item is beta only
        /// </summary>
        public bool BetaOnly { get; set; }

        /// <summary>
        /// Gets the bitmap sheet identification
        /// </summary>
        public string BitmapSheetID { get; set; }

        /// <summary>
        /// Gets if the item is reusable
        /// </summary>
        public bool Reusable { get; set; }

        /// <summary>
        /// Gets if the item is new
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// Gets if the item is for developers only
        /// </summary>
        public bool IsDevOnly { get; set; }

        /// <summary>
        /// Gets the vault shop item debug message
        /// </summary>
        /// <returns>The vault shop item debug message</returns>
        public override string ToString()
        {
            return string.Format("Id: {0}", Type ?? "null");
        }
    }
}
