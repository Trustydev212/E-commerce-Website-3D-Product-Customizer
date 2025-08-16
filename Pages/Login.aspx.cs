using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using App_Code;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtEmail.Attributes.Add("placeholder", "Nhập email của bạn");
            txtPassword.Attributes.Add("placeholder", "Nhập mật khẩu");
            
            // Redirect if already logged in
            if (Session["UserId"] != null)
            {
                Response.Redirect("~/Pages/UserProfile.aspx");
            }
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowMessage("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            // Authenticate user (truyền password gốc)
            UserDAL userDAL = new UserDAL();
            User user = userDAL.AuthenticateUser(email, password);
            
            if (user != null)
            {
                if (!user.IsActive)
                {
                    ShowMessage("Tài khoản của bạn đã bị khóa. Vui lòng liên hệ admin.");
                    return;
                }
                
                // Set session variables
                Session["UserId"] = user.Id;
                Session["Username"] = user.Username;
                Session["Email"] = user.Email;
                Session["FullName"] = user.FullName;
                Session["UserRole"] = user.Role;
                
                // Update last login date
                userDAL.UpdateLastLoginDate(user.Id);
                
                // Set remember me cookie if checked
                if (chkRememberMe.Checked)
                {
                    HttpCookie rememberCookie = new HttpCookie("RememberMe", user.Id.ToString());
                    rememberCookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(rememberCookie);
                }
                
                // Redirect based on role
                if (user.Role == "Admin")
                {
                    Response.Redirect("~/AdminPages/Dashboard.aspx");
                }
                else
                {
                    string returnUrl = Request.QueryString["ReturnUrl"];
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        Response.Redirect(returnUrl);
                    }
                    else
                    {
                        Response.Redirect("~/Pages/UserProfile.aspx");
                    }
                }
            }
            else
            {
                ShowMessage("Email hoặc mật khẩu không chính xác.");
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Đã xảy ra lỗi trong quá trình đăng nhập. Vui lòng thử lại.");
            // Log error
            LogError(ex);
        }
    }

    private void ShowMessage(string message)
    {
        lblMessage.Text = message;
        pnlMessage.Visible = true;
    }

    private void LogError(Exception ex)
    {
        try
        {
            OrderDAL orderDAL = new OrderDAL();
            orderDAL.LogAdminAction(0, "Login Error", ex.Message + " - " + ex.StackTrace);
        }
        catch
        {
            // Ignore logging errors
        }
    }
}