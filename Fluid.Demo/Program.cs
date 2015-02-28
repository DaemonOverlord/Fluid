using Fluid.Auth;
using Fluid.ServerEvents;
using System;
using System.Collections.Generic;

namespace Fluid.Demo
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            GuestAuth guestAuth = new GuestAuth();

            FluidClient client = new FluidClient(guestAuth);
            client.Log.Output = Console.Out;

            if (client.LogIn())
            {
                client.GetWorldConnection("PW_1hlEAB_bUI").Join();
                Console.ReadKey();
            }
        }
    }
}
