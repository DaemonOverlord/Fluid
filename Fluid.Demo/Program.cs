using Fluid.Auth;
using Fluid.Room;
using Fluid.ServerEvents;
using System;
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
            if (c.LogIn())
            {
                var con = c.GetWorldConnection("PWAEQiKc2Ma0I");
                con.Join();

                for (int x = 0; x < 200; x++)
                {
                    con.UploadBlockAsync(BlockIDs.Blocks.Basic.Purple, x, 0, 10);
                }

                con.Uploader.WaitForBlocks();
            }

            Console.ReadKey();
        }
    }
}
