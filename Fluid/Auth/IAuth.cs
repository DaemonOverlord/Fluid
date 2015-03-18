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
        /// <param name="clientCallback">The client success callback</param>
        /// <param name="errorCallback">The playerio error callback</param>
        void LogIn(Config config, Callback<Client> clientCallback, Callback<PlayerIOError> errorCallback);
    }
}
