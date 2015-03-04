using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace Fluid
{
    [DebuggerDisplay("Count = {Count}")]
    public class PlayerManager
    {
        private ConcurrentDictionary<int, WorldPlayer> m_Players;

        /// <summary>
        /// Gets the amount of players
        /// </summary>
        public int Count { get { return m_Players.Count; } }

        /// <summary>
        /// Trys to add the player
        /// </summary>
        /// <param name="player">The player</param>
        /// <returns>True if added; otherwise false</returns>
        public bool Add(WorldPlayer player)
        {
            return m_Players.TryAdd(player.Id, player);
        }

        /// <summary>
        /// Trys to remove a player
        /// </summary>
        /// <param name="id">The player's id</param>
        /// <returns>True if removed; otherwise false</returns>
        public bool Remove(int id)
        {
            WorldPlayer player = null;
            return m_Players.TryRemove(id, out player);
        }

        /// <summary>
        /// Trys to remove a player
        /// </summary>
        /// <param name="username">The player's username</param>
        /// <returns>True if removed; otherwise false</returns>
        public bool Remove(string username)
        {
            using (IEnumerator<KeyValuePair<int, WorldPlayer>> enumerator = m_Players.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Value.IsConnectedPlayer)
                    {
                        continue;
                    }

                    if (string.Compare(enumerator.Current.Value.Username, username, true) == 0)
                    {
                        return this.Remove(enumerator.Current.Key);
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a player
        /// </summary>
        /// <param name="id">The player's id</param>
        /// <returns>The player if found; otherwise null</returns>
        public WorldPlayer Get(int id)
        {
            WorldPlayer player = null;
            if (m_Players.TryGetValue(id, out player))
            {
                return player;
            }

            return null;
        }

        /// <summary>
        /// Gets a player
        /// </summary>
        /// <param name="username">The player's username</param>
        /// <returns>The player if found; otherwise null</returns>
        /// <param name="includeConnectedPlayer">Whether to include the connected player in the search</param>
        public WorldPlayer Get(string username, bool includeConnectedPlayer = false)
        {
            using (IEnumerator<KeyValuePair<int, WorldPlayer>> enumerator = m_Players.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Value.IsConnectedPlayer && !includeConnectedPlayer)
                    {
                        continue;
                    }

                    if (string.Compare(enumerator.Current.Value.Username, username, true) == 0)
                    {
                        return enumerator.Current.Value;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a player
        /// </summary>
        /// <param name="playerSelector">The player selector</param>
        public WorldPlayer Get(Predicate<WorldPlayer> playerSelector)
        {
            using (IEnumerator<KeyValuePair<int, WorldPlayer>> enumerator = m_Players.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (playerSelector(enumerator.Current.Value))
                    {
                        return enumerator.Current.Value;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the player dictionary
        /// </summary>
        public ConcurrentDictionary<int, WorldPlayer> GetList()
        {
            return m_Players;
        }

        /// <summary>
        /// Gets a player
        /// </summary>
        /// <param name="id">The player's id</param>
        /// <returns>The player if found; otherwise null</returns>
        public WorldPlayer this[int id]
        {
            get { return Get(id); }
        }

        /// <summary>
        /// Gets a player
        /// </summary>
        /// <param name="username">The player's username</param>
        /// <returns>The player if found; otherwise null</returns>
        public WorldPlayer this[string username]
        {
            get { return Get(username); }
        }

        /// <summary>
        /// Creates a new player manager
        /// </summary>
        public PlayerManager()
        {
            m_Players = new ConcurrentDictionary<int, WorldPlayer>();
        }
    }
}
