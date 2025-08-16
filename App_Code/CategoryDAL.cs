using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
// using Source.Models; // Now in App_Code, so no namespace needed

public class CategoryDAL
{
    private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    public List<Category> GetAllCategories()
    {
        List<Category> categories = new List<Category>();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Categories ORDER BY Name";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Category category = new Category
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                ImagePath = reader["ImagePath"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedDate = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]),
                                DisplayOrder = 1 // Default value if column doesn't exist
                            };
                            categories.Add(category);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogError("GetAllCategories", ex);
        }
        return categories;
    }

    public Category GetCategoryById(int id)
    {
        Category category = null;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Categories WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            category = new Category
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                ImagePath = reader["ImagePath"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedDate = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]),
                                DisplayOrder = 1 // Default value if column doesn't exist
                            };
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogError("GetCategoryById", ex);
        }
        return category;
    }

    public int InsertCategory(Category category)
    {
        int newId = 0;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Categories (Name, Description, ImagePath, IsActive, CreatedAt) 
                               VALUES (@Name, @Description, @ImagePath, @IsActive, @CreatedAt);
                               SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", category.Name ?? "");
                    cmd.Parameters.AddWithValue("@Description", category.Description ?? "");
                    cmd.Parameters.AddWithValue("@ImagePath", category.ImagePath ?? "");
                    cmd.Parameters.AddWithValue("@IsActive", category.IsActive);
                    cmd.Parameters.AddWithValue("@CreatedAt", category.CreatedDate);
                    
                    newId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        catch (Exception ex)
        {
            LogError("InsertCategory", ex);
        }
        return newId;
    }

    public bool UpdateCategory(Category category)
    {
        bool result = false;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE Categories SET 
                               Name = @Name, 
                               Description = @Description, 
                               ImagePath = @ImagePath, 
                               IsActive = @IsActive, 
                               UpdatedAt = @UpdatedAt 
                               WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", category.Id);
                    cmd.Parameters.AddWithValue("@Name", category.Name ?? "");
                    cmd.Parameters.AddWithValue("@Description", category.Description ?? "");
                    cmd.Parameters.AddWithValue("@ImagePath", category.ImagePath ?? "");
                    cmd.Parameters.AddWithValue("@IsActive", category.IsActive);
                    cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    
                    int rowsAffected = cmd.ExecuteNonQuery();
                    result = rowsAffected > 0;
                }
            }
        }
        catch (Exception ex)
        {
            LogError("UpdateCategory", ex);
        }
        return result;
    }

    public bool DeleteCategory(int id)
    {
        bool result = false;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Categories WHERE Id = @Id";
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
            LogError("DeleteCategory", ex);
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
                    cmd.Parameters.AddWithValue("@Action", "CategoryDAL." + methodName);
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