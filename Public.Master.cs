using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Public : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set userId vào hidden field cho JavaScript
            if (Session["UserId"] != null)
            {
                hdnUserId.Value = Session["UserId"].ToString();
            }

            // Hiển thị/ẩn menu dựa trên trạng thái đăng nhập
            bool isLoggedIn = Session["UserId"] != null;
            
            liCart.Visible = isLoggedIn;
            liProfile.Visible = isLoggedIn;
            liLogin.Visible = !isLoggedIn;
            liRegister.Visible = !isLoggedIn;
            liLogout.Visible = isLoggedIn;
            
            // Cập nhật số lượng giỏ hàng nếu user đã đăng nhập
            if (isLoggedIn)
            {
                UpdateCartCount();
            }
        }
    }
    
    public void UpdateCartCount()
    {
        try
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            CartDAL cartDAL = new CartDAL();
            int cartItemCount = cartDAL.GetCartItemCount(userId);
            
            // Cập nhật số lượng hiển thị trong navigation
            cartCount.InnerText = cartItemCount.ToString();
            
            System.Diagnostics.Debug.WriteLine(String.Format("[DEBUG] Public.Master - Cart count updated: {0} items for user {1}", cartItemCount, userId));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("[ERROR] Public.Master.UpdateCartCount: {0}", ex.Message));
            cartCount.InnerText = "0";
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        // Xóa session
        Session.Clear();
        Session.Abandon();
        
        // Chuyển về trang chủ
        Response.Redirect("~/Pages/Default.aspx");
    }
}