using System;
using App_Code;

public partial class OrderDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int orderId;
            if (int.TryParse(Request.QueryString["id"], out orderId))
            {
                OrderDAL orderDal = new OrderDAL();
                Order order = orderDal.GetOrderById(orderId);
                if (order != null)
                {
                    lblOrderInfo.Text = string.Format(@"
    <div class='order-details-row'><span class='order-details-label'>Mã đơn:</span> {0}</div>
    <div class='order-details-row'><span class='order-details-label'>Khách hàng:</span> {1}</div>
    <div class='order-details-row'><span class='order-details-label'>Email:</span> {2}</div>
    <div class='order-details-row'><span class='order-details-label'>Số điện thoại:</span> {3}</div>
    <div class='order-details-row'><span class='order-details-label'>Địa chỉ giao hàng:</span> {4}</div>
    <div class='order-details-row'><span class='order-details-label'>Trạng thái:</span> {5}</div>
    <div class='order-details-row'><span class='order-details-label'>Ngày tạo:</span> {6:dd/MM/yyyy HH:mm}</div>
    <div class='order-details-row'><span class='order-details-label'>Tạm tính:</span> {8:N0} đ</div>
    <div class='order-details-row'><span class='order-details-label'>Phí vận chuyển:</span> {9:N0} đ</div>
    <div class='order-details-row'><span class='order-details-label'>Thuế VAT:</span> {10:N0} đ</div>
    <div class='order-details-row'><span class='order-details-label'>Tổng tiền:</span> <span class='text-primary fw-bold'>{7:N0} đ</span></div>
    <div class='order-details-row'><span class='order-details-label'>TotalAmount:</span> {11:N0} đ</div>
",
    order.Id,
    order.CustomerName,
    order.CustomerEmail,
    order.CustomerPhone,
    order.ShippingAddress,
    order.Status,
    order.CreatedDate,
    order.Total,
    order.Subtotal,
    order.ShippingFee,
    order.Tax,
    order.TotalAmount
);
                }
                else
                {
                    lblOrderInfo.Text = "Không tìm thấy đơn hàng.";
                }
            }
            else
            {
                lblOrderInfo.Text = "Thiếu mã đơn hàng.";
            }
        }
    }
} 