using PlayerIOClient;

namespace Fluid.Handlers
{
    public interface IMessageHandler
    {
        /// <summary>
        /// Gets the types of message this handler this process
        /// </summary>
        string[] HandleTypes { get; }

        /// <summary>
        /// Handle the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The message</param>
        /// <param name="handled">Whether the message was handled by the user</param>
        void Process(FluidConnectionBase connectionBase, Message message, bool handled);
    }
}
