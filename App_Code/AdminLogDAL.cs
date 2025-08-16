using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
// using Source.Models; // Now in App_Code, so no namespace needed

public class AdminLogDAL
{
    private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    public void LogError(string action, string message, string stackTrace)
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
                    cmd.Parameters.AddWithValue("@Action", action);
                    cmd.Parameters.AddWithValue("@ErrorMessage", message + " - " + stackTrace);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch
        {
            // Fallback if logging fails - don't throw exception
        }
    }

    public void LogAdminAction(int userId, string action, string message)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO AdminLogs (UserId, Action, ErrorMessage, CreatedDate) 
                               VALUES (@UserId, @Action, @ErrorMessage, @CreatedDate)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId > 0 ? (object)userId : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Action", action);
                    cmd.Parameters.AddWithValue("@ErrorMessage", message);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch
        {
            // Fallback if logging fails - don't throw exception
        }
    }

    public List<AdminLog> GetAllLogs()
    {
        List<AdminLog> logs = new List<AdminLog>();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM AdminLogs ORDER BY CreatedDate DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AdminLog log = new AdminLog
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Action = reader["Action"].ToString(),
                                ErrorMessage = reader["ErrorMessage"].ToString(),
                                UserId = reader["UserId"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["UserId"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                            };
                            logs.Add(log);
                        }
                    }
                }
            }
        }
        catch
        {
            // Return empty list if error occurs
        }
        return logs;
    }
} 