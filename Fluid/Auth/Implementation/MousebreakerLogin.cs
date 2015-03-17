using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;

namespace Fluid.Auth.Implementation
{
    public class MousebreakerLogin
    {
        /// <summary>
        /// Opens the mousebreaker website and lets the user log in
        /// </summary>
        /// <param name="config">The configuration</param>
        /// <param name="timeout">The timeout in milliseconds</param>
        public MousebreakerAuth GetUserLogin(Config config, int timeout)
        {
            using (FluidWire wireSniffer = new FluidWire(null))
            {
                if (!wireSniffer.Open())
                {
                    return null;
                }

                ProcessStartInfo startInfo = new ProcessStartInfo(config.MouseBreakerGameUrl);
                startInfo.UseShellExecute = true; 

                Process website = Process.Start(startInfo);
                NameValueCollection query = wireSniffer.SniffQuery("/clientintegrations/mousebreaker/auth?game=3&token=", 60 * 1000);
                if (query == null)
                {
                    return null;
                }

                if (query["token"] != null)
                {
                    if (website != null)
                    {
                        if (!website.HasExited)
                        {
                            website.Kill();
                        }
                    }

                    return new MousebreakerAuth(query["token"]);
                }
            }

            return null;
        }
    }
}
