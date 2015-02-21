using Fluid.Auth;
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
        static void Main(string[] args)
        {
            Fluid.FluidClient client = new Fluid.FluidClient(new GuestAuth());

            if (client.LogIn())
            {
                WorldConnection worldCon = client.GetWorldConnection("PWQxrrGEEib0I");
                worldCon.AddServerEventHandler<BlockEvent>(OnBlock);
                worldCon.Join();
                Console.ReadKey();
            }
        }

        private static void OnBlock(BlockEvent eventMessage)
        {
            if (!eventMessage.Block.Placer.IsConnectedPlayer)
            {
                eventMessage.Block.ID++;
            }
        }
    }
}
