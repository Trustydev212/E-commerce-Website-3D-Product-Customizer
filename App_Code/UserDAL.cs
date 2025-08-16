using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
// using Source.Models; // Now in App_Code, so no namespace needed

namespace App_Code
{
    public class UserDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Users ORDER BY CreatedAt DESC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Avatar = reader["Avatar"].ToString(),
                                    Role = reader["Role"].ToString(),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    BirthDate = reader["BirthDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["BirthDate"]),
                                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                    UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]),
                                    LastLogin = reader["LastLogin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LastLogin"])
                                };
                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("GetAllUsers", ex);
            }
            return users;
        }

        public User GetUserById(int id)
        {
            User user = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Users WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Avatar = reader["Avatar"].ToString(),
                                    Role = reader["Role"].ToString(),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    BirthDate = reader["BirthDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["BirthDate"]),
                                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                    UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]),
                                    LastLogin = reader["LastLogin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LastLogin"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("GetUserById", ex);
            }
            return user;
        }

        public User GetUserByUsername(string username)
        {
            User user = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Users WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Avatar = reader["Avatar"].ToString(),
                                    Role = reader["Role"].ToString(),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    BirthDate = reader["BirthDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["BirthDate"]),
                                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                    UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]),
                                    LastLogin = reader["LastLogin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LastLogin"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("GetUserByUsername", ex);
            }
            return user;
        }

        public int InsertUser(User user)
        {
            int newId = 0;
            try
            {
                // Debug connection string
                System.Diagnostics.Debug.WriteLine("Connection String: " + connectionString);
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    System.Diagnostics.Debug.WriteLine("Database connection opened successfully");
                    
                    string query = @"INSERT INTO Users (Username, Email, PasswordHash, FullName, Phone, Address, Avatar, Role, IsActive, BirthDate, CreatedAt) 
                                   VALUES (@Username, @Email, @PasswordHash, @FullName, @Phone, @Address, @Avatar, @Role, @IsActive, @BirthDate, @CreatedAt);
                                   SELECT SCOPE_IDENTITY();";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", user.Username ?? "");
                        cmd.Parameters.AddWithValue("@Email", user.Email ?? "");
                        cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash ?? "");
                        cmd.Parameters.AddWithValue("@FullName", user.FullName ?? "");
                        cmd.Parameters.AddWithValue("@Phone", user.Phone ?? "");
                        cmd.Parameters.AddWithValue("@Address", user.Address ?? "");
                        cmd.Parameters.AddWithValue("@Avatar", user.Avatar ?? "");
                        cmd.Parameters.AddWithValue("@Role", user.Role ?? "User");
                        cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                        cmd.Parameters.AddWithValue("@BirthDate", user.BirthDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);
                        
                        System.Diagnostics.Debug.WriteLine("Executing InsertUser query...");
                        newId = Convert.ToInt32(cmd.ExecuteScalar());
                        System.Diagnostics.Debug.WriteLine("User inserted successfully with ID: " + newId);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("InsertUser", ex);
            }
            return newId;
        }

        public bool UpdateUser(User user)
        {
            bool result = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE Users SET 
                                   Username = @Username, 
                                   Email = @Email, 
                                   PasswordHash = @PasswordHash, 
                                   FullName = @FullName, 
                                   Phone = @Phone, 
                                   Address = @Address, 
                                   Avatar = @Avatar, 
                                   Role = @Role, 
                                   IsActive = @IsActive, 
                                   BirthDate = @BirthDate,
                                   UpdatedAt = @UpdatedAt 
                                   WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", user.Id);
                        cmd.Parameters.AddWithValue("@Username", user.Username ?? "");
                        cmd.Parameters.AddWithValue("@Email", user.Email ?? "");
                        cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash ?? "");
                        cmd.Parameters.AddWithValue("@FullName", user.FullName ?? "");
                        cmd.Parameters.AddWithValue("@Phone", user.Phone ?? "");
                        cmd.Parameters.AddWithValue("@Address", user.Address ?? "");
                        cmd.Parameters.AddWithValue("@Avatar", user.Avatar ?? "");
                        cmd.Parameters.AddWithValue("@Role", user.Role ?? "User");
                        cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                        cmd.Parameters.AddWithValue("@BirthDate", user.BirthDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        
                        int rowsAffected = cmd.ExecuteNonQuery();
                        result = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("UpdateUser", ex);
            }
            return result;
        }

        public bool DeleteUser(int id)
        {
            bool result = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Users WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        result = cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("DeleteUser", ex);
            }
            return result;
        }

        public User AuthenticateUser(string email, string password)
        {
            User user = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // First, get user by email only
                    string query = "SELECT * FROM Users WHERE Email = @Email AND IsActive = 1";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPasswordHash = reader["PasswordHash"].ToString();
                                
                                // Hash the input password and compare with stored hash
                                string inputPasswordHash = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
                                
                                if (inputPasswordHash == storedPasswordHash)
                                {
                                    user = new User
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        Username = reader["Username"].ToString(),
                                        Email = reader["Email"].ToString(),
                                        PasswordHash = reader["PasswordHash"].ToString(),
                                        FullName = reader["FullName"].ToString(),
                                        Phone = reader["Phone"].ToString(),
                                        Address = reader["Address"].ToString(),
                                        Avatar = reader["Avatar"].ToString(),
                                        Role = reader["Role"].ToString(),
                                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                                        BirthDate = reader["BirthDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["BirthDate"]),
                                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                        UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]),
                                        LastLogin = reader["LastLogin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LastLogin"])
                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("AuthenticateUser", ex);
            }
            return user;
        }

        public bool IsUsernameExists(string username)
        {
            bool exists = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        exists = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("IsUsernameExists", ex);
            }
            return exists;
        }

        public bool IsEmailExists(string email)
        {
            bool exists = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        exists = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("IsEmailExists", ex);
            }
            return exists;
        }

        public void UpdateLastLoginDate(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Users SET LastLogin = @LastLogin WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@LastLogin", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Id", userId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("UpdateLastLoginDate", ex);
            }
        }

        public bool TestDatabaseConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    System.Diagnostics.Debug.WriteLine("Database connection test successful");
                    
                    // Check if Users table exists
                    string checkTableQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users'";
                    using (SqlCommand cmd = new SqlCommand(checkTableQuery, conn))
                    {
                        int tableExists = Convert.ToInt32(cmd.ExecuteScalar());
                        System.Diagnostics.Debug.WriteLine("Users table exists: " + (tableExists > 0));
                        
                        if (tableExists == 0)
                        {
                            System.Diagnostics.Debug.WriteLine("ERROR: Users table does not exist!");
                            return false;
                        }
                    }
                    
                    // Check table structure
                    string checkColumnsQuery = "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' ORDER BY ORDINAL_POSITION";
                    using (SqlCommand cmd = new SqlCommand(checkColumnsQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            System.Diagnostics.Debug.WriteLine("Users table columns:");
                            while (reader.Read())
                            {
                                System.Diagnostics.Debug.WriteLine("  " + reader["COLUMN_NAME"] + " - " + reader["DATA_TYPE"]);
                            }
                        }
                    }
                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogError("TestDatabaseConnection", ex);
                return false;
            }
        }

        private void LogError(string methodName, Exception ex)
        {
            // Log error to file or database
            string errorMessage = string.Format("Error in {0}: {1} - StackTrace: {2}", methodName, ex.Message, ex.StackTrace);
            System.Diagnostics.Debug.WriteLine(errorMessage);
            
            // Also log to Response for debugging
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Response != null)
            {
                System.Web.HttpContext.Current.Response.Write("<!-- Error: " + errorMessage + " -->");
            }
        }
    }
} 