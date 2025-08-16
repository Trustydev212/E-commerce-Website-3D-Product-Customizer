using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminPages_Orders : System.Web.UI.Page
{
    private OrderDAL orderDAL = new OrderDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadOrders();
        }
    }

    private void LoadOrders()
    {
        try
        {
            List<Order> orders = orderDAL.GetAllOrders();
            gvOrders.DataSource = orders;
            gvOrders.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>alert('Lỗi tải danh sách đơn hàng: {0}');</script>", ex.Message));
        }
    }

    protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int orderId = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "EditOrder")
        {
            Response.Redirect(string.Format("OrderEdit.aspx?id={0}", orderId));
        }
        else if (e.CommandName == "DeleteOrder")
        {
            try
            {
                bool result = orderDAL.DeleteOrder(orderId);
                if (result)
                {
                    Response.Write("<script>alert('Xóa đơn hàng thành công!');</script>");
                    LoadOrders();
                }
                else
                {
                    Response.Write("<script>alert('Xóa đơn hàng thất bại!');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('Lỗi xóa đơn hàng: {0}');</script>", ex.Message));
            }
        }
    }

    public string GetStatusDisplayName(string status)
    {
        switch (status.ToLower())
        {
            case "pending": return "Chờ xác nhận";
            case "confirmed": return "Đã xác nhận";
            case "processing": return "Đang xử lý";
            case "shipped": return "Đã gửi hàng";
            case "delivered": return "Đã giao hàng";
            case "cancelled": return "Đã hủy";
            default: return status;
        }
    }

    public string GetPaymentStatusDisplayName(string paymentStatus)
    {
        switch (paymentStatus.ToLower())
        {
            case "pending": return "Chờ thanh toán";
            case "paid": return "Đã thanh toán";
            case "failed": return "Thanh toán thất bại";
            case "refunded": return "Đã hoàn tiền";
            default: return paymentStatus;
        }
    }
}