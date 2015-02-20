using Fluid.ServerEvents;
using PlayerIOClient;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Fluid.Auth
{
    public class ArmorgamesAuth : IAuth
    {
        /// <summary>
        /// The email
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Log's in to armor games with the username and password provided and gets the login cookie
        /// </summary>
        /// <returns>The cookie if a valid session was retrieved; otherwise null</returns>
        internal string GetArmorgamesLoginCookie()
        {
            HttpWebRequest loginRequest = (HttpWebRequest)HttpWebRequest.Create("https://armorgames.com/login");
            loginRequest.Method = "POST";

            loginRequest.CookieContainer = new CookieContainer();
            loginRequest.ContentType = "application/x-www-form-urlencoded";
            loginRequest.AllowAutoRedirect = false;

            string postdata = string.Format("act=login&username={0}&password={1}", Username, Password);
            byte[] postBodyBytes = Encoding.ASCII.GetBytes(postdata);

            loginRequest.ContentLength = postBodyBytes.Length;
            using (Stream sw = loginRequest.GetRequestStream())
            {
                sw.Write(postBodyBytes, 0, postBodyBytes.Length);
            }

            try
            {
                using (HttpWebResponse webRes = (HttpWebResponse)loginRequest.GetResponse())
                {
                    if (webRes.StatusCode == (HttpStatusCode)302)
                    {
                        string session_id = webRes.Cookies["session_id"].Value;
                        string user_id = webRes.Cookies["user_id"].Value;
                        string logged_in = webRes.Cookies["logged_in"].Value;

                        string templateCookie = "pcheck=1; session_id={0}; user_id={1}; logged_in={2}";
                        return string.Format(templateCookie, session_id, user_id, logged_in);
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        /// <summary>
        /// Log's in to everybody edits with the armorgames login cookie
        /// </summary>
        /// <param name="cookie"></param>
        internal ArmorgamesFlashvars GetFlashvars(string loginCookie)
        {
            HttpWebRequest gameRequest = (HttpWebRequest)HttpWebRequest.Create("http://armorgames.com/play/11986/everybody-edits");
            gameRequest.Method = "GET";
            gameRequest.Headers["Cookie"] = loginCookie;

            try
            {
                using (HttpWebResponse webRes = (HttpWebResponse)gameRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(webRes.GetResponseStream()))
                    {
                        string webpage = streamReader.ReadToEnd();
                        int flashVarsIndex = webpage.IndexOf("flashvars=");
                        if (flashVarsIndex == -1)
                        {
                            return null;
                        }

                        string flashVars = webpage.Substring(flashVarsIndex);
                        string authStr = "auth_token=";
                        int authIndex = flashVars.IndexOf(authStr);
                        if (authIndex == -1)
                        {
                            return null;
                        }

                        string authVars = flashVars.Substring(authIndex);
                        int endAuthIndex = authVars.IndexOf("&");
                        if (endAuthIndex == -1)
                        {
                            return null;
                        }

                        string authToken = authVars.Substring(authStr.Length, endAuthIndex - authStr.Length);
                        string userStr = "user_id=";
                        string userVars = authVars.Substring(endAuthIndex + 1);
                        int endUserId = userVars.IndexOf("\"");
                        if (endUserId == -1)
                        {
                            return null;
                        }

                        string userId = userVars.Substring(userStr.Length, endUserId - userStr.Length);
                        return new ArmorgamesFlashvars(authToken, userId);
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Log in through armorgames
        /// </summary>
        /// <param name="config">The config</param>
        /// <returns>The established client if valid; otherwise null</returns>
        public Client LogIn(Config config)
        {
            string loginCookie = GetArmorgamesLoginCookie();
            if (loginCookie == null)
            {
                return null;
            }

            ArmorgamesFlashvars flashVars = GetFlashvars(loginCookie);
            if (flashVars == null)
            {
                return null;
            }

            FluidClient guestClient = new FluidClient(new GuestAuth());
            if (!guestClient.LogIn())
            {
                return null;
            }

            SecureConnection secureConnection = guestClient.GetSecureConnection();
            if (secureConnection == null)
            {
                return null;
            }

            secureConnection.SendAuth(flashVars.UserId, flashVars.AuthToken);
            AuthEvent armorGamesAuth = secureConnection.WaitForServerEvent<AuthEvent>();

            if (armorGamesAuth.IsAuthenitcated())
            {
                return PlayerIO.Connect(config.GameID, "secure", armorGamesAuth.UserID, armorGamesAuth.AuthToken, "armorgames");
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a new armorgames authentication
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        public ArmorgamesAuth(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
