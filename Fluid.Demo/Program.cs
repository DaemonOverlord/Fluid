using Fluid.Auth;
using Fluid.Room;
using Fluid.ServerEvents;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace Fluid.Demo
{
    internal class Program
    {
        public static void Main()
        {
            FluidClient c = new FluidClient(new SimpleAuth("aught2begreen@gmail.com", "UG0TPWN3D"));
            c.Config.AddProfilesToDatabase = true;
            c.Log.Output = Console.Out;

            if (c.LogIn())
            {
                var con = c.GetWorldConnection("PWQxrrGEEib0I");
                con.Join();

                Console.WriteLine("Done");
                Console.ReadKey();
            }
        }
    }
}
