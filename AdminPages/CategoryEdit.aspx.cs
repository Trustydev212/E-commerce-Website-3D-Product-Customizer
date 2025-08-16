using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminPages_CategoryEdit : System.Web.UI.Page
{
    private CategoryDAL categoryDAL = new CategoryDAL();
    private int categoryId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                if (int.TryParse(Request.QueryString["id"], out categoryId))
                {
                    LoadCategoryData();
                    lblTitle.Text = "Chỉnh sửa danh mục";
                }
                else
                {
                    Response.Redirect("Categories.aspx");
                }
            }
            else
            {
                lblTitle.Text = "Thêm danh mục mới";
                lblNoImage.Visible = true;
                imgPreview.Visible = false;
            }
        }
    }

    private void LoadCategoryData()
    {
        try
        {
            Category category = categoryDAL.GetCategoryById(categoryId);
            if (category != null)
            {
                txtName.Text = category.Name;
                txtDescription.Text = category.Description;
                txtDisplayOrder.Text = category.DisplayOrder.ToString();
                cbIsActive.Checked = category.IsActive;
                
                if (!string.IsNullOrEmpty(category.ImagePath))
                {
                    lblCurrentImage.Text = "Hình ảnh hiện tại: " + category.ImagePath;
                    imgPreview.ImageUrl = category.ImagePath;
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
                Response.Redirect("Categories.aspx");
            }
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>alert('Lỗi tải dữ liệu danh mục: {0}');</script>", ex.Message));
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Category category = new Category();
                
                // Basic info
                category.Name = txtName.Text.Trim();
                category.Description = txtDescription.Text.Trim();
                category.DisplayOrder = int.Parse(txtDisplayOrder.Text);
                category.IsActive = cbIsActive.Checked;
                
                // Handle image upload
                string imagePath = HandleImageUpload();
                if (!string.IsNullOrEmpty(imagePath))
                {
                    category.ImagePath = imagePath;
                }
                else if (Request.QueryString["id"] != null)
                {
                    // Keep existing image for edit
                    Category existingCategory = categoryDAL.GetCategoryById(int.Parse(Request.QueryString["id"]));
                    if (existingCategory != null)
                    {
                        category.ImagePath = existingCategory.ImagePath;
                    }
                }
                
                bool result = false;
                if (Request.QueryString["id"] != null)
                {
                    // Update existing category
                    category.Id = int.Parse(Request.QueryString["id"]);
                    result = categoryDAL.UpdateCategory(category);
                }
                else
                {
                    // Create new category
                    category.CreatedDate = DateTime.Now;
                    int newId = categoryDAL.InsertCategory(category);
                    result = newId > 0;
                }
                
                if (result)
                {
                    Response.Redirect("Categories.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Lưu danh mục thất bại!');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('Lỗi lưu danh mục: {0}');</script>", ex.Message));
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
                string fileExtension = Path.GetExtension(fileName).ToLower();
                
                // Check file extension
                if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".gif")
                {
                    // Generate unique filename
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    string uploadPath = Server.MapPath("~/Upload/Images/Categories/");
                    
                    // Create directory if not exists
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    
                    string fullPath = Path.Combine(uploadPath, uniqueFileName);
                    fuImage.SaveAs(fullPath);
                    
                    return "/Upload/Images/Categories/" + uniqueFileName;
                }
                else
                {
                    Response.Write("<script>alert('Chỉ chấp nhận file hình ảnh (.jpg, .jpeg, .png, .gif)');</script>");
                    return "";
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('Lỗi upload hình ảnh: {0}');</script>", ex.Message));
                return "";
            }
        }
        return "";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Categories.aspx");
    }
}