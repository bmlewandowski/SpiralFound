using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;

namespace SpiralFound
{
    public partial class Item : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateLabels();
            }

            if (Request.IsAuthenticated == true)
            {
                // Show User Commands
                this.user_command_panel.Visible = true;


                if (hyp_AuthorName.Text.Trim().ToLower() == HttpContext.Current.User.Identity.Name.ToLower())
                {
                    // Show Author Panel
                    this.author_command_panel.Visible = true;
                }
            }

            else
            {
                // User is not logged in
                this.txt_comment.Visible = false;
                this.imb_addComment.Visible = false;
            }

        }


        // Backend copy of Frontend Masonry JS call
        public void Masonry()
        {
            string script = " $(function () { var $container = $('#container'); $container.imagesLoaded(function () { $container.masonry({ itemSelector: '.item' }); }); });";
            ToolkitScriptManager.RegisterStartupScript(this, GetType(), "masonry", script, true);
        }


        public void PopulateLabels()
        {
            // Grab Querystring

            string qString;

            qString = Request.QueryString["ID"];

            if (string.IsNullOrEmpty(qString))
            {
                // Get From Route
                qString = Page.RouteData.Values["ID"] as string;
            }


            // Update Select Parameter
            Sql_Items.SelectParameters["itemId"].DefaultValue = qString;
            Sql_Comments.SelectParameters["itemId"].DefaultValue = qString;

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT itemId, itemName, itemDescription, itemUrl, groupId, tbl_user.userId, tbl_item.userId, userName  FROM tbl_item, tbl_user WHERE tbl_user.userId = tbl_item.userId AND itemId = '" + qString + "'", sqlConn);
            cmd.Connection.Open();

            SqlDataReader SqlDrItemList;
            SqlDrItemList = cmd.ExecuteReader();
            while (SqlDrItemList.Read())
            {
                string ItemTitle = SqlDrItemList["itemName"].ToString();
                string ItemDescription = SqlDrItemList["itemDescription"].ToString();
                string AuthorName = SqlDrItemList["userName"].ToString();
                string ItemUrl = SqlDrItemList["itemUrl"].ToString();
                this.lit_itemUrl.Text = ItemUrl;
                this.lit_itemId.Text = SqlDrItemList["itemId"].ToString();
                this.lit_groupId.Text = SqlDrItemList["groupId"].ToString();
                this.hyp_AuthorName.Text = AuthorName;
                this.hyp_AuthorName.NavigateUrl = "/" + AuthorName;
                this.imb_author.PostBackUrl = "/" + AuthorName;
                this.txt_rename.Text = ItemTitle;
                this.txt_renameDescription.Text = ItemDescription;


                // Set Image Control URL 
                imb_author.ImageUrl = "~/Images/Users/" + AuthorName + ".jpg";

                // Set Edit Controls
                //this.txt_itemTitle.Text = ItemTitle;
                //this.txt_itemDescription.Text = ItemDescription;

            }

        }

        protected void imb_addComment_Click(object sender, ImageClickEventArgs e)
        {
            if (Page.IsValid == true)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Item_Comment";
                cmd.Parameters.Add("@ItemId", SqlDbType.Int).Value = Int32.Parse(this.lit_itemId.Text);
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Int32.Parse(Session["UserID"].ToString());
                cmd.Parameters.Add("@CommentText", SqlDbType.VarChar, 420).Value = this.txt_comment.Text;

                cmd.Connection = con;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    this.txt_comment.Text = "";
                    Rep_Comments.DataBind();
                    Masonry();
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

        protected void btn_rename_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                string sitemName = txt_rename.Text.Trim();
                string sitemDescription = txt_renameDescription.Text.Trim();
                int itemId = Int32.Parse(lit_itemId.Text.Trim());

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Item_Update";
                cmd.Parameters.Add("@Itemname", SqlDbType.VarChar, 255).Value = sitemName;
                cmd.Parameters.Add("@Itemdescription", SqlDbType.VarChar, 420).Value = sitemDescription;
                cmd.Parameters.Add("@ItemId", SqlDbType.Int).Value = itemId;
                cmd.Connection = con;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
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
                    // Rebind to update Name and Description

                    Rep_Items.DataBind();
                    // this.commands-boxedit.Visible = false;
                }


            }
        }


        protected void DeleteItem_Click(object sender, EventArgs e)
        {
            int ItemId = Int32.Parse(lit_itemId.Text.Trim());

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Item_Delete";
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = ItemId;
            cmd.Connection = con;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
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

                // Send to Book
                string groupId = lit_groupId.Text;
                Response.Redirect("/Page/" + groupId);
            }

        }

        protected void CopyItem_Click(object sender, EventArgs e)
        {
            int itemId = Int32.Parse(lit_itemId.Text.Trim());
            int userId = Int32.Parse(Session["UserID"].ToString());

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Item_Copy";
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = itemId;
            cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
            cmd.Connection = con;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
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

                // Send to user page
                Response.Redirect("/" + HttpContext.Current.User.Identity.Name);
            }

        }

    }
}