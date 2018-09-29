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
    public partial class Book : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // First Load Events
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
            // Grab QueryString Parameter
            string qString;

            qString = Request.QueryString["ID"];

            if (string.IsNullOrEmpty(qString))
            {
                // Get from Route
                qString = Page.RouteData.Values["ID"] as string;
            }

            // Update Select Parameter
            Sql_BookPages.SelectParameters["collectionId"].DefaultValue = qString;
            Sql_Comments.SelectParameters["collectionId"].DefaultValue = qString;

            // Update Literal for Reference
            this.lit_collectionId.Text = qString;


            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT tbl_collection.collectionId, collectionName, collectionDescription, tbl_collection.userId, tbl_user.userId, userName  FROM tbl_collection, tbl_user WHERE tbl_user.userId = tbl_collection.userId AND collectionId = '" + qString + "'", sqlConn);
            cmd.Connection.Open();

            SqlDataReader SqlDrItemList;
            SqlDrItemList = cmd.ExecuteReader();
            while (SqlDrItemList.Read())
            {
                string bookName = SqlDrItemList["collectionName"].ToString();
                string bookDescription = SqlDrItemList["collectionDescription"].ToString();
                string AuthorName = SqlDrItemList["userName"].ToString();
                string bookId = SqlDrItemList["collectionId"].ToString();

                this.lit_collectionId.Text = bookId;
                this.lit_collectionName.Text = Helper.Trim(bookName, 30);
                this.lit_collectionDescription.Text = Helper.Trim(bookDescription, 420);
                this.hyp_AuthorName.Text = AuthorName;
                this.hyp_AuthorName.NavigateUrl = "/" + AuthorName;
                this.imb_author.PostBackUrl = "/" + AuthorName;
                this.txt_rename.Text = bookName;
                this.txt_renameDescription.Text = bookDescription;
                // Set Image Control URL 
                imb_author.ImageUrl = "~/Images/Users/" + AuthorName + ".jpg";
            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();

        }


        protected void imb_addComment_Click(object sender, ImageClickEventArgs e)
        {
            if (Page.IsValid == true)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Collection_Comment";
                cmd.Parameters.Add("@CollectionId", SqlDbType.Int).Value = Int32.Parse(this.lit_collectionId.Text);
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Int32.Parse(Session["UserID"].ToString());
                cmd.Parameters.Add("@CommentText", SqlDbType.VarChar, 420).Value = this.txt_comment.Text;

                cmd.Connection = con;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    this.txt_comment.Text = "";
                    Rep_Comments.DataBind();
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
                string scollectionName = txt_rename.Text.Trim();
                string scollectionDescription = txt_renameDescription.Text.Trim();
                int CollectionId = Int32.Parse(lit_collectionId.Text.Trim());

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Collection_Update";
                cmd.Parameters.Add("@Collectionname", SqlDbType.VarChar, 255).Value = scollectionName;
                cmd.Parameters.Add("@Collectiondescription", SqlDbType.VarChar, 420).Value = scollectionDescription;
                cmd.Parameters.Add("@CollectionId", SqlDbType.Int).Value = CollectionId;
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
                    // this.commands-boxedit.Visible = false;
                }


            }
        }

        protected void btn_add_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                string sgroupName = txt_add.Text.Trim();
                string sgroupDescription = txt_addDescription.Text.Trim();
                int CollectionId = Int32.Parse(lit_collectionId.Text.Trim());

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Group_Create";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Int32.Parse(Session["UserID"].ToString());
                cmd.Parameters.Add("@CollectionId", SqlDbType.Int).Value = CollectionId;
                cmd.Parameters.Add("@GroupName", SqlDbType.VarChar, 255).Value = sgroupName;
                cmd.Parameters.Add("@GroupDescription", SqlDbType.VarChar, 420).Value = sgroupDescription;
                cmd.Parameters.Add("@GroupPrivate", SqlDbType.Bit).Value = false;
                cmd.Parameters.Add("@GroupLayout", SqlDbType.Int).Value = "1";

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
                    this.txt_add.Text = "";
                    this.txt_addDescription.Text = "";
                    // Rebind Repeater
                    this.Rep_BookPages.DataBind();
                    // this.commands-boxadd.Visible = false;
                }


            }
        }


        protected void DeleteBook_Click(object sender, EventArgs e)
        {
            int CollectionId = Int32.Parse(lit_collectionId.Text.Trim());

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Collection_Delete";
            cmd.Parameters.Add("@collectionId", SqlDbType.Int).Value = CollectionId;
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

        protected void CopyBook_Click(object sender, EventArgs e)
        {
            int CollectionId = Int32.Parse(lit_collectionId.Text.Trim());
            int userId = Int32.Parse(Session["UserID"].ToString());

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Collection_Copy";
            cmd.Parameters.Add("@collectionId", SqlDbType.Int).Value = CollectionId;
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