using PlayerIOClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fluid.Auth
{
    public class KongregateAuth : IAuth
    {

        public string Username { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// Gets a unique kongregate authenitcity token
        /// </summary>
        internal KongregateSession GetNewKongregateSession()
        {
            HttpWebRequest loginRequest = (HttpWebRequest)HttpWebRequest.Create("http://www.kongregate.com/");
            loginRequest.Method = "GET";
            loginRequest.CookieContainer = new CookieContainer();

            try
            {
                using (HttpWebResponse webRes = (HttpWebResponse)loginRequest.GetResponse())
                {
                    if (webRes.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader streamReader = new StreamReader(webRes.GetResponseStream()))
                        {
                            string webPage = streamReader.ReadToEnd();

                            string authenticityInputStr = "<input name=\"authenticity_token\" type=\"hidden\" value=\"";
                            int inputIndex = webPage.IndexOf(authenticityInputStr);
                            if (inputIndex == -1)
                            {
                                return null;
                            }

                            string authenticityTokenStart = webPage.Substring(inputIndex + authenticityInputStr.Length);
                            int authenticityTokenEndIndex = authenticityTokenStart.IndexOf('\"');
                            if (authenticityTokenEndIndex == -1)
                            {
                                return null;
                            }

                            string authenticityToken = authenticityTokenStart.Substring(0, authenticityTokenEndIndex);
                            string kongSvid = webRes.Cookies["kong_svid"].Value;
                            string kongSession = webRes.Cookies["_kongregate_session"].Value;
                            return new KongregateSession(authenticityToken, kongSvid, kongSession);
                        }
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
        /// Logs In to kongregate with a basic kongregate session and upgrades the session
        /// </summary>
        /// <param name="kongSession">The basic kongregate session</param>
        /// <returns></returns>
        internal bool LoginToKongregate(KongregateSession kongSession)
        {
            HttpWebRequest loginRequest = (HttpWebRequest)HttpWebRequest.Create("https://www.kongregate.com/session");
            loginRequest.Method = "POST";
            loginRequest.Headers["Cookie"] = string.Format("kong_svid={0}; _kongregate_session={1};", kongSession.KongSvid, kongSession.SessionToken);

            loginRequest.CookieContainer = new CookieContainer();
            loginRequest.ContentType = "application/x-www-form-urlencoded";
            loginRequest.AllowAutoRedirect = false;

            string postdata = string.Format("utf8=%E2%9C%93&authenticity_token={0}&from_welcome_box=true&username={1}&password={2}&remember_me=false", kongSession.AuthenticityToken, Username, Password);
            byte[] postBodyBytes = Encoding.UTF8.GetBytes(postdata);

            loginRequest.ContentLength = postBodyBytes.Length;
            using (Stream sw = loginRequest.GetRequestStream())
            {
                sw.Write(postBodyBytes, 0, postBodyBytes.Length);
            }
            try
            {
                using (HttpWebResponse webRes = (HttpWebResponse)loginRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(webRes.GetResponseStream()))
                    {
                        string kongSvid = webRes.Cookies["kong_svid"].Value;
                        string sessionToken = webRes.Cookies["_kongregate_session"].Value;

                        if (webRes.Cookies["www_pass"] != null)
                        {
                            //Login successful
                            string wwwPass = webRes.Cookies["www_pass"].Value;

                            kongSession.KongSvid = kongSvid;
                            kongSession.SessionToken = sessionToken;
                            kongSession.WWWPass = wwwPass;
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Gets the player object
        /// </summary>
        /// <param name="loginSession">The logged in session</param>
        internal KongregatePlayerObject GetPlayerObject(KongregateSession loginSession)
        {
            if (!loginSession.IsAuthenticated())
            {
                return null;
            }

            HttpWebRequest loginRequest = (HttpWebRequest)HttpWebRequest.Create("http://www.kongregate.com/games/QRious/everybody-edits");
            loginRequest.Method = "GET";
            loginRequest.Headers["Cookie"] = string.Format("kong_svid={0}; _kongregate_session={1}; www_pass={2};", loginSession.KongSvid, loginSession.SessionToken, loginSession.WWWPass);

            try
            {
                using (HttpWebResponse webRes = (HttpWebResponse)loginRequest.GetResponse())
                {
                    if (webRes.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader streamReader = new StreamReader(webRes.GetResponseStream()))
                        {
                            string webPage = streamReader.ReadToEnd();

                            string userIdStr = "\"id\":";
                            int userIdIndex = webPage.IndexOf(userIdStr);
                            if (userIdIndex == -1)
                            {
                                return null;
                            }

                            string userIdStart = webPage.Substring(userIdIndex + userIdStr.Length);
                            int endUserIdIndex = userIdStart.IndexOf(',');
                            if (endUserIdIndex == -1)
                            {
                                return null;
                            }

                            string userId = userIdStart.Substring(0, endUserIdIndex);
                            
                            string gameAuthTokenStr = "\"game_auth_token\":\"";
                            int gameAuthTokenIndex = webPage.IndexOf(gameAuthTokenStr);
                            if (gameAuthTokenIndex == -1)
                            {
                                return null;
                            }

                            string gameAuthTokenStart = webPage.Substring(gameAuthTokenIndex + gameAuthTokenStr.Length);
                            int endGameAuthTokenIndex = gameAuthTokenStart.IndexOf('\"');
                            if (endGameAuthTokenIndex == -1)
                            {
                                return null;
                            }

                            string gameAuthToken = gameAuthTokenStart.Substring(0, endGameAuthTokenIndex);
                            return new KongregatePlayerObject(userId, gameAuthToken);
                        }
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
        /// Log in through kongregate
        /// </summary>
        /// <param name="config">The config</param>
        /// <param name="clientCallback">The client success callback</param>
        /// <param name="errorCallback">The playerio error callback</param>
        public void LogIn(Config config, Callback<Client> clientCallback, Callback<PlayerIOError> errorCallback)
        {
            KongregateSession kongSession = GetNewKongregateSession();

            if (LoginToKongregate(kongSession))
            {
                KongregatePlayerObject playerObject = GetPlayerObject(kongSession);
                if (playerObject != null)
                {
                    PlayerIO.QuickConnect.KongregateConnect(config.GameID, playerObject.UserID, playerObject.GameAuthToken, null, clientCallback, errorCallback);
                    return;
                }

                errorCallback(new PlayerIOError(ErrorCode.ExternalError, "Failed to load kongregate authentication."));
                return;
            }

            errorCallback(new PlayerIOError(ErrorCode.ExternalError, "Failed to login to kongregate."));
            return;
        }

        /// <summary>
        /// Creates a new kongregate authentication
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        public KongregateAuth(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
