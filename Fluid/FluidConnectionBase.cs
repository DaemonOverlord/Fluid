using Fluid.Events;
using Fluid.Handlers;
using Fluid.ServerEvents;
using PlayerIOClient;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fluid
{
    public abstract class FluidConnectionBase
    {
        protected FluidClient m_Client;
        private Connection m_Connection;

        /// <summary>
        /// The list of core message processors
        /// </summary>
        protected Dictionary<string, IMessageHandler> m_MessageHandlers;

        /// <summary>
        /// The list of core message awaiters
        /// </summary>
        protected Dictionary<Type, MessageReceivedEvent> m_MessageAwaiters;

        /// <summary>
        /// The list of server event handlers
        /// </summary>
        protected Dictionary<Type, Delegate> m_ServerEventHandlers;

        /// <summary>
        /// The list of event handlers
        /// </summary>
        protected Dictionary<Type, Delegate> m_EventHandlers;

        /// <summary>
        /// The event for incoming playerio messages
        /// </summary>
        public event PlayerIOMessageHandler OnPlayerIOMessage;

        /// <summary>
        /// Gets whether the connection is alive
        /// </summary>
        public bool Connected { get { return m_Connection.Connected; } }

        /// <summary>
        /// Gets the client used to establish the connection
        /// </summary>
        public FluidClient Client { get { return m_Client; } }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="type">The message type</param>
        /// <param name="parameters">The message parameters</param>
        public void SendMessage(string type, params object[] parameters)
        {
            this.SendMessage(Message.Create(type, parameters));
        }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendMessage(Message message)
        {
            if (m_Connection.Connected)
            {
                m_Connection.Send(message);
            }
        }

        /// <summary>
        /// Disconnects the connection
        /// </summary>
        public void Disconnect()
        {
            if (m_Connection != null)
            {
                if (m_Connection.Connected)
                {
                    m_Connection.Disconnect();
                }
            }
        }

        /// <summary>
        /// Invokes a server event
        /// </summary>
        /// <typeparam name="T">The server event</typeparam>
        /// <param name="message">The message</param>
        public void RaiseEvent<T>(T message) where T : IEvent
        {
            Type messageType = typeof(T);
            if (m_EventHandlers.ContainsKey(messageType))
            {
                m_EventHandlers[messageType].DynamicInvoke(this, message);
            }
        }

        /// <summary>
        /// Invokes a server event asyncronously
        /// </summary>
        /// <typeparam name="T">The server event</typeparam>
        /// <param name="message">The message</param>
        public Task RaiseEventAsync<T>(T message) where T : IEvent
        {
            Type messageType = typeof(T);
            if (m_EventHandlers.ContainsKey(messageType))
            {
                return Task.Run(() => m_EventHandlers[messageType].DynamicInvoke(this, message));         
            }

            return null;
        }

        /// <summary>
        /// Invokes a event
        /// </summary>
        /// <typeparam name="T">The event</typeparam>
        /// <param name="message">The message</param>
        public void RaiseServerEvent<T>(T message) where T : IServerEvent
        {
            Type messageType = typeof(T);
            if (m_MessageAwaiters.ContainsKey(messageType))
            {
                MessageReceivedEvent awaiter = m_MessageAwaiters[messageType];
                awaiter.Invoke((object)message);
            }

            if (m_ServerEventHandlers.ContainsKey(messageType))
            {
                m_ServerEventHandlers[messageType].DynamicInvoke(message);
            }
        }

        /// <summary>
        /// Waits for a message to be received
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <returns>The message type</returns>
        public T WaitForServerEvent<T>() where T : IServerEvent
        {
            return WaitForServerEvent<T>(-1);
        }

        /// <summary>
        /// Waits for a message to be received
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <returns>The message type</returns>
        /// <param name="timeout">The timeout in milliseconds</param>
        public T WaitForServerEvent<T>(int timeout) where T : IServerEvent
        {
            MessageReceivedEvent messageEvent = new MessageReceivedEvent();
            m_MessageAwaiters.Add(typeof(T), messageEvent);
            messageEvent.WaitForMessage(timeout);
            T eventResult = (T)messageEvent.Message;

            messageEvent.Dispose();
            m_MessageAwaiters.Remove(typeof(T));

            return (T)messageEvent.Message;
        }

        /// <summary>
        /// Adds a Fluid server event handler
        /// </summary>
        /// <typeparam name="T">The server event type</typeparam>
        /// <param name="eventHandler">The Fluid event handler</param>
        public void AddServerEventHandler<T>(FluidEventHandler<T> eventHandler) where T : IServerEvent
        {
            Delegate dynamicHandler = eventHandler;
            m_ServerEventHandlers.Add(typeof(T), dynamicHandler);
        }

        /// <summary>
        /// Adds a regular event handler
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <param name="eventHandler">The event handler</param>
        public void AddEventHandler<T>(EventHandler<T> eventHandler) where T : IEvent
        {
            Delegate dynamicHandler = eventHandler;
            m_EventHandlers.Add(typeof(T), dynamicHandler);
        }
        
        /// <summary>
        /// Removes a Fluid event handler
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        public void RemoveServerEventHandler<T>()
        {
            Type type = typeof(T);
            if (m_ServerEventHandlers.ContainsKey(type))
            {
                m_ServerEventHandlers.Remove(type);
            }
        }

        /// <summary>
        /// Processes a message received from the server
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="m">The playerio message</param>
        private void MessageProcessor(object sender, Message m)
        {
            bool handled = false;
            if (OnPlayerIOMessage != null)
            {
                PlayerIOMessage playerIOMessage = new PlayerIOMessage(m);
                OnPlayerIOMessage(playerIOMessage);
                if (playerIOMessage.Handled)
                {
                    handled = true;
                }
            }

            if (m_MessageHandlers.ContainsKey(m.Type))
            {
                IMessageHandler messageHandler = m_MessageHandlers[m.Type];

                try
                {
                    messageHandler.Process(this, m, handled);
                }
                catch
                {
                    m_Client.Log.Add(FluidLogCategory.Fail, string.Format("The handler for '{0}' failed.", m.Type));
                }
            }
            else
            {
                m_Client.Log.Add(FluidLogCategory.Suggestion, string.Format("The message type '{0}' was unhandled, you might want to consider creating your own message handler, or checking for a update.", m.Type));
            }
        }

        /// <summary>
        /// Processes a disconnection message received from the server
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="message">The disconnection reason</param>
        private void DisconnectProcessor(object sender, string message)
        {
            DisconnectEvent disconnectEvent = new DisconnectEvent()
            {
                Connection = this,
                Message = message,
            };

            RaiseEvent<DisconnectEvent>(disconnectEvent);
        }

        /// <summary>
        /// Shutdown immediate active resources
        /// </summary>
        internal virtual void Shutdown()
        {
        }

        /// <summary>
        /// Adds a message handler for the connection to use
        /// </summary>
        /// <param name="type">The message type</param>
        /// <param name="messageHandler">The message handler</param>
        protected void AddMessageHandler(IMessageHandler messageHandler)
        {
            for (int i = 0; i < messageHandler.HandleTypes.Length; i++)
            {
                m_MessageHandlers.Add(messageHandler.HandleTypes[i], messageHandler);
            }
        }

        /// <summary>
        /// Sets the connection
        /// </summary>
        /// <param name="connection">The playerio connection</param>
        internal void SetConnection(Connection connection)
        {
            this.m_Connection = connection;
            if (m_Connection != null)
            {
                this.m_Connection.AddOnMessage(new MessageReceivedEventHandler(MessageProcessor));
                this.m_Connection.AddOnDisconnect(new DisconnectEventHandler(DisconnectProcessor));
            }
        }

        /// <summary>
        /// Creates a new Fluid connection base
        /// </summary>
        /// <param name="client">The Fluid client</param>
        protected FluidConnectionBase(FluidClient client)
        {
            this.m_Client = client;

            this.m_MessageHandlers = new Dictionary<string, IMessageHandler>();
            this.m_MessageAwaiters = new Dictionary<Type, MessageReceivedEvent>();
            this.m_EventHandlers = new Dictionary<Type, Delegate>();
            this.m_ServerEventHandlers = new Dictionary<Type, Delegate>();
        }

        /// <summary>
        /// Creates a new Fluid connection base
        /// </summary>
        /// <param name="client">The Fluid client</param>
        /// <param name="connection">The playerio connection</param>
        protected FluidConnectionBase(FluidClient client, Connection connection) : this(client)
        {
            SetConnection(connection);
        }
    }
}
