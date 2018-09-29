using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;

namespace SpiralFound
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        void RegisterRoutes(RouteCollection routes)
        {

            // Ignore Routing for Script Manager Resources
            routes.Ignore("{resource}.axd/{*pathInfo}");

            // Suppor Pages
            routes.MapPageRoute("",
                "about",
                "~/About.aspx");

            routes.MapPageRoute("",
                "terms",
                "~/Terms.aspx");

            routes.MapPageRoute("",
                "privacy",
                "~/Privacy.aspx");

            routes.MapPageRoute("",
                "contact",
                "~/Contact.aspx");

            // Page Types
            routes.MapPageRoute("",
                "book/{ID}",
                "~/Book.aspx");

            routes.MapPageRoute("",
                "page/{ID}",
                "~/Page.aspx");

            routes.MapPageRoute("",
                "item/{ID}",
                "~/Item.aspx");


            // Plural Versions
            routes.MapPageRoute("",
                "books/{ID}",
                "~/Book.aspx");

            routes.MapPageRoute("",
                "pages/{ID}",
                "~/Page.aspx");

            routes.MapPageRoute("",
                "items/{ID}",
                "~/Item.aspx");

            // Followers & Following
            routes.MapPageRoute("",
                "{Name}/Followers",
                "~/Followers.aspx");

            routes.MapPageRoute("",
                "{Name}/Following",
                "~/Following.aspx");

            // Username Default
            routes.MapPageRoute("",
                "{Name}",
                "~/User.aspx");

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}