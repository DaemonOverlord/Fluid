namespace Fluid.Events
{
    public class PhysicsUpdateEvent : IEvent
    {
        /// <summary>
        /// Gets the player whose physics were updated
        /// </summary>
        public WorldPlayer Player { get; internal set; }
    }
}
