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

            FluidClient client = new FluidClient(new SimpleAuth("ugotpwned75@hotmail.com", "Code99"));
            client.Log.Output = Console.Out;

            if (client.LogIn())
            {
                var c = client.GetWorldConnection("http://everybodyedits.com/games/PWjF93CG-2a0I");
                c.Physics.EventMode = Physics.PhysicsEventMode.Ignore;
                c.Join();

                Console.ReadKey();
            }
        }
    }
}
