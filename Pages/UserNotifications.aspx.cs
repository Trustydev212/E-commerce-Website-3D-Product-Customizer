using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserNotifications : System.Web.UI.Page
{
    private NotificationDAL notificationDAL = new NotificationDAL();
    private int currentPage = 1;
    private int pageSize = 10;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Kiểm tra đăng nhập
        if (Session["UserId"] == null)
        {
            Response.Redirect("Login.aspx?ReturnUrl=" + Request.RawUrl);
            return;
        }

        if (!IsPostBack)
        {
            LoadNotifications();
        }
        else
        {
            // Reload data on postback to avoid ViewState serialization issues
            LoadNotifications();
        }
    }

    private void LoadNotifications()
    {
        try
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            List<Notification> allNotifications = notificationDAL.GetNotificationsByUserId(userId, 100); // Lấy 100 thông báo gần nhất
            
            var filteredNotifications = ApplyFilters(allNotifications);
            BindNotifications(filteredNotifications);
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi khi tải thông báo: " + ex.Message, "danger");
        }
    }

    private List<Notification> ApplyFilters(List<Notification> allNotifications)
    {
        var filteredNotifications = allNotifications.AsEnumerable();

        // Lọc theo loại
        if (!string.IsNullOrEmpty(ddlFilterType.SelectedValue))
        {
            filteredNotifications = filteredNotifications.Where(n => n.Type == ddlFilterType.SelectedValue);
        }

        // Lọc theo trạng thái
        if (!string.IsNullOrEmpty(ddlFilterStatus.SelectedValue))
        {
            if (ddlFilterStatus.SelectedValue == "unread")
            {
                filteredNotifications = filteredNotifications.Where(n => n.IsRead == false);
            }
            else if (ddlFilterStatus.SelectedValue == "read")
            {
                filteredNotifications = filteredNotifications.Where(n => n.IsRead == true);
            }
        }

        // Lọc theo phạm vi
        if (!string.IsNullOrEmpty(ddlFilterScope.SelectedValue))
        {
            if (ddlFilterScope.SelectedValue == "personal")
            {
                filteredNotifications = filteredNotifications.Where(n => n.IsGlobal == false);
            }
            else if (ddlFilterScope.SelectedValue == "global")
            {
                filteredNotifications = filteredNotifications.Where(n => n.IsGlobal == true);
            }
        }

        // Sắp xếp theo thời gian tạo (mới nhất trước)
        filteredNotifications = filteredNotifications.OrderByDescending(n => n.CreatedAt);

        return filteredNotifications.ToList();
    }

    private void BindNotifications(List<Notification> filteredNotifications)
    {
        // Phân trang
        int totalItems = filteredNotifications.Count;
        int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        
        if (currentPage > totalPages && totalPages > 0)
            currentPage = totalPages;
        
        var pagedNotifications = filteredNotifications
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        rptNotifications.DataSource = pagedNotifications;
        rptNotifications.DataBind();

        // Hiển thị trạng thái trống
        pnlNoData.Visible = pagedNotifications.Count == 0;

        // Hiển thị phân trang
        if (totalPages > 1)
        {
            litCurrentPage.Text = currentPage.ToString();
            litTotalPages.Text = totalPages.ToString();
            btnPrev.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;
            paginationContainer.Visible = true;
        }
        else
        {
            paginationContainer.Visible = false;
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        LoadNotifications();
        ShowMessage("Đã làm mới danh sách thông báo.", "info");
    }

    protected void btnMarkAllRead_Click(object sender, EventArgs e)
    {
        try
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            bool success = notificationDAL.MarkAllAsRead(userId);
            
            if (success)
            {
                ShowMessage("Đã đánh dấu tất cả thông báo là đã đọc.", "success");
                LoadNotifications();
            }
            else
            {
                ShowMessage("Có lỗi xảy ra khi đánh dấu đã đọc.", "danger");
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi: " + ex.Message, "danger");
        }
    }

    protected void rptNotifications_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            int notificationId = Convert.ToInt32(e.CommandArgument);
            int userId = Convert.ToInt32(Session["UserId"]);

            switch (e.CommandName)
            {
                case "MarkRead":
                    bool markSuccess = notificationDAL.MarkAsRead(notificationId, userId);
                    if (markSuccess)
                    {
                        ShowMessage("Đã đánh dấu thông báo là đã đọc.", "success");
                        LoadNotifications();
                    }
                    break;

                case "Delete":
                    bool deleteSuccess = notificationDAL.DeleteNotification(notificationId, userId);
                    if (deleteSuccess)
                    {
                        ShowMessage("Đã xóa thông báo thành công.", "success");
                        LoadNotifications();
                    }
                    else
                    {
                        ShowMessage("Có lỗi xảy ra khi xóa thông báo.", "danger");
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi: " + ex.Message, "danger");
        }
    }

    protected void ddlFilterType_SelectedIndexChanged(object sender, EventArgs e)
    {
        currentPage = 1;
        LoadNotifications();
    }

    protected void ddlFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        currentPage = 1;
        LoadNotifications();
    }

    protected void ddlFilterScope_SelectedIndexChanged(object sender, EventArgs e)
    {
        currentPage = 1;
        LoadNotifications();
    }

    protected void btnPrev_Click(object sender, EventArgs e)
    {
        if (currentPage > 1)
        {
            currentPage--;
            LoadNotifications();
        }
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        // Reload data to get current count for pagination
        int userId = Convert.ToInt32(Session["UserId"]);
        List<Notification> allNotifications = notificationDAL.GetNotificationsByUserId(userId, 100);
        var filteredNotifications = ApplyFilters(allNotifications);
        
        int totalPages = (int)Math.Ceiling((double)filteredNotifications.Count / pageSize);
        
        if (currentPage < totalPages)
        {
            currentPage++;
            LoadNotifications();
        }
    }

    public string GetTypeDisplayName(string type)
    {
        if (string.IsNullOrEmpty(type))
            return "Thông tin";
            
        switch (type.ToLower())
        {
            case "info": return "Thông tin";
            case "success": return "Thành công";
            case "warning": return "Cảnh báo";
            case "error": return "Lỗi";
            default: return type;
        }
    }

    private void ShowMessage(string message, string type = "info")
    {
        lblMessage.Text = message;
        pnlMessage.CssClass = "alert alert-" + type;
        pnlMessage.Visible = true;
    }
} 