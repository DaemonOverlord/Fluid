using Fluid.Auth;
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
                client.GetWorldConnection("PWKK8zFHH8bEI").Join();
                Console.ReadKey();
            }
        }
    }
}
