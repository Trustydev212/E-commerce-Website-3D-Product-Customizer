using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code;

public partial class AdminPages_ProductEdit : System.Web.UI.Page
{
    private ProductDAL productDAL = new ProductDAL();
    private CategoryDAL categoryDAL = new CategoryDAL();
    private ProductImageDAL productImageDAL = new ProductImageDAL();

    // Use property to persist productId across postbacks
    private int ProductId
    {
        get
        {
            if (ViewState["ProductId"] != null)
                return (int)ViewState["ProductId"];
            if (Request.QueryString["id"] != null)
                return Convert.ToInt32(Request.QueryString["id"]);
            return 0;
        }
        set
        {
            ViewState["ProductId"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCategories();
            
            if (Request.QueryString["id"] != null)
            {
                int id;
                if (int.TryParse(Request.QueryString["id"], out id))
                {
                    ProductId = id;
                    LoadProductData();
                    lblTitle.Text = "Chỉnh sửa sản phẩm";
                }
                else
                {
                    Response.Redirect("Products.aspx");
                }
            }
            else
            {
                lblTitle.Text = "Thêm sản phẩm mới";
            }
        }
    }

    private void LoadCategories()
    {
        try
        {
            List<Category> categories = categoryDAL.GetAllCategories()
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToList();
            
            ddlCategory.DataSource = categories;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "Id";
            ddlCategory.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>alert('Lỗi tải danh mục: {0}');</script>", ex.Message));
        }
    }

    private void LoadProductData()
    {
        try
        {
            Product product = productDAL.GetProductById(ProductId);
            if (product != null)
            {
                txtName.Text = product.Name;
                txtDescription.Text = product.Description;
                ddlCategory.SelectedValue = product.CategoryId.ToString();
                txtPrice.Text = product.Price.ToString();
                if (product.SalePrice.HasValue)
                {
                    txtSalePrice.Text = product.SalePrice.Value.ToString();
                }
                txtStockQuantity.Text = product.StockQuantity.ToString();
                txtSize.Text = product.Size;
                txtColor.Text = product.Color;
                txtMaterial.Text = product.Material;
                cbIsActive.Checked = product.IsActive;
                
                // Display existing main image if available
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    string mainImageScript = string.Format(
                        "<script>var mainImageDisplay = document.getElementById('mainImageDisplay'); " +
                        "var mainImagePlaceholder = document.getElementById('mainImagePlaceholder'); " +
                        "if (mainImageDisplay) {{ mainImageDisplay.src = '{0}'; mainImageDisplay.style.display = 'block'; }} " +
                        "if (mainImagePlaceholder) mainImagePlaceholder.style.display = 'none';</script>", 
                        product.ImagePath);
                    Response.Write(mainImageScript);
                }
                
                // Display existing 3D model info if available
                if (!string.IsNullOrEmpty(product.ModelPath))
                {
                    string fileName = System.IO.Path.GetFileName(product.ModelPath);
                    Response.Write(string.Format("<script>var placeholder = document.getElementById('model3dPlaceholder'); var info = document.getElementById('model3dInfo'); var fileName = document.getElementById('model3dFileName'); if (placeholder) placeholder.style.display = 'none'; if (info) info.style.display = 'block'; if (fileName) fileName.textContent = '{0}';</script>", fileName));
                }
                
                // Load and display existing product images status
                LoadProductImagesStatus(product.Id);
            }
            else
            {
                Response.Redirect("Products.aspx");
            }
        }
        catch (Exception ex)
        {
            // Removed debug alert - Lỗi tải dữ liệu sản phẩm: {0}
        }
    }

    private void LoadProductImagesStatus(int productId)
    {
        try
        {
            // Get all product images
            List<ProductImage> allImages = productImageDAL.GetProductImagesByProductId(productId);
            
            // Check which colors have images
            string[] colors = { "white", "black", "red", "blue" };
            var colorStatus = new Dictionary<string, bool>();
            var colorImages = new Dictionary<string, string>();
            
            foreach (string color in colors)
            {
                var mainImage = allImages.FirstOrDefault(img => img.Color.ToLower() == color);
                bool hasImage = mainImage != null;
                colorStatus[color] = hasImage;
                colorImages[color] = hasImage ? mainImage.ImagePath : "";
            }
            
            // Generate JavaScript to update status display and image previews
            var statusScript = new System.Text.StringBuilder();
            statusScript.AppendLine("<script>");
            
            // Store image data for JavaScript
            statusScript.AppendLine("window.productImageData = {");
            foreach (string color in colors)
            {
                statusScript.AppendLine(string.Format("  '{0}': '{1}',", color, colorImages[color]));
            }
            statusScript.AppendLine("};");
            
            // Update status display
            foreach (string color in colors)
            {
                bool hasImage = colorStatus[color];
                string statusText = hasImage ? "Đã có" : "Chưa có";
                string statusClass = hasImage ? "status has-image" : "status";
                
                statusScript.AppendLine(string.Format("var statusElement = document.querySelector('[data-color=\"{0}\"] .status');", color));
                statusScript.AppendLine(string.Format("if (statusElement) {{ statusElement.textContent = '{0}'; statusElement.className = '{1}'; }}", statusText, statusClass));
            }
            
            // Show existing images for each color upload control
            foreach (string color in colors)
            {
                bool hasImage = colorStatus[color];
                if (hasImage)
                {
                    string imagePath = colorImages[color];
                    statusScript.AppendLine(string.Format("var {0}Display = document.getElementById('{0}ImageDisplay');", color));
                    statusScript.AppendLine(string.Format("var {0}Placeholder = document.getElementById('{0}ImagePlaceholder');", color));
                    statusScript.AppendLine(string.Format("if ({0}Display) {{ {0}Display.src = '{1}'; {0}Display.style.display = 'block'; }}", color, imagePath));
                    statusScript.AppendLine(string.Format("if ({0}Placeholder) {{ {0}Placeholder.style.display = 'none'; }}", color));
                }
            }
            
            statusScript.AppendLine("</script>");
            
            Response.Write(statusScript.ToString());
            
            Response.Write(string.Format("<script>console.log('Product {0} has {1} images total');</script>", productId, allImages.Count));
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>console.error('Error loading image status: {0}');</script>", ex.Message));
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Product product = new Product();
                
                // Basic info
                product.Name = txtName.Text.Trim();
                product.Description = txtDescription.Text.Trim();
                product.CategoryId = int.Parse(ddlCategory.SelectedValue);
                product.Price = decimal.Parse(txtPrice.Text);
                
                if (!string.IsNullOrEmpty(txtSalePrice.Text))
                {
                    product.SalePrice = decimal.Parse(txtSalePrice.Text);
                }
                
                product.StockQuantity = int.Parse(txtStockQuantity.Text);
                product.Size = txtSize.Text.Trim();
                product.Color = txtColor.Text.Trim();
                product.Material = txtMaterial.Text.Trim();
                product.IsActive = cbIsActive.Checked;
                
                // Handle main product image upload
                string mainImagePath = HandleImageUpload(fuMainImage, "main");
                if (!string.IsNullOrEmpty(mainImagePath))
                {
                    product.ImagePath = mainImagePath;
                }
                
                // Handle 3D model upload
                string model3DPath = Handle3DModelUpload(fu3DModel);
                if (!string.IsNullOrEmpty(model3DPath))
                {
                    product.ModelPath = model3DPath;
                }
                
                bool result = false;
                if (Request.QueryString["id"] != null)
                {
                    // Update existing product
                    product.Id = ProductId;
                    result = productDAL.UpdateProduct(product);
                    
                    if (result)
                    {
                        // Delete existing product images
                        productImageDAL.DeleteProductImages(product.Id);
                    }
                }
                else
                {
                    // Create new product
                    product.CreatedDate = DateTime.Now;
                    int newId = productDAL.InsertProduct(product);
                    result = newId > 0;
                    product.Id = newId;
                    ProductId = newId; // Set the new ID for image saving
                }
                
                if (result)
                {
                    // Save product images for each color
                    SaveProductImages(product.Id);
                    
                    Response.Redirect("Products.aspx");
                }
                else
                {
                    // Removed debug alert - Lưu sản phẩm thất bại!
                }
            }
            catch (Exception ex)
            {
                // Removed debug alert - Lỗi lưu sản phẩm: {0}
            }
        }
    }

    private void SaveProductImages(int productId)
    {
        try
        {
            // Debug logging
            Response.Write(string.Format("<script>console.log('SaveProductImages called with productId: {0}');</script>", productId));
            
            // Handle 3D model upload
            string model3DPath = Handle3DModelUpload(fu3DModel);
            if (!string.IsNullOrEmpty(model3DPath))
            {
                Response.Write(string.Format("<script>console.log('3D Model path: {0}');</script>", model3DPath));
                // Update product with 3D model path
                Product product = productDAL.GetProductById(productId);
                if (product != null)
                {
                    product.ModelPath = model3DPath;
                    productDAL.UpdateProduct(product);
                }
            }

            // Handle product image uploads for all colors
            string[] colors = { "white", "black", "red", "blue" };
            FileUpload[] fileUploads = { fuWhiteImage, fuBlackImage, fuRedImage, fuBlueImage };
            
            int savedCount = 0;
            
            for (int i = 0; i < colors.Length; i++)
            {
                string color = colors[i];
                FileUpload fileUpload = fileUploads[i];
                
                if (fileUpload.HasFile)
                {
                    string productImagePath = HandleImageUpload(fileUpload, "products");
                    Response.Write(string.Format("<script>console.log('{0} color image path: {1}');</script>", color, productImagePath));
                    
                    if (!string.IsNullOrEmpty(productImagePath))
                    {
                        // Save the uploaded image for this color
                        ProductImage productImage = new ProductImage
                        {
                            ProductId = productId,
                            Color = color,
                            ImagePath = productImagePath,
                            SortOrder = 1
                        };
                        
                        int imageId = productImageDAL.InsertProductImage(productImage);
                        Response.Write(string.Format("<script>console.log('{0} color image saved with ID: {1}');</script>", color, imageId));
                        savedCount++;
                    }
                }
            }
            
            if (savedCount > 0)
            {
                // Removed debug alert - Đã lưu thành công {0} ảnh cho sản phẩm!
            }
            else
            {
                Response.Write("<script>console.log('No images uploaded');</script>");
            }
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>console.error('Error in SaveProductImages: {0}');</script>", ex.Message));
            // Removed debug alert - Lỗi lưu hình ảnh sản phẩm: {0}
        }
    }

    private string GetImagePathFromHiddenField(string fieldName)
    {
        // This method is no longer needed as we handle file uploads directly
        return "";
    }

    private string HandleImageUpload(FileUpload fileUpload, string folder)
    {
        if (fileUpload.HasFile)
        {
            try
            {
                string fileName = fileUpload.FileName;
                string fileExtension = Path.GetExtension(fileName).ToLower();
                
                // Check file extension
                if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".gif")
                {
                    // Generate unique filename
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    string uploadPath = Server.MapPath(string.Format("~/Upload/Images/Products/{0}/", folder));
                    
                    // Create directory if not exists
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    
                    string fullPath = Path.Combine(uploadPath, uniqueFileName);
                    fileUpload.SaveAs(fullPath);
                    
                    return string.Format("/Upload/Images/Products/{0}/{1}", folder, uniqueFileName);
                }
                else
                {
                    // Removed debug alert - Chỉ chấp nhận file hình ảnh (.jpg, .jpeg, .png, .gif)
                    return "";
                }
            }
            catch (Exception ex)
            {
                // Removed debug alert - Lỗi upload hình ảnh: {0}
                return "";
            }
        }
        return "";
    }

    private string Handle3DModelUpload(FileUpload fileUpload)
    {
        if (fileUpload.HasFile)
        {
            try
            {
                string fileName = fileUpload.FileName;
                string fileExtension = Path.GetExtension(fileName).ToLower();
                
                // Check file extension
                if (fileExtension == ".glb")
                {
                    // Get category name for folder structure
                    string categoryName = GetCategoryFolderName();
                    
                    // Generate unique filename
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    
                    // Create folder path based on category
                    string uploadPath = Server.MapPath(string.Format("~/Upload/Models/3D/{0}/", categoryName));
                    
                    // Create directory if not exists
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    
                    string fullPath = Path.Combine(uploadPath, uniqueFileName);
                    fileUpload.SaveAs(fullPath);
                    
                    return string.Format("/Upload/Models/3D/{0}/{1}", categoryName, uniqueFileName);
                }
                else
                {
                    Response.Write("<script>alert('Chỉ chấp nhận file 3D model (.glb)');</script>");
                    return "";
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('Lỗi upload file 3D model: {0}');</script>", ex.Message));
                return "";
            }
        }
        return "";
    }

    private string GetCategoryFolderName()
    {
        try
        {
            if (!string.IsNullOrEmpty(ddlCategory.SelectedValue))
            {
                int categoryId = int.Parse(ddlCategory.SelectedValue);
                Category category = categoryDAL.GetCategoryById(categoryId);
                if (category != null)
                {
                    // Convert category name to folder-friendly name
                    return ConvertToFolderName(category.Name);
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>console.error('Lỗi lấy tên danh mục: {0}');</script>", ex.Message));
        }
        
        // Default folder name if category not found
        return "Other";
    }

    private string ConvertToFolderName(string categoryName)
    {
        if (string.IsNullOrEmpty(categoryName))
            return "Other";
            
        // Remove special characters and spaces, convert to lowercase
        string folderName = categoryName
            .Replace(" ", "_")
            .Replace("-", "_")
            .Replace("(", "")
            .Replace(")", "")
            .Replace("[", "")
            .Replace("]", "")
            .Replace("{", "")
            .Replace("}", "")
            .Replace("&", "and")
            .Replace("+", "plus")
            .Replace("#", "")
            .Replace("@", "")
            .Replace("!", "")
            .Replace("?", "")
            .Replace(".", "")
            .Replace(",", "")
            .Replace(";", "")
            .Replace(":", "")
            .Replace("'", "")
            .Replace("\"", "")
            .Replace("\\", "")
            .Replace("/", "")
            .Replace("|", "")
            .Replace("*", "")
            .Replace("<", "")
            .Replace(">", "")
            .Replace("=", "")
            .Replace("~", "")
            .Replace("`", "")
            .Replace("$", "")
            .Replace("%", "")
            .Replace("^", "");
            
        // Convert Vietnamese characters to ASCII
        folderName = folderName
            .Replace("á", "a").Replace("à", "a").Replace("ả", "a").Replace("ã", "a").Replace("ạ", "a")
            .Replace("ă", "a").Replace("ắ", "a").Replace("ằ", "a").Replace("ẳ", "a").Replace("ẵ", "a").Replace("ặ", "a")
            .Replace("â", "a").Replace("ấ", "a").Replace("ầ", "a").Replace("ẩ", "a").Replace("ẫ", "a").Replace("ậ", "a")
            .Replace("é", "e").Replace("è", "e").Replace("ẻ", "e").Replace("ẽ", "e").Replace("ẹ", "e")
            .Replace("ê", "e").Replace("ế", "e").Replace("ề", "e").Replace("ể", "e").Replace("ễ", "e").Replace("ệ", "e")
            .Replace("í", "i").Replace("ì", "i").Replace("ỉ", "i").Replace("ĩ", "i").Replace("ị", "i")
            .Replace("ó", "o").Replace("ò", "o").Replace("ỏ", "o").Replace("õ", "o").Replace("ọ", "o")
            .Replace("ô", "o").Replace("ố", "o").Replace("ồ", "o").Replace("ổ", "o").Replace("ỗ", "o").Replace("ộ", "o")
            .Replace("ơ", "o").Replace("ớ", "o").Replace("ờ", "o").Replace("ở", "o").Replace("ỡ", "o").Replace("ợ", "o")
            .Replace("ú", "u").Replace("ù", "u").Replace("ủ", "u").Replace("ũ", "u").Replace("ụ", "u")
            .Replace("ư", "u").Replace("ứ", "u").Replace("ừ", "u").Replace("ử", "u").Replace("ữ", "u").Replace("ự", "u")
            .Replace("ý", "y").Replace("ỳ", "y").Replace("ỷ", "y").Replace("ỹ", "y").Replace("ỵ", "y")
            .Replace("đ", "d")
            .Replace("Á", "A").Replace("À", "A").Replace("Ả", "A").Replace("Ã", "A").Replace("Ạ", "A")
            .Replace("Ă", "A").Replace("Ắ", "A").Replace("Ằ", "A").Replace("Ẳ", "A").Replace("Ẵ", "A").Replace("Ặ", "A")
            .Replace("Â", "A").Replace("Ấ", "A").Replace("Ầ", "A").Replace("Ẩ", "A").Replace("Ẫ", "A").Replace("Ậ", "A")
            .Replace("É", "E").Replace("È", "E").Replace("Ẻ", "E").Replace("Ẽ", "E").Replace("Ẹ", "E")
            .Replace("Ê", "E").Replace("Ế", "E").Replace("Ề", "E").Replace("Ể", "E").Replace("Ễ", "E").Replace("Ệ", "E")
            .Replace("Í", "I").Replace("Ì", "I").Replace("Ỉ", "I").Replace("Ĩ", "I").Replace("Ị", "I")
            .Replace("Ó", "O").Replace("Ò", "O").Replace("Ỏ", "O").Replace("Õ", "O").Replace("Ọ", "O")
            .Replace("Ô", "O").Replace("Ố", "O").Replace("Ồ", "O").Replace("Ổ", "O").Replace("Ỗ", "O").Replace("Ộ", "O")
            .Replace("Ơ", "O").Replace("Ớ", "O").Replace("Ờ", "O").Replace("Ở", "O").Replace("Ỡ", "O").Replace("Ợ", "O")
            .Replace("Ú", "U").Replace("Ù", "U").Replace("Ủ", "U").Replace("Ũ", "U").Replace("Ụ", "U")
            .Replace("Ư", "U").Replace("Ứ", "U").Replace("Ừ", "U").Replace("Ử", "U").Replace("Ữ", "U").Replace("Ự", "U")
            .Replace("Ý", "Y").Replace("Ỳ", "Y").Replace("Ỷ", "Y").Replace("Ỹ", "Y").Replace("Ỵ", "Y")
            .Replace("Đ", "D");
            
        return folderName.ToLower();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Products.aspx");
    }

    protected void cvMainImage_ServerValidate(object source, ServerValidateEventArgs args)
    {
        // Main image is only required when creating a new product
        if (Request.QueryString["id"] == null)
        {
            // Creating new product - main image is required
            args.IsValid = fuMainImage.HasFile;
        }
        else
        {
            // Editing existing product - main image is optional
            args.IsValid = true;
        }
    }
}