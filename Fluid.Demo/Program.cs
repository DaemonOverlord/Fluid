using Fluid.Auth;
using Fluid.Room;
using System;

namespace Fluid.Demo
{
    internal class Program
    {
        public static void Main()
        {
            FluidClient c = new FluidClient(new GuestAuth());

            if (c.LogIn())
            {
                var con = c.GetWorldConnection("PWWfBtRhUAbEI");
                con.Join();
            }
  
            Console.ReadKey();
        }
    }
}
