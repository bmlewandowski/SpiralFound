using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.IO;
using AjaxControlToolkit;

namespace SpiralFound
{
    public partial class Page : System.Web.UI.Page
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
            Sql_PageItems.SelectParameters["groupId"].DefaultValue = qString;
            Sql_Comments.SelectParameters["groupId"].DefaultValue = qString;

            // Update Literal for Reference
            this.lit_groupId.Text = qString;

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT tbl_collection.collectionId, collectionName, tbl_group.collectionId, groupName, groupDescription, tbl_user.userId, tbl_group.userId, userName  FROM tbl_collection, tbl_group, tbl_user WHERE tbl_user.userId = tbl_group.userId AND tbl_collection.collectionId = tbl_group.collectionId AND groupId = '" + qString + "'", sqlConn);
            cmd.Connection.Open();

            SqlDataReader SqlDrItemList;
            SqlDrItemList = cmd.ExecuteReader();
            while (SqlDrItemList.Read())
            {
                string GroupTitle = SqlDrItemList["groupName"].ToString();
                string GroupDescription = SqlDrItemList["groupDescription"].ToString();
                string AuthorName = SqlDrItemList["userName"].ToString();
                string CollectionId = SqlDrItemList["collectionId"].ToString();
                string CollectionName = SqlDrItemList["collectionName"].ToString();

                this.lit_collectionId.Text = CollectionId;
                this.lit_collectionName.Text = CollectionName;
                this.lit_GroupTitle.Text = Helper.Trim(GroupTitle, 30);
                //this.txt_renamePage.Text = PageTitle;
                this.lit_GroupDescription.Text = Helper.Trim(GroupDescription, 420);
                //this.txt_renameDescription.Text = PageDescription;
                this.hyp_AuthorName.Text = AuthorName;
                this.hyp_AuthorName.NavigateUrl = "/" + AuthorName;
                this.imb_author.PostBackUrl = "/" + AuthorName;
                this.txt_rename.Text = GroupTitle;
                this.txt_renameDescription.Text = GroupDescription;
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
                cmd.CommandText = "Group_Comment";
                cmd.Parameters.Add("@GroupId", SqlDbType.Int).Value = Int32.Parse(this.lit_groupId.Text);
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Int32.Parse(Session["UserID"].ToString());
                cmd.Parameters.Add("@CommentText", SqlDbType.VarChar, 420).Value = this.txt_comment.Text;

                cmd.Connection = con;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    this.txt_comment.Text = "";
                    Rep_Comments.DataBind();
                    ListView_PageItems.DataBind();
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
                string sgroupName = txt_rename.Text.Trim();
                string sgroupDescription = txt_renameDescription.Text.Trim();
                int groupId = Int32.Parse(lit_groupId.Text.Trim());

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Group_Update";
                cmd.Parameters.Add("@Groupname", SqlDbType.VarChar, 255).Value = sgroupName;
                cmd.Parameters.Add("@Groupdescription", SqlDbType.VarChar, 420).Value = sgroupDescription;
                cmd.Parameters.Add("@GroupId", SqlDbType.Int).Value = groupId;
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
                    ListView_PageItems.DataBind();
                    Masonry();
                }


            }
        }

        protected void btn_add_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                string sItemUrl = txt_itemUrl.Text.Trim();
                string sTitle = txt_itemTitle.Text.Trim();
                string sDescription = txt_itemDescription.Text.Trim();
                int GroupId = Int32.Parse(lit_groupId.Text.Trim());
                int UserId = Int32.Parse(Session["UserID"].ToString());

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Item_Create";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.Add("@GroupId", SqlDbType.Int).Value = GroupId;
                cmd.Parameters.Add("@ItemUrl", SqlDbType.VarChar, 255).Value = sItemUrl;
                cmd.Parameters.Add("@ItemTypeId", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@ItemName", SqlDbType.VarChar, 100).Value = sTitle;
                cmd.Parameters.Add("@ItemDescription", SqlDbType.VarChar, 255).Value = sDescription;
                cmd.Parameters.Add("@ItemPrivate", SqlDbType.Bit).Value = false;
                cmd.Parameters.Add("@ItemLayout", SqlDbType.Int).Value = 1;

                cmd.Connection = con;

                try
                {
                    con.Open();
                    Int32 newItemId = Convert.ToInt32(cmd.ExecuteScalar());


                    if (this.rbn_grabSnapshot.Checked == true)
                    {

                        PS.OnlineImageOptimizer.ImageOptimizer op = new PS.OnlineImageOptimizer.ImageOptimizer();
                        op.ImgQuality = 100;
                        op.MaxHeight = 640;
                        op.MaxWidth = 640;

                        Bitmap bmp = GetImage.GetWebSiteThumbnail(sItemUrl, 1024, 1024, 640, 640);
                        bmp.Save(Server.MapPath("~/images/items/" + newItemId + ".jpg"), ImageFormat.Jpeg);
                        op.Optimize(Server.MapPath("~/images/items/" + newItemId + ".jpg"));


                        PS.OnlineImageOptimizer.ImageOptimizer tb = new PS.OnlineImageOptimizer.ImageOptimizer();
                        tb.ImgQuality = 100;
                        tb.MaxHeight = 300;
                        tb.MaxWidth = 170;

                        Bitmap thb = GetImage.GetWebSiteThumbnail(sItemUrl, 1024, 1024, 200, 200);
                        thb.Save(Server.MapPath("~/images/items/" + newItemId + "_s.jpg"), ImageFormat.Jpeg);
                        tb.Optimize(Server.MapPath("~/images/items/" + newItemId + "_s.jpg"));
                    }

                    if (this.rbn_uploadImage.Checked == true)
                    {
                        if (upl_itemImage.HasFile)
                        {
                            Bitmap bmp = ResizeImage(upl_itemImage.PostedFile.InputStream, 600, 600);
                            bmp.Save(Server.MapPath("~/images/items/" + newItemId + ".jpg"), ImageFormat.Jpeg);
                            Bitmap thb = ResizeImage(upl_itemImage.PostedFile.InputStream, 170, 600);
                            thb.Save(Server.MapPath("~/images/upload/items/" + newItemId + "_s.jpg"), ImageFormat.Jpeg);
                        }
                    }


                    if (this.rbn_grabPicture.Checked == true)
                    {
                        var webClient = new WebClient();
                        using (var fileStream = webClient.OpenRead(this.txt_imageUrl.Text))
                        {
                            Bitmap bmp = ResizeImage(fileStream, 640, 3000);
                            bmp.Save(Server.MapPath("~/images/items/" + newItemId + ".jpg"), ImageFormat.Jpeg);

                        }


                        var webClientthb = new WebClient();
                        using (var fileStream = webClientthb.OpenRead(this.txt_imageUrl.Text))
                        {
                            Bitmap thb = ResizeImage(fileStream, 170, 1000);
                            thb.Save(Server.MapPath("~/images/items/" + newItemId + "_s.jpg"), ImageFormat.Jpeg);
                        }
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

                    txt_itemUrl.Text = "";
                    txt_itemTitle.Text = "";
                    txt_itemDescription.Text = "";
                    //box_add.Visible = false;
                    ListView_PageItems.DataBind();
                    Masonry();
                }


            }
        }

        protected void rbn_uploadImage_CheckedChanged(object sender, EventArgs e)
        {
            this.upl_itemImage.Enabled = true;
            this.val_reqimage.Enabled = true;
            this.val_validimage.Enabled = true;
            this.val_imageAddress.Enabled = false;
            this.txt_imageUrl.Enabled = false;
            Masonry();
        }

        protected void rbn_grabPicture_CheckedChanged(object sender, EventArgs e)
        {
            this.upl_itemImage.Enabled = false;
            this.val_reqimage.Enabled = false;
            this.val_validimage.Enabled = false;
            this.val_imageAddress.Enabled = true;
            this.txt_imageUrl.Enabled = true;
            Masonry();
        }

        protected void rbn_grabSnapshot_CheckedChanged(object sender, EventArgs e)
        {
            this.upl_itemImage.Enabled = false;
            this.val_reqimage.Enabled = false;
            this.val_validimage.Enabled = false;
            this.val_imageAddress.Enabled = false;
            this.txt_imageUrl.Enabled = false;
            Masonry();
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


        protected void DeletePage_Click(object sender, EventArgs e)
        {
            int GroupId = Int32.Parse(lit_groupId.Text.Trim());

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Group_Delete";
            cmd.Parameters.Add("@groupId", SqlDbType.Int).Value = GroupId;
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
                string collectionId = lit_collectionId.Text;
                Response.Redirect("/Book/" + collectionId);
            }

        }

        protected void CopyPage_Click(object sender, EventArgs e)
        {
            int groupId = Int32.Parse(lit_groupId.Text.Trim());
            int userId = Int32.Parse(Session["UserID"].ToString());

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Group_Copy";
            cmd.Parameters.Add("@groupId", SqlDbType.Int).Value = groupId;
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