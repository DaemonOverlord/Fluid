using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluid
{
    public class PotionManager
    {
        private Dictionary<Potion, int> m_Inventory;
        private Dictionary<Potion, bool> m_Enabled;

        /// <summary>
        /// Gets the total amount of potions in the inventory
        /// </summary>
        public int TotalPotionCount 
        {
            get
            {
                lock (m_Inventory)
                {
                    int total = 0;
                    foreach (KeyValuePair<Potion, int> pair in m_Inventory)
                    {
                        total += pair.Value;
                    }

                    return total;
                }
            }
        }

        /// <summary>
        /// Gets whether the world has the potion enabled
        /// </summary>
        /// <param name="potion">The potion</param>
        /// <returns>False if not enabled; otherwise true</returns>
        public bool IsEnabled(Potion potion)
        {
            lock (m_Enabled)
            {
                if (!m_Enabled.ContainsKey(potion))
                {
                    return true;
                }

                return m_Enabled[potion];
            }
        }

        /// <summary>
        /// Sets whether the world has the potion enabled
        /// </summary>
        /// <param name="potion">The potion</param>
        /// <param name="enabled">Whether the potion is enabled</param>
        internal void SetEnabled(Potion potion, bool enabled)
        {
            lock (m_Enabled)
            {
                if (!m_Enabled.ContainsKey(potion))
                {
                    m_Enabled.Add(potion, enabled);
                    return;
                }

                m_Enabled[potion] = enabled;
            }
        }

        /// <summary>
        /// Gets the amount of potions in the inventory
        /// </summary>
        /// <param name="potion">The potion</param>
        /// <returns>The amount of potions</returns>
        public int GetInventoryCount(Potion potion)
        {
            lock (m_Inventory)
            {
                if (!m_Inventory.ContainsKey(potion))
                {
                    return 0;
                }

                return m_Inventory[potion];
            }
        }


        /// <summary>
        /// Sets the amount of potions for a potion
        /// </summary>
        /// <param name="potion">The potion</param>
        /// <param name="count">The count of potions</param>
        internal void SetPotionCount(Potion potion, int count)
        {
            lock (m_Inventory)
            {
                if (m_Inventory.ContainsKey(potion))
                {
                    m_Inventory[potion] = count;
                }
                else
                {
                    m_Inventory.Add(potion, count);
                }
            }
        }

        /// <summary>
        /// Gets the potion manager debug message
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Total Potions: {0}", TotalPotionCount);
        }

        /// <summary>
        /// Creates a new potion manager
        /// </summary>
        public PotionManager()
        {
            m_Inventory = new Dictionary<Potion, int>();
            m_Enabled = new Dictionary<Potion,bool>();
        }
    }
}
