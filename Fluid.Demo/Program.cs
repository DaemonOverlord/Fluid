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

            if (client.LogIn())
            {
                List<WorldReference> worlds = client.GetLobbyRooms();

                Console.WriteLine("Done.");
                Console.ReadKey();
            }
        }
    }
}
