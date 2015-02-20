using System.Collections.Generic;

namespace Fluid
{
    public class Shop
    {
        /// <summary>
        /// Gets the amount of energy
        /// </summary>
        public int Energy { get; set; }

        /// <summary>
        /// Gets the time until next energy
        /// </summary>
        public int TimeToEnergy { get; set; }

        /// <summary>
        /// Gets the total energy
        /// </summary>
        public int TotalEnergy { get; set; }

        /// <summary>
        /// Gets the seconds between energy
        /// </summary>
        public int SecondsBetweenEnergy { get; set; }

        /// <summary>
        /// Gets the list of all shop item's data
        /// </summary>
        public List<ShopItem> ShopItems { get; set; }
    }
}
