using System;
using System.Web.UI;

public partial class Header : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UpdateNavigation();
            UpdateCartCount();
        }
    }

    private void UpdateNavigation()
    {
        if (Session["UserId"] != null)
        {
            // User is logged in
            liLogin.Visible = false;
            liRegister.Visible = false;
            liProfile.Visible = true;
            liLogout.Visible = true;
            liCart.Visible = true;
        }
        else
        {
            // User is not logged in
            liLogin.Visible = true;
            liRegister.Visible = true;
            liProfile.Visible = false;
            liLogout.Visible = false;
            liCart.Visible = false;
        }
    }

    private void UpdateCartCount()
    {
        if (Session["UserId"] != null)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                CartDAL cartDAL = new CartDAL();
                int count = cartDAL.GetCartItemCount(userId);
                cartCount.InnerText = count.ToString();
            }
            catch
            {
                cartCount.InnerText = "0";
            }
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Redirect("~/Pages/Default.aspx");
    }
} 