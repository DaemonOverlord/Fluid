using PlayerIOClient;

namespace Fluid.ServerEvents
{
    /// <summary>
    /// The message interface
    /// </summary>
    public interface IServerEvent 
    {
        /// <summary>
        /// Gets the raw message
        /// </summary>
        Message Raw { get; set; }
    }
}
