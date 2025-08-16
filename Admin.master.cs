using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckAuthentication();
        }
    }

    private void CheckAuthentication()
    {
        // Kiểm tra user đã đăng nhập chưa
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/Pages/Login.aspx");
            return;
        }

        // Kiểm tra role có phải Admin không
        if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
        {
            Response.Redirect("~/Pages/UserProfile.aspx");
            return;
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Redirect("~/Pages/Login.aspx");
    }
}
