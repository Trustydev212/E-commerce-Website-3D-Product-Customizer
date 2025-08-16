using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Footer : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubscribe_Click(object sender, EventArgs e)
    {
        string email = txtEmail.Text.Trim();
        if (!string.IsNullOrEmpty(email))
        {
            try
            {
                // TODO: Add email to newsletter subscription
                // For now, just show success message
                Response.Write("<script>alert('Đăng ký thành công! Cảm ơn bạn đã đăng ký nhận thông tin.');</script>");
                txtEmail.Text = "";
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('Lỗi đăng ký: {0}');</script>", ex.Message));
            }
        }
        else
        {
            Response.Write("<script>alert('Vui lòng nhập địa chỉ email.');</script>");
        }
    }
} 