using Fluid.Auth;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fluid.Tests
{
    [TestClass]
    public class FluidTests
    {
        [TestMethod]
        public void TestDeserialization()
        {
            GuestAuth guestAuth = new GuestAuth();

            FluidClient client = new FluidClient(guestAuth);
            if (client.LogIn())
            {
                World world = client.LoadWorld("PW1W4Ubqjwa0I");
                Assert.IsTrue(world.IsLoaded);
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void TestJoin()
        {
            GuestAuth guestAuth = new GuestAuth();

            FluidClient client = new FluidClient(guestAuth);
            if (client.LogIn())
            {
                WorldConnection con = client.GetWorldConnection("PW1W4Ubqjwa0I");
                con.Join();

                Assert.IsTrue(con.World.IsLoaded);
                return;
            }

            Assert.Fail();
        }
        
    }
}
