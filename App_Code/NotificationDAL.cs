using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

public class NotificationDAL
{
    private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    public int InsertNotification(Notification notification)
    {
        int newId = 0;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Notifications (UserId, Title, Message, Type, Icon, ActionUrl, IsRead, IsGlobal, ExpiresAt, CreatedAt, CreatedBy)
                                 VALUES (@UserId, @Title, @Message, @Type, @Icon, @ActionUrl, @IsRead, @IsGlobal, @ExpiresAt, @CreatedAt, @CreatedBy);
                                 SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", notification.UserId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Title", notification.Title);
                    cmd.Parameters.AddWithValue("@Message", notification.Message);
                    cmd.Parameters.AddWithValue("@Type", notification.Type ?? "info");
                    cmd.Parameters.AddWithValue("@Icon", notification.Icon ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ActionUrl", notification.ActionUrl ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsRead", notification.IsRead ?? false);
                    cmd.Parameters.AddWithValue("@IsGlobal", notification.IsGlobal ?? false);
                    cmd.Parameters.AddWithValue("@ExpiresAt", notification.ExpiresAt ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedAt", notification.CreatedAt ?? DateTime.Now);
                    cmd.Parameters.AddWithValue("@CreatedBy", notification.CreatedBy ?? (object)DBNull.Value);
                    
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        newId = Convert.ToInt32(result);
                }
            }
        }
        catch (Exception ex)
        {
            // Log error if needed
            System.Diagnostics.Debug.WriteLine("NotificationDAL.InsertNotification Error: " + ex.Message);
        }
        return newId;
    }

    public List<Notification> GetNotificationsByUserId(int userId, int limit = 20)
    {
        List<Notification> notifications = new List<Notification>();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT TOP (@Limit) Id, UserId, Title, Message, Type, Icon, ActionUrl, IsRead, IsGlobal, ExpiresAt, CreatedAt, ReadAt, CreatedBy
                                 FROM Notifications 
                                 WHERE (UserId = @UserId OR IsGlobal = 1)
                                 AND (ExpiresAt IS NULL OR ExpiresAt > GETDATE())
                                 ORDER BY CreatedAt DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Limit", limit);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notifications.Add(new Notification
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : (int?)null,
                                Title = reader["Title"].ToString(),
                                Message = reader["Message"].ToString(),
                                Type = reader["Type"].ToString(),
                                Icon = reader["Icon"] != DBNull.Value ? reader["Icon"].ToString() : null,
                                ActionUrl = reader["ActionUrl"] != DBNull.Value ? reader["ActionUrl"].ToString() : null,
                                IsRead = reader["IsRead"] != DBNull.Value ? Convert.ToBoolean(reader["IsRead"]) : false,
                                IsGlobal = reader["IsGlobal"] != DBNull.Value ? Convert.ToBoolean(reader["IsGlobal"]) : false,
                                ExpiresAt = reader["ExpiresAt"] != DBNull.Value ? Convert.ToDateTime(reader["ExpiresAt"]) : (DateTime?)null,
                                CreatedAt = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : (DateTime?)null,
                                ReadAt = reader["ReadAt"] != DBNull.Value ? Convert.ToDateTime(reader["ReadAt"]) : (DateTime?)null,
                                CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : null
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("NotificationDAL.GetNotificationsByUserId Error: " + ex.Message);
        }
        return notifications;
    }

    public List<Notification> GetGlobalNotifications(int limit = 10)
    {
        List<Notification> notifications = new List<Notification>();
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT TOP (@Limit) Id, UserId, Title, Message, Type, Icon, ActionUrl, IsRead, IsGlobal, ExpiresAt, CreatedAt, ReadAt, CreatedBy
                                 FROM Notifications 
                                 WHERE IsGlobal = 1
                                 AND (ExpiresAt IS NULL OR ExpiresAt > GETDATE())
                                 ORDER BY CreatedAt DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Limit", limit);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notifications.Add(new Notification
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : (int?)null,
                                Title = reader["Title"].ToString(),
                                Message = reader["Message"].ToString(),
                                Type = reader["Type"].ToString(),
                                Icon = reader["Icon"] != DBNull.Value ? reader["Icon"].ToString() : null,
                                ActionUrl = reader["ActionUrl"] != DBNull.Value ? reader["ActionUrl"].ToString() : null,
                                IsRead = reader["IsRead"] != DBNull.Value ? Convert.ToBoolean(reader["IsRead"]) : false,
                                IsGlobal = reader["IsGlobal"] != DBNull.Value ? Convert.ToBoolean(reader["IsGlobal"]) : false,
                                ExpiresAt = reader["ExpiresAt"] != DBNull.Value ? Convert.ToDateTime(reader["ExpiresAt"]) : (DateTime?)null,
                                CreatedAt = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : (DateTime?)null,
                                ReadAt = reader["ReadAt"] != DBNull.Value ? Convert.ToDateTime(reader["ReadAt"]) : (DateTime?)null,
                                CreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : null
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("NotificationDAL.GetGlobalNotifications Error: " + ex.Message);
        }
        return notifications;
    }

    public bool MarkAsRead(int notificationId, int userId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE Notifications 
                                 SET IsRead = 1, ReadAt = GETDATE()
                                 WHERE Id = @Id AND (UserId = @UserId OR IsGlobal = 1)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", notificationId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("NotificationDAL.MarkAsRead Error: " + ex.Message);
            return false;
        }
    }

    public bool MarkAllAsRead(int userId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE Notifications 
                                 SET IsRead = 1, ReadAt = GETDATE()
                                 WHERE (UserId = @UserId OR IsGlobal = 1) AND IsRead = 0";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("NotificationDAL.MarkAllAsRead Error: " + ex.Message);
            return false;
        }
    }

    public bool DeleteNotification(int notificationId, int userId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"DELETE FROM Notifications 
                                 WHERE Id = @Id AND (UserId = @UserId OR IsGlobal = 1)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", notificationId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("NotificationDAL.DeleteNotification Error: " + ex.Message);
            return false;
        }
    }

    public int GetUnreadCount(int userId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT COUNT(*) 
                                 FROM Notifications 
                                 WHERE (UserId = @UserId OR IsGlobal = 1)
                                 AND IsRead = 0
                                 AND (ExpiresAt IS NULL OR ExpiresAt > GETDATE())";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("NotificationDAL.GetUnreadCount Error: " + ex.Message);
            return 0;
        }
    }
} 