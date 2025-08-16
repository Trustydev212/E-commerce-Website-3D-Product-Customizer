using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
// using Source.Models; // Now in App_Code, so no namespace needed

public class OrderDAL
{
    private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    public List<Order> GetAllOrders()
    {
        List<Order> orders = new List<Order>();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Orders ORDER BY CreatedAt DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                OrderNumber = reader["OrderNumber"].ToString(),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Status = reader["Status"].ToString(),
                                CustomerName = reader["CustomerName"].ToString(),
                                CustomerPhone = reader["CustomerPhone"].ToString(),
                                CustomerEmail = reader["CustomerEmail"].ToString(),
                                ShippingAddress = reader["ShippingAddress"].ToString(),
                                PaymentMethod = reader["PaymentMethod"].ToString(),
                                PaymentStatus = reader["PaymentStatus"].ToString(),
                                Subtotal = Convert.ToDecimal(reader["Subtotal"]),
                                ShippingFee = Convert.ToDecimal(reader["ShippingFee"]),
                                Tax = Convert.ToDecimal(reader["Tax"]),
                                Total = Convert.ToDecimal(reader["Total"]),
                                Notes = reader["Notes"].ToString(),
                                TrackingNumber = reader["TrackingNumber"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedDate = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"])
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogError("GetAllOrders", ex);
        }
        return orders;
    }

    public Order GetOrderById(int id)
    {
        Order order = null;
        try
        {
            // Debug: Log the search
            System.Diagnostics.Debug.WriteLine(string.Format("OrderDAL.GetOrderById - Searching for ID: {0}", id));
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Orders WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    
                    // Debug: Log the query
                    System.Diagnostics.Debug.WriteLine(string.Format("OrderDAL.GetOrderById - Executing query: {0} with ID: {1}", query, id));
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Debug: Log that we found a record
                            System.Diagnostics.Debug.WriteLine(string.Format("OrderDAL.GetOrderById - Found order record for ID: {0}", id));
                            
                            order = new Order
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                OrderNumber = reader["OrderNumber"].ToString(),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Status = reader["Status"].ToString(),
                                CustomerName = reader["CustomerName"].ToString(),
                                CustomerPhone = reader["CustomerPhone"].ToString(),
                                CustomerEmail = reader["CustomerEmail"].ToString(),
                                ShippingAddress = reader["ShippingAddress"].ToString(),
                                PaymentMethod = reader["PaymentMethod"].ToString(),
                                PaymentStatus = reader["PaymentStatus"].ToString(),
                                Subtotal = Convert.ToDecimal(reader["Subtotal"]),
                                ShippingFee = Convert.ToDecimal(reader["ShippingFee"]),
                                Tax = Convert.ToDecimal(reader["Tax"]),
                                Total = Convert.ToDecimal(reader["Total"]),
                                Notes = reader["Notes"].ToString(),
                                TrackingNumber = reader["TrackingNumber"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedDate = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"])
                            };
                            
                            // Debug: Log order details
                            System.Diagnostics.Debug.WriteLine(string.Format("OrderDAL.GetOrderById - Order loaded: ID={0}, Status={1}, Customer={2}", 
                                order.Id, order.Status, order.CustomerName));
                        }
                        else
                        {
                            // Debug: Log that no record was found
                            System.Diagnostics.Debug.WriteLine(string.Format("OrderDAL.GetOrderById - No record found for ID: {0}", id));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Debug: Log the exception
            System.Diagnostics.Debug.WriteLine(string.Format("OrderDAL.GetOrderById - Exception: {0}", ex.Message));
            LogError("GetOrderById", ex);
        }
        return order;
    }

    public List<Order> GetOrdersByUserId(int userId)
    {
        List<Order> orders = new List<Order>();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Orders WHERE UserId = @UserId ORDER BY CreatedAt DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                OrderNumber = reader["OrderNumber"].ToString(),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Status = reader["Status"].ToString(),
                                CustomerName = reader["CustomerName"].ToString(),
                                CustomerPhone = reader["CustomerPhone"].ToString(),
                                CustomerEmail = reader["CustomerEmail"].ToString(),
                                ShippingAddress = reader["ShippingAddress"].ToString(),
                                PaymentMethod = reader["PaymentMethod"].ToString(),
                                PaymentStatus = reader["PaymentStatus"].ToString(),
                                Subtotal = Convert.ToDecimal(reader["Subtotal"]),
                                ShippingFee = Convert.ToDecimal(reader["ShippingFee"]),
                                Tax = Convert.ToDecimal(reader["Tax"]),
                                Total = Convert.ToDecimal(reader["Total"]),
                                Notes = reader["Notes"].ToString(),
                                TrackingNumber = reader["TrackingNumber"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedDate = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"])
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogError("GetOrdersByUserId", ex);
        }
        return orders;
    }

    public bool UpdateOrder(Order order)
    {
        bool result = false;
        try
        {
            // Debug: Log thông tin update
            System.Diagnostics.Debug.WriteLine(string.Format("OrderDAL.UpdateOrder - OrderId: {0}, Status: {1}, PaymentStatus: {2}", 
                order.Id, order.Status, order.PaymentStatus));
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE Orders SET 
                               Status = @Status, 
                               PaymentStatus = @PaymentStatus, 
                               TrackingNumber = @TrackingNumber, 
                               Notes = @Notes, 
                               UpdatedAt = @UpdatedAt 
                               WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", order.Id);
                    cmd.Parameters.AddWithValue("@Status", order.Status ?? "");
                    cmd.Parameters.AddWithValue("@PaymentStatus", order.PaymentStatus ?? "");
                    cmd.Parameters.AddWithValue("@TrackingNumber", order.TrackingNumber ?? "");
                    cmd.Parameters.AddWithValue("@Notes", order.Notes ?? "");
                    cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    
                    // Debug: Log SQL parameters
                    System.Diagnostics.Debug.WriteLine(string.Format("SQL Parameters - Id: {0}, Status: {1}, PaymentStatus: {2}", 
                        order.Id, order.Status ?? "", order.PaymentStatus ?? ""));
                    
                    int rowsAffected = cmd.ExecuteNonQuery();
                    result = rowsAffected > 0;
                    
                    // Debug: Log kết quả
                    System.Diagnostics.Debug.WriteLine(string.Format("Rows affected: {0}, Result: {1}", rowsAffected, result));
                }
            }
        }
        catch (Exception ex)
        {
            // Debug: Log lỗi
            System.Diagnostics.Debug.WriteLine(string.Format("OrderDAL.UpdateOrder Error: {0}", ex.Message));
            LogError("UpdateOrder", ex);
        }
        return result;
    }

    public bool DeleteOrder(int id)
    {
        bool result = false;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Orders WHERE Id = @Id";
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
            LogError("DeleteOrder", ex);
        }
        return result;
    }

    public void InsertOrderItem(OrderItem item)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO OrderDetails (OrderId, ProductId, DesignId, Quantity, Price, Color, Size, Subtotal, CreatedAt) 
                               VALUES (@OrderId, @ProductId, @DesignId, @Quantity, @Price, @Color, @Size, @Subtotal, @CreatedAt)";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderId", item.OrderId);
                    cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                    cmd.Parameters.AddWithValue("@DesignId", item.DesignId.HasValue ? (object)item.DesignId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                    cmd.Parameters.AddWithValue("@Price", item.Price);
                    cmd.Parameters.AddWithValue("@Color", item.Color ?? "");
                    cmd.Parameters.AddWithValue("@Size", item.Size ?? "");
                    cmd.Parameters.AddWithValue("@Subtotal", item.Price * item.Quantity); // Tính Subtotal
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    
                    int rowsAffected = cmd.ExecuteNonQuery();
                    
                    // Log success
                    LogAdminAction(item.OrderId, "InsertOrderItem Success", 
                        string.Format("OrderId: {0}, ProductId: {1}, Rows affected: {2}", 
                        item.OrderId, item.ProductId, rowsAffected));
                }
            }
        }
        catch (Exception ex)
        {
            // Log error with details
            LogAdminAction(item.OrderId, "InsertOrderItem Error", 
                string.Format("OrderId: {0}, ProductId: {1}, Error: {2}", 
                item.OrderId, item.ProductId, ex.Message));
            
            // Re-throw exception so calling code knows about the error
            throw;
        }
    }

    public List<OrderItem> GetOrderItemsByOrderId(int orderId)
    {
        List<OrderItem> orderItems = new List<OrderItem>();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT od.*, p.Name as ProductName, p.ImagePath as ProductImage,
                               d.Name as DesignName, d.LogoPath as DesignLogoPath, d.PositionData as DesignPositionData, 
                               d.PreviewPath as DesignPreviewPath, d.CreatedAt as DesignCreatedAt
                               FROM OrderDetails od
                               LEFT JOIN Products p ON od.ProductId = p.Id
                               LEFT JOIN Designs d ON od.DesignId = d.Id
                               WHERE od.OrderId = @OrderId";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int rowCount = 0;
                        while (reader.Read())
                        {
                            rowCount++;
                            OrderItem item = new OrderItem
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                OrderId = Convert.ToInt32(reader["OrderId"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                DesignId = reader["DesignId"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["DesignId"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Color = reader["Color"].ToString(),
                                Size = reader["Size"].ToString(),
                                CustomDesign = reader["DesignName"] != DBNull.Value ? reader["DesignName"].ToString() : "",
                                CustomDesignPath = reader["DesignLogoPath"] != DBNull.Value ? reader["DesignLogoPath"].ToString() : null,
                                ProductName = reader["ProductName"].ToString(),
                                ProductImage = reader["ProductImage"].ToString(),
                                ProductPrice = Convert.ToDecimal(reader["Price"]), // Use Price from OrderDetails instead
                                DesignName = reader["DesignName"] != DBNull.Value ? reader["DesignName"].ToString() : "",
                                DesignLogoPath = reader["DesignLogoPath"] != DBNull.Value ? reader["DesignLogoPath"].ToString() : "",
                                DesignPositionData = reader["DesignPositionData"] != DBNull.Value ? reader["DesignPositionData"].ToString() : "",
                                DesignPreviewPath = reader["DesignPreviewPath"] != DBNull.Value ? reader["DesignPreviewPath"].ToString() : "",
                                DesignCreatedAt = reader["DesignCreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["DesignCreatedAt"]) : (DateTime?)null
                            };
                            orderItems.Add(item);
                        }
                        
                        // Log only essential info
                        LogAdminAction(orderId, "GetOrderItemsByOrderId", 
                            string.Format("Found {0} items", rowCount));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogError("GetOrderItemsByOrderId", ex);
        }
        return orderItems;
    }

    public int InsertOrder(Order order)
    {
        int orderId = 0;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Orders (OrderNumber, UserId, Status, CustomerName, CustomerPhone, CustomerEmail, 
                               ShippingAddress, PaymentMethod, PaymentStatus, Subtotal, ShippingFee, Tax, Total, Notes, CreatedAt) 
                               VALUES (@OrderNumber, @UserId, @Status, @CustomerName, @CustomerPhone, @CustomerEmail, 
                               @ShippingAddress, @PaymentMethod, @PaymentStatus, @Subtotal, @ShippingFee, @Tax, @Total, @Notes, @CreatedAt);
                               SELECT SCOPE_IDENTITY();";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderNumber", GenerateOrderNumber());
                    cmd.Parameters.AddWithValue("@UserId", order.UserId);
                    cmd.Parameters.AddWithValue("@Status", order.Status ?? "Pending");
                    cmd.Parameters.AddWithValue("@CustomerName", order.CustomerName ?? "");
                    cmd.Parameters.AddWithValue("@CustomerPhone", order.CustomerPhone ?? "");
                    cmd.Parameters.AddWithValue("@CustomerEmail", order.CustomerEmail ?? "");
                    cmd.Parameters.AddWithValue("@ShippingAddress", order.ShippingAddress ?? "");
                    cmd.Parameters.AddWithValue("@PaymentMethod", order.PaymentMethod ?? "");
                    cmd.Parameters.AddWithValue("@PaymentStatus", order.PaymentStatus ?? "Pending");
                    cmd.Parameters.AddWithValue("@Subtotal", order.Subtotal);
                    cmd.Parameters.AddWithValue("@ShippingFee", order.ShippingFee);
                    cmd.Parameters.AddWithValue("@Tax", order.Tax);
                    cmd.Parameters.AddWithValue("@Total", order.Total);
                    cmd.Parameters.AddWithValue("@Notes", order.Notes ?? "");
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        orderId = Convert.ToInt32(result);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogError("InsertOrder", ex);
        }
        return orderId;
    }

    private string GenerateOrderNumber()
    {
        return "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss");
    }

    public void LogAdminAction(int userId, string action, string message)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO AdminLogs (UserId, Action, ErrorMessage, CreatedAt) 
                               VALUES (@UserId, @Action, @ErrorMessage, @CreatedAt)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId > 0 ? (object)userId : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Action", action);
                    cmd.Parameters.AddWithValue("@ErrorMessage", message);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch
        {
            // Fallback if logging fails
        }
    }

    private void LogError(string methodName, Exception ex)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO AdminLogs (Action, ErrorMessage, CreatedAt) 
                               VALUES (@Action, @ErrorMessage, @CreatedAt)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Action", "OrderDAL." + methodName);
                    cmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch
        {
            // Fallback if logging fails
        }
    }

    // Debug method to check database directly
    public void DebugOrderDetails(int orderId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // Check if order details exist
                string detailsQuery = "SELECT COUNT(*) FROM OrderDetails WHERE OrderId = @OrderId";
                using (SqlCommand cmd = new SqlCommand(detailsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    int detailsCount = Convert.ToInt32(cmd.ExecuteScalar());
                    LogAdminAction(orderId, "DebugOrderDetails", 
                        string.Format("OrderDetails count: {0}", detailsCount));
                }
            }
        }
        catch (Exception ex)
        {
            LogAdminAction(orderId, "DebugOrderDetails Error", 
                string.Format("Error: {0}", ex.Message));
        }
    }
} 