using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class CartDAL
{
    private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    public bool AddToCart(CartItem item)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("[DEBUG] CartDAL.AddToCart - Starting with item: " + item.ProductName);
            System.Diagnostics.Debug.WriteLine("[DEBUG] Connection string: " + connectionString);
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                System.Diagnostics.Debug.WriteLine("[DEBUG] Opening database connection...");
                conn.Open();
                System.Diagnostics.Debug.WriteLine("[DEBUG] Database connection opened successfully");
                
                string query = @"INSERT INTO Cart (UserId, ProductId, ProductName, ProductImagePath, Size, Color, Price, Quantity, CustomDesign, CreatedDate) 
                               VALUES (@UserId, @ProductId, @ProductName, @ProductImagePath, @Size, @Color, @Price, @Quantity, @CustomDesign, @CreatedDate)";
                
                System.Diagnostics.Debug.WriteLine("[DEBUG] SQL Query: " + query);
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", item.UserId);
                    cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName ?? "");
                    cmd.Parameters.AddWithValue("@ProductImagePath", item.ProductImagePath ?? "");
                    cmd.Parameters.AddWithValue("@Size", item.Size ?? "");
                    cmd.Parameters.AddWithValue("@Color", item.Color ?? "");
                    cmd.Parameters.AddWithValue("@Price", item.Price);
                    cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                    cmd.Parameters.AddWithValue("@CustomDesign", item.CustomDesign ?? "");
                    cmd.Parameters.AddWithValue("@CreatedDate", item.CreatedDate);
                    
                    System.Diagnostics.Debug.WriteLine("[DEBUG] Parameters set: UserId=" + item.UserId + ", ProductId=" + item.ProductId + ", Price=" + item.Price);
                    
                    System.Diagnostics.Debug.WriteLine("[DEBUG] Executing SQL command...");
                    int rowsAffected = cmd.ExecuteNonQuery();
                    System.Diagnostics.Debug.WriteLine("[DEBUG] SQL command executed. Rows affected: " + rowsAffected);
                    
                    return rowsAffected > 0;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("[ERROR] CartDAL.AddToCart Exception: " + ex.Message);
            System.Diagnostics.Debug.WriteLine("[ERROR] Stack trace: " + ex.StackTrace);
            LogError("AddToCart", ex);
            return false;
        }
    }

    public List<Cart> GetCartByUserId(int userId)
    {
        List<Cart> carts = new List<Cart>();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Cart WHERE UserId = @UserId ORDER BY CreatedDate DESC";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cart cart = new Cart
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductName = reader["ProductName"].ToString(),
                                ProductImagePath = reader["ProductImagePath"].ToString(),
                                Size = reader["Size"].ToString(),
                                Color = reader["Color"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                CustomDesign = reader["CustomDesign"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                            };
                            carts.Add(cart);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogError("GetCartByUserId", ex);
        }
        return carts;
    }

    public List<CartItem> GetCartItemsByUserId(int userId)
    {
        List<CartItem> cartItems = new List<CartItem>();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Cart WHERE UserId = @UserId ORDER BY CreatedDate DESC";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CartItem item = new CartItem
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductName = reader["ProductName"].ToString(),
                                ProductImagePath = reader["ProductImagePath"].ToString(),
                                Size = reader["Size"].ToString(),
                                Color = reader["Color"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                CustomDesign = reader["CustomDesign"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                            };
                            cartItems.Add(item);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogError("GetCartItemsByUserId", ex);
        }
        return cartItems;
    }

    public int GetCartItemCount(int userId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Cart WHERE UserId = @UserId";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        catch (Exception ex)
        {
            LogError("GetCartItemCount", ex);
            return 0;
        }
    }

    public void RemoveFromCart(int cartId)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine(String.Format("[DEBUG] CartDAL.RemoveFromCart - Attempting to remove cart item with ID: {0}", cartId));
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Cart WHERE Id = @CartId";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CartId", cartId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    System.Diagnostics.Debug.WriteLine(String.Format("[DEBUG] CartDAL.RemoveFromCart - Rows affected: {0}", rowsAffected));
                    
                    if (rowsAffected > 0)
                    {
                        System.Diagnostics.Debug.WriteLine(String.Format("[DEBUG] CartDAL.RemoveFromCart - Successfully removed cart item: {0}", cartId));
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(String.Format("[DEBUG] CartDAL.RemoveFromCart - No rows affected, cart item may not exist: {0}", cartId));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("[ERROR] CartDAL.RemoveFromCart: {0}", ex.Message));
            LogError("RemoveFromCart", ex);
        }
    }

    public void RemoveFromCart(int userId, int productId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Cart WHERE UserId = @UserId AND ProductId = @ProductId";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            LogError("RemoveFromCart", ex);
        }
    }

    public void UpdateCartQuantity(int cartId, int quantity)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Cart SET Quantity = @Quantity WHERE Id = @CartId";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@CartId", cartId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            LogError("UpdateCartQuantity", ex);
        }
    }

    public void UpdateCartQuantity(int userId, int productId, int quantity)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Cart SET Quantity = @Quantity WHERE UserId = @UserId AND ProductId = @ProductId";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            LogError("UpdateCartQuantity", ex);
        }
    }

    public void ClearCart(int userId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Cart WHERE UserId = @UserId";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            LogError("ClearCart", ex);
        }
    }

    private void LogError(string method, Exception ex)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine(string.Format("CartDAL.{0} Error: {1}", method, ex.Message));
        }
        catch
        {
            // Ignore logging errors
        }
    }
} 