using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code;

public partial class AdminPages_Users : System.Web.UI.Page
{
    private UserDAL userDAL = new UserDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUsers();
        }
    }

    private void LoadUsers()
    {
        try
        {
            List<User> users = userDAL.GetAllUsers();
            gvUsers.DataSource = users;
            gvUsers.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>alert('Lỗi tải danh sách người dùng: {0}');</script>", ex.Message));
        }
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("UserEdit.aspx");
    }

    protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int userId = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "EditUser")
        {
            Response.Redirect("UserEdit.aspx?id=" + userId);
        }
        else if (e.CommandName == "DeleteUser")
        {
            try
            {
                bool result = userDAL.DeleteUser(userId);
                if (result)
                {
                    Response.Write("<script>alert('Xóa người dùng thành công!');</script>");
                    LoadUsers();
                }
                else
                {
                    Response.Write("<script>alert('Xóa người dùng thất bại!');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('Lỗi xóa người dùng: {0}');</script>", ex.Message));
            }
        }
    }
}