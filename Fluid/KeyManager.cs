using Fluid.Room;
using Fluid.Room;
using System.Collections.Generic;

namespace Fluid
{
    public class KeyManager
    {
        /// <summary>
        /// Gets the list of keys and if they're active
        /// </summary>
        private Dictionary<Key, bool> m_KeyStates;

        /// <summary>
        /// Checks if a key is hidden
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>True if the key is hidden; otherwise false</returns>
        public bool IsKeyHidden(Key key)
        {
            lock (m_KeyStates)
            {
                if (!m_KeyStates.ContainsKey(key))
                {
                    return true;
                }

                return !m_KeyStates[key];
            }
        }

        /// <summary>
        /// Checks if a key is active
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>True if the key is active; otherwise false</returns>
        public bool IsKeyActive(Key key)
        {
            lock (m_KeyStates)
            {
                if (!m_KeyStates.ContainsKey(key))
                {
                    return false;
                }

                return m_KeyStates[key];
            }
        }

        /// <summary>
        /// Sets the key as hidden
        /// </summary>
        /// <param name="key">The key</param>
        internal void SetKeyHidden(Key key)
        {
            lock (m_KeyStates)
            {
                if (!m_KeyStates.ContainsKey(key))
                {
                    m_KeyStates.Add(key, true);
                    return;
                }

                m_KeyStates[key] = true;
            }
        }

        /// <summary>
        /// Sets the key as active
        /// </summary>
        /// <param name="key">The key</param>
        internal void SetKeyActive(Key key)
        {
            lock (m_KeyStates)
            {
                if (!m_KeyStates.ContainsKey(key))
                {
                    m_KeyStates.Add(key, false);
                    return;
                }

                m_KeyStates[key] = false;
            }
        }

        /// <summary>
        /// Creates a new key manager
        /// </summary>
        public KeyManager()
        {
            m_KeyStates = new Dictionary<Key, bool>();
        }
    }
}
