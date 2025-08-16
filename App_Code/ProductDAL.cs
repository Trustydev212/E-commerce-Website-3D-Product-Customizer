using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
// using Source.Models; // Now in App_Code, so no namespace needed

public class ProductDAL
{
    private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    public List<Product> GetAllProducts()
    {
        List<Product> products = new List<Product>();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Products ORDER BY CreatedAt DESC";
                
                // Debug log
                System.Web.HttpContext.Current.Response.Write("<script>console.log('Kết nối DB thành công');</script>");
                System.Web.HttpContext.Current.Response.Write("<script>console.log('Query: " + query + "');</script>");
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int count = 0;
                        while (reader.Read())
                        {
                            count++;
                            Product product = new Product
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                SalePrice = null, // Cột SalePrice không tồn tại trong DB
                                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                                ImagePath = reader["ImagePath"].ToString(),
                                ModelPath = reader["Model3DPath"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedDate = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]),
                                StockQuantity = Convert.ToInt32(reader["Stock"]),
                                Size = reader["Sizes"].ToString(),
                                Color = reader["Colors"].ToString(),
                                Material = "" // Cột Material không tồn tại trong DB
                            };
                            products.Add(product);
                        }
                        System.Web.HttpContext.Current.Response.Write("<script>console.log('Đọc được " + count + " sản phẩm từ DB');</script>");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Web.HttpContext.Current.Response.Write("<script>console.error('Lỗi GetAllProducts: " + ex.Message + "');</script>");
            LogError("GetAllProducts", ex);
        }
        return products;
    }

    public Product GetProductById(int id)
    {
        Product product = null;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Products WHERE Id = @Id";
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
                                SalePrice = null, // Cột SalePrice không tồn tại trong DB
                                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                                ImagePath = reader["ImagePath"].ToString(),
                                ModelPath = reader["Model3DPath"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedDate = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]),
                                StockQuantity = Convert.ToInt32(reader["Stock"]),
                                Size = reader["Sizes"].ToString(),
                                Color = reader["Colors"].ToString(),
                                Material = "" // Cột Material không tồn tại trong DB
                            };
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogError("GetProductById", ex);
        }
        return product;
    }

    public int InsertProduct(Product product)
    {
        int newId = 0;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Products (Name, Description, Price, CategoryId, ImagePath, Model3DPath, IsActive, CreatedAt, Stock, Sizes, Colors) 
                               VALUES (@Name, @Description, @Price, @CategoryId, @ImagePath, @Model3DPath, @IsActive, @CreatedAt, @Stock, @Sizes, @Colors);
                               SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", product.Name ?? "");
                    cmd.Parameters.AddWithValue("@Description", product.Description ?? "");
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                    cmd.Parameters.AddWithValue("@ImagePath", product.ImagePath ?? "");
                    cmd.Parameters.AddWithValue("@Model3DPath", product.ModelPath ?? "");
                    cmd.Parameters.AddWithValue("@IsActive", product.IsActive);
                    cmd.Parameters.AddWithValue("@CreatedAt", product.CreatedDate);
                    cmd.Parameters.AddWithValue("@Stock", product.StockQuantity);
                    cmd.Parameters.AddWithValue("@Sizes", product.Size ?? "");
                    cmd.Parameters.AddWithValue("@Colors", product.Color ?? "");
                    newId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        catch (Exception ex)
        {
            LogError("InsertProduct", ex);
        }
        return newId;
    }

    public bool UpdateProduct(Product product)
    {
        bool result = false;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE Products SET 
                               Name = @Name, 
                               Description = @Description, 
                               Price = @Price, 
                               CategoryId = @CategoryId, 
                               ImagePath = @ImagePath, 
                               Model3DPath = @Model3DPath, 
                               IsActive = @IsActive, 
                               UpdatedAt = @UpdatedAt, 
                               Stock = @Stock, 
                               Sizes = @Sizes, 
                               Colors = @Colors 
                               WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", product.Id);
                    cmd.Parameters.AddWithValue("@Name", product.Name ?? "");
                    cmd.Parameters.AddWithValue("@Description", product.Description ?? "");
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                    cmd.Parameters.AddWithValue("@ImagePath", product.ImagePath ?? "");
                    cmd.Parameters.AddWithValue("@Model3DPath", product.ModelPath ?? "");
                    cmd.Parameters.AddWithValue("@IsActive", product.IsActive);
                    cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Stock", product.StockQuantity);
                    cmd.Parameters.AddWithValue("@Sizes", product.Size ?? "");
                    cmd.Parameters.AddWithValue("@Colors", product.Color ?? "");
                    int rowsAffected = cmd.ExecuteNonQuery();
                    result = rowsAffected > 0;
                }
            }
        }
        catch (Exception ex)
        {
            LogError("UpdateProduct", ex);
        }
        return result;
    }

    public bool DeleteProduct(int id)
    {
        bool result = false;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Products WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    result = rowsAffected > 0;
                }
            }
        }
        catch (Exception ex)
        {
            LogError("DeleteProduct", ex);
        }
        return result;
    }

    public List<Product> GetProductsByCategory(int categoryId)
    {
        List<Product> products = new List<Product>();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Products WHERE CategoryId = @CategoryId AND IsActive = 1 ORDER BY CreatedAt DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                SalePrice = null, // Cột SalePrice không tồn tại trong DB
                                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                                ImagePath = reader["ImagePath"].ToString(),
                                ModelPath = reader["Model3DPath"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedDate = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]),
                                StockQuantity = Convert.ToInt32(reader["Stock"]),
                                Size = reader["Sizes"].ToString(),
                                Color = reader["Colors"].ToString(),
                                Material = "" // Cột Material không tồn tại trong DB
                            };
                            products.Add(product);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogError("GetProductsByCategory", ex);
        }
        return products;
    }

    private void LogError(string methodName, Exception ex)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO AdminLogs (Action, ErrorMessage, CreatedDate) 
                               VALUES (@Action, @ErrorMessage, @CreatedDate)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Action", "ProductDAL." + methodName);
                    cmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch
        {
            // Fallback if logging fails
        }
    }
} 