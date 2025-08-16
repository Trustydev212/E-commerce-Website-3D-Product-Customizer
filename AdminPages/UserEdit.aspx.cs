using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code;

public partial class AdminPages_UserEdit : System.Web.UI.Page
{
    private UserDAL userDAL = new UserDAL();
    private int userId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                if (int.TryParse(Request.QueryString["id"], out userId))
                {
                    LoadUserData();
                    lblTitle.Text = "Chỉnh sửa người dùng";
                }
                else
                {
                    Response.Redirect("Users.aspx");
                }
            }
            else
            {
                lblTitle.Text = "Thêm người dùng mới";
                lblNoAvatar.Visible = true;
                imgPreview.Visible = false;
            }
        }
    }

    private void LoadUserData()
    {
        try
        {
            User user = userDAL.GetUserById(userId);
            if (user != null)
            {
                txtUsername.Text = user.Username;
                txtEmail.Text = user.Email;
                txtFullName.Text = user.FullName;
                txtPhone.Text = user.Phone;
                txtAddress.Text = user.Address;
                ddlRole.SelectedValue = user.Role;
                cbIsActive.Checked = user.IsActive;
                
                // Password field should be empty for editing
                txtPassword.Text = "";
                rfvPassword.Enabled = false; // Don't require password for editing
                
                if (!string.IsNullOrEmpty(user.AvatarPath))
                {
                    lblCurrentAvatar.Text = "Avatar hiện tại: " + user.AvatarPath;
                    imgPreview.ImageUrl = "~/" + user.AvatarPath;
                    imgPreview.Visible = true;
                    lblNoAvatar.Visible = false;
                }
                else
                {
                    lblNoAvatar.Visible = true;
                    imgPreview.Visible = false;
                }
            }
            else
            {
                Response.Redirect("Users.aspx");
            }
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>alert('Lỗi tải dữ liệu người dùng: {0}');</script>", ex.Message));
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                User user = new User();
                
                // Basic info
                user.Username = txtUsername.Text.Trim();
                user.Email = txtEmail.Text.Trim();
                user.FullName = txtFullName.Text.Trim();
                user.Phone = txtPhone.Text.Trim();
                user.Address = txtAddress.Text.Trim();
                user.Role = ddlRole.SelectedValue;
                user.IsActive = cbIsActive.Checked;
                
                // Handle password
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    // Hash password in real application
                    user.Password = txtPassword.Text; // Should be hashed
                }
                else if (Request.QueryString["id"] != null)
                {
                    // Keep existing password for edit
                    User existingUser = userDAL.GetUserById(int.Parse(Request.QueryString["id"]));
                    if (existingUser != null)
                    {
                        user.Password = existingUser.Password;
                    }
                }
                
                // Handle avatar upload
                string avatarPath = HandleAvatarUpload();
                if (!string.IsNullOrEmpty(avatarPath))
                {
                    user.AvatarPath = avatarPath;
                }
                else if (Request.QueryString["id"] != null)
                {
                    // Keep existing avatar for edit
                    User existingUser = userDAL.GetUserById(int.Parse(Request.QueryString["id"]));
                    if (existingUser != null)
                    {
                        user.AvatarPath = existingUser.AvatarPath;
                    }
                }
                
                bool result = false;
                if (Request.QueryString["id"] != null)
                {
                    // Update existing user
                    user.Id = int.Parse(Request.QueryString["id"]);
                    result = userDAL.UpdateUser(user);
                }
                else
                {
                    // Create new user
                    user.CreatedDate = DateTime.Now;
                    int newId = userDAL.InsertUser(user);
                    result = newId > 0;
                }
                
                if (result)
                {
                    Response.Redirect("Users.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Lưu người dùng thất bại!');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('Lỗi lưu người dùng: {0}');</script>", ex.Message));
            }
        }
    }

    private string HandleAvatarUpload()
    {
        if (fuAvatar.HasFile)
        {
            try
            {
                string fileName = fuAvatar.FileName;
                string fileExtension = Path.GetExtension(fileName).ToLower();
                
                // Check file extension
                if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".gif")
                {
                    // Generate unique filename
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    string uploadPath = Server.MapPath("~/Upload/Avatars/");
                    
                    // Create directory if not exists
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    
                    string fullPath = Path.Combine(uploadPath, uniqueFileName);
                    fuAvatar.SaveAs(fullPath);
                    
                    return "Upload/Avatars/" + uniqueFileName;
                }
                else
                {
                    Response.Write("<script>alert('Chỉ chấp nhận file hình ảnh (.jpg, .jpeg, .png, .gif)');</script>");
                    return "";
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('Lỗi upload avatar: {0}');</script>", ex.Message));
                return "";
            }
        }
        return "";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Users.aspx");
    }
}