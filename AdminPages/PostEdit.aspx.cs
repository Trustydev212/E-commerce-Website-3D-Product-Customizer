using System;
using System.Web.UI;
using App_Code;

public partial class AdminPages_PostEdit : System.Web.UI.Page
{
    private PostDAL postDAL = new PostDAL();
    private int postId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                if (int.TryParse(Request.QueryString["id"], out postId))
                {
                    LoadPostData();
                    lblTitle.Text = "Chỉnh sửa bài viết";
                }
                else
                {
                    Response.Redirect("Posts.aspx");
                }
            }
            else
            {
                lblTitle.Text = "Thêm bài viết mới";
                lblNoImage.Visible = true;
                imgPreview.Visible = false;
            }
        }
    }

    private void LoadPostData()
    {
        try
        {
            Post post = postDAL.GetPostById(postId);
            if (post != null)
            {
                txtTitle.Text = post.Title;
                txtSummary.Text = post.Summary;
                txtContent.Text = post.Content;
                ddlCategory.SelectedValue = post.Category;
                txtAuthor.Text = post.Author;
                txtPublishedDate.Text = post.PublishedDate.ToString("yyyy-MM-dd");
                cbIsPublished.Checked = post.IsPublished;
                if (!string.IsNullOrEmpty(post.ImagePath))
                {
                    lblCurrentImage.Text = "Ảnh hiện tại: " + post.ImagePath;
                    imgPreview.ImageUrl = "~/" + post.ImagePath;
                    imgPreview.Visible = true;
                    lblNoImage.Visible = false;
                }
                else
                {
                    lblNoImage.Visible = true;
                    imgPreview.Visible = false;
                }
            }
            else
            {
                Response.Redirect("Posts.aspx");
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('Lỗi tải dữ liệu bài viết: " + ex.Message + "');</script>");
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Post post = new Post();
                post.Title = txtTitle.Text.Trim();
                post.Summary = txtSummary.Text.Trim();
                post.Content = txtContent.Text.Trim();
                post.Category = ddlCategory.SelectedValue;
                post.Author = txtAuthor.Text.Trim();
                post.IsPublished = cbIsPublished.Checked;
                DateTime pd;
                post.PublishedDate = DateTime.TryParse(txtPublishedDate.Text, out pd) ? pd : DateTime.Now;
                post.IsActive = true;

                // Handle image upload
                string imagePath = HandleImageUpload();
                if (!string.IsNullOrEmpty(imagePath))
                {
                    post.ImagePath = imagePath;
                }
                else if (Request.QueryString["id"] != null)
                {
                    // Keep existing image for edit
                    Post existingPost = postDAL.GetPostById(int.Parse(Request.QueryString["id"]));
                    if (existingPost != null)
                    {
                        post.ImagePath = existingPost.ImagePath;
                    }
                }

                bool result = false;
                if (Request.QueryString["id"] != null)
                {
                    // Update existing post
                    post.Id = int.Parse(Request.QueryString["id"]);
                    result = postDAL.UpdatePost(post);
                }
                else
                {
                    // Create new post
                    post.CreatedDate = DateTime.Now;
                    int newId = postDAL.InsertPost(post);
                    result = newId > 0;
                }

                if (result)
                {
                    Response.Redirect("Posts.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Lưu bài viết thất bại!');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Lỗi lưu bài viết: " + ex.Message + "');</script>");
            }
        }
    }

    private string HandleImageUpload()
    {
        if (fuImage.HasFile)
        {
            try
            {
                string fileName = fuImage.FileName;
                string fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".gif")
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    string uploadPath = Server.MapPath("~/Upload/Images/Posts/");
                    if (!System.IO.Directory.Exists(uploadPath))
                    {
                        System.IO.Directory.CreateDirectory(uploadPath);
                    }
                    string fullPath = System.IO.Path.Combine(uploadPath, uniqueFileName);
                    fuImage.SaveAs(fullPath);
                    return "/Upload/Images/Posts/" + uniqueFileName;
                }
                else
                {
                    Response.Write("<script>alert('Chỉ chấp nhận file hình ảnh (.jpg, .jpeg, .png, .gif)');</script>");
                    return "";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Lỗi upload ảnh: " + ex.Message + "');</script>");
                return "";
            }
        }
        return "";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Posts.aspx");
    }
} 