using PlayerIOClient;
using System.IO;
using System.Net;
using System.Web;

namespace Fluid.Auth
{
    public class MousebreakerAuth : IAuth
    {
        /// <summary>
        /// Gets or Sets the token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets a playerio implementation request
        /// </summary>
        internal MousebreakerApiData GetAPIRequest()
        {
            HttpWebRequest apiRequest = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://api.playerio.com/clientintegrations/mousebreaker/auth?game=3&token={0}", HttpUtility.UrlEncode(Token)));
            apiRequest.Method = "GET";

            try
            {
                using (HttpWebResponse webRes = (HttpWebResponse)apiRequest.GetResponse())
                {
                    using (StreamReader webRdr = new StreamReader(webRes.GetResponseStream()))
                    {
                        string apiResponse = webRdr.ReadToEnd();
                        string[] vars = apiResponse.Split('\n');
                        if (vars.Length != 2)
                        {
                            return null;
                        }

                        return new MousebreakerApiData()
                        {
                            MBuid = vars[0],
                            MAuth = vars[1]
                        };
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Logs in with mouse breaker
        /// </summary>
        /// <param name="config">The config</param>
        /// <param name="clientCallback">The client success callback</param>
        /// <param name="errorCallback">The playerio error callback</param>
        public void LogIn(Config config, Callback<Client> clientCallback, Callback<PlayerIOError> errorCallback)
        {
            MousebreakerApiData apiData = GetAPIRequest();
            if (apiData == null)
            {
                errorCallback(new PlayerIOError(ErrorCode.ExternalError, "Invalid or missing mousebreaker credentials."));
                return;
            }

            PlayerIO.Connect(config.GameID, "secure", apiData.MBuid, apiData.MAuth, "mousebreaker", null, clientCallback, errorCallback);
        }

        /// <summary>
        /// Creates a new mousebreaker authentication
        /// </summary>
        /// <param name="token">The token</param>
        public MousebreakerAuth(string token)
        {
            Token = token;
        }
    }
}
