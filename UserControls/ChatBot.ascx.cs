using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Web.Services;

public partial class ChatBot : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Kiểm tra nếu là AJAX request
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            HandleAjaxRequest();
        }
    }

    private void HandleAjaxRequest()
    {
        string userMessage = Request.Form[txtMessage.UniqueID];
        if (!string.IsNullOrEmpty(userMessage))
        {
            try
            {
                // Generate AI response
                string botResponse = GenerateAIResponse(userMessage);
                
                // Trả về JSON response
                Response.ContentType = "application/json";
                Response.Write("{\"botResponse\": \"" + HttpUtility.JavaScriptStringEncode(botResponse) + "\"}");
                Response.End();
            }
            catch (Exception ex)
            {
                Response.ContentType = "application/json";
                Response.Write("{\"botResponse\": \"Xin lỗi, có lỗi xảy ra. Vui lòng thử lại.\"}");
                Response.End();
            }
        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        // Fallback cho trường hợp không phải AJAX
        string userMessage = txtMessage.Text.Trim();
        if (!string.IsNullOrEmpty(userMessage))
        {
            try
            {
                // Add user message to chat
                string script = String.Format("addMessage('{0}', false);", userMessage);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "userMessage", script, true);
                
                // Generate AI response
                string botResponse = GenerateAIResponse(userMessage);
                
                // Add bot response to chat
                script = String.Format("addMessage('{0}', true);", botResponse);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "botMessage", script, true);
                
                // Clear input
                txtMessage.Text = "";
            }
            catch (Exception ex)
            {
                string errorScript = String.Format("addMessage('Xin lỗi, có lỗi xảy ra. Vui lòng thử lại.', true);");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "errorMessage", errorScript, true);
            }
        }
    }

    private string GenerateAIResponse(string userMessage)
    {
        // Simple AI response logic - in real application, this would connect to AI service
        userMessage = userMessage.ToLower();
        
        if (userMessage.Contains("sản phẩm") || userMessage.Contains("áo"))
        {
            return "Chúng tôi có các sản phẩm: Áo thun, Áo hoodie, Áo polo, Áo jacket. Tất cả đều có thể thiết kế 3D tùy chỉnh. Bạn quan tâm sản phẩm nào?";
        }
        else if (userMessage.Contains("thiết kế") || userMessage.Contains("3d"))
        {
            return "Để thiết kế 3D, bạn có thể: <br/>1. Chọn sản phẩm<br/>2. Upload logo/hình ảnh<br/>3. Sử dụng công cụ thiết kế 3D<br/>4. Xem trước mockup<br/>5. Đặt hàng<br/>Bạn cần hỗ trợ bước nào?";
        }
        else if (userMessage.Contains("giá") || userMessage.Contains("tiền"))
        {
            return "Giá sản phẩm từ 150,000 - 500,000 VND tùy loại. Thiết kế 3D miễn phí. Có chương trình khuyến mãi cho đơn hàng từ 2 sản phẩm trở lên!";
        }
        else if (userMessage.Contains("đổi") || userMessage.Contains("trả"))
        {
            return "Chính sách đổi trả trong 7 ngày với sản phẩm chưa qua sử dụng. Sản phẩm thiết kế riêng chỉ đổi khi lỗi từ nhà sản xuất. Bạn cần hỗ trợ gì thêm?";
        }
        else if (userMessage.Contains("giao hàng") || userMessage.Contains("ship"))
        {
            return "Giao hàng toàn quốc:<br/>- Nội thành: 1-2 ngày<br/>- Tỉnh thành: 2-3 ngày<br/>- Miễn phí ship đơn từ 300,000 VND<br/>- COD hỗ trợ toàn quốc";
        }
        else if (userMessage.Contains("xin chào") || userMessage.Contains("hello"))
        {
            return "Xin chào! Chào mừng bạn đến với 3D T-Shirt Design. Tôi có thể giúp bạn tìm hiểu về sản phẩm, thiết kế 3D, hoặc hỗ trợ đặt hàng. Bạn cần hỗ trợ gì?";
        }
        else
        {
            return "Cảm ơn bạn đã liên hệ! Tôi chưa hiểu rõ câu hỏi. Bạn có thể hỏi về:<br/>- Sản phẩm và giá cả<br/>- Hướng dẫn thiết kế 3D<br/>- Chính sách đổi trả<br/>- Giao hàng<br/>Hoặc liên hệ hotline: 0123-456-789";
        }
    }
}