using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiralFound
{
    public class FacebookApp
    {

        public static string AuthenticationUrl()
        {
            return
                string.Format("https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope={2}", FacebookApp.ClientId(), HttpUtility.UrlEncode(FacebookApp.RedirectUrl()), FacebookApp.Scope());
        }

        public static string RedirectUrl()
        {
            return "http://www.spiralfound.com/Error.aspx";
        }

        public static string RegAuthenticationUrl()
        {
            return
                string.Format("https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope={2}", FacebookApp.ClientId(), HttpUtility.UrlEncode(FacebookApp.RegRedirectUrl()), FacebookApp.Scope());
        }

        public static string RegRedirectUrl()
        {
            return "http://www.spiralfound.com/Account.aspx";
        }

        public static string Scope()
        {
            return "user_about_me,friends_about_me,email,user_birthday,publish_stream,user_photos";
        }

        public static string ClientId()
        {
            return "345128432203657";
        }

        public static string ClientSecret()
        {
            return "a9d950a74f78a5fa627e9cff824d3d7e";
        }

    }
}