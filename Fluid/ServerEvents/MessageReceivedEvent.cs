using System;
using System.Threading;

namespace Fluid.ServerEvents
{
    public class MessageReceivedEvent : IDisposable
    {
        private AutoResetEvent resetEvent;

        /// <summary>
        /// The message received
        /// </summary>
        public object Message { get; private set; }

        /// <summary>
        /// Waits for the message
        /// </summary>
        /// <param name="timeout">The timeout in milliseconds</param>
        public void WaitForMessage(int timeout)
        {
            resetEvent.WaitOne(timeout);
        }

        /// <summary>
        /// Invokes the event
        /// </summary>
        /// <param name="message">The message received</param>
        public void Invoke(object message)
        {
            Message = message;
            resetEvent.Set();
        }

        /// <summary>
        /// Disposes the event's resources
        /// </summary>
        public void Dispose()
        {
            if (resetEvent != null)
            {
                resetEvent.Dispose();
            }
        }

        /// <summary>
        /// Creates a new message received event
        /// </summary>
        public MessageReceivedEvent()
        {
            resetEvent = new AutoResetEvent(false);
        }
    }
}
