using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace App_Code
{
    public class DesignDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<Design> GetDesignsByUserId(int userId)
        {
            List<Design> designs = new List<Design>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Designs WHERE UserId = @UserId ORDER BY CreatedAt DESC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Design design = new Design
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    Name = reader["Name"].ToString(),
                                    LogoPath = reader["LogoPath"].ToString(),
                                    PositionData = reader["PositionData"].ToString(),
                                    PreviewPath = reader["PreviewPath"].ToString(),
                                    IsPublic = Convert.ToBoolean(reader["IsPublic"]),
                                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                    UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"])
                                };
                                designs.Add(design);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("GetDesignsByUserId", ex);
            }
            return designs;
        }

        public Design GetDesignById(int id)
        {
            Design design = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Designs WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                design = new Design
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    Name = reader["Name"].ToString(),
                                    LogoPath = reader["LogoPath"].ToString(),
                                    PositionData = reader["PositionData"].ToString(),
                                    PreviewPath = reader["PreviewPath"].ToString(),
                                    IsPublic = Convert.ToBoolean(reader["IsPublic"]),
                                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                    UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("GetDesignById", ex);
            }
            return design;
        }

        public int InsertDesign(Design design)
        {
            int newId = 0;
            try
            {
                System.Diagnostics.Debug.WriteLine("[DEBUG] DesignDAL.InsertDesign - Starting with design: " + design.Name);
                System.Diagnostics.Debug.WriteLine("[DEBUG] DesignDAL.InsertDesign - Connection string: " + connectionString);
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    System.Diagnostics.Debug.WriteLine("[DEBUG] DesignDAL.InsertDesign - Opening connection...");
                    conn.Open();
                    System.Diagnostics.Debug.WriteLine("[DEBUG] DesignDAL.InsertDesign - Connection opened successfully");
                    
                    // Kiểm tra xem bảng Designs có tồn tại không
                    string checkTableQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Designs'";
                    using (SqlCommand checkCmd = new SqlCommand(checkTableQuery, conn))
                    {
                        int tableCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        System.Diagnostics.Debug.WriteLine("[DEBUG] DesignDAL.InsertDesign - Designs table count: " + tableCount);
                        if (tableCount == 0)
                        {
                            System.Diagnostics.Debug.WriteLine("[ERROR] DesignDAL.InsertDesign - Designs table does not exist!");
                            return 0;
                        }
                    }
                    
                    // Kiểm tra UserId có tồn tại không
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Id = @UserId";
                    using (SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, conn))
                    {
                        checkUserCmd.Parameters.AddWithValue("@UserId", design.UserId);
                        int userCount = Convert.ToInt32(checkUserCmd.ExecuteScalar());
                        System.Diagnostics.Debug.WriteLine("[DEBUG] DesignDAL.InsertDesign - User count for ID " + design.UserId + ": " + userCount);
                        if (userCount == 0)
                        {
                            System.Diagnostics.Debug.WriteLine("[ERROR] DesignDAL.InsertDesign - User with ID " + design.UserId + " does not exist!");
                            return 0;
                        }
                    }
                    
                    // Kiểm tra ProductId có tồn tại không
                    string checkProductQuery = "SELECT COUNT(*) FROM Products WHERE Id = @ProductId";
                    using (SqlCommand checkProductCmd = new SqlCommand(checkProductQuery, conn))
                    {
                        checkProductCmd.Parameters.AddWithValue("@ProductId", design.ProductId);
                        int productCount = Convert.ToInt32(checkProductCmd.ExecuteScalar());
                        System.Diagnostics.Debug.WriteLine("[DEBUG] DesignDAL.InsertDesign - Product count for ID " + design.ProductId + ": " + productCount);
                        if (productCount == 0)
                        {
                            System.Diagnostics.Debug.WriteLine("[ERROR] DesignDAL.InsertDesign - Product with ID " + design.ProductId + " does not exist!");
                            return 0;
                        }
                    }
                    
                    string query = @"INSERT INTO Designs (UserId, ProductId, Name, LogoPath, PositionData, PreviewPath, IsPublic, CreatedAt, UpdatedAt) 
                             VALUES (@UserId, @ProductId, @Name, @LogoPath, @PositionData, @PreviewPath, @IsPublic, @CreatedAt, @UpdatedAt);
                             SELECT SCOPE_IDENTITY();";
                    
                    System.Diagnostics.Debug.WriteLine("[DEBUG] DesignDAL.InsertDesign - SQL Query: " + query);
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", design.UserId);
                        cmd.Parameters.AddWithValue("@ProductId", design.ProductId);
                        cmd.Parameters.AddWithValue("@Name", design.Name ?? "");
                        cmd.Parameters.AddWithValue("@LogoPath", design.LogoPath ?? "");
                        cmd.Parameters.AddWithValue("@PositionData", design.PositionData ?? "");
                        cmd.Parameters.AddWithValue("@PreviewPath", design.PreviewPath ?? "");
                        cmd.Parameters.AddWithValue("@IsPublic", design.IsPublic);
                        cmd.Parameters.AddWithValue("@CreatedAt", design.CreatedAt);
                        cmd.Parameters.AddWithValue("@UpdatedAt", design.UpdatedAt ?? (object)DBNull.Value);
                        
                        // Log dữ liệu trước khi insert
                        System.Diagnostics.Debug.WriteLine(
                            string.Format(
                                "[DEBUG] InsertDesign Parameters: UserId={0}, ProductId={1}, Name={2}, LogoPath={3}, PositionData={4}, PreviewPath={5}, IsPublic={6}, CreatedAt={7}, UpdatedAt={8}",
                                design.UserId, design.ProductId, design.Name, 
                                (design.LogoPath != null ? design.LogoPath.Substring(0, Math.Min(50, design.LogoPath.Length)) + "..." : "null"), 
                                (design.PositionData != null ? design.PositionData.Substring(0, Math.Min(50, design.PositionData.Length)) + "..." : "null"), 
                                design.PreviewPath, design.IsPublic, design.CreatedAt, design.UpdatedAt
                            )
                        );

                        System.Diagnostics.Debug.WriteLine("[DEBUG] DesignDAL.InsertDesign - Executing SQL command...");
                        object result = cmd.ExecuteScalar();
                        System.Diagnostics.Debug.WriteLine("[DEBUG] DesignDAL.InsertDesign - SQL command executed. Result: " + result);
                        
                        if (result != null)
                        {
                            newId = Convert.ToInt32(result);
                            System.Diagnostics.Debug.WriteLine("[DEBUG] DesignDAL.InsertDesign - New ID: " + newId);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("[ERROR] DesignDAL.InsertDesign - ExecuteScalar returned null!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                System.Diagnostics.Debug.WriteLine("[ERROR] DesignDAL.InsertDesign Exception: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("[ERROR] DesignDAL.InsertDesign StackTrace: " + ex.StackTrace);
                
                string logMsg = string.Format(
                    "InsertDesign ERROR: {0} | UserId={1}, ProductId={2}, Name={3}, LogoPath={4}, PositionData={5}, PreviewPath={6}, IsPublic={7}, CreatedAt={8}, UpdatedAt={9}",
                    ex.Message, design.UserId, design.ProductId, design.Name, 
                    (design.LogoPath != null ? design.LogoPath.Substring(0, Math.Min(50, design.LogoPath.Length)) + "..." : "null"), 
                    (design.PositionData != null ? design.PositionData.Substring(0, Math.Min(50, design.PositionData.Length)) + "..." : "null"), 
                    design.PreviewPath, design.IsPublic, design.CreatedAt, design.UpdatedAt
                );
                LogError("InsertDesign", new Exception(logMsg));
            }
            return newId;
        }

        public bool UpdateDesign(Design design)
        {
            bool result = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE Designs SET 
                                   Name = @Name, 
                                   LogoPath = @LogoPath, 
                                   PositionData = @PositionData,
                                   PreviewPath = @PreviewPath, 
                                   IsPublic = @IsPublic, 
                                   UpdatedAt = @UpdatedAt 
                                   WHERE Id = @Id AND UserId = @UserId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", design.Id);
                        cmd.Parameters.AddWithValue("@UserId", design.UserId);
                        cmd.Parameters.AddWithValue("@Name", design.Name ?? "");
                        cmd.Parameters.AddWithValue("@LogoPath", design.LogoPath ?? "");
                        cmd.Parameters.AddWithValue("@PositionData", design.PositionData ?? "");
                        cmd.Parameters.AddWithValue("@PreviewPath", design.PreviewPath ?? "");
                        cmd.Parameters.AddWithValue("@IsPublic", design.IsPublic);
                        cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        
                        int rowsAffected = cmd.ExecuteNonQuery();
                        result = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("UpdateDesign", ex);
            }
            return result;
        }

        public bool DeleteDesign(int designId, int userId)
        {
            bool result = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Designs WHERE Id = @Id AND UserId = @UserId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", designId);
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        result = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("DeleteDesign", ex);
            }
            return result;
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
                        cmd.Parameters.AddWithValue("@Action", "DesignDAL." + methodName);
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
    }
} 