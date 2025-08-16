using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Products : System.Web.UI.Page
{
    private ProductDAL productDAL = new ProductDAL();
    private CategoryDAL categoryDAL = new CategoryDAL();
    
    private const int PageSize = 12;
    public int CurrentPage { get; set; }
    private string CurrentSort { get; set; }
    private string CurrentSearch { get; set; }
    private string CurrentCategory { get; set; }

    public Products()
    {
        CurrentPage = 1;
        CurrentSort = "newest";
        CurrentSearch = "";
        CurrentCategory = "";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadFilters();
            LoadProducts();
        }
    }

    private void LoadFilters()
    {
        try
        {
            // Load categories
            List<Category> categories = categoryDAL.GetAllCategories()
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToList();
            
            ddlCategory.DataSource = categories;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "Id";
            ddlCategory.DataBind();
            
            // Set from query string
            if (!string.IsNullOrEmpty(Request.QueryString["category"]))
            {
                ddlCategory.SelectedValue = Request.QueryString["category"];
                CurrentCategory = Request.QueryString["category"];
            }
            
            if (!string.IsNullOrEmpty(Request.QueryString["search"]))
            {
                txtSearch.Text = Request.QueryString["search"];
                CurrentSearch = Request.QueryString["search"];
            }
            
            if (!string.IsNullOrEmpty(Request.QueryString["sort"]))
            {
                ddlSort.SelectedValue = Request.QueryString["sort"];
                CurrentSort = Request.QueryString["sort"];
            }
            
            if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            {
                int page;
                if (int.TryParse(Request.QueryString["page"], out page))
                {
                    CurrentPage = page;
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>console.error('Error loading filters: " + ex.Message + "');</script>");
        }
    }

    private void LoadProducts()
    {
        try
        {
            List<Product> allProducts = productDAL.GetAllProducts()
                .Where(p => p.IsActive)
                .ToList();
            
            // Debug log
            Response.Write("<script>console.log('Tổng sản phẩm từ DB: " + allProducts.Count + "');</script>");
            
            // Apply filters
            if (!string.IsNullOrEmpty(CurrentCategory))
            {
                int categoryId = Convert.ToInt32(CurrentCategory);
                allProducts = allProducts.Where(p => p.CategoryId == categoryId).ToList();
                Response.Write("<script>console.log('Sau lọc category: " + allProducts.Count + "');</script>");
            }
            
            if (!string.IsNullOrEmpty(CurrentSearch))
            {
                allProducts = allProducts.Where(p => 
                    p.Name.ToLower().Contains(CurrentSearch.ToLower()) ||
                    p.Description.ToLower().Contains(CurrentSearch.ToLower())
                ).ToList();
                Response.Write("<script>console.log('Sau lọc search: " + allProducts.Count + "');</script>");
            }
            
            // Apply sorting
            switch (CurrentSort)
            {
                case "price_asc":
                    allProducts = allProducts.OrderBy(p => p.SalePrice ?? p.Price).ToList();
                    break;
                case "price_desc":
                    allProducts = allProducts.OrderByDescending(p => p.SalePrice ?? p.Price).ToList();
                    break;
                case "name_asc":
                    allProducts = allProducts.OrderBy(p => p.Name).ToList();
                    break;
                case "name_desc":
                    allProducts = allProducts.OrderByDescending(p => p.Name).ToList();
                    break;
                default:
                    allProducts = allProducts.OrderByDescending(p => p.CreatedDate).ToList();
                    break;
            }
            
            // Pagination
            int totalProducts = allProducts.Count;
            int totalPages = (int)Math.Ceiling((double)totalProducts / PageSize);
            
            List<Product> pagedProducts = allProducts
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            
            Response.Write("<script>console.log('Sản phẩm cuối cùng: " + pagedProducts.Count + "');</script>");
            
            // Bind data
            rptProducts.DataSource = pagedProducts;
            rptProducts.DataBind();
            
            rptProductsList.DataSource = pagedProducts;
            rptProductsList.DataBind();
            
            // Update UI
            lblProductCount.InnerText = totalProducts.ToString();
            
            if (pagedProducts.Count == 0)
            {
                pnlNoProducts.Visible = true;
                pnlGridView.Visible = false;
                pnlListView.Visible = false;
                Response.Write("<script>console.log('Không có sản phẩm - hiện panel thông báo');</script>");
            }
            else
            {
                pnlNoProducts.Visible = false;
                Response.Write("<script>console.log('Có sản phẩm - ẩn panel thông báo');</script>");
            }
            
            // Generate pagination
            GeneratePagination(totalPages);
        }
        catch (Exception ex)
        {
            Response.Write("<script>console.error('Error loading products: " + ex.Message + "');</script>");
        }
    }

    private void GeneratePagination(int totalPages)
    {
        if (totalPages <= 1) return;
        
        List<object> pages = new List<object>();
        
        for (int i = 1; i <= totalPages; i++)
        {
            pages.Add(new { PageNumber = i });
        }
        
        rptPagination.DataSource = pages;
        rptPagination.DataBind();
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        CurrentCategory = ddlCategory.SelectedValue;
        CurrentPage = 1;
        LoadProducts();
    }

    protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
    {
        CurrentSort = ddlSort.SelectedValue;
        CurrentPage = 1;
        LoadProducts();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        CurrentSearch = txtSearch.Text.Trim();
        CurrentPage = 1;
        LoadProducts();
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        if (ddlCategory.Items.Count > 0)
            ddlCategory.SelectedIndex = 0;
        if (ddlSort.Items.Count > 0)
            ddlSort.SelectedIndex = 0;
        txtSearch.Text = "";
        CurrentCategory = "";
        CurrentSort = "newest";
        CurrentSearch = "";
        CurrentPage = 1;
        LoadProducts();
    }

    protected void btnGridView_Click(object sender, EventArgs e)
    {
        pnlGridView.Visible = true;
        pnlListView.Visible = false;
        btnGridView.CssClass = "btn btn-outline-primary active";
        btnListView.CssClass = "btn btn-outline-primary";
    }

    protected void btnListView_Click(object sender, EventArgs e)
    {
        pnlGridView.Visible = false;
        pnlListView.Visible = true;
        btnGridView.CssClass = "btn btn-outline-primary";
        btnListView.CssClass = "btn btn-outline-primary active";
    }

    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        Response.Redirect("Products.aspx");
    }

    protected void lnkPage_Command(object sender, CommandEventArgs e)
    {
        CurrentPage = Convert.ToInt32(e.CommandArgument);
        LoadProducts();
    }

    public string IsActivePage(object pageNumber)
    {
        return Convert.ToInt32(pageNumber) == CurrentPage ? "active" : "";
    }

    [System.Web.Services.WebMethod]
    public static object AddToCart(int productId)
    {
        try
        {
            // Here you would implement the actual cart logic
            // For now, we'll return a success response
            return new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng", cartCount = 1 };
        }
        catch (Exception ex)
        {
            return new { success = false, message = ex.Message };
        }
    }
} 