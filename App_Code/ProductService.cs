using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class ProductService : System.Web.Services.WebService
{
    private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public object AddToCart(int productId, int quantity = 1)
    {
        try
        {
            // Lấy user ID từ session (nếu đã đăng nhập)
            int? userId = null;
            if (Session["UserId"] != null)
            {
                userId = Convert.ToInt32(Session["UserId"]);
            }

            if (!userId.HasValue)
            {
                return new { success = false, message = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng" };
            }

            // Kiểm tra sản phẩm có tồn tại không
            Product product = GetProductById(productId);
            if (product == null)
            {
                return new { success = false, message = "Sản phẩm không tồn tại" };
            }

            // Kiểm tra số lượng tồn kho
            if (product.StockQuantity < quantity)
            {
                return new { success = false, message = "Số lượng sản phẩm trong kho không đủ" };
            }

            // Thêm vào giỏ hàng
            bool result = AddProductToCart(userId.Value, productId, quantity);
            
            if (result)
            {
                // Cập nhật số lượng trong giỏ hàng
                int cartCount = GetCartItemCount(userId.Value);
                return new { success = true, message = "Đã thêm sản phẩm vào giỏ hàng", cartCount = cartCount };
            }
            else
            {
                return new { success = false, message = "Có lỗi xảy ra khi thêm vào giỏ hàng" };
            }
        }
        catch (Exception ex)
        {
            return new { success = false, message = "Có lỗi xảy ra: " + ex.Message };
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public object GetProductQuickView(int productId)
    {
        try
        {
            Product product = GetProductById(productId);
            if (product == null)
            {
                return new { success = false, message = "Sản phẩm không tồn tại" };
            }

            // Lấy thông tin category
            string categoryName = GetCategoryName(product.CategoryId);

            var productInfo = new
            {
                id = product.Id,
                name = product.Name,
                description = product.Description,
                price = product.Price,
                salePrice = product.SalePrice,
                imagePath = product.ImagePath,
                categoryName = categoryName,
                stockQuantity = product.StockQuantity,
                size = product.Size,
                color = product.Color,
                material = product.Material
            };

            return new { success = true, product = productInfo };
        }
        catch (Exception ex)
        {
            return new { success = false, message = "Có lỗi xảy ra: " + ex.Message };
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public object GetDesignInfo(int productId)
    {
        try
        {
            Product product = GetProductById(productId);
            if (product == null)
            {
                return new { success = false, message = "Sản phẩm không tồn tại" };
            }

            var designInfo = new
            {
                productId = product.Id,
                productName = product.Name,
                modelPath = product.ModelPath,
                basePrice = product.Price,
                availableColors = product.Color != null ? product.Color.Split(',').Select(c => c.Trim()).ToArray() : new string[0],
                availableSizes = product.Size != null ? product.Size.Split(',').Select(s => s.Trim()).ToArray() : new string[0]
            };

            return new { success = true, designInfo = designInfo };
        }
        catch (Exception ex)
        {
            return new { success = false, message = "Có lỗi xảy ra: " + ex.Message };
        }
    }

    private Product GetProductById(int id)
    {
        Product product = null;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Products WHERE Id = @Id AND IsActive = 1";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Product
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                SalePrice = null,
                                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                                ImagePath = reader["ImagePath"].ToString(),
                                ModelPath = reader["Model3DPath"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedDate = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]),
                                StockQuantity = Convert.ToInt32(reader["Stock"]),
                                Size = reader["Sizes"].ToString(),
                                Color = reader["Colors"].ToString(),
                                Material = ""
                            };
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log error
            System.Diagnostics.Debug.WriteLine("GetProductById ERROR: " + ex.Message);
        }
        return product;
    }

    private string GetCategoryName(int categoryId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Name FROM Categories WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", categoryId);
                    object result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : "";
                }
            }
        }
        catch
        {
            return "";
        }
    }

    private bool AddProductToCart(int userId, int productId, int quantity)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
                string checkQuery = "SELECT Id, Quantity FROM CartItems WHERE UserId = @UserId AND ProductId = @ProductId";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@UserId", userId);
                    checkCmd.Parameters.AddWithValue("@ProductId", productId);
                    
                    using (SqlDataReader reader = checkCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Sản phẩm đã có, cập nhật số lượng
                            int existingId = Convert.ToInt32(reader["Id"]);
                            int existingQuantity = Convert.ToInt32(reader["Quantity"]);
                            reader.Close();
                            
                            string updateQuery = "UPDATE CartItems SET Quantity = @Quantity WHERE Id = @Id";
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@Quantity", existingQuantity + quantity);
                                updateCmd.Parameters.AddWithValue("@Id", existingId);
                                return updateCmd.ExecuteNonQuery() > 0;
                            }
                        }
                        else
                        {
                            reader.Close();
                            
                            // Thêm sản phẩm mới vào giỏ hàng
                            string insertQuery = "INSERT INTO CartItems (UserId, ProductId, Quantity, CreatedAt) VALUES (@UserId, @ProductId, @Quantity, @CreatedAt)";
                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@UserId", userId);
                                insertCmd.Parameters.AddWithValue("@ProductId", productId);
                                insertCmd.Parameters.AddWithValue("@Quantity", quantity);
                                insertCmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                                return insertCmd.ExecuteNonQuery() > 0;
                            }
                        }
                    }
                }
            }
        }
        catch
        {
            return false;
        }
    }

    private int GetCartItemCount(int userId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM CartItems WHERE UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        catch
        {
            return 0;
        }
    }
} 