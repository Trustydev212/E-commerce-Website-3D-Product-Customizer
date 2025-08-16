using System;

public static class NotificationHelper
{
    private static NotificationDAL notificationDAL = new NotificationDAL();

    /// <summary>
    /// Tạo thông báo khi user đặt hàng thành công
    /// </summary>
    public static void CreateOrderSuccessNotification(int userId, int orderId, decimal total)
    {
        try
        {
            Notification notification = new Notification
            {
                UserId = userId,
                Title = "Đặt hàng thành công!",
                Message = string.Format("Đơn hàng #{0} của bạn đã được đặt thành công với tổng tiền {1:N0} đ. Chúng tôi sẽ xử lý và giao hàng sớm nhất có thể.", orderId, total),
                Type = "success",
                Icon = "fas fa-check-circle",
                ActionUrl = "UserProfile.aspx?tab=orders",
                IsRead = false,
                IsGlobal = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };

            notificationDAL.InsertNotification(notification);
        }
        catch (Exception ex)
        {
            // Log error if needed
            System.Diagnostics.Debug.WriteLine("Error creating order success notification: " + ex.Message);
        }
    }

    /// <summary>
    /// Tạo thông báo khi đơn hàng được cập nhật trạng thái
    /// </summary>
    public static void CreateOrderStatusNotification(int userId, int orderId, string status, string message)
    {
        try
        {
            string title = "";
            string type = "info";
            string icon = "fas fa-info-circle";

            switch (status.ToLower())
            {
                case "processing":
                    title = "Đơn hàng đang được xử lý";
                    type = "info";
                    icon = "fas fa-cog";
                    break;
                case "shipped":
                    title = "Đơn hàng đã được gửi";
                    type = "success";
                    icon = "fas fa-shipping-fast";
                    break;
                case "delivered":
                    title = "Đơn hàng đã được giao";
                    type = "success";
                    icon = "fas fa-check-double";
                    break;
                case "cancelled":
                    title = "Đơn hàng đã bị hủy";
                    type = "error";
                    icon = "fas fa-times-circle";
                    break;
                default:
                    title = "Cập nhật đơn hàng";
                    break;
            }

            Notification notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                Icon = icon,
                ActionUrl = "UserProfile.aspx?tab=orders",
                IsRead = false,
                IsGlobal = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };

            notificationDAL.InsertNotification(notification);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error creating order status notification: " + ex.Message);
        }
    }

    /// <summary>
    /// Tạo thông báo chào mừng khi user đăng ký
    /// </summary>
    public static void CreateWelcomeNotification(int userId, string userName)
    {
        try
        {
            Notification notification = new Notification
            {
                UserId = userId,
                Title = "Chào mừng bạn đến với 3D T-Shirt!",
                Message = string.Format("Xin chào {0}! Cảm ơn bạn đã đăng ký tài khoản. Hãy khám phá các tính năng thiết kế áo thun 3D tuyệt vời của chúng tôi.", userName),
                Type = "success",
                Icon = "fas fa-heart",
                ActionUrl = "Products.aspx",
                IsRead = false,
                IsGlobal = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };

            notificationDAL.InsertNotification(notification);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error creating welcome notification: " + ex.Message);
        }
    }

    /// <summary>
    /// Tạo thông báo khuyến mãi toàn hệ thống
    /// </summary>
    public static void CreatePromotionNotification(string title, string message, DateTime? expiresAt = null)
    {
        try
        {
            Notification notification = new Notification
            {
                UserId = null, // null = toàn hệ thống
                Title = title,
                Message = message,
                Type = "warning",
                Icon = "fas fa-gift",
                ActionUrl = "Products.aspx",
                IsRead = false,
                IsGlobal = true,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.Now,
                CreatedBy = "Admin"
            };

            notificationDAL.InsertNotification(notification);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error creating promotion notification: " + ex.Message);
        }
    }

    /// <summary>
    /// Tạo thông báo bảo trì hệ thống
    /// </summary>
    public static void CreateMaintenanceNotification(string message, DateTime? expiresAt = null)
    {
        try
        {
            Notification notification = new Notification
            {
                UserId = null,
                Title = "Thông báo bảo trì hệ thống",
                Message = message,
                Type = "warning",
                Icon = "fas fa-tools",
                ActionUrl = null,
                IsRead = false,
                IsGlobal = true,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };

            notificationDAL.InsertNotification(notification);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error creating maintenance notification: " + ex.Message);
        }
    }

    /// <summary>
    /// Tạo thông báo lỗi cho user
    /// </summary>
    public static void CreateErrorNotification(int userId, string title, string message)
    {
        try
        {
            Notification notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = "error",
                Icon = "fas fa-exclamation-triangle",
                ActionUrl = null,
                IsRead = false,
                IsGlobal = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };

            notificationDAL.InsertNotification(notification);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error creating error notification: " + ex.Message);
        }
    }

    /// <summary>
    /// Tạo thông báo thông tin chung
    /// </summary>
    public static void CreateInfoNotification(int userId, string title, string message, string actionUrl = null)
    {
        try
        {
            Notification notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = "info",
                Icon = "fas fa-info-circle",
                ActionUrl = actionUrl,
                IsRead = false,
                IsGlobal = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };

            notificationDAL.InsertNotification(notification);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error creating info notification: " + ex.Message);
        }
    }

    /// <summary>
    /// Tạo thông báo tùy chỉnh
    /// </summary>
    public static int CreateCustomNotification(int? userId, string title, string message, string type = "info", 
        string icon = null, string actionUrl = null, bool isGlobal = false, DateTime? expiresAt = null, string createdBy = "System")
    {
        try
        {
            Notification notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                Icon = icon,
                ActionUrl = actionUrl,
                IsRead = false,
                IsGlobal = isGlobal,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.Now,
                CreatedBy = createdBy
            };

            return notificationDAL.InsertNotification(notification);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error creating custom notification: " + ex.Message);
            return 0;
        }
    }
} 