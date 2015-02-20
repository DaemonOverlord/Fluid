using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Fluid
{
    /// <summary>
    /// Fluid Parser Utilities
    /// </summary>
    public class FluidParser
    {
        /// <summary>
        /// Trys to parse a url or id
        /// </summary>
        public bool Parse(string urlOrId, out string parsed)
        {
            parsed = null;
            if (string.IsNullOrEmpty(urlOrId))
            {
                return false;
            }

            if (IsValidStrictRoomId(urlOrId))
            {
                parsed = urlOrId;
                return true;
            }

            try
            {
                Uri parsedUrl = new Uri(urlOrId);
                string[] hostParts = parsedUrl.Host.Split('.');
                string domain = string.Join(".", hostParts.Skip(Math.Max(0, hostParts.Length - 2)).Take(2).ToArray());

                if (string.Compare(domain, "everybodyedits.com", StringComparison.CurrentCultureIgnoreCase) != 0)
                {
                    return false;
                }

                var pathId = parsedUrl.GetComponents(UriComponents.Path, UriFormat.Unescaped);

                int indexOfLastPathPart = pathId.LastIndexOf('/');
                if (indexOfLastPathPart == -1 || indexOfLastPathPart == pathId.Length - 1)
                {
                    return false;
                }

                string lastPath = pathId.Substring(indexOfLastPathPart + 1);

                if (IsValidStrictRoomId(lastPath))
                {
                    parsed = lastPath;
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Check if the string is a valid room id
        /// </summary>
        public bool IsValidStrictRoomId(string id)
        {
            return Regex.IsMatch(id, @"^([P|B|O]W[a-zA-Z0-9_-]+)|\$service-room\$$");
        }

        /// <summary>
        /// Checks if the world id is a beta room
        /// </summary>
        /// <param name="id">The world id</param>
        /// <returns>true if the world id is beta; otherwise false</returns>
        public bool IsBeta(string id)
        {
            return id.StartsWith("BW");
        }
    }
}
