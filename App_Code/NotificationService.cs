using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class NotificationService : System.Web.Services.WebService
{
    private NotificationDAL notificationDAL = new NotificationDAL();

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public object GetNotifications(int userId, int limit = 20)
    {
        try
        {
            var notifications = notificationDAL.GetNotificationsByUserId(userId, limit);
            return new { success = true, data = notifications };
        }
        catch (Exception ex)
        {
            return new { success = false, message = ex.Message };
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public object GetUnreadCount(int userId)
    {
        try
        {
            int count = notificationDAL.GetUnreadCount(userId);
            return new { success = true, count = count };
        }
        catch (Exception ex)
        {
            return new { success = false, message = ex.Message };
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public object MarkAsRead(int notificationId, int userId)
    {
        try
        {
            bool success = notificationDAL.MarkAsRead(notificationId, userId);
            return new { success = success };
        }
        catch (Exception ex)
        {
            return new { success = false, message = ex.Message };
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public object MarkAllAsRead(int userId)
    {
        try
        {
            bool success = notificationDAL.MarkAllAsRead(userId);
            return new { success = success };
        }
        catch (Exception ex)
        {
            return new { success = false, message = ex.Message };
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public object DeleteNotification(int notificationId, int userId)
    {
        try
        {
            bool success = notificationDAL.DeleteNotification(notificationId, userId);
            return new { success = success };
        }
        catch (Exception ex)
        {
            return new { success = false, message = ex.Message };
        }
    }
} 