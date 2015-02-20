using PlayerIOClient;

namespace Fluid
{
    /// <summary>
    /// The IAuth interface for different game authentication's
    /// </summary>
    public interface IAuth
    {
        /// <summary>
        /// Log's in to the game
        /// </summary>
        /// <param name="config">The game configuration</param>
        /// <returns>PlayerIO Client</returns>
        Client LogIn(Config config);
    }
}
