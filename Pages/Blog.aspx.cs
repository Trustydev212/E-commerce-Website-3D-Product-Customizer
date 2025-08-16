using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Blog : System.Web.UI.Page
{
    private const int PageSize = 6;
    private int currentPage = 1;
    private int totalPages = 1;
    private string searchKeyword = "";
    private string sortBy = "newest";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtSearch.Attributes.Add("placeholder", "Tìm kiếm bài viết...");
            txtNewsletterEmail.Attributes.Add("placeholder", "Email của bạn");
            
            LoadPageParameters();
            LoadPosts();
            LoadRecentPosts();
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

        // Get sort by
        if (Request.QueryString["sort"] != null)
        {
            sortBy = Request.QueryString["sort"];
            ddlSortBy.SelectedValue = sortBy;
        }
    }

    private void LoadPosts()
    {
        try
        {
            PostDAL postDAL = new PostDAL();
            
            // Get total count for pagination
            int totalRecords = postDAL.GetPostCount(searchKeyword, true);
            totalPages = (int)Math.Ceiling((double)totalRecords / PageSize);
            
            // Get posts for current page
            List<Post> posts = postDAL.GetPosts(currentPage, PageSize, searchKeyword, sortBy, true);
            
            // DEBUG: Hiển thị số lượng bài viết trả về
            System.Diagnostics.Debug.WriteLine("Total posts: " + posts.Count);

            if (posts != null && posts.Count > 0)
            {
                rptPosts.DataSource = posts;
                rptPosts.DataBind();
                pnlNoPosts.Visible = false;
            }
            else
            {
                rptPosts.DataSource = null;
                rptPosts.DataBind();
                pnlNoPosts.Visible = true;
            }
            
            // Update pagination controls
            UpdatePaginationControls();
        }
        catch (Exception ex)
        {
            pnlNoPosts.Visible = true;
            pnlNoPosts.Controls.Add(new Literal { Text = "<div class='alert alert-danger'>" + ex.ToString() + "</div>" });
        }
    }

    private void LoadRecentPosts()
    {
        try
        {
            PostDAL postDAL = new PostDAL();
            List<Post> recentPosts = postDAL.GetRecentPosts(5, true);
            
            if (recentPosts != null && recentPosts.Count > 0)
            {
                rptRecentPosts.DataSource = recentPosts;
                rptRecentPosts.DataBind();
            }
        }
        catch (Exception ex)
        {
            // Log error
            LogError(ex);
        }
    }

    private void UpdatePaginationControls()
    {
        lblCurrentPage.Text = currentPage.ToString();
        lblTotalPages.Text = totalPages.ToString();
        
        lnkPrevious.Enabled = currentPage > 1;
        lnkNext.Enabled = currentPage < totalPages;
        
        if (!lnkPrevious.Enabled)
            lnkPrevious.CssClass = "page-link disabled";
        else
            lnkPrevious.CssClass = "page-link";
            
        if (!lnkNext.Enabled)
            lnkNext.CssClass = "page-link disabled";
        else
            lnkNext.CssClass = "page-link";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        searchKeyword = txtSearch.Text.Trim();
        currentPage = 1;
        RedirectToPage();
    }

    protected void ddlSortBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        sortBy = ddlSortBy.SelectedValue;
        currentPage = 1;
        RedirectToPage();
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        if (currentPage > 1)
        {
            currentPage--;
            RedirectToPage();
        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        if (currentPage < totalPages)
        {
            currentPage++;
            RedirectToPage();
        }
    }

    protected void btnSubscribe_Click(object sender, EventArgs e)
    {
        string email = txtNewsletterEmail.Text.Trim();
        
        if (string.IsNullOrEmpty(email))
        {
            // Show error message
            return;
        }
        
        // TODO: Implement newsletter subscription logic
        // For now, just show success message
        txtNewsletterEmail.Text = "";
        // You could show a success message here
    }

    private void RedirectToPage()
    {
        string url = "Blog.aspx?page=" + currentPage;
        
        if (!string.IsNullOrEmpty(searchKeyword))
            url += "&search=" + HttpUtility.UrlEncode(searchKeyword);
            
        if (sortBy != "newest")
            url += "&sort=" + sortBy;
            
        Response.Redirect(url);
    }

    private void LogError(Exception ex)
    {
        try
        {
            OrderDAL orderDAL = new OrderDAL();
            orderDAL.LogAdminAction(0, "Blog Error", ex.Message + " - " + ex.StackTrace);
        }
        catch
        {
            // Ignore logging errors
        }
    }
}