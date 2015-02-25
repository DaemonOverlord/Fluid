using Fluid.Auth;
using Fluid.Blocks;
using Fluid.Events;
using Fluid.ServerEvents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluid.Demo
{
    class Program
    {
        static WorldConnection con;

        static void Main(string[] args)
        {
            GuestAuth guestAuth = new GuestAuth();

            FluidClient client = new FluidClient(guestAuth);

            if (client.LogIn())
            {
                Console.WriteLine("Done.");
                Console.ReadKey();
            }
        }
    }
}
