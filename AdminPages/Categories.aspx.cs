using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminPages_Categories : System.Web.UI.Page
{
    private CategoryDAL categoryDAL = new CategoryDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCategories();
        }
    }

    private void LoadCategories()
    {
        try
        {
            List<Category> categories = categoryDAL.GetAllCategories();
            gvCategories.DataSource = categories;
            gvCategories.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>alert('Lỗi tải danh sách danh mục: {0}');</script>", ex.Message));
        }
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("CategoryEdit.aspx");
    }

    protected void gvCategories_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int categoryId = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "EditCategory")
        {
            Response.Redirect("CategoryEdit.aspx?id=" + categoryId);
        }
        else if (e.CommandName == "DeleteCategory")
        {
            try
            {
                bool result = categoryDAL.DeleteCategory(categoryId);
                if (result)
                {
                    Response.Write("<script>alert('Xóa danh mục thành công!');</script>");
                    LoadCategories();
                }
                else
                {
                    Response.Write("<script>alert('Xóa danh mục thất bại!');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('Lỗi xóa danh mục: {0}');</script>", ex.Message));
            }
        }
    }
}