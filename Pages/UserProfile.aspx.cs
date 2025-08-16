using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using App_Code; // <-- Dòng này rất quan trọng!

public partial class UserProfile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUserProfile();
            LoadUserOrders();
            LoadUserDesigns();
        }
    }

    private void LoadUserProfile()
    {
        try
        {
            // Kiểm tra session
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx?returnUrl=" + Server.UrlEncode(Request.Url.ToString()));
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            UserDAL userDal = new UserDAL();
            User user = userDal.GetUserById(userId);

            if (user != null)
            {
                // Hiển thị thông tin user
                userName.InnerText = user.FullName;
                userEmail.InnerText = user.Email;
                
                // Load thông tin vào form
                txtFullName.Text = user.FullName;
                txtEmail.Text = user.Email;
                txtPhone.Text = user.Phone;
                txtAddress.Text = user.Address;
                
                if (user.BirthDate.HasValue)
                {
                    txtBirthDate.Text = user.BirthDate.Value.ToString("yyyy-MM-dd");
                }

                // Load avatar
                if (!string.IsNullOrEmpty(user.AvatarPath))
                {
                    imgAvatar.ImageUrl = user.AvatarPath;
                }
                else
                {
                    imgAvatar.ImageUrl = "~/Images/avatar-default.png";
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi khi tải thông tin cá nhân: " + ex.Message, "error");
        }
    }

    private void LoadUserOrders()
    {
        try
        {
            if (Session["UserId"] == null) return;

            int userId = Convert.ToInt32(Session["UserId"]);
            OrderDAL orderDal = new OrderDAL();
            List<Order> orders = orderDal.GetOrdersByUserId(userId);

            rptOrders.DataSource = orders;
            rptOrders.DataBind();

            if (orders.Count == 0)
            {
                // Hiển thị thông báo không có đơn hàng
                rptOrders.Controls.Add(new LiteralControl("<div class='text-center text-muted py-4'><i class='fas fa-shopping-bag fa-3x mb-3'></i><p>Bạn chưa có đơn hàng nào</p></div>"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi khi tải danh sách đơn hàng: " + ex.Message, "error");
        }
    }

    private void LoadUserDesigns()
    {
        try
        {
            if (Session["UserId"] == null) return;

            int userId = Convert.ToInt32(Session["UserId"]);
            DesignDAL designDal = new DesignDAL();
            List<Design> designs = designDal.GetDesignsByUserId(userId);
            
            rptDesigns.DataSource = designs;
            rptDesigns.DataBind();

            if (designs.Count == 0)
            {
                // Hiển thị thông báo không có thiết kế
                rptDesigns.Controls.Add(new LiteralControl("<div class='text-center text-muted py-4'><i class='fas fa-paint-brush fa-3x mb-3'></i><p>Bạn chưa có thiết kế nào</p></div>"));
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi khi tải danh sách thiết kế: " + ex.Message, "error");
        }
    }

    protected string GetStatusColor(string status)
    {
        switch (status.ToLower())
        {
            case "pending":
            case "chờ xử lý":
                return "warning";
            case "processing":
            case "đang xử lý":
                return "info";
            case "shipped":
            case "đã gửi":
                return "primary";
            case "delivered":
            case "đã giao":
                return "success";
            case "cancelled":
            case "đã hủy":
                return "danger";
            default:
                return "secondary";
        }
    }

    protected void btnUpdateProfile_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserId"] == null)
            {
                ShowMessage("Vui lòng đăng nhập để cập nhật thông tin", "error");
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            UserDAL userDal = new UserDAL();
            User user = userDal.GetUserById(userId);

            if (user != null)
            {
                // Cập nhật thông tin
                user.FullName = txtFullName.Text.Trim();
                user.Phone = txtPhone.Text.Trim();
                user.Address = txtAddress.Text.Trim();
                
                if (!string.IsNullOrEmpty(txtBirthDate.Text))
                {
                    user.BirthDate = DateTime.Parse(txtBirthDate.Text);
                }

                // Lưu vào database
                bool success = userDal.UpdateUser(user);
                
                if (success)
                {
                    ShowMessage("Cập nhật thông tin thành công!", "success");
                    // Cập nhật session
                    Session["FullName"] = user.FullName;
                    LoadUserProfile(); // Reload để hiển thị thông tin mới
                }
                else
                {
                    ShowMessage("Có lỗi xảy ra khi cập nhật thông tin", "error");
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi khi cập nhật thông tin: " + ex.Message, "error");
        }
    }

    protected void btnChangeAvatar_Click(object sender, EventArgs e)
    {
        try
        {
            // Debug: Kiểm tra session chi tiết
            ShowMessage("Debug - Session UserId: " + (Session["UserId"] ?? "NULL"), "info");
            ShowMessage("Debug - Session FullName: " + (Session["FullName"] ?? "NULL"), "info");
            ShowMessage("Debug - Session Email: " + (Session["Email"] ?? "NULL"), "info");
            ShowMessage("Debug - Session Username: " + (Session["Username"] ?? "NULL"), "info");
            ShowMessage("Debug - Session Count: " + Session.Count, "info");
            ShowMessage("Debug - Session ID: " + Session.SessionID, "info");
            
            if (Session["UserId"] == null)
            {
                ShowMessage("Vui lòng đăng nhập để đổi ảnh đại diện", "error");
                return;
            }

            if (fuAvatar.HasFile)
            {
                // Kiểm tra file type
                string extension = Path.GetExtension(fuAvatar.FileName).ToLower();
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                
                if (!allowedExtensions.Contains(extension))
                {
                    ShowMessage("Chỉ chấp nhận file ảnh: JPG, PNG, GIF", "error");
                    return;
                }

                // Kiểm tra kích thước file (max 5MB)
                if (fuAvatar.FileBytes.Length > 5 * 1024 * 1024)
                {
                    ShowMessage("Kích thước file không được vượt quá 5MB", "error");
                    return;
                }

                int userId = Convert.ToInt32(Session["UserId"]);
                string fileName = "avatar_" + userId + "_" + DateTime.Now.Ticks + extension;
                string uploadPath = Server.MapPath("~/Images/Avatars/");
                
                // Tạo thư mục nếu chưa có
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string filePath = Path.Combine(uploadPath, fileName);
                
                // Lưu file trước
                fuAvatar.SaveAs(filePath);

                // Xử lý resize ảnh an toàn hơn
                try
                {
                    using (var originalImage = System.Drawing.Image.FromFile(filePath))
                    {
                        // Chỉ resize nếu ảnh quá lớn
                        if (originalImage.Width > 300 || originalImage.Height > 300)
                        {
                            // Tính toán kích thước mới giữ nguyên tỷ lệ
                            int newWidth, newHeight;
                            if (originalImage.Width > originalImage.Height)
                            {
                                newWidth = 300;
                                newHeight = (int)((float)originalImage.Height * 300 / originalImage.Width);
                            }
                            else
                            {
                                newHeight = 300;
                                newWidth = (int)((float)originalImage.Width * 300 / originalImage.Height);
                            }

                            using (var resizedImage = new System.Drawing.Bitmap(originalImage, newWidth, newHeight))
                            {
                                // Tạo file tạm
                                string tempPath = filePath + ".tmp";
                                resizedImage.Save(tempPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                
                                // Xóa file cũ và đổi tên file tạm
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }
                                File.Move(tempPath, filePath);
                            }
                        }
                    }
                }
                catch (Exception imgEx)
                {
                    // Nếu lỗi resize, vẫn giữ file gốc
                    ShowMessage("Lưu ảnh thành công nhưng không thể resize. Lỗi: " + imgEx.Message, "warning");
                }

                // Cập nhật database
                UserDAL userDal = new UserDAL();
                User user = userDal.GetUserById(userId);
                if (user != null)
                {
                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(user.AvatarPath) && user.AvatarPath != "~/Images/avatar-default.png")
                    {
                        string oldAvatarPath = Server.MapPath(user.AvatarPath);
                        if (File.Exists(oldAvatarPath))
                        {
                            try
                            {
                                File.Delete(oldAvatarPath);
                            }
                            catch
                            {
                                // Bỏ qua lỗi xóa file cũ
                            }
                        }
                    }

                    user.AvatarPath = "~/Images/Avatars/" + fileName;
                    bool success = userDal.UpdateUser(user);
                    
                    if (success)
                    {
                        ShowMessage("Đổi ảnh đại diện thành công!", "success");
                        imgAvatar.ImageUrl = user.AvatarPath;
                    }
                    else
                    {
                        ShowMessage("Có lỗi xảy ra khi cập nhật ảnh đại diện", "error");
                    }
                }
            }
            else
            {
                ShowMessage("Vui lòng chọn file ảnh", "error");
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi khi đổi ảnh đại diện: " + ex.Message, "error");
        }
    }

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserId"] == null)
            {
                ShowMessage("Vui lòng đăng nhập để đổi mật khẩu", "error");
                return;
            }

            string currentPassword = txtCurrentPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // Validation
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                ShowMessage("Vui lòng điền đầy đủ thông tin", "error");
                return;
            }

            if (newPassword != confirmPassword)
            {
                ShowMessage("Mật khẩu mới và xác nhận mật khẩu không khớp", "error");
                return;
            }

            if (newPassword.Length < 6)
            {
                ShowMessage("Mật khẩu mới phải có ít nhất 6 ký tự", "error");
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            UserDAL userDal = new UserDAL();
            User user = userDal.GetUserById(userId);

            if (user != null)
            {
                // Kiểm tra mật khẩu hiện tại
                string hashedCurrentPassword = HashPassword(currentPassword);
                if (user.Password != hashedCurrentPassword)
                {
                    ShowMessage("Mật khẩu hiện tại không đúng", "error");
                    return;
                }

                // Cập nhật mật khẩu mới
                user.Password = HashPassword(newPassword);
                bool success = userDal.UpdateUser(user);

                if (success)
                {
                    ShowMessage("Đổi mật khẩu thành công!", "success");
                    // Clear form
                    txtCurrentPassword.Text = "";
                    txtNewPassword.Text = "";
                    txtConfirmPassword.Text = "";
                }
                else
                {
                    ShowMessage("Có lỗi xảy ra khi đổi mật khẩu", "error");
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi khi đổi mật khẩu: " + ex.Message, "error");
        }
    }

    protected void btnDeleteDesign_Command(object sender, CommandEventArgs e)
    {
        try
        {
            if (Session["UserId"] == null)
            {
                ShowMessage("Vui lòng đăng nhập để xóa thiết kế", "error");
                return;
            }

            if (e.CommandName == "DeleteDesign")
            {
                int designId = Convert.ToInt32(e.CommandArgument);
                int userId = Convert.ToInt32(Session["UserId"]);

                DesignDAL designDal = new DesignDAL();
                bool success = designDal.DeleteDesign(designId, userId);
                
                if (success)
                {
                    ShowMessage("Xóa thiết kế thành công!", "success");
                    LoadUserDesigns(); // Reload danh sách
                }
                else
                {
                    ShowMessage("Không thể xóa thiết kế hoặc thiết kế không tồn tại", "error");
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Lỗi khi xóa thiết kế: " + ex.Message, "error");
        }
    }

    private string HashPassword(string password)
    {
        // Sử dụng cùng logic hash như trong Login
        return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
    }

    private void ShowMessage(string message, string type)
    {
        string script = string.Format("showMessage('{0}', '{1}');", message.Replace("'", "\\'"), type);
        ScriptManager.RegisterStartupScript(this, GetType(), "ShowMessage", script, true);
    }
}