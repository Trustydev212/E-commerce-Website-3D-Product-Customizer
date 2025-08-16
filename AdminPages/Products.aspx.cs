using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminPages_Products : System.Web.UI.Page
{
    private ProductDAL productDAL = new ProductDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadProducts();
        }
    }

    private void LoadProducts()
    {
        try
        {
            List<Product> products = productDAL.GetAllProducts();
            gvProducts.DataSource = products;
            gvProducts.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>alert('Lỗi tải danh sách sản phẩm: {0}');</script>", ex.Message));
        }
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProductEdit.aspx");
    }

    protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int productId = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "EditProduct")
        {
            Response.Redirect(string.Format("ProductEdit.aspx?id={0}", productId));
        }
        else if (e.CommandName == "DeleteProduct")
        {
            try
            {
                bool result = productDAL.DeleteProduct(productId);
                if (result)
                {
                    Response.Write("<script>alert('Xóa sản phẩm thành công!');</script>");
                    LoadProducts();
                }
                else
                {
                    Response.Write("<script>alert('Xóa sản phẩm thất bại!');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('Lỗi xóa sản phẩm: {0}');</script>", ex.Message));
            }
        }
    }
}