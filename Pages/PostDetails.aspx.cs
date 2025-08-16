using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code;

public partial class PostDetails : System.Web.UI.Page
{
    private PostDAL postDAL = new PostDAL();
    private Post currentPost;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadPostDetails();
        }
    }

    private void LoadPostDetails()
    {
        try
        {
            int postId;
            if (int.TryParse(Request.QueryString["id"], out postId))
            {
                currentPost = postDAL.GetPostById(postId);
                
                if (currentPost != null && currentPost.IsPublished)
                {
                    // Update view count
                    postDAL.IncrementViewCount(postId);
                    
                    // Set page title and meta
                    Page.Title = currentPost.Title + " - 3D T-Shirt Design Platform";
                    
                    // Display post information
                    postTitle.InnerText = currentPost.Title;
                    postDate.InnerText = currentPost.PublishedDate.ToString("dd/MM/yyyy HH:mm");
                    postAuthor.InnerText = currentPost.Author;
                    
                    // Set image if exists
                    if (!string.IsNullOrEmpty(currentPost.ImagePath))
                    {
                        postImage.Src = currentPost.ImagePath;
                        postImage.Alt = currentPost.Title;
                    }
                    else
                    {
                        postImage.Style.Add("display", "none");
                    }
                    
                    // Display content
                    litContent.Text = currentPost.Content;
                    
                    // Load related posts
                    LoadRelatedPosts();
                    LoadPopularPosts();
                }
                else
                {
                    Response.Redirect("Blog.aspx");
                }
            }
            else
            {
                Response.Redirect("Blog.aspx");
            }
        }
        catch (Exception ex)
        {
            // Log error
            LogError(ex);
            Response.Redirect("Blog.aspx");
        }
    }

    private void LoadRelatedPosts()
    {
        try
        {
            if (currentPost != null)
            {
                List<Post> relatedPosts = postDAL.GetRelatedPosts(currentPost.Id, currentPost.Category, 3);
                
                if (relatedPosts != null && relatedPosts.Count > 0)
                {
                    rptRelatedPosts.DataSource = relatedPosts;
                    rptRelatedPosts.DataBind();
                    pnlRelatedPosts.Visible = true;
                }
                else
                {
                    pnlRelatedPosts.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            // Log error but don't break the page
            LogError(ex);
            pnlRelatedPosts.Visible = false;
        }
    }

    private void LoadPopularPosts()
    {
        try
        {
            List<Post> popularPosts = postDAL.GetPopularPosts(5, true);
            
            if (popularPosts != null && popularPosts.Count > 0)
            {
                rptPopularPosts.DataSource = popularPosts;
                rptPopularPosts.DataBind();
            }
        }
        catch (Exception ex)
        {
            // Log error but don't break the page
            LogError(ex);
        }
    }

    private void LogError(Exception ex)
    {
        try
        {
            OrderDAL orderDAL = new OrderDAL();
            orderDAL.LogAdminAction(0, "PostDetails Error", ex.Message + " - " + ex.StackTrace);
        }
        catch
        {
            // Ignore logging errors
        }
    }
} 