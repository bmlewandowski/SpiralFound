using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Net;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace SpiralFound
{
    public partial class User : System.Web.UI.Page
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

                    // Show Add Book
                    this.btn_addBook.Visible = true;
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
                Sql_RepBooks.SelectParameters["userId"].DefaultValue = cmd.Parameters["@UserId"].Value.ToString();

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

        protected void btn_add_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                string scollectionName = txt_add.Text.Trim();
                string scollectionDescription = txt_addDescription.Text.Trim();

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Collection_Create";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Int32.Parse(Session["UserID"].ToString());
                cmd.Parameters.Add("@CollectionName", SqlDbType.VarChar, 255).Value = scollectionName;
                cmd.Parameters.Add("@CollectionDescription", SqlDbType.VarChar, 420).Value = scollectionDescription;
                cmd.Parameters.Add("@CollectionPrivate", SqlDbType.Bit).Value = false;
                cmd.Parameters.Add("@CollectionLayout", SqlDbType.Int).Value = "1";
                // cmd.Parameters.Add("@bookPrivate", SqlDbType.Bit).Value = false;


                cmd.Connection = con;

                try
                {
                    con.Open();
                    Int32 newCollectionId = Convert.ToInt32(cmd.ExecuteScalar());


                    var webClient = new WebClient();
                    using (var fileStream = webClient.OpenRead("http://www.spiralfound.com/images/books/cover1.png"))
                    {
                        Bitmap bmp = ResizeImage(fileStream, 530, 360);
                        bmp.Save(Server.MapPath("~/images/books/" + newCollectionId + ".jpg"), ImageFormat.Jpeg);

                    }


                    var webClientthb = new WebClient();
                    using (var fileStream = webClientthb.OpenRead("http://www.spiralfound.com/images/books/bookthumb-cover1.png"))
                    {
                        Bitmap thb = ResizeImage(fileStream, 150, 100);
                        thb.Save(Server.MapPath("~/images/books/" + newCollectionId + "_s.jpg"), ImageFormat.Jpeg);
                    }


                    // Create New Page for Book
                    CreatePage(newCollectionId);


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

                    PopulateLabels();
                    this.RepBooks.DataBind();
                    this.txt_add.Text = "";
                    this.txt_addDescription.Text = "";

                }

            }

        }

        private void CreatePage(int collectionId)
        {

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Group_Create";
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Int32.Parse(Session["UserID"].ToString());
            cmd.Parameters.Add("@CollectionId", SqlDbType.Int).Value = collectionId;
            cmd.Parameters.Add("@GroupName", SqlDbType.VarChar, 255).Value = "Page One";
            cmd.Parameters.Add("@GroupDescription", SqlDbType.VarChar, 420).Value = "First page of a new book";
            cmd.Parameters.Add("@GroupPrivate", SqlDbType.Bit).Value = false;
            cmd.Parameters.Add("@GroupLayout", SqlDbType.Int).Value = "1";

            cmd.Connection = con;


            try
            {
                con.Open();
                Int32 newGroupId = Convert.ToInt32(cmd.ExecuteScalar());


                //var webClient = new WebClient();
                //using (var fileStream = webClient.OpenRead("http://www.spiralfound.com/images/books/cover1.png"))
                //{
                //    Bitmap bmp = ResizeImage(fileStream, 530, 360);
                //    bmp.Save(Server.MapPath("~/images/pages/" + newGroupId + ".jpg"), ImageFormat.Jpeg);

                //}


                //var webClientthb = new WebClient();
                //using (var fileStream = webClientthb.OpenRead("http://www.spiralfound.com/images/books/bookthumb-cover1.png"))
                //{
                //    Bitmap thb = ResizeImage(fileStream, 150, 100);
                //    thb.Save(Server.MapPath("~/images/pages/" + newGroupId + "_s.jpg"), ImageFormat.Jpeg);
                //}


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

        private Bitmap ResizeImage(Stream streamImage, int maxWidth, int maxHeight)
        {
            Bitmap originalImage = new Bitmap(streamImage);
            int newWidth = originalImage.Width;
            int newHeight = originalImage.Height;
            double aspectRatio = (double)originalImage.Width / (double)originalImage.Height;

            if (aspectRatio <= 1 && originalImage.Width > maxWidth)
            {
                newWidth = maxWidth;
                newHeight = (int)Math.Round(newWidth / aspectRatio);
            }
            else if (aspectRatio > 1 && originalImage.Height > maxHeight)
            {
                newHeight = maxHeight;
                newWidth = (int)Math.Round(newHeight * aspectRatio);
            }

            return new Bitmap(originalImage, newWidth, newHeight);
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
            this.lit_authorsCollections.Text = userName.ToUpper();

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