using Fluid.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluid.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            IAuth auth = new GuestAuth();

            FluidClient client = new FluidClient(auth);

            if (client.LogIn())
            {
                
            }

            return;
        }
    }
}
