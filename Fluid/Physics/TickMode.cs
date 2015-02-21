namespace Fluid.Physics
{
    public enum TickMode
    {
        /// <summary>
        /// Attempts to keep up with the actual realtime action of the world
        /// </summary>
        RealTime,
        
        /// <summary>
        /// Keeps at a local pace but behind the world's time
        /// </summary>
        Local
    }
}
