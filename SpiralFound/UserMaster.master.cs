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
    public partial class UserMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void imb_followUser_Command(object sender, CommandEventArgs e)
        {

            if (e.CommandName == "Follow")
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "User_Follow";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Int32.Parse(this.lit_authorId.Text);
                cmd.Parameters.Add("@FollowerId", SqlDbType.Int).Value = Int32.Parse(Session["UserID"].ToString());

                cmd.Connection = con;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    this.imb_followUser.ImageUrl = "~/Images/Interface/Owner/ownercard-unfollow.png";
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

                        // Reload Page to Reflect Counts
                        Response.Redirect(Request.RawUrl);
                        
                    }
                }

            }

            if (e.CommandName == "Unfollow")
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "User_Follow_Delete";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Int32.Parse(this.lit_authorId.Text);
                cmd.Parameters.Add("@FollowerId", SqlDbType.Int).Value = Int32.Parse(Session["UserID"].ToString());

                cmd.Connection = con;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    this.imb_followUser.ImageUrl = "~/Images/Interface/Owner/ownercard-follow.png";
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

                        // Reload Page to Reflect Counts
                        Response.Redirect(Request.RawUrl);
                    }
                }

            }
        }
    }
}