using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace SpiralFound
{
    public partial class Followers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

            PopulateLabels();
            GetUserDetails();


            if (Request.IsAuthenticated == true)
            {

                Control user_command_panel = (Control)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("user_command_panel");
                Control author_command_panel = (Control)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("author_command_panel");
                ImageButton imb_followUser = (ImageButton)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("imb_followUser");

                // Show User Commands
                user_command_panel.Visible = true;

                // Show Follow Button
                imb_followUser.Visible = true;

                // User is Author
                if (this.lit_userName.Text.Trim().ToLower() == HttpContext.Current.User.Identity.Name.ToLower())
                {
                    // Show Author Panel
                    author_command_panel.Visible = true;

                    // Hide Follow Button
                    imb_followUser.Visible = false;

                }
                else // User is Not Author
                {
                    SetFollowStatus();
                }

            }

        }

        public void SetFollowStatus()
        {
            // See if User is Following Author
            {
                Literal lit_authorId = (Literal)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("lit_authorId");
                ImageButton imb_followUser = (ImageButton)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("imb_followUser");

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "User_Follow_Check";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Int32.Parse(lit_authorId.Text);
                cmd.Parameters.Add("@FollowerId", SqlDbType.Int).Value = Int32.Parse(Session["UserID"].ToString());
                cmd.Parameters.Add("@OutRes", SqlDbType.Int, 4);
                cmd.Parameters["@OutRes"].Direction = ParameterDirection.Output;
                cmd.Connection = con;
                int Results = 0;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    Results = (int)cmd.Parameters["@OutRes"].Value;

                    if (Results == 1)
                    {
                        imb_followUser.ImageUrl = "~/Images/Interface/Owner/ownercard-unfollow.png";
                        imb_followUser.CommandName = "Unfollow";                     
                    }
                    else
                    {
                        imb_followUser.ImageUrl = "~/Images/Interface/Owner/ownercard-follow.png";
                        imb_followUser.CommandName = "Follow";
                        
                    }
                }
                catch (SqlException ex)
                {

                }
                finally
                {
                    cmd.Dispose();
                    if (con != null)
                    {
                        con.Close();
                    }
                }

            }
        }

        public void PopulateLabels()
        {

            // Grab QueryString Parameter
            string qString;

            qString = Request.QueryString["Name"];

            if (string.IsNullOrEmpty(qString))
            {
                // Get from Route
                qString = Page.RouteData.Values["Name"] as string;
            }

            if (string.IsNullOrEmpty(qString))
            {
                // Set Default
                qString = "Empty";
            }

            // Set userName Literal for Later Use
            this.lit_userName.Text = qString;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "User_Get_Id";
            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 100).Value = qString;
            cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
            cmd.Connection = con;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

                // Update Select Parameter
                Sql_RepFollow.SelectParameters["userId"].DefaultValue = cmd.Parameters["@UserId"].Value.ToString();

            }
            //catch (SqlException ex)
            //{
            //   ErrorMessage.Text = ex.Message;
            //}
            finally
            {
                cmd.Dispose();
                if (con != null)
                {
                    con.Close();
                }
            }

        }


        public void GetUserDetails()
        {

            string userName = this.lit_userName.Text;
            Literal lit_userFollowers = (Literal)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("lit_userFollowers");
            Literal lit_userFollowing = (Literal)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("lit_userFollowing");
            HyperLink hyp_userFollowing = (HyperLink)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("hyp_userFollowing");
            HyperLink hyp_userFollowers = (HyperLink)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("hyp_userFollowers");
            ImageButton imb_author = (ImageButton)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("imb_author");
            Literal lit_authorId = (Literal)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("lit_authorId");
            HyperLink hyp_authorName = (HyperLink)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("hyp_authorName");
            HyperLink hyp_myCollections = (HyperLink)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("hyp_myCollections");

            hyp_userFollowers.NavigateUrl = "/" + userName + "/Followers";
            hyp_userFollowing.NavigateUrl = "/" + userName + "/Following";

            hyp_authorName.Text = userName;
            hyp_authorName.NavigateUrl = "/" + userName;
            hyp_myCollections.NavigateUrl = "/" + userName;

            // Set Image Control URL 
            imb_author.ImageUrl = "~/Images/Users/" + userName + ".jpg";
            imb_author.PostBackUrl = "~/" + userName;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "User_Get_Details";
            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 100).Value = userName;
            cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@UserFollowers", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@UserFollowing", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
            cmd.Connection = con;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                lit_userFollowers.Text = cmd.Parameters["@UserFollowers"].Value.ToString();
                lit_userFollowing.Text = cmd.Parameters["@UserFollowing"].Value.ToString();
                lit_authorId.Text = cmd.Parameters["@UserId"].Value.ToString();


            }
            //catch (SqlException ex)
            //{
            //   ErrorMessage.Text = ex.Message;
            //}
            finally
            {
                cmd.Dispose();
                if (con != null)
                {
                    con.Close();
                }
            }
        }

    }
}