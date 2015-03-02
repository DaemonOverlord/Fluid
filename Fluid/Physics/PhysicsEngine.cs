using Fluid.Blocks;
using Fluid.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Fluid.Physics
{
    public class PhysicsEngine
    {
        private WorldConnection m_WorldConnection;
        private Thread physicsThread;

        private bool m_Running = false;

        /// <summary>
        /// Gets the milliseconds per tick
        /// </summary>
        public const int MsPerTick = 10;

        /// <summary>
        /// Gets the amount of switch id
        /// </summary>
        public const int SwitchIDCount = 100;

        /// <summary>
        /// Gets the variable multiplier
        /// </summary>
        public const double VariableMultiplier = 7.752;

        /// <summary>
        /// Gets the base drag
        /// </summary>
        public static readonly double BaseDrag = (Math.Pow(0.9981, MsPerTick) * 1.00016093);

        /// <summary>
        /// Gets the no modifier drag
        /// </summary>
        public static readonly double NoModifierDrag = (Math.Pow(0.99, MsPerTick) * 1.00016093);

        /// <summary>
        /// Gets the water drag
        /// </summary>
        public static readonly double WaterDrag = (Math.Pow(0.995, MsPerTick) * 1.00016093);

        /// <summary>
        /// Gets the mud drag
        /// </summary>
        public static readonly double MudDrag = (Math.Pow(0.975, MsPerTick) * 1.00016093);

        /// <summary>
        /// Gets the jump multiplier
        /// </summary>
        public const double JumpHeight = 26;

        /// <summary>
        /// Gets the gravity arrows multiplier
        /// </summary>
        public const int Gravity = 2;

        /// <summary>
        /// Gets the maximum thrust
        /// </summary>
        public const double MaxThrust = 0.2;

        /// <summary>
        /// Gets the thrust burn off
        /// </summary>
        public const double ThrustBurnOff = 0.01;

        /// <summary>
        /// Gets the boost multiplier
        /// </summary>
        public const double Boost = 16;

        /// <summary>
        /// Gets the watyer buoyancy
        /// </summary>
        public const double WaterBuoyancy = -0.5;

        /// <summary>
        /// Gets the mud buoyancy
        /// </summary>
        public const double MudBuoyancy = 0.4;

        /// <summary>
        /// Gets the physics queue array length
        /// </summary>
        public const int QueueLength = 2;

        /// <summary>
        /// Gets the physics portal multiplier
        /// </summary>
        public const double PortalMultiplier = 1.42;

        /// <summary>
        /// Gets or sets the tick mode
        /// </summary>
        public TickMode TickMode { get; set; }

        /// <summary>
        /// Gets or sets the physics event mode
        /// </summary>
        public PhysicsEventMode EventMode { get; set; }

        /// <summary>
        /// Gets the current world
        /// </summary>
        public World World { get { return m_WorldConnection.World; } }

        /// <summary>
        /// Creates a physics thread
        /// </summary>
        private void CreateThread()
        {
            physicsThread = new Thread(Update);
            physicsThread.Name = "Fluid Physics";
            physicsThread.Priority = ThreadPriority.AboveNormal;
            physicsThread.IsBackground = true;
        }

        /// <summary>
        /// Updates all players
        /// </summary>
        private void Update()
        {
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                while (m_Running)
                {
                    if (m_WorldConnection == null)
                    {
                        break;
                    }
                    else if (!m_WorldConnection.Connected)
                    {
                        break;
                    }

                    long frameStartTime = sw.ElapsedMilliseconds;
                    foreach (KeyValuePair<int, WorldPlayer> playerNode in m_WorldConnection.Players.GetList())
                    {
                        if (playerNode.Value.IsConnectedPlayer)
                        {
                            this.Tick(playerNode.Value);
                        }

                        PhysicsUpdateEvent updateEvent = new PhysicsUpdateEvent()
                        {
                            Player = playerNode.Value
                        };

                        m_WorldConnection.RaiseEventAsync<PhysicsUpdateEvent>(updateEvent);
                    }

                    long frameEndTime = sw.ElapsedMilliseconds;
                    long waitTime = MsPerTick - (frameEndTime - frameStartTime);
                    if (waitTime > 0)
                    {
                        Thread.Sleep((int)waitTime);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                return;
                //Thread is being aborted
            }
            catch (Exception ex)
            {
                return;
            }

            return;
        }

        /// <summary>
        /// Gets whether a block is climbable
        /// </summary>
        /// <param name="blockId">The block</param>
        /// <returns>True if climbable; otherwise false</returns>
        public bool IsClimbable(BlockID blockId)
        {
            switch (blockId)
            {
                case BlockID.LadderNinja:
                case BlockID.LadderCastle:
                case BlockID.LadderJungleHorizontal:
                case BlockID.LadderJungleVertical:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets whether a block is solid
        /// </summary>
        /// <param name="blockId">The block id</param>
        public bool IsSolid(BlockID blockId)
        {
            int id = (int)blockId;
            return (9 <= id && id <= 97) || (122 <= id && id <= 217) || (1001 <= id && id <= 1026);
        }

        /// <summary>
        /// Gets whether a block can be jumped through
        /// </summary>
        /// <param name="blockId">The block</param>
        /// <returns>True if you can jump through; otherwise false</returns>
        public bool CanJumpThrough(BlockID blockId)
        {
            switch (blockId)
            {
                case (BlockID)61:
                case (BlockID)62:
                case (BlockID)63:
                case (BlockID)64:
                case (BlockID)89:
                case (BlockID)90:
                case (BlockID)91:
                case (BlockID)96:
                case (BlockID)97:
                case (BlockID)122:
                case (BlockID)123:
                case (BlockID)124:
                case (BlockID)125:
                case (BlockID)126:
                case (BlockID)127:
                case (BlockID)146:
                case (BlockID)154:
                case (BlockID)158:
                case (BlockID)194:
                case (BlockID)211:
                case (BlockID)216:
                case BlockID.OneWayCyan:
                case BlockID.OneWayRed:
                case BlockID.OneWayYellow:
                case BlockID.OneWayPink:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets whether a number is approaching zero
        /// </summary>
        /// <param name="number">The number</param>
        internal bool ApproachingZero(double number)
        {
            return Math.Abs(number) < 0.00000001;
        }

        /// <summary>
        /// Processes the portal physiscs
        /// </summary>
        /// <param name="worldPlayer">The player</param>
        /// <param name="remainderX">The x remainder</param>
        /// <param name="currentSX">The current speed in the x direction</param>
        /// <param name="remainderY">The y remainder</param>
        /// <param name="currentSY">The current speed in the y direction</param>
        private void ProcessPortals(WorldPlayer worldPlayer, ref double remainderX, ref double currentSX, ref double remainderY, ref double currentSY)
        {
            int cx = ((int)(worldPlayer.X + 8) >> 4);
            int cy = ((int)(worldPlayer.Y + 8) >> 4);

            Block on = World[cx, cy, Layer.Foreground];

            worldPlayer.m_current = (int)on.ID;
            if (!worldPlayer.IsGod() && (worldPlayer.Current == BlockID.Portal || worldPlayer.Current == BlockID.InvisiblePortal))
            {
                Portal portal = (Portal)on;

                if (!worldPlayer.m_hasLastPortal)
                {
                    worldPlayer.m_hasLastPortal = true;

                    Portal target = World.GetPortalByID(portal.Target);
                    if (target != null)
                    {
                        uint rot1 = (uint)portal.Rotation;
                        uint rot2 = (uint)target.Rotation;

                        if (rot1 < rot2)
                        {
                            rot1 += 4;
                        }
                        switch (rot1 - rot2)
                        {
                            case 1:
                                worldPlayer.SpeedX = worldPlayer.SpeedY * PortalMultiplier;
                                worldPlayer.SpeedY = -worldPlayer.SpeedX * PortalMultiplier;
                                worldPlayer.ModifierX = worldPlayer.ModifierY * PortalMultiplier;
                                worldPlayer.ModifierY = -worldPlayer.ModifierX * PortalMultiplier;
                                remainderY = -remainderY;
                                currentSY = -currentSY;
                                break;
                            case 2:
                                worldPlayer.SpeedX = -worldPlayer.SpeedX * PortalMultiplier;
                                worldPlayer.SpeedY = -worldPlayer.SpeedY * PortalMultiplier;
                                worldPlayer.ModifierX = -worldPlayer.ModifierX * PortalMultiplier;
                                worldPlayer.ModifierY = -worldPlayer.ModifierY * PortalMultiplier;
                                remainderY = -remainderY;
                                currentSY = -currentSY;
                                remainderX = -remainderX;
                                currentSX = -currentSX;
                                break;
                            case 3:
                                worldPlayer.SpeedX = -worldPlayer.SpeedY * PortalMultiplier;
                                worldPlayer.SpeedY = worldPlayer.SpeedX * PortalMultiplier;
                                worldPlayer.ModifierX = -worldPlayer.ModifierY * PortalMultiplier;
                                worldPlayer.ModifierY = worldPlayer.ModifierX * PortalMultiplier;
                                remainderX = -remainderX;
                                currentSX = -currentSX;
                                break;
                        }

                        worldPlayer.X = target.X * 16;
                        worldPlayer.Y = target.Y * 16;
                    }
                }
            }
            else
            {
                worldPlayer.m_hasLastPortal = false;
            }
        }

        /// <summary>
        /// Processes the x step
        /// </summary>
        /// <param name="worldPlayer">The player</param>
        /// <param name="remainderX">The x remainder</param>
        /// <param name="currentSX">The current speed in the x direction</param>
        /// <param name="remainderY">The y remainder</param>
        /// <param name="currentSY">The current speed in the y direction</param>
        private void StepX(WorldPlayer worldPlayer, ref double remainderX, ref double currentSX, ref double remainderY, ref double currentSY)
        {
            double ox = worldPlayer.X;
            double osx = currentSX;

            if (currentSX > 0)
            {
                if ((currentSX + remainderX) >= 1)
                {
                    worldPlayer.X += 1 - remainderX;
                    worldPlayer.X = Math.Floor(worldPlayer.X);
                    currentSX -= 1 - remainderX;
                    remainderX = 0;
                }
                else
                {
                    worldPlayer.X += currentSX;
                    currentSX = 0;
                }
            }
            else
            {
                if (currentSX < 0)
                {
                    if (!ApproachingZero(remainderX) && (remainderX + currentSX) < 0)
                    {
                        currentSX += remainderX;
                        worldPlayer.X -= remainderX;
                        worldPlayer.X = Math.Floor(worldPlayer.X);
                        remainderX = 1;
                    }
                    else
                    {
                        worldPlayer.X += currentSX;
                        currentSX = 0;
                    }
                }
            }

            if (Overlaps(worldPlayer))
            {
                worldPlayer.X = ox;
                worldPlayer.m_speedX = 0;
                currentSX = osx;
                worldPlayer.m_donex = true;
            }
        }

        /// <summary>
        /// Processes the y step
        /// </summary>
        /// <param name="worldPlayer">The player</param>
        /// <param name="remainderX">The x remainder</param>
        /// <param name="currentSX">The current speed in the x direction</param>
        /// <param name="remainderY">The y remainder</param>
        /// <param name="currentSY">The current speed in the y direction</param>
        private void StepY(WorldPlayer worldPlayer, ref double remainderX, ref double currentSX, ref double remainderY, ref double currentSY)
        {
            double oy = worldPlayer.Y;
            double osy = currentSY;

            if (currentSY > 0)
            {
                if ((currentSY + remainderY) >= 1)
                {
                    worldPlayer.Y += 1 - remainderY;
                    worldPlayer.Y = Math.Floor(worldPlayer.Y);
                    currentSY -= 1 - remainderY;
                    remainderY = 0;
                }
                else
                {
                    worldPlayer.Y += currentSY;
                    currentSY = 0;
                }
            }
            else
            {
                if (currentSY < 0)
                {
                    if (!ApproachingZero(remainderY) && (remainderY + currentSY) < 0)
                    {
                        worldPlayer.Y -= remainderY;
                        worldPlayer.Y = Math.Floor(worldPlayer.Y);
                        currentSY += remainderY;
                        remainderY = 1;
                    }
                    else
                    {
                        worldPlayer.Y += currentSY;
                        currentSY = 0;
                    }
                }
            }

            if (Overlaps(worldPlayer))
            {
                worldPlayer.Y = oy;
                worldPlayer.m_speedY = 0;
                currentSY = osy;
                worldPlayer.m_doney = true;
            }
        }

        /// <summary>
        /// Updates a players thrust
        /// </summary>
        /// <param name="worldPlayer">The world player</param>
        private void UpdateThrust(WorldPlayer worldPlayer)
        {
            if (worldPlayer.m_mory != 0)
            {
                worldPlayer.SpeedY -= (worldPlayer.m_currentThrust * (JumpHeight / 2)) * (worldPlayer.m_mory * 0.5);
            }

            if (worldPlayer.m_morx != 0)
            {
                worldPlayer.SpeedX -= (worldPlayer.m_currentThrust * (JumpHeight / 2)) * (worldPlayer.m_morx * 0.5);
            }

            if (!worldPlayer.m_isThrusting)
            {
                if (worldPlayer.m_currentThrust > 0)
                {
                    worldPlayer.m_currentThrust -= ThrustBurnOff;
                }
                else
                {
                    worldPlayer.m_currentThrust = 0;
                }
            }
        }

        /// <summary>
        /// Peforms a physics tick
        /// </summary>
        /// <param name="worldPlayer">The player</param>
        private void Tick(WorldPlayer worldPlayer)
        {
            int cx = ((int)(worldPlayer.X + 8) >> 4);
            int cy = ((int)(worldPlayer.Y + 8) >> 4);

            worldPlayer.m_current = (int)World[cx, cy, Layer.Foreground].ID;
            if (worldPlayer.Current == (BlockID)4 || IsClimbable(worldPlayer.Current))
            {
                worldPlayer.m_delayed = worldPlayer.m_queue[1];
                worldPlayer.m_queue[0] = worldPlayer.m_current;
            }
            else
            {
                worldPlayer.m_delayed = worldPlayer.m_queue[0];
                worldPlayer.m_queue[0] = worldPlayer.m_queue[1];
            }

            worldPlayer.m_queue[1] = worldPlayer.m_current;

            if (worldPlayer.IsDead)
            {
                worldPlayer.Horizontal = 0;
                worldPlayer.Vertical = 0;
            }

            bool isGodMode = worldPlayer.IsGod();
            if (isGodMode)
            {
                worldPlayer.m_morx = 0;
                worldPlayer.m_mory = 0;
                worldPlayer.m_mox = 0;
                worldPlayer.m_moy = 0;
            }
            else
            {
                switch (worldPlayer.Current)
                {
                    case BlockID.GravityLeft:
                    case BlockID.InvisibleGravityLeft:
                        worldPlayer.m_morx = -Gravity;
                        worldPlayer.m_mory = 0;
                        break;
                    case BlockID.GravityUp:
                    case BlockID.InvisibleGravityUp:
                        worldPlayer.m_morx = 0;
                        worldPlayer.m_mory = -Gravity;
                        break;
                    case BlockID.GravityRight:
                    case BlockID.InvisibleGravityRight:
                        worldPlayer.m_morx = Gravity;
                        worldPlayer.m_mory = 0;
                        break;
                    case BlockID.BoostLeft:
                    case BlockID.BoostRight:
                    case BlockID.BoostUp:
                    case BlockID.BoostDown:
                    case BlockID.LadderNinja:
                    case BlockID.LadderCastle:
                    case BlockID.LadderJungleHorizontal:
                    case BlockID.LadderJungleVertical:
                    case BlockID.InvisibleGravityDot:
                    case BlockID.GravityDot:
                        worldPlayer.m_morx = 0;
                        worldPlayer.m_mory = 0;
                        break;
                    case BlockID.Water:
                        worldPlayer.m_morx = Gravity;
                        worldPlayer.m_mory = (int)WaterBuoyancy;
                        break;
                    case BlockID.Mud:
                        worldPlayer.m_morx = Gravity;
                        worldPlayer.m_mory = (int)MudBuoyancy;
                        break;
                    case BlockID.HazardFire:
                    case BlockID.HazardSpike:
                        if (!worldPlayer.IsDead && !worldPlayer.HasProtection)
                        {
                            worldPlayer.KillPlayerInternal();
                        }
                        break;
                    default:
                        worldPlayer.m_morx = 0;
                        worldPlayer.m_mory = Gravity;
                        break;
                }

                switch ((BlockID)worldPlayer.m_delayed)
                {
                    case BlockID.GravityLeft:
                    case BlockID.InvisibleGravityLeft:
                        worldPlayer.m_mox = -Gravity;
                        worldPlayer.m_moy = 0;
                        break;
                    case BlockID.GravityUp:
                    case BlockID.InvisibleGravityUp:
                        worldPlayer.m_mox = 0;
                        worldPlayer.m_moy = -Gravity;
                        break;
                    case BlockID.GravityRight:
                    case BlockID.InvisibleGravityRight:
                        worldPlayer.m_mox = Gravity;
                        worldPlayer.m_moy = 0;
                        break;
                    case BlockID.BoostLeft:
                    case BlockID.BoostRight:
                    case BlockID.BoostUp:
                    case BlockID.BoostDown:
                    case BlockID.LadderNinja:
                    case BlockID.LadderCastle:
                    case BlockID.LadderJungleHorizontal:
                    case BlockID.LadderJungleVertical:
                    case BlockID.InvisibleGravityDot:
                    case BlockID.GravityDot:
                        worldPlayer.m_mox = 0;
                        worldPlayer.m_moy = 0;
                        break;
                    case BlockID.Water:
                        worldPlayer.m_mox = Gravity;
                        worldPlayer.m_moy = (int)WaterBuoyancy;
                        break;
                    case BlockID.Mud:
                        worldPlayer.m_mox = Gravity;
                        worldPlayer.m_moy = (int)MudBuoyancy;
                        break;
                    default:
                        worldPlayer.m_mox = 0;
                        worldPlayer.m_moy = Gravity;
                        break;
                }
            }

            if (worldPlayer.m_moy == WaterBuoyancy || worldPlayer.m_moy == MudBuoyancy)
            {
                worldPlayer.m_mx = worldPlayer.Horizontal;
                worldPlayer.m_my = worldPlayer.Vertical;
            }
            else if (worldPlayer.m_moy != 0)
            {
                worldPlayer.m_mx = worldPlayer.Horizontal;
                worldPlayer.m_my = 0;
            }
            else if (worldPlayer.m_mox != 0)
            {
                worldPlayer.m_mx = 0;
                worldPlayer.m_my = worldPlayer.Vertical;
            }
            else
            {
                worldPlayer.m_mx = worldPlayer.Horizontal;
                worldPlayer.m_my = worldPlayer.Vertical;
            }

            worldPlayer.m_mx *= worldPlayer.SpeedMultiplier;
            worldPlayer.m_my *= worldPlayer.SpeedMultiplier;
            worldPlayer.m_mox *= World.Gravity;
            worldPlayer.m_moy *= World.Gravity;

            worldPlayer.ModifierX = worldPlayer.m_mox + worldPlayer.m_mx;
            worldPlayer.ModifierY = worldPlayer.m_moy + worldPlayer.m_my;

            if (!ApproachingZero(worldPlayer.m_speedX) || worldPlayer.m_modifierX != 0)
            {
                worldPlayer.m_speedX += worldPlayer.m_modifierX;
                worldPlayer.m_speedX *= BaseDrag;
                if ((worldPlayer.m_mx == 0 && worldPlayer.m_moy != 0) ||
                    (worldPlayer.m_speedX < 0 && worldPlayer.m_mx > 0) ||
                    (worldPlayer.m_speedX > 0 && worldPlayer.m_mx < 0) ||
                    (IsClimbable(worldPlayer.Current) && !isGodMode))
                {
                    worldPlayer.m_speedX *= NoModifierDrag;
                }
                else if (worldPlayer.Current == BlockID.Water && !isGodMode)
                {
                    worldPlayer.m_speedX *= WaterDrag;
                }
                else if (worldPlayer.Current == BlockID.Mud && !isGodMode)
                {
                    worldPlayer.m_speedX *= MudDrag;
                }

                if (worldPlayer.m_speedX > 16)
                {
                    worldPlayer.m_speedX = 16;
                }
                else if (worldPlayer.m_speedX < -16)
                {
                    worldPlayer.m_speedX = -16;
                }
                else if (worldPlayer.m_speedX < 0.0001 && worldPlayer.m_speedX > -0.0001)
                {
                    worldPlayer.m_speedX = 0;
                }
            }
            if (!ApproachingZero(worldPlayer.m_speedY) || worldPlayer.m_modifierY != 0)
            {
                worldPlayer.m_speedY += worldPlayer.m_modifierY;
                worldPlayer.m_speedY *= BaseDrag;
                if ((worldPlayer.m_my == 0 && worldPlayer.m_mox != 0) ||
                    (worldPlayer.m_speedY < 0 && worldPlayer.m_my > 0) ||
                    (worldPlayer.m_speedY > 0 && worldPlayer.m_my < 0) ||
                    (IsClimbable(worldPlayer.Current) && !isGodMode))
                {
                    worldPlayer.m_speedY *= NoModifierDrag;
                }
                else if (worldPlayer.Current == BlockID.Water && !isGodMode) 
                {
                    worldPlayer.m_speedY *= WaterDrag;
                }
                else if (worldPlayer.Current == BlockID.Mud && !isGodMode)
                {
                    worldPlayer.m_speedY *= MudDrag;
                }

                if (worldPlayer.m_speedY > 16)
                {
                    worldPlayer.m_speedY = 16;
                }
                else if (worldPlayer.m_speedY < -16)              
                {
                    worldPlayer.m_speedY = -16;
                }
                else if (worldPlayer.m_speedY < 0.0001 && worldPlayer.m_speedY > -0.0001)
                {
                    worldPlayer.m_speedY = 0;
                }
            }
            if (!isGodMode)
            {
                switch (worldPlayer.Current)
                {
                    case BlockID.BoostLeft:
                        worldPlayer.m_speedX = -Boost;
                        break;
                    case BlockID.BoostRight:
                        worldPlayer.m_speedX = Boost;
                        break;
                    case BlockID.BoostUp:
                        worldPlayer.m_speedY = -Boost;
                        break;
                    case BlockID.BoostDown:
                        worldPlayer.m_speedY = Boost;
                        break;
                }
            }

            double remainderX = worldPlayer.X % 1;
            double currentSX = worldPlayer.m_speedX;
            double remainderY = worldPlayer.Y % 1;
            double currentSY = worldPlayer.m_speedY;
            worldPlayer.m_donex = false;
            worldPlayer.m_doney = false;

            while ((currentSX != 0 && !worldPlayer.m_donex) || (currentSY != 0 && !worldPlayer.m_doney))
            {
                ProcessPortals(worldPlayer, ref remainderX, ref currentSX, ref remainderY, ref currentSY);

                StepX(worldPlayer, ref remainderX, ref currentSX, ref remainderY, ref currentSY);
                StepY(worldPlayer, ref remainderX, ref currentSX, ref remainderY, ref currentSY);
            }

            if (!worldPlayer.IsDead)
            {
                if (worldPlayer.SpaceDown)
                {
                    if (worldPlayer.HasLevitation)
                    {
                        worldPlayer.m_isThrusting = true;
                        worldPlayer.m_currentThrust = MaxThrust;
                    }
                }
                else if (worldPlayer.HasLevitation)
                {
                    worldPlayer.m_isThrusting = false;
                }

                bool coinsChanged = false;
                switch (worldPlayer.Current)
                {
                    case BlockID.CoinGold:
                        lock (worldPlayer.CollectedGoldCoins)
                        {                            
                            bool alreadyCollected = false;
                            for (int i = 0; i < worldPlayer.CollectedGoldCoins.Count; i++)
                            {
                                if (worldPlayer.CollectedGoldCoins[i].X == cx &&
                                    worldPlayer.CollectedGoldCoins[i].Y == cy)
                                {
                                    alreadyCollected = true;
                                    break;
                                }
                            }

                            if (!alreadyCollected)
                            {
                                coinsChanged = true;
                                worldPlayer.CollectedGoldCoins.Add(World[cx, cy, Layer.Foreground]);
                            }
                        }
                        break;
                    case BlockID.CoinBlue:
                        lock (worldPlayer.CollectedBlueCoins)
                        {
                            
                            bool alreadyCollected = false;
                            for (int i = 0; i < worldPlayer.CollectedBlueCoins.Count; i++)
                            {
                                if (worldPlayer.CollectedBlueCoins[i].X == cx &&
                                    worldPlayer.CollectedBlueCoins[i].Y == cy)
                                {
                                    alreadyCollected = true;
                                    break;
                                }
                            }

                            if (!alreadyCollected)
                            {
                                coinsChanged = true;
                                worldPlayer.CollectedBlueCoins.Add(World[cx, cy, Layer.Foreground]);
                            }
                        }
                        break;
                }

                if (worldPlayer.IsConnectedPlayer && EventMode == PhysicsEventMode.Send)
                {
                    switch (worldPlayer.Current)
                    {
                        case BlockID.Crown:
                            m_WorldConnection.GetCrown();
                            break;
                        case BlockID.KeyRed:
                            m_WorldConnection.ActivateKey(Key.Red);
                            break;
                        case BlockID.KeyGreen:
                            m_WorldConnection.ActivateKey(Key.Green);
                            break;
                        case BlockID.KeyBlue:
                            m_WorldConnection.ActivateKey(Key.Blue);
                            break;
                        case BlockID.KeyCyan:
                            m_WorldConnection.ActivateKey(Key.Cyan);
                            break;
                        case BlockID.KeyMagenta:
                            m_WorldConnection.ActivateKey(Key.Magenta);
                            break;
                        case BlockID.KeyYellow:
                            m_WorldConnection.ActivateKey(Key.Yellow);
                            break;
                        case BlockID.Diamond:
                            m_WorldConnection.TouchDiamond(cx, cy);
                            break;
                        case BlockID.Cake:
                            m_WorldConnection.TouchCake(cx, cy);
                            break;
                        case BlockID.Hologram:
                            m_WorldConnection.TouchHologram(cx, cy);
                            break;
                        case BlockID.ToolCheckpoint:
                            m_WorldConnection.TouchCheckpoint(cx, cy);
                            break;
                        case BlockID.ToolWinTrophy:
                            m_WorldConnection.GetSilverCrown();
                            break;
                    }

                    switch (worldPlayer.Current)
                    {
                        case BlockID.SwitchPurple:
                            if (World[cx, cy, Layer.Foreground] is PurpleBlock)
                            {
                                PurpleBlock block = (PurpleBlock)World[cx, cy, Layer.Foreground];
                                worldPlayer.m_switches[block.SwitchID] = !worldPlayer.m_switches[block.SwitchID];
                            }
                            break;
                        case BlockID.ToolCheckpoint:
                            if (!isGodMode)
                            {
                                worldPlayer.LastCheckpoint = World[cx, cy, Layer.Foreground];
                            }
                            break;
                    }

                    worldPlayer.m_pastx = cx;
                    worldPlayer.m_pasty = cy;
                }

                if (worldPlayer.HasLevitation)
                {
                    UpdateThrust(worldPlayer);
                }

                if (worldPlayer.IsConnectedPlayer && coinsChanged && EventMode == PhysicsEventMode.Send)
                {
                    m_WorldConnection.SendMessage("c", worldPlayer.GoldCoins, worldPlayer.BlueCoins, cx, cy);
                }
            }

            var imx = ((int)worldPlayer.m_speedX << 8);
            var imy = ((int)worldPlayer.m_speedY << 8);

            if (worldPlayer.Current != BlockID.Water && worldPlayer.Current != BlockID.Mud)
            {
                if (imx == 0)
                {
                    if (worldPlayer.m_modifierX < 0.1 && worldPlayer.m_modifierX > -0.1)
                    {
                        double tx = worldPlayer.X % 16;
                        if (tx < 2)
                        {
                            if (tx < 0.2)
                            {
                                worldPlayer.X = Math.Floor(worldPlayer.X);
                            }
                            else
                            {
                                worldPlayer.X -= tx / 15;
                            }
                        }
                        else
                        {
                            if (tx > 14)
                            {
                                if (tx > 15.8)
                                {
                                    worldPlayer.X = Math.Ceiling(worldPlayer.X);
                                }
                                else
                                {
                                    worldPlayer.X += (tx - 14) / 15;
                                }
                            }
                        }
                    }
                }

                if (imy == 0)
                {
                    if (worldPlayer.m_modifierY < 0.1 && worldPlayer.m_modifierY > -0.1)
                    {
                        double ty = worldPlayer.Y % 16;
                        if (ty < 2)
                        {
                            if (ty < 0.2)
                            {
                                worldPlayer.Y = Math.Floor(worldPlayer.Y);
                            }
                            else
                            {
                                worldPlayer.Y -= ty / 15;
                            }
                        }
                        else
                        {
                            if (ty > 14)
                            {
                                if (ty > 15.8)
                                {
                                    worldPlayer.Y = Math.Ceiling(worldPlayer.Y);
                                }
                                else
                                {
                                    worldPlayer.Y += (ty - 14) / 15;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tests if the player is overlapping a block
        /// </summary>
        /// <param name="p">The player</param>
        private bool Overlaps(WorldPlayer p)
        {
            if ((p.X < 0 || p.Y < 0) || ((p.X > World.Width * 16 - 16) || (p.Y > World.Height * 16 - 16)))
            {
                return true;
            }

            if (p.IsGod())
            {
                return false;
            }

            var firstX = ((int)p.X >> 4);
            var firstY = ((int)p.Y >> 4);
            double lastX = (p.X + 16) / 16;
            double lastY = (p.Y + 16) / 16;
            bool skip = false;

            int x;
            int y = firstY;

            int a = firstY;
            int b;
            while (y < lastY)
            {
                x = firstX;
                b = firstX;
                for (; x < lastX; x++)
                {
                    Block block = World[x, y, Layer.Foreground];
                    BlockID tileId = block.ID;

                    if (IsSolid(tileId))
                    {
                        if (CanJumpThrough(tileId))
                        {
                            uint rot = 0;
                            if (block is RotatableBlock)
                            {
                                RotatableBlock rotatableBlock = (RotatableBlock)block;
                                rot = (uint)rotatableBlock.Rotation;
                            }

                            if (tileId == BlockID.OneWayCyan || tileId == BlockID.OneWayPink || tileId == BlockID.OneWayRed || tileId == BlockID.OneWayYellow)
                            {
                                if ((p.SpeedY < 0 || a <= p.m_overlapy) && rot == 1)
                                {
                                    if (a != firstY || p.m_overlapy == -1)
                                    {
                                        p.m_overlapy = a;
                                    }

                                    skip = true;
                                    continue;
                                }

                                if ((p.SpeedX > 0 || b <= p.m_overlapy) && rot == 2)
                                {
                                    if (b == firstX || p.m_overlapy == -1)
                                    {
                                        p.m_overlapy = b;
                                    }

                                    skip = true;
                                    continue;
                                }

                                if ((p.SpeedY > 0 || a <= p.m_overlapy) && rot == 3)
                                {
                                    if (a == firstY || p.m_overlapy == -1)
                                    {
                                        p.m_overlapy = a;
                                    }

                                    skip = true;
                                    continue;
                                }
                                if ((p.SpeedX < 0 || b <= p.m_overlapy) && rot == 0)
                                {
                                    if (b != firstX || p.m_overlapy == -1)
                                    {
                                        p.m_overlapy = b;
                                    }

                                    skip = true;
                                    continue;
                                }
                            }
                            else
                            {
                                if (p.SpeedY < 0 || a <= p.m_overlapy)
                                {
                                    if (a != y || p.m_overlapy == -1)
                                    {
                                        p.m_overlapy = a;
                                    }

                                    skip = true;
                                    continue;
                                }
                            }
                        }

                        switch (tileId)
                        {
                            case (BlockID)23:
                                if (m_WorldConnection.Keys.IsKeyActive(Key.Red))
                                {
                                    continue;
                                }
                                break;
                            case (BlockID)24:
                                if (m_WorldConnection.Keys.IsKeyActive(Key.Green))
                                {
                                    continue;
                                }
                                break;
                            case (BlockID)25:
                                if (m_WorldConnection.Keys.IsKeyActive(Key.Blue))
                                {
                                    continue;
                                }
                                break;
                            case (BlockID)26:
                                if (m_WorldConnection.Keys.IsKeyHidden(Key.Red))
                                {
                                    continue;
                                }
                                break;
                            case (BlockID)27:
                                if (m_WorldConnection.Keys.IsKeyHidden(Key.Green))
                                {
                                    continue;
                                }
                                break;
                            case (BlockID)28:
                                if (m_WorldConnection.Keys.IsKeyHidden(Key.Blue))
                                {
                                    continue;
                                }
                                break;
                            case (BlockID)156:
                                if (m_WorldConnection.Keys.IsKeyHidden(Key.TimeDoor))
                                {
                                    continue;
                                }
                                break;
                            case (BlockID)157:
                                if (m_WorldConnection.Keys.IsKeyActive(Key.TimeDoor))
                                {
                                    continue;
                                }
                                break;
                            case BlockID.CyanDoor:
                                if (m_WorldConnection.Keys.IsKeyActive(Key.Cyan))
                                {
                                    continue;
                                }
                                break;
                            case BlockID.MagentaDoor:
                                if (m_WorldConnection.Keys.IsKeyActive(Key.Magenta))
                                {
                                    continue;
                                }
                                break;
                            case BlockID.YellowDoor:
                                if (m_WorldConnection.Keys.IsKeyActive(Key.Yellow))
                                {
                                    continue;
                                }
                                break;
                            case BlockID.CyanGate:
                                if (m_WorldConnection.Keys.IsKeyHidden(Key.Cyan))
                                {
                                    continue;
                                }
                                break;
                            case BlockID.MagentaGate:
                                if (m_WorldConnection.Keys.IsKeyHidden(Key.Magenta))
                                {
                                    continue;
                                }
                                break;
                            case BlockID.YellowGate:
                                if (m_WorldConnection.Keys.IsKeyHidden(Key.Yellow))
                                {
                                    continue;
                                }
                                break;
                            case BlockID.PurpleSwitchDoor:
                                {
                                    PurpleBlock purpleBlock = (PurpleBlock)World[x, y, Layer.Foreground];

                                    if (p.m_switches[purpleBlock.SwitchID])
                                    {
                                        continue;
                                    }
                                }
                                break;
                            case BlockID.PurpleSwitchGate:
                                {
                                    PurpleBlock purpleBlock = (PurpleBlock)World[x, y, Layer.Foreground];

                                    if (!p.m_switches[purpleBlock.SwitchID])
                                    {
                                        continue;
                                    }
                                }
                                break;
                            case BlockID.DeathDoor:
                                {
                                    DeathBlock deathBlock = (DeathBlock)World[x, y, Layer.Foreground];

                                    if (p.m_deaths >= deathBlock.RequiredDeaths)
                                    {
                                        continue;
                                    }
                                }
                                break;
                            case BlockID.DeathGate:
                                {
                                    DeathBlock deathBlock = (DeathBlock)World[x, y, Layer.Foreground];

                                    if (p.m_deaths < deathBlock.RequiredDeaths)
                                    {
                                        continue;
                                    }
                                }
                                break;
                            case BlockID.DoorBuildersClub:
                                if (p.HasBuildersClub)
                                {
                                    continue;
                                }
                                break;
                            case BlockID.GateBuildersClub:
                                if (!p.HasBuildersClub)
                                {
                                    continue;
                                }
                                break;
                            case BlockID.CoinDoor:
                            case BlockID.BlueCoinDoor:
                                {
                                    CoinBlock coinBlock = (CoinBlock)World[x, y, Layer.Foreground];

                                    if (coinBlock.Goal <= p.GoldCoins)
                                    {
                                        continue;
                                    }
                                }
                                break;
                            case BlockID.CoinGate:
                            case BlockID.BlueCoinGate:
                                {
                                    CoinBlock coinBlock = (CoinBlock)World[x, y, Layer.Foreground];

                                    if (coinBlock.Goal > p.BlueCoins)
                                    {
                                        continue;
                                    }
                                }
                                break;
                            case BlockID.GateZombie:
                                {
                                    if (p.HasPotionActive(Potion.Zombie))
                                    {
                                        continue;
                                    }
                                }
                                break;
                            case BlockID.DoorZombie:
                                {
                                    if (!p.HasPotionActive(Potion.Zombie))
                                    {
                                        continue;
                                    }
                                }
                                break;
                            case (BlockID)61:
                            case (BlockID)62:
                            case (BlockID)63:
                            case (BlockID)64:
                            case (BlockID)89:
                            case (BlockID)90:
                            case (BlockID)91:
                            case (BlockID)96:
                            case (BlockID)97:
                            case (BlockID)122:
                            case (BlockID)123:
                            case (BlockID)124:
                            case (BlockID)125:
                            case (BlockID)126:
                            case (BlockID)127:
                            case (BlockID)146:
                            case (BlockID)154:
                            case (BlockID)158:
                            case (BlockID)194:
                            case (BlockID)211:
                                if (p.SpeedY < 0 || y <= p.m_overlapy)
                                {
                                    if (y != firstY || p.m_overlapy == -1)
                                    {
                                        p.m_overlapy = y;
                                    }

                                    skip = true;
                                    continue;
                                }
                                break;
                            case (BlockID)83:
                            case (BlockID)77:
                                continue;
                        }
                        return true;
                    }
                }
                y++;
            }

            if (!skip)
            {
                p.m_overlapy = -1;
            }
            return false;
        }

        /// <summary>
        /// Performs a server update
        /// </summary>
        internal void ServerUpdate(WorldPlayer player)
        {
            if (TickMode == TickMode.RealTime)
            {
                TickRealtimeAsync(player);
            }
        }

        /// <summary>
        /// Ticks up to real time
        /// </summary>
        private void TickRealtimeAsync(WorldPlayer player)
        {
            Task.Run(delegate()
            {
                //Catch up to the server whom is five ticks ahead approximatly
                for (int t = 0; t < 5; t++)
                {
                    this.Tick(player);

                    PhysicsUpdateEvent updateEvent = new PhysicsUpdateEvent()
                    {
                        Player = player
                    };

                    m_WorldConnection.RaiseEventAsync<PhysicsUpdateEvent>(updateEvent);
                }
            });
        }

        /// <summary>
        /// Removes the coin from all players
        /// </summary>
        /// <param name="block">The gold or blue coin</param>
        internal void RemoveCoin(Block block)
        {
            foreach (KeyValuePair<int, WorldPlayer> playerNode in m_WorldConnection.Players.GetList())
            {
                if (block.ID == BlockID.CoinGold)
                {
                    playerNode.Value.CollectedGoldCoins.Remove(block);
                }
                else if (block.ID == BlockID.CoinBlue)
                {
                    playerNode.Value.CollectedBlueCoins.Remove(block);
                }
            }
        }

        /// <summary>
        /// Starts the engine
        /// </summary>
        public void Start()
        {
            if (physicsThread == null)
            {
                CreateThread();
            }
            else if (physicsThread.ThreadState != System.Threading.ThreadState.Running)
            {
                CreateThread();               
            }

            m_Running = true;
            physicsThread.Start();
        }

        /// <summary>
        /// Stops the engine
        /// </summary>
        public void Stop()
        {
            m_Running = false;
            if (physicsThread != null)
            {
                if (physicsThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    physicsThread.Abort();
                }
            }
        }

        /// <summary>
        /// Creates a new physics engine
        /// </summary>
        /// <param name="worldConnection">The world connection</param>
        public PhysicsEngine(WorldConnection worldConnection)
        {
            this.m_WorldConnection = worldConnection;

            TickMode = TickMode.RealTime;
            EventMode = PhysicsEventMode.Ignore;
        }
    }
}
