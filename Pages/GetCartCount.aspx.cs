using System;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

public partial class GetCartCount : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.RequestType == "POST")
        {
            try
            {
                // Kiểm tra user đã đăng nhập chưa
                if (Session["UserId"] == null)
                {
                    Response.Write("{\"success\":false,\"message\":\"User not logged in\",\"count\":0}");
                    return;
                }

                int userId = Convert.ToInt32(Session["UserId"]);
                
                // Lấy số lượng giỏ hàng
                CartDAL cartDAL = new CartDAL();
                int cartCount = cartDAL.GetCartItemCount(userId);
                
                // Trả về JSON response
                Response.ContentType = "application/json";
                Response.Write(String.Format("{{\"success\":true,\"count\":{0}}}", cartCount));
            }
            catch (Exception ex)
            {
                Response.ContentType = "application/json";
                Response.Write(String.Format("{{\"success\":false,\"message\":\"{0}\",\"count\":0}}", ex.Message));
            }
        }
        else
        {
            Response.ContentType = "application/json";
            Response.Write("{\"success\":false,\"message\":\"Invalid request method\",\"count\":0}");
        }
    }
} 