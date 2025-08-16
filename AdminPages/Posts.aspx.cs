 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code;

public partial class AdminPages_Posts : System.Web.UI.Page
{
    private PostDAL postDAL = new PostDAL();
    private const int PageSize = 10;
    private int currentPage = 1;
    private string searchKeyword = "";
    private string categoryFilter = "";
    private string statusFilter = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadPageParameters();
            LoadPosts();
        }
    }

    private void LoadPageParameters()
    {
        // Get page number from query string
        if (Request.QueryString["page"] != null)
        {
            int.TryParse(Request.QueryString["page"], out currentPage);
            if (currentPage < 1) currentPage = 1;
        }

        // Get search keyword
        if (Request.QueryString["search"] != null)
        {
            searchKeyword = Request.QueryString["search"];
            txtSearch.Text = searchKeyword;
        }

        // Get category filter
        if (Request.QueryString["category"] != null)
        {
            categoryFilter = Request.QueryString["category"];
            ddlCategory.SelectedValue = categoryFilter;
        }

        // Get status filter
        if (Request.QueryString["status"] != null)
        {
            statusFilter = Request.QueryString["status"];
            ddlStatus.SelectedValue = statusFilter;
        }
    }

    private void LoadPosts()
    {
        try
        {
            // Get all posts (not just published ones for admin)
            List<Post> allPosts = postDAL.GetPosts(currentPage, PageSize, searchKeyword, "newest", false);
            
            // Apply filters
            var filteredPosts = allPosts.AsQueryable();
            
            if (!string.IsNullOrEmpty(categoryFilter))
            {
                filteredPosts = filteredPosts.Where(p => p.Category == categoryFilter);
            }
            
            if (!string.IsNullOrEmpty(statusFilter))
            {
                bool isPublished = statusFilter == "1";
                filteredPosts = filteredPosts.Where(p => p.IsPublished == isPublished);
            }
            
            // Bind to grid
            gvPosts.DataSource = filteredPosts.ToList();
            gvPosts.DataBind();
            
            // Update pagination info
            int totalRecords = postDAL.GetPostCount(searchKeyword, false);
            int totalPages = (int)Math.Ceiling((double)totalRecords / PageSize);
            
            lblPageInfo.Text = string.Format("Trang {0} của {1} ({2} bài viết)", currentPage, totalPages, totalRecords);
            
            btnPrevious.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;
        }
        catch (Exception ex)
        {
            // Log error
            LogError(ex);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        searchKeyword = txtSearch.Text.Trim();
        currentPage = 1;
        RedirectToPage();
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        categoryFilter = ddlCategory.SelectedValue;
        currentPage = 1;
        RedirectToPage();
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        statusFilter = ddlStatus.SelectedValue;
        currentPage = 1;
        RedirectToPage();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        searchKeyword = "";
        categoryFilter = "";
        statusFilter = "";
        currentPage = 1;
        RedirectToPage();
    }

    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        if (currentPage > 1)
        {
            currentPage--;
            RedirectToPage();
        }
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        currentPage++;
        RedirectToPage();
    }

    protected void gvPosts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int postId = Convert.ToInt32(e.CommandArgument);
            
            if (e.CommandName == "TogglePublish")
            {
                TogglePostPublishStatus(postId);
            }
            else if (e.CommandName == "DeletePost")
            {
                DeletePost(postId);
            }
            
            LoadPosts(); // Reload the grid
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }

    protected void gvPosts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Add any row-specific logic here
        }
    }

    private void TogglePostPublishStatus(int postId)
    {
        try
        {
            Post post = postDAL.GetPostById(postId);
            if (post != null)
            {
                post.IsPublished = !post.IsPublished;
                bool success = postDAL.UpdatePost(post);
                
                if (success)
                {
                    // Log admin action
                    LogAdminAction("Toggle Post Publish", string.Format("Post ID: {0}, New Status: {1}", postId, post.IsPublished ? "Published" : "Unpublished"));
                }
            }
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }

    private void DeletePost(int postId)
    {
        try
        {
            bool success = postDAL.DeletePost(postId);
            
            if (success)
            {
                // Log admin action
                LogAdminAction("Delete Post", string.Format("Post ID: {0}", postId));
            }
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }

    private void RedirectToPage()
    {
        string url = "Posts.aspx?page=" + currentPage;
        
        if (!string.IsNullOrEmpty(searchKeyword))
            url += "&search=" + HttpUtility.UrlEncode(searchKeyword);
            
        if (!string.IsNullOrEmpty(categoryFilter))
            url += "&category=" + HttpUtility.UrlEncode(categoryFilter);
            
        if (!string.IsNullOrEmpty(statusFilter))
            url += "&status=" + statusFilter;
            
        Response.Redirect(url);
    }

    private void LogError(Exception ex)
    {
        try
        {
            OrderDAL orderDAL = new OrderDAL();
            orderDAL.LogAdminAction(0, "Posts Admin Error", ex.Message + " - " + ex.StackTrace);
        }
        catch
        {
            // Ignore logging errors
        }
    }

    private void LogAdminAction(string action, string description)
    {
        try
        {
            OrderDAL orderDAL = new OrderDAL();
            int adminId = Session["AdminId"] != null ? Convert.ToInt32(Session["AdminId"]) : 0;
            orderDAL.LogAdminAction(adminId, action, description);
        }
        catch
        {
            // Ignore logging errors
        }
    }
}