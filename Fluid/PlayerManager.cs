using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Fluid
{
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
        /// <param name="player">The player's id</param>
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
        public WorldPlayer GetPlayer(int id)
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
        public WorldPlayer GetPlayer(string username)
        {
            using (IEnumerator<KeyValuePair<int, WorldPlayer>> enumerator = m_Players.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
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
        public WorldPlayer GetPlayer(Predicate<WorldPlayer> playerSelector)
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
        public ConcurrentDictionary<int, WorldPlayer> GetDictionary()
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
            get { return GetPlayer(id); }
        }

        /// <summary>
        /// Gets a player
        /// </summary>
        /// <param name="username">The player's username</param>
        /// <returns>The player if found; otherwise null</returns>
        public WorldPlayer this[string username]
        {
            get { return GetPlayer(username); }
        }

        /// <summary>
        /// Gets the player manager debug message
        /// </summary>
        public override string ToString()
        {
            return string.Format("Count: {0}", Count);
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
