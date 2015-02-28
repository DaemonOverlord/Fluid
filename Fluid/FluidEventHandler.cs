﻿using Fluid.ServerEvents;

namespace Fluid
{
    /// <summary>
    /// Creates a new Fluid event handler
    /// </summary>
    /// <typeparam name="T">The type of message</typeparam>
    /// <param name="connection">The connection</param>
    /// <param name="eventMessage">The message</param>
    public delegate void FluidEventHandler<T>(ConnectionBase connection, T eventMessage) where T : IServerEvent;
}
