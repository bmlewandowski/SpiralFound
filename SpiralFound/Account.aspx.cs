using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using System.Net;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SpiralFound
{
    public partial class Account : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FacebookLoginHelper helper = new FacebookLoginHelper();
            if (Request.Params.AllKeys.Contains("code"))
            {
                Dictionary<string, string> dicAccessToken = helper.GetAccessToken(Request["code"].ToString(), FacebookApp.Scope(), FacebookApp.RegRedirectUrl());

                var accessToken = dicAccessToken["access_token"];
                var client = new FacebookClient(accessToken);
                dynamic me = client.Get("me");
                Session["fbUserId"] = me["id"];
               

                // Check Database for Facebook Lookup
                string Results = "";
                Results = Validate_Facebook(me["id"]);
                if (Results.Length > 4) // User already has an account
                {
                    FormsAuthentication.RedirectFromLoginPage(Results, false);
                }

                else // User Doesn't have an account yet
                {
                    img_fbUserImage.ImageUrl = "https://graph.facebook.com/" + me["id"] + "/picture?type=large";
                    lit_fbUserName.Text = me["first_name"];


                    if (!IsPostBack)
                    {
                        this.Email.Text = me["email"];
                        this.UserName.Text = me["username"];

                    }

                    // Write Friend IDs from JSON
                    var json = new WebClient().DownloadString("https://api.facebook.com/method/friends.getAppUsers?access_token=" + accessToken + "&format=json");
                    var jss = new JavaScriptSerializer();
                    dynamic data = jss.Deserialize<dynamic>(json);
                    //foreach (dynamic friend in data)
                    //{
                    //    Response.Write(friend);
                    //    Response.Write("<br>");
                    //}
                }

            }

            else
            {
                Response.Redirect("Error.aspx");
            }
        }


        public string Validate_Facebook(String FacebookId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmdselect = new SqlCommand();
            cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.CommandText = "User_Check_Facebook";
            cmdselect.Parameters.Add("@FacebookId", SqlDbType.VarChar, 100).Value = FacebookId;
            cmdselect.Parameters.Add("@UserName", SqlDbType.VarChar, 50); cmdselect.Parameters["@UserName"].Direction = ParameterDirection.Output;
            cmdselect.Connection = con; string Results = "";
            try
            {
                con.Open();
                cmdselect.ExecuteNonQuery();
                Results = (string)cmdselect.Parameters["@UserName"].Value;
            }
            catch (SqlException ex)
            {
                ErrorMessage.Text = ex.Message;
            }
            finally
            {
                cmdselect.Dispose();
                if (con != null)
                {
                    con.Close();
                }
            }
            return Results;
        }

        public int Validate_UserName(String Username)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmdselect = new SqlCommand();
            cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.CommandText = "User_Check_Name";
            cmdselect.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = Username;
            cmdselect.Parameters.Add("@OutRes", SqlDbType.Int, 4); cmdselect.Parameters["@OutRes"].Direction = ParameterDirection.Output;
            cmdselect.Connection = con; int Results = 0;
            try
            {
                con.Open();
                cmdselect.ExecuteNonQuery();
                Results = (int)cmdselect.Parameters["@OutRes"].Value;
            }
            catch (SqlException ex)
            {
                ErrorMessage.Text = ex.Message;
            }
            finally
            {
                cmdselect.Dispose();
                if (con != null)
                {
                    con.Close();
                }
            }
            return Results;
        }




        protected void btn_Register_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                int Results = 0;
                Results = Validate_UserName(UserName.Text.Trim());
                if (Results == 1)
                {
                    ErrorMessage.Text = "That Username Already Exists, Please Select Another";
                }
                else
                {
                    // Create User
                    string sUserName = UserName.Text.Trim();
                    string sPassword = Password.Text.Trim();
                    string sEmail = Email.Text.Trim();

                    var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    var encoding = new System.Text.ASCIIEncoding();
                    byte[] pwBytes = md5.ComputeHash(encoding.GetBytes(sPassword));
                    string passwordHash;
                    System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                    passwordHash = enc.GetString(pwBytes);


                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                    SqlCommand cmdinsert = new SqlCommand();
                    cmdinsert.CommandType = CommandType.StoredProcedure;
                    cmdinsert.CommandText = "User_Create";
                    cmdinsert.Parameters.Add("@Username", SqlDbType.VarChar, 100).Value = sUserName;
                    cmdinsert.Parameters.Add("@Password", SqlDbType.VarChar, 100).Value = passwordHash;
                    cmdinsert.Parameters.Add("@Email", SqlDbType.VarChar, 255).Value = sEmail;
                    cmdinsert.Parameters.Add("@FacebookId", SqlDbType.VarChar, 255).Value = Session["fbUserId"].ToString();
                    // Add New Book Commands
                    cmdinsert.Parameters.Add("@collectionName", SqlDbType.VarChar, 100).Value = sUserName + "'s First Book";
                    cmdinsert.Parameters.Add("@collectionDescription", SqlDbType.VarChar, 255).Value = "This first book was created automatically for " + sUserName + ".";
                    cmdinsert.Parameters.Add("@groupName", SqlDbType.VarChar, 100).Value = sUserName + "'s First Page";
                    cmdinsert.Parameters.Add("@groupDescription", SqlDbType.VarChar, 255).Value = "This page was created along with the book.";


                    cmdinsert.Connection = con;

                    try
                    {
                        con.Open();
                        //cmdinsert.ExecuteNonQuery();
                        Int32 newBookId = Convert.ToInt32(cmdinsert.ExecuteScalar());


                        var webClientful = new WebClient();
                        using (var fileStream = webClientful.OpenRead("http://www.spiralfound.com/images/books/cover1.png"))
                        {
                            Bitmap bmp = ResizeImage(fileStream, 530, 360);
                            bmp.Save(Server.MapPath("~/images/books/" + newBookId + ".jpg"), ImageFormat.Jpeg);

                        }


                        var webClientthb = new WebClient();
                        using (var fileStream = webClientthb.OpenRead("http://www.spiralfound.com/images/books/bookthumb-cover1.png"))
                        {
                            Bitmap thb = ResizeImage(fileStream, 150, 100);
                            thb.Save(Server.MapPath("~/images/books/" + newBookId + "_s.jpg"), ImageFormat.Jpeg);
                        }

                    }
                    catch (SqlException ex)
                    {
                        ErrorMessage.Text = ex.Message;
                    }
                    finally
                    {
                        cmdinsert.Dispose();
                        if (con != null)
                        {
                            con.Close();
                        }
                    }

                    //// Copy Default User Image
                    //string oldPath = Server.MapPath("~/Images/Users/user.jpg");
                    //string newPath = Server.MapPath("~/Images/Users/" + sUserName + ".jpg");
                    //System.IO.File.Copy(oldPath, newPath);

                    var webClient = new WebClient();
                    using (var fileStream = webClient.OpenRead("https://graph.facebook.com/" + Session["fbUserId"].ToString() + "/picture?type=large"))
                    {
                        //Bitmap bmp = new Bitmap(fileStream);
                        Bitmap bmp = ResizeImage(fileStream, 180, 241);
                        bmp.Save(Server.MapPath("~/Images/Users/" + sUserName + ".jpg"), ImageFormat.Jpeg);
                    }


                    FacebookLoginHelper helper = new FacebookLoginHelper();
                    if (Request.Params.AllKeys.Contains("code"))
                    {
                        Dictionary<string, string> dicAccessToken = helper.GetAccessToken(Request["code"].ToString(), FacebookApp.Scope(), FacebookApp.RegRedirectUrl());

                        var accessToken = dicAccessToken["access_token"];
                        // Write Friend IDs from JSON
                        var json = new WebClient().DownloadString("https://api.facebook.com/method/friends.getAppUsers?access_token=" + accessToken + "&format=json");
                        var jss = new JavaScriptSerializer();
                        dynamic data = jss.Deserialize<dynamic>(json);

                        con.Open();

                        foreach (dynamic friend in data)
                        {
                            SqlCommand cmdfollow = new SqlCommand();
                            cmdfollow.CommandType = CommandType.StoredProcedure;
                            cmdfollow.CommandText = "User_Follow_Facebook";
                            cmdfollow.Parameters.Add("@UserFbId", SqlDbType.VarChar, 100).Value = Session["fbUserId"].ToString();
                            cmdfollow.Parameters.Add("@FriendFbId", SqlDbType.VarChar, 100).Value = friend;
                            cmdfollow.Connection = con;

                            cmdfollow.ExecuteNonQuery();
                            cmdfollow.Dispose();

                        }

                        if (con != null)
                        {
                            con.Close();
                        }

                    }

                    // Log User In
                    FormsAuthentication.SetAuthCookie(sUserName, false);
                    Response.Redirect("Default.aspx");

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
    }
}