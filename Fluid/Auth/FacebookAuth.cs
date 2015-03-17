using PlayerIOClient;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Fluid.Auth
{
    public class FacebookAuth : IAuth
    {
        /// <summary>
        /// Gets or Sets the facebook user email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets the facebook user password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets a new facebook session
        /// </summary>
        /// <returns>A collection of default facebook cookies</returns>
        internal CookieCollection GetNewFacebookSession()
        {
            HttpWebRequest loginRequest = (HttpWebRequest)HttpWebRequest.Create("https://www.facebook.com/");
            loginRequest.Method = "GET";
            loginRequest.CookieContainer = new CookieContainer();

            try
            {
                using (HttpWebResponse webRes = (HttpWebResponse)loginRequest.GetResponse())
                {
                    if (webRes.StatusCode == HttpStatusCode.OK)
                    {
                        return webRes.Cookies;
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
        /// Gets a collection of authenticated login cookies
        /// </summary>
        /// <param name="email">The facebook user email</param>
        /// <param name="password">The facebook user password</param>
        /// <returns>A collection of authenticated login cookies</returns>
        internal CookieCollection GetLoginCookies(string email, string password)
        {
            CookieCollection session = GetNewFacebookSession();

            string postData = string.Format("email={0}&pass={1}", HttpUtility.UrlEncode(email), password);
            byte[] byteArray = Encoding.ASCII.GetBytes(postData);

            Uri url = new Uri("https://www.facebook.com/login.php?login_attempt=1");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(session);

            request.Method = "POST";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            request.ContentType = "application/x-www-form-urlencoded";
            request.AllowWriteStreamBuffering = true;
            request.ProtocolVersion = HttpVersion.Version11;
            request.AllowAutoRedirect = true;
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            using (WebResponse res = request.GetResponse())
            {
                return request.CookieContainer.GetCookies(res.ResponseUri);
            }
        }

        /// <summary>
        /// Gets a facebook access token
        /// </summary>
        /// <param name="email">The facebook email</param>
        /// <param name="password">The facebook password</param>
        /// <returns>The facebook game access token</returns>
        internal string GetAccessToken(string email, string password)
        {
            CookieCollection cookies = GetLoginCookies(email, password);
            if (cookies.Count != 7)
            {
                return null;
            }

            Uri url = new Uri("https://developers.facebook.com/tools/explorer?method=GET&path=me%3Ffields%3Did%2Cname&version=v2.2");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(cookies);

            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            request.ContentType = "application/x-www-form-urlencoded";
            request.AllowWriteStreamBuffering = true;
            request.ProtocolVersion = HttpVersion.Version11;
            request.AllowAutoRedirect = true;

            using (WebResponse res = request.GetResponse())
            using (StreamReader rdr = new StreamReader(res.GetResponseStream()))
            {
                if (cookies["c_user"] == null)
                {
                    return null;
                }

                string userId = cookies["c_user"].Value;
                string response = rdr.ReadToEnd();

                int index = -1;
                for (int s = 0; s < 3; s++)
                {
                    index = response.IndexOf(userId);
                    if (index == -1)
                    {
                        return null;
                    }

                    response = response.Substring(index + userId.Length);
                }

                string accessTokenStart = response.Substring(2);

                int endIndex = accessTokenStart.IndexOf('\"');
                if (endIndex == -1)
                {
                    return null;
                }

                return accessTokenStart.Substring(0, endIndex);
            }
        }

        /// <summary>
        /// Log's in through facebook
        /// </summary>
        /// <param name="config">The config</param>
        public Client LogIn(Config config)
        {
            string accessToken = GetAccessToken(Email, Password);
            if (accessToken != null)
            {
                return PlayerIO.QuickConnect.FacebookOAuthConnect(config.GameID, accessToken, null, null);
            }

            return null;
        }

        /// <summary>
        /// Creates a new facebook authentication
        /// </summary>
        /// <param name="email">The facebook email</param>
        /// <param name="password">The facebook password</param>
        public FacebookAuth(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
