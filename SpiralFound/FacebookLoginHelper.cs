using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

namespace SpiralFound
{
    public class FacebookLoginHelper
    {
        /// <summary>
        /// Facebook Helper methods
        /// </summary>
        /// <returns></returns>
        public FacebookLoginHelper()
        {

        }
        /// <summary>
        /// Get the authorisation token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetAccessToken(string code, string scope, string redirectUrl)
        {
            Dictionary<string, string> tokens = new Dictionary<string, string>();
            // string clientId = "125788630880182";
            //string clientSecret = "350d206a2dd0cc775f1b76fdafe859f3";
            // App ID  208878159224995
            // App Secret  e0ac797bf3135232df717ed976c27453


            string url = string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}&scope={4}",
                            FacebookApp.ClientId(), redirectUrl, FacebookApp.ClientSecret(), code, scope);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string retVal = reader.ReadToEnd();

                foreach (string token in retVal.Split('&'))
                {
                    tokens.Add(token.Substring(0, token.IndexOf("=")),
                        token.Substring(token.IndexOf("=") + 1, token.Length - token.IndexOf("=") - 1));
                }
            }
            return tokens;
        }
    }
}
