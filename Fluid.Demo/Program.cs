using Fluid.Auth;
using Fluid.Room;
using Fluid.ServerEvents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Fluid.Demo
{
    internal class Program
    {
        public static void Main()
        {
            FluidClient c = new FluidClient(new GuestAuth());
            c.Config.AddProfilesToDatabase = true;

            if (c.LogIn())
            {
                var con = c.GetWorldConnection("PWyiCdOcZEbEI");
                con.Physics.EventMode = Physics.PhysicsEventMode.Send;
                con.Join();

                for (int i = 0; i < 20; i++)
                {
                    con.Say("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean at gravida felis. Pellentesque condimentum vulputate diam, nec ullamcorper nulla volutpat eu. Nunc posuere lobortis quam, a vehicula mi aliquet vel.");
                }

                Console.ReadKey();
            }
        }
    }
}
