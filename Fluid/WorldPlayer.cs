using Fluid.Blocks;
using Fluid.Physics;
using PlayerIOClient;
using System;
using System.Collections.Generic;

namespace Fluid
{
    public class WorldPlayer : Player
    {
        private WorldConnection m_Connection;

        internal int m_current;

        internal double m_mox, m_moy;

        internal int m_morx, m_mory;

        internal int m_pastx, m_pasty;

        internal int m_overlapy;

        internal double m_mx, m_my;

        internal bool m_donex, m_doney;

        internal int m_delayed;

        internal bool m_isThrusting = false;

        internal double m_currentThrust = PhysicsEngine.MaxThrust;

        internal bool m_hasLastPortal;

        internal int[] m_queue = new int[PhysicsEngine.QueueLength];

        internal bool[] m_switches = new bool[PhysicsEngine.SwitchIDCount];

        /// <summary>
        /// Gets the player's id
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// Gets the player's face
        /// </summary>
        public FaceID Face { get; internal set; }

        /// <summary>
        /// Gets whether the player is in god mode
        /// </summary>
        public bool InGodMode { get; internal set; }

        /// <summary>
        /// Gets whether the player is in moderator mode
        /// </summary>
        public bool InModMode { get; internal set; }

        /// <summary>
        /// Gets whether the player is in guardian mode
        /// </summary>
        public bool InGuadianMode { get; internal set; }

        /// <summary>
        /// Gets whether the player is a moderator
        /// </summary>
        public bool IsModerator { get; internal set; }

        /// <summary>
        /// Gets whether the player is a guardian
        /// </summary>
        public bool IsGuardian { get; internal set; }

        /// <summary>
        /// Gets whether the player is the connected player
        /// </summary>
        public bool IsConnectedPlayer { get { return m_Connection.Me.Id == Id; } }

        /// <summary>
        /// Gets whether the player has chat
        /// </summary>
        public bool HasChat { get; internal set; }

        /// <summary>
        /// Gets the player's gold coin count
        /// </summary>
        public int GoldCoins { get; internal set; }

        /// <summary>
        /// Gets the collected coins
        /// </summary>
        public List<Block> CollectedGoldCoins { get; internal set; }

        /// <summary>
        /// Gets the player's blue coin count
        /// </summary>
        public int BlueCoins { get; internal set; }

        /// <summary>
        /// Gets the collected coins
        /// </summary>
        public List<Block> CollectedBlueCoins { get; internal set; }

        /// <summary>
        /// Gets whether the player is friends with you
        /// </summary>
        public bool IsFriendsWithYou { get; internal set; }

        /// <summary>
        /// Gets the players magic level
        /// </summary>
        public int MagicLevel { get; internal set; }

        /// <summary>
        /// Gets whether the player has builder's club
        /// </summary>
        public bool HasBuildersClub { get; internal set; }

        /// <summary>
        /// Gets whether the player has the golden crown
        /// </summary>
        public bool HasCrown { get; internal set; }

        /// <summary>
        /// Gets whether the player has a silver crown
        /// </summary>
        public bool HasSilverCrown { get; internal set; }

        /// <summary>
        /// Gets the player's active potions
        /// </summary>
        public Dictionary<Potion, bool> Potions { get; internal set; }

        /// <summary>
        /// Gets the x coordinate
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets the player's block x coordinate
        /// </summary>
        public int BlockX { get { return (int)Math.Round(X / 16); } }

        /// <summary>
        /// Gets the y coordinate
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets the player's block y coordinate
        /// </summary>
        public int BlockY { get { return (int)Math.Round(Y / 16); } }

        /// <summary>
        /// Gets the player's horizontal input
        /// </summary>
        public int Horizontal { get; internal set; }

        /// <summary>
        /// Gets the player's vertical input
        /// </summary>
        public int Vertical { get; internal set; }

        internal double m_speedX;

        /// <summary>
        /// Gets the current speed in the x direction
        /// </summary>
        public double SpeedX { get { return m_speedX * PhysicsEngine.VariableMultiplier; } internal set { m_speedX = value / PhysicsEngine.VariableMultiplier; } }

        internal double m_speedY;

        /// <summary>
        /// Gets the current speed in the y direction
        /// </summary>
        public double SpeedY { get { return m_speedY * PhysicsEngine.VariableMultiplier; } internal set { m_speedY = value / PhysicsEngine.VariableMultiplier; } }

        internal double m_modifierX;

        /// <summary>
        /// Gets the current modifier in the x direction
        /// </summary>
        public double ModifierX { get { return m_modifierX * PhysicsEngine.VariableMultiplier; } internal set { m_modifierX = value / PhysicsEngine.VariableMultiplier; } }

        internal double m_modifierY;

        /// <summary>
        /// Gets the current modifier in the y direction
        /// </summary>
        public double ModifierY { get { return m_modifierY * PhysicsEngine.VariableMultiplier; } internal set { m_modifierY = value / PhysicsEngine.VariableMultiplier; } }

        /// <summary>
        /// Gets the last checkpoint
        /// </summary>
        public Block LastCheckpoint { get; internal set; }

        /// <summary>
        /// Gets the current block
        /// </summary>
        public BlockID Current { get { return (BlockID)m_current; } }

        /// <summary>
        /// Gets whether the player is dead
        /// </summary>
        public bool IsDead { get; internal set; }

        internal int m_deaths = 0;

        /// <summary>
        /// Gets the amount of deaths the player has
        /// </summary>
        public int Deaths { get { return m_deaths; } }

        /// <summary>
        /// Gets whether the player is hoolding down space
        /// </summary>
        public bool SpaceDown { get; internal set; }

        /// <summary>
        /// Gets whether the player has levitation
        /// </summary>
        public bool HasLevitation { get { return HasPotionActive(Potion.Levitation); } }

        /// <summary>
        /// Gets whether the player has protection
        /// </summary>
        public bool HasProtection { get { return HasPotionActive(Potion.Protection); } }

        /// <summary>
        /// Checks if the player is in god, guardian, or moderator mode
        /// </summary>
        public bool IsGod()
        {
            return InGodMode || InGuadianMode || InModMode;
        }

        /// <summary>
        /// Checks if a player has a potion active
        /// </summary>
        /// <param name="potion">The potion</param>
        public bool HasPotionActive(Potion potion)
        {
            if (Potions.ContainsKey(potion))
            {
                return Potions[potion];
            }

            return false;
        }

        /// <summary>
        /// Gets the player's speed multiplier
        /// </summary>
        public double SpeedMultiplier
        {
            get
            {
                double value = 1;
                if (HasPotionActive(Potion.Zombie))
                {
                    value *= 0.6;
                }

                if (HasPotionActive(Potion.Speed))
                {
                    value *= 1.9;
                }

                return value;
            }
        }

        /// <summary>
        /// Gets the block location of the player
        /// </summary>
        /// <returns>The block location</returns>
        public Vector GetLocation()
        {
            return new Vector(BlockX, BlockY);
        }

        /// <summary>
        /// Respawns the player
        /// </summary>
        internal void Respawn()
        {
            ModifierX = 0;
            ModifierY = 0;
            SpeedX = 0;
            SpeedY = 0;
            IsDead = false;
        }

        /// <summary>
        /// Resets the player
        /// </summary>
        internal void Reset()
        {
            lock (CollectedGoldCoins) { CollectedGoldCoins.Clear(); }
            lock (CollectedBlueCoins) { CollectedBlueCoins.Clear(); }

            m_deaths = 0;
        }

        /// <summary>
        /// Kills the player internally
        /// </summary>
        internal void KillPlayerInternal()
        {
            m_deaths++;
            IsDead = true;
        }

        /// <summary>
        /// Sets a potion activity
        /// </summary>
        /// <param name="potion">The potion</param>
        /// <param name="active">The activity</param>
        internal void SetPotion(Potion potion, bool active)
        {
            Potions[potion] = active;
        }

        /// <summary>
        /// Sets the player's location without sending the location to the server
        /// </summary>
        /// <param name="loc">The location vector</param>
        internal void SetLocationInternal(Vector loc)
        {
            X = loc.X;
            Y = loc.Y;
        }

        /// <summary>
        /// Creates a new 
        /// </summary>
        /// <param name="client">The Fluid client</param>
        /// <param name="username">The username</param>
        /// <param name="id">The id</param>
        public WorldPlayer(WorldConnection connection, string username, int id) : base(connection.Client, username)
        {
            this.m_Connection = connection;
            Id = id;
    
            CollectedGoldCoins = new List<Block>();
            CollectedBlueCoins = new List<Block>();
            Potions = new Dictionary<Potion, bool>();

            Potion[] potiontypes = (Potion[])Enum.GetValues(typeof(Potion));
            for (int i = 0; i < potiontypes.Length; i++)
            {
                Potions.Add(potiontypes[i], false);
            }
        }
    }
}
