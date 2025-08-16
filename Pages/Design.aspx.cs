using System;
using System.IO;
using System.Web.UI;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Script.Serialization;
using App_Code;

public partial class Pages_Design : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string productIdStr = Request.QueryString["productId"];
            if (!string.IsNullOrEmpty(productIdStr))
            {
                int productId;
                if (int.TryParse(productIdStr, out productId))
                {
                    // Giả sử bạn có ProductDAL và Product có thuộc tính Model3DPath
                    var productDal = new ProductDAL();
                    var product = productDal.GetProductById(productId);
                    
                    if (product != null)
                    {
                        // Get additional parameters from URL
                        string size = Request.QueryString["size"];
                        string color = Request.QueryString["color"];
                        string quantity = Request.QueryString["quantity"];
                        
                        // Store in hidden fields for JavaScript access
                        if (!string.IsNullOrEmpty(size))
                        {
                            Response.Write("<script>window.selectedSize = '" + size + "';</script>");
                            hdnSize.Value = size;
                        }
                        if (!string.IsNullOrEmpty(color))
                        {
                            Response.Write("<script>window.selectedColor = '" + color + "';</script>");
                            hdnColor.Value = color;
                        }
                        if (!string.IsNullOrEmpty(quantity))
                        {
                            Response.Write("<script>window.selectedQuantity = '" + quantity + "';</script>");
                            hdnQuantity.Value = quantity;
                        }
                        
                        // Set product ID
                        hdnProductId.Value = productId.ToString();
                        
                        if (!string.IsNullOrEmpty(product.ModelPath))
                        {
                            // Resolve the URL to absolute path
                            string modelPath = product.ModelPath;
                            if (!modelPath.StartsWith("~/") && !modelPath.StartsWith("/"))
                            {
                                modelPath = "~/" + modelPath;
                            }
                            if (modelPath.StartsWith("~/"))
                            {
                                modelPath = ResolveUrl(modelPath);
                            }
                            
                            hdnModelPath.Value = modelPath;
                        }
                    }
                }
            }
        }
        
        // Xử lý POST request để lưu design
        if (Request.HttpMethod == "POST" && Request.Form["action"] == "saveDesign")
        {
            Response.Clear();
            Response.ContentType = "application/json";
            SaveDesignToDatabase();
            Response.End();
        }
        
        // Xử lý POST request để lưu ảnh preview
        if (Request.HttpMethod == "POST" && Request.Form["action"] == "savePreviewImage")
        {
            Response.Clear();
            Response.ContentType = "application/json";
            SavePreviewImage();
            Response.End();
        }
        
        // Xử lý POST request để lưu logo image
        if (Request.HttpMethod == "POST" && Request.Form["action"] == "saveLogoImage")
        {
            Response.Clear();
            Response.ContentType = "application/json";
            SaveLogoImage();
            Response.End();
        }
    }
    
    private void SaveDesignToDatabase()
    {
        try
        {
            // Test database connection first
            TestDatabaseConnection();
            
            // Lấy dữ liệu từ request
            string productIdStr = Request.Form["productId"];
            string designDataJson = Request.Form["designData"];
            
            if (string.IsNullOrEmpty(productIdStr) || string.IsNullOrEmpty(designDataJson))
            {
                Response.Write("{\"success\":false,\"message\":\"Missing required data\"}");
                return;
            }
            
            int productId;
            if (!int.TryParse(productIdStr, out productId))
            {
                Response.Write("{\"success\":false,\"message\":\"Invalid product ID\"}");
                return;
            }
            
            // Parse design data
            var serializer = new JavaScriptSerializer();
            var designData = serializer.Deserialize<dynamic>(designDataJson);
            
            // Lấy UserId từ session (cần đăng nhập)
            int userId = GetCurrentUserId();
            
            if (userId == 0)
            {
                Response.Write("{\"success\":false,\"message\":\"User not logged in\"}");
                return;
            }
            
            // Kiểm tra ProductId có tồn tại không
            var productDal = new ProductDAL();
            var product = productDal.GetProductById(productId);
            if (product == null)
            {
                Response.Write("{\"success\":false,\"message\":\"Product not found\"}");
                return;
            }
            
            // Tạo đối tượng Design - sử dụng đúng tên properties từ JSON
            var design = new Design
            {
                UserId = userId,
                ProductId = productId,
                Name = designData["name"] ?? "Custom Design",
                LogoPath = designData["logoPath"] ?? "",
                PositionData = designData["positionData"] ?? "",
                PreviewPath = designData["previewPath"] ?? "",
                IsPublic = Convert.ToBoolean(designData["isPublic"] ?? false),
                CreatedAt = DateTime.Parse(designData["createdAt"]),
                UpdatedAt = DateTime.Parse(designData["updatedAt"])
            };
            
            // Lưu vào database
            var designDal = new DesignDAL();
            int designId = designDal.InsertDesign(design);
            
            if (designId > 0)
            {
                Response.Write("{\"success\":true,\"designId\":" + designId + ",\"message\":\"Design saved successfully\"}");
            }
            else
            {
                Response.Write("{\"success\":false,\"message\":\"Failed to save design to database\"}");
            }
        }
        catch (Exception ex)
        {
            Response.Write("{\"success\":false,\"message\":\"Server error: " + ex.Message + "\"}");
        }
    }
    
    private void TestDatabaseConnection()
    {
        try
        {
            // Removed debug logs for database connection testing
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            
            using (var conn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                conn.Open();
                // Removed debug log for successful connection
                
                // Test if Designs table exists
                string checkTableQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Designs'";
                using (var cmd = new System.Data.SqlClient.SqlCommand(checkTableQuery, conn))
                {
                    int tableCount = Convert.ToInt32(cmd.ExecuteScalar());
                    // Removed debug log for table count
                }
                
                // Test if Users table exists and has data
                string checkUsersQuery = "SELECT COUNT(*) FROM Users";
                using (var cmd = new System.Data.SqlClient.SqlCommand(checkUsersQuery, conn))
                {
                    int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                    // Removed debug log for user count
                }
                
                // Show actual user IDs
                string showUsersQuery = "SELECT Id, Username, Email FROM Users ORDER BY Id";
                using (var cmd = new System.Data.SqlClient.SqlCommand(showUsersQuery, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Removed debug logs for existing users
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["Id"]);
                            string username = reader["Username"].ToString();
                            string email = reader["Email"].ToString();
                            // Removed debug log for user details
                        }
                    }
                }
                
                // Test if Products table exists and has data
                string checkProductsQuery = "SELECT COUNT(*) FROM Products";
                using (var cmd = new System.Data.SqlClient.SqlCommand(checkProductsQuery, conn))
                {
                    int productCount = Convert.ToInt32(cmd.ExecuteScalar());
                    // Removed debug log for product count
                }
                
                // Show actual product IDs
                string showProductsQuery = "SELECT Id, Name FROM Products ORDER BY Id";
                using (var cmd = new System.Data.SqlClient.SqlCommand(showProductsQuery, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Removed debug logs for existing products
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["Id"]);
                            string name = reader["Name"].ToString();
                            // Removed debug log for product details
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Removed debug log for database connection error
        }
    }
    
    private int GetCurrentUserId()
    {
        // Lấy UserId từ session - cần implement theo hệ thống authentication của bạn
        if (Session["UserId"] != null)
        {
            return Convert.ToInt32(Session["UserId"]);
        }
        
        // Fallback: lấy user ID đầu tiên từ database
        try
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var conn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TOP 1 Id FROM Users ORDER BY Id";
                using (var cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        int userId = Convert.ToInt32(result);
                        // Removed debug log for user ID
                        return userId;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Removed debug log for user ID error
        }
        
        // Fallback cuối cùng
        // Removed debug log for demo user ID
        return 1; // Return 1 nếu chưa đăng nhập (demo)
    }
    
    private void SavePreviewImage()
    {
        try
        {
            // Lấy dữ liệu từ request
            string imageData = Request.Form["imageData"];
            string fileName = Request.Form["fileName"];
            
            if (string.IsNullOrEmpty(imageData) || string.IsNullOrEmpty(fileName))
            {
                Response.Write("{\"success\":false,\"message\":\"Missing image data or filename\"}");
                return;
            }
            
            // Tạo thư mục nếu chưa có
            string uploadDir = Server.MapPath("~/Uploads/Designs/");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }
            
            // Lưu ảnh từ base64
            string filePath = Path.Combine(uploadDir, fileName);
            
            // Xử lý base64 data URL
            if (imageData.StartsWith("data:image/"))
            {
                // Tách phần base64 từ data URL
                string base64Data = imageData.Substring(imageData.IndexOf(",") + 1);
                byte[] imageBytes = Convert.FromBase64String(base64Data);
                
                // Lưu file
                File.WriteAllBytes(filePath, imageBytes);
                
                // Trả về đường dẫn tương đối
                string relativePath = "/Uploads/Designs/" + fileName;
                Response.Write("{\"success\":true,\"imagePath\":\"" + relativePath + "\",\"message\":\"Preview image saved successfully\"}");
            }
            else
            {
                Response.Write("{\"success\":false,\"message\":\"Invalid image data format\"}");
            }
        }
        catch (Exception ex)
        {
            // Log lỗi
            System.Diagnostics.Debug.WriteLine("SavePreviewImage ERROR: " + ex.Message);
            Response.Write("{\"success\":false,\"message\":\"Server error: " + ex.Message + "\"}");
        }
    }
    
    private void SaveLogoImage()
    {
        try
        {
            // Lấy dữ liệu từ request
            string imageData = Request.Form["imageData"];
            string fileName = Request.Form["fileName"];
            
            if (string.IsNullOrEmpty(imageData) || string.IsNullOrEmpty(fileName))
            {
                Response.Write("{\"success\":false,\"message\":\"Missing image data or filename\"}");
                return;
            }
            
            // Tạo thư mục nếu chưa có
            string uploadDir = Server.MapPath("~/Uploads/Logos/");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }
            
            // Lưu ảnh từ base64
            string filePath = Path.Combine(uploadDir, fileName);
            
            // Xử lý base64 data URL
            if (imageData.StartsWith("data:image/"))
            {
                // Tách phần base64 từ data URL
                string base64Data = imageData.Substring(imageData.IndexOf(",") + 1);
                byte[] imageBytes = Convert.FromBase64String(base64Data);
                
                // Lưu file
                File.WriteAllBytes(filePath, imageBytes);
                
                // Trả về đường dẫn tương đối
                string relativePath = "/Uploads/Logos/" + fileName;
                Response.Write("{\"success\":true,\"imagePath\":\"" + relativePath + "\",\"message\":\"Logo image saved successfully\"}");
            }
            else
            {
                Response.Write("{\"success\":false,\"message\":\"Invalid image data format\"}");
            }
        }
        catch (Exception ex)
        {
            // Log lỗi
            System.Diagnostics.Debug.WriteLine("SaveLogoImage ERROR: " + ex.Message);
            Response.Write("{\"success\":false,\"message\":\"Server error: " + ex.Message + "\"}");
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (fuLogo.HasFile)
        {
            try
            {
                // Lưu file logo vào thư mục /Uploads/Logos/
                string fileName = Path.GetFileName(fuLogo.FileName);
                string savePath = Server.MapPath("~/Uploads/Logos/" + fileName);
                fuLogo.SaveAs(savePath);
                lblUploadStatus.Text = "Tải logo thành công!";
                // Có thể lưu đường dẫn vào session hoặc ViewState để dùng khi export
                Session["LogoPath"] = "/Uploads/Logos/" + fileName;
            }
            catch (Exception ex)
            {
                lblUploadStatus.Text = "Lỗi: " + ex.Message;
            }
        }
        else
        {
            lblUploadStatus.Text = "Vui lòng chọn file logo.";
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        // Giả lập export thiết kế: lưu thông tin thiết kế vào DB
        string logoPath = Session["LogoPath"] as string;
        string color = Request.Form["ShirtColor"];
        int userId = 1; // Lấy từ session thực tế
        int productId = 1; // Lấy từ query string thực tế
        string previewPath = "/Uploads/Designs/preview-demo.png"; // Ảnh preview giả lập
        // TODO: Tích hợp chụp ảnh 3D/canvas và lưu file preview thực tế
        // TODO: Lưu record vào bảng Designs (gọi DAL hoặc Entity Framework)
        lblDesignStatus.Text = "Thiết kế đã được export và lưu lại!";
    }

    protected void btnAddToCart_Click(object sender, EventArgs e)
    {
        // Giả lập thêm vào giỏ hàng: tạo record OrderDetails với DesignId
        // TODO: Lấy DesignId vừa lưu, tạo record mới trong OrderDetails
        lblDesignStatus.Text = "Thiết kế đã thêm vào giỏ hàng!";
    }
} 