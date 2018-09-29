using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.IO;

namespace SpiralFound
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated == true)
            {
                this.navright.Visible = true;
                this.navlogin.Visible = false;
                this.hyp_UserName.Text = HttpContext.Current.User.Identity.Name;
                this.hyp_UserName.NavigateUrl = "/" + HttpContext.Current.User.Identity.Name;
                // Set Image Control URL 
                imb_user_profile.ImageUrl = "~/Images/Users/" + HttpContext.Current.User.Identity.Name + ".jpg";
                //imb_user_profile.PostBackUrl = "User.aspx?Name=" + HttpContext.Current.User.Identity.Name;
                this.hyp_following.NavigateUrl = "/" + HttpContext.Current.User.Identity.Name + "/Following";
                this.hyp_followers.NavigateUrl = "/" + HttpContext.Current.User.Identity.Name + "/Followers";

                // Set Session if empty
                //if (Session["UserID"] == null)
                //{

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "User_Get_Details";
                cmd.Parameters.Add("@Username", SqlDbType.VarChar, 100).Value = HttpContext.Current.User.Identity.Name;
                cmd.Parameters.Add("@UserId", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@UserFollowers", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@UserFollowing", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                cmd.Connection = con;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    lit_followers.Text = cmd.Parameters["@UserFollowers"].Value.ToString();
                    lit_following.Text = cmd.Parameters["@UserFollowing"].Value.ToString();

                    // Set Session if empty
                    if (Session["UserID"] == null)
                    {
                        Session["UserID"] = cmd.Parameters["@UserId"].Value.ToString();
                    }

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
                // }


            }
            else
            {
                this.navright.Visible = false;
                this.navlogin.Visible = true;
            }

        }


        protected void lbn_logout_Click(object sender, EventArgs e)
        {

            Session["UserID"] = null;
            FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx");

        }

        protected void imb_user_profile_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/" + HttpContext.Current.User.Identity.Name);
        }


        public int Validate_Login(String Username, String Password)
        {

            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var encoding = new System.Text.ASCIIEncoding();
            byte[] pwBytes = md5.ComputeHash(encoding.GetBytes(Password));
            string passwordHash;
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            passwordHash = enc.GetString(pwBytes);

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmdselect = new SqlCommand();
            cmdselect.CommandType = CommandType.StoredProcedure;
            cmdselect.CommandText = "User_Login";
            cmdselect.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = Username;
            cmdselect.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = passwordHash;
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
                lbl_ErrorMessage.Text = ex.Message;
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


        protected void btnlogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {

                int Results = 0;
                if (txtUsername.Text != string.Empty && txtPassword.Text != string.Empty)
                {
                    Results = Validate_Login(txtUsername.Text.Trim(), txtPassword.Text.Trim());
                    if (Results == 1)
                    {


                        if (this.chk_rememberme.Checked)
                        {

                            //HttpCookie myCookie = new HttpCookie("BookPagerLogin");
                            //myCookie["BookPagerLogin"] = txtUsername.Text.Trim();
                            //myCookie.Expires = DateTime.Now.AddDays(1d);
                            //Response.Cookies.Add(myCookie);

                            // FormsAuthentication.SetAuthCookie(txtUsername.Text.Trim(), true);
                            FormsAuthentication.RedirectFromLoginPage(txtUsername.Text.Trim(), true);

                        }

                        else
                        {
                            // FormsAuthentication.SetAuthCookie(txtUsername.Text.Trim(), false);
                            FormsAuthentication.RedirectFromLoginPage(txtUsername.Text.Trim(), false);
                        }


                        //Session["UserID"] = txtUsername.Text.Trim();                      
                        // Response.Redirect("~/Default.aspx");
                    }
                    else
                    {
                        lbl_ErrorMessage.Text = "Invalid Login";
                        //Dont Give too much information this might tell a hacker what is wrong in the login
                    }
                }
                else
                {
                    lbl_ErrorMessage.Text = "Please make sure that the username and the password is Correct";
                    lbl_ErrorMessage.ForeColor = System.Drawing.Color.Red;
                }
            }

        }

    }
}