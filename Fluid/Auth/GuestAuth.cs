using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluid.Auth
{
    public class GuestAuth : SimpleAuth
    {
        /// <summary>
        /// Guest authentication
        /// </summary>
        public GuestAuth() : base("guest", "guest") { }
    }
}
