using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default : System.Web.UI.Page
{
    private CategoryDAL categoryDAL = new CategoryDAL();
    private ProductDAL productDAL = new ProductDAL();
    private PostDAL postDAL = new PostDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadHomePageData();
        }
    }

    private void LoadHomePageData()
    {
        LoadCategories();
        LoadFeaturedProducts();
        LoadLatestPosts();
    }

    private void LoadCategories()
    {
        try
        {
            List<Category> categories = categoryDAL.GetAllCategories()
                .Where(c => c.IsActive)
                .Take(4)
                .ToList();
            
            rptCategories.DataSource = categories;
            rptCategories.DataBind();
        }
        catch (Exception ex)
        {
            // Log error
            Response.Write("<script>console.error('Error loading categories: " + ex.Message + "');</script>");
        }
    }

    private void LoadFeaturedProducts()
    {
        try
        {
            List<Product> products = productDAL.GetAllProducts()
                .Where(p => p.IsActive)
                .OrderBy(p => Guid.NewGuid())
                .Take(6)
                .ToList();
            
            rptFeaturedProducts.DataSource = products;
            rptFeaturedProducts.DataBind();
        }
        catch (Exception ex)
        {
            // Log error
            Response.Write("<script>console.error('Error loading products: " + ex.Message + "');</script>");
        }
    }

    private void LoadLatestPosts()
    {
        try
        {
            List<Post> posts = postDAL.GetPublishedPosts()
                .Take(3)
                .ToList();
            
            rptLatestPosts.DataSource = posts;
            rptLatestPosts.DataBind();
        }
        catch (Exception ex)
        {
            // Log error
            Response.Write("<script>console.error('Error loading posts: " + ex.Message + "');</script>");
        }
    }
}