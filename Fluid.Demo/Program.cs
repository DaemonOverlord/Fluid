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
                var con = c.GetWorldConnection("PWjF93CG-2a0I");
                con.Join();

                bool blue = false;
                while (true)
                {
                    for (int i = 0; i < 18; i++)
                    {
                        con.UploadBlock((blue) ? BlockIDs.Action.Coins.Blue : BlockIDs.Action.Coins.Gold, 1, i + 1, 10);
                    }

                    blue = !blue;
                }
            }

            Console.ReadKey();
        }
    }
}
