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
                con = client.GetWorldConnection("PWXZJhVZ5ma0I");
                con.AddServerEventHandler<MovementEvent>(OnMovement);
                con.Join();

                con.SendMovementInput(Input.HoldRight);
                Console.WriteLine("Done.");
                Console.ReadKey();
            }
        }

        private static void OnMovement(MovementEvent eventMessage)
        {
            Console.WriteLine(eventMessage.Input);
        }
    }
}
