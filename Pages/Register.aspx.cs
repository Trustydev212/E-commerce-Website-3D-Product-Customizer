using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using App_Code;

public partial class Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtUsername.Attributes.Add("placeholder", "Nhập tên đăng nhập");
            txtEmail.Attributes.Add("placeholder", "Nhập email của bạn");
            txtFullName.Attributes.Add("placeholder", "Nhập họ và tên đầy đủ");
            txtPhone.Attributes.Add("placeholder", "Nhập số điện thoại");
            txtAddress.Attributes.Add("placeholder", "Nhập địa chỉ của bạn");
            txtPassword.Attributes.Add("placeholder", "Nhập mật khẩu");
            txtConfirmPassword.Attributes.Add("placeholder", "Nhập lại mật khẩu");
            
            // Redirect if already logged in
            if (Session["UserId"] != null)
            {
                Response.Redirect("~/Pages/UserProfile.aspx");
            }
        }
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        try
        {
            // Validate form
            if (!Page.IsValid)
            {
                ShowMessage("Vui lòng kiểm tra lại thông tin đăng ký.", "warning");
                return;
            }

            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string fullName = txtFullName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            string password = txtPassword.Text.Trim();
            
            // Validate terms agreement
            if (!chkAgreeTerms.Checked)
            {
                ShowMessage("Vui lòng đồng ý với điều khoản sử dụng.", "warning");
                return;
            }
            
            // Check if username or email already exists
            UserDAL userDAL = new UserDAL();
            
            // Test database connection first
            if (!userDAL.TestDatabaseConnection())
            {
                ShowMessage("Lỗi kết nối database. Vui lòng liên hệ quản trị viên.", "danger");
                return;
            }
            
            if (userDAL.IsUsernameExists(username))
            {
                ShowMessage("Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.", "danger");
                return;
            }
            
            if (userDAL.IsEmailExists(email))
            {
                ShowMessage("Email đã được đăng ký. Vui lòng sử dụng email khác hoặc đăng nhập.", "danger");
                return;
            }
            
            // Hash password
            string hashedPassword = HashPassword(password);
            
            // Create user object
            User newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = hashedPassword,
                FullName = fullName,
                Phone = phone,
                Address = address,
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.Now
            };
            
            // Insert user into database
            int userId = userDAL.InsertUser(newUser);
            
            if (userId > 0)
            {
                // Registration successful
                ShowMessage("Đăng ký thành công! Bạn có thể đăng nhập ngay bây giờ.", "success");
                
                // Clear form
                ClearForm();
                
                // Redirect to login page after 2 seconds
                Response.AddHeader("REFRESH", "2;URL=Login.aspx");
            }
            else
            {
                ShowMessage("Đã xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại.", "danger");
            }
        }
        catch (Exception ex)
        {
            // Log detailed error information
            string errorDetails = string.Format("Registration Error: {0} - StackTrace: {1}", ex.Message, ex.StackTrace);
            System.Diagnostics.Debug.WriteLine(errorDetails);
            
            // Show user-friendly error message with details
            ShowMessage("Lỗi đăng ký: " + ex.Message, "danger");
            
            // Log error
            LogError(ex);
        }
    }

    private string HashPassword(string password)
    {
        // Dùng SHA1 giống như khi đăng nhập
        return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
    }

    private void ShowMessage(string message, string type = "info")
    {
        lblMessage.Text = message;
        pnlMessage.CssClass = "alert alert-" + type;
        pnlMessage.Visible = true;
    }

    private void ClearForm()
    {
        txtUsername.Text = "";
        txtEmail.Text = "";
        txtFullName.Text = "";
        txtPhone.Text = "";
        txtAddress.Text = "";
        txtPassword.Text = "";
        txtConfirmPassword.Text = "";
        chkAgreeTerms.Checked = false;
    }

    private void LogError(Exception ex)
    {
        try
        {
            OrderDAL orderDAL = new OrderDAL();
            orderDAL.LogAdminAction(0, "Register Error", ex.Message + " - " + ex.StackTrace);
        }
        catch
        {
            // Ignore logging errors
        }
    }
} 