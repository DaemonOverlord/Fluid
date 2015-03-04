using Fluid.Auth;
using Fluid.Blocks;
using Fluid.ServerEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fluid.Demo
{
    internal class Program
    {
        public static void Main()
        {
            FluidClient client = new FluidClient(new GuestAuth());
            if (client.LogIn())
            {
                return;
            }
        }
    }
}
