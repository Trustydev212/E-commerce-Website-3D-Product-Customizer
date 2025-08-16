using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code;

public partial class AdminPages_AdminNotifications : System.Web.UI.Page
{
    private NotificationDAL notificationDAL = new NotificationDAL();
    private UserDAL userDAL = new UserDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        // Debug: Log session info
        System.Diagnostics.Debug.WriteLine("Session UserId: " + (Session["UserId"] ?? "null"));
        System.Diagnostics.Debug.WriteLine("Session UserRole: " + (Session["UserRole"] ?? "null"));
        System.Diagnostics.Debug.WriteLine("Session Role: " + (Session["Role"] ?? "null"));
        
        // Kiểm tra quyền admin
        if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
        {
            Response.Redirect("../Pages/Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            LoadUsers();
            LoadNotifications();
        }
    }

    private void LoadUsers()
    {
        try
        {
            var users = userDAL.GetAllUsers();
            ddlUsers.DataSource = users;
            ddlUsers.DataTextField = "FullName";
            ddlUsers.DataValueField = "Id";
            ddlUsers.DataBind();
            ddlUsers.Items.Insert(0, new ListItem("-- Chọn user --", ""));
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi khi tải danh sách user: " + ex.Message, "danger");
        }
    }

    private void LoadNotifications()
    {
        try
        {
            var notifications = notificationDAL.GetGlobalNotifications(50); // Lấy 50 thông báo gần nhất
            rptNotifications.DataSource = notifications;
            rptNotifications.DataBind();

            pnlNoData.Visible = notifications.Count == 0;
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi khi tải danh sách thông báo: " + ex.Message, "danger");
        }
    }

    protected void btnCreateNotification_Click(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsValid)
                return;

            Notification notification = new Notification
            {
                Title = txtTitle.Text.Trim(),
                Message = txtMessage.Text.Trim(),
                Type = ddlType.SelectedValue,
                Icon = string.IsNullOrEmpty(txtIcon.Text) ? null : txtIcon.Text.Trim(),
                ActionUrl = string.IsNullOrEmpty(txtActionUrl.Text) ? null : txtActionUrl.Text.Trim(),
                IsRead = false,
                IsGlobal = chkIsGlobal.Checked,
                CreatedAt = DateTime.Now,
                CreatedBy = Session["UserName"] != null ? Session["UserName"].ToString() : "Admin"
            };

            // Xử lý thời gian hết hạn
            if (!string.IsNullOrEmpty(txtExpiresAt.Text))
            {
                DateTime expiresAt;
                if (DateTime.TryParse(txtExpiresAt.Text, out expiresAt))
                {
                    notification.ExpiresAt = expiresAt;
                }
            }

            // Xử lý user cụ thể
            if (!chkIsGlobal.Checked && !string.IsNullOrEmpty(ddlUsers.SelectedValue))
            {
                int userId;
                if (int.TryParse(ddlUsers.SelectedValue, out userId))
                {
                    notification.UserId = userId;
                }
            }

            int notificationId = notificationDAL.InsertNotification(notification);
            if (notificationId > 0)
            {
                ShowMessage("Tạo thông báo thành công!", "success");
                ClearForm();
                LoadNotifications();
            }
            else
            {
                ShowMessage("Có lỗi xảy ra khi tạo thông báo.", "danger");
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi khi tạo thông báo: " + ex.Message, "danger");
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
    }

    private void ClearForm()
    {
        txtTitle.Text = "";
        txtMessage.Text = "";
        ddlType.SelectedIndex = 0;
        txtIcon.Text = "";
        txtActionUrl.Text = "";
        txtExpiresAt.Text = "";
        chkIsGlobal.Checked = false;
        ddlUsers.SelectedIndex = 0;
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
            // Lấy user ID hiện tại (admin)
            int adminId = Convert.ToInt32(Session["UserId"]);
            bool success = notificationDAL.MarkAllAsRead(adminId);
            
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
            int adminId = Convert.ToInt32(Session["UserId"]);

            switch (e.CommandName)
            {
                case "MarkRead":
                    bool markSuccess = notificationDAL.MarkAsRead(notificationId, adminId);
                    if (markSuccess)
                    {
                        ShowMessage("Đã đánh dấu thông báo là đã đọc.", "success");
                        LoadNotifications();
                    }
                    break;

                case "Delete":
                    bool deleteSuccess = notificationDAL.DeleteNotification(notificationId, adminId);
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

                case "Edit":
                    // TODO: Implement edit functionality
                    ShowMessage("Chức năng chỉnh sửa đang được phát triển.", "info");
                    break;
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi: " + ex.Message, "danger");
        }
    }

    public string GetTypeDisplayName(string type)
    {
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