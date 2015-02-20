using PlayerIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluid
{
    public class FluidToolbelt
    {
        /// <summary>
        /// Run's a unsafe action
        /// </summary>
        /// <param name="method">Unsafe method</param>
        public T RunSafe<T>(UnsafeAction<T> method)
        {
            T safeResult = default(T);

            try
            {
                safeResult = method.Invoke();
            }
            catch (Exception)
            {

            }

            return safeResult;
        }

        /// <summary>
        /// Run's a unsafe method and returns the whether successful
        /// </summary>
        /// <param name="method">Unsafe method to run</param>
        /// <returns>True if successful; false otherwise</returns>
        public bool RunSafe(Action method)
        {
            try
            {
                method.Invoke();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the value of a property in a vault item
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="vaultItem">The vault item</param>
        /// <param name="property">The property</param>
        public T GetValueIfExists<T>(VaultItem vaultItem, string property)
        {
            if (vaultItem.Contains(property))
            {
                if (vaultItem[property] is T)
                {
                    return (T)vaultItem[property];
                }
            }

            return default(T);

        }
    }
}
