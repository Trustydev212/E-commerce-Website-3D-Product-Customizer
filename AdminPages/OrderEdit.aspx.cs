using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code;

public partial class AdminPages_OrderEdit : System.Web.UI.Page
{
    private OrderDAL orderDAL = new OrderDAL();
    private UserDAL userDAL = new UserDAL();
    private ProductDAL productDAL = new ProductDAL();
    private int orderId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Parse orderId on every request (both initial load and PostBack)
        if (Request.QueryString["id"] != null)
        {
            if (int.TryParse(Request.QueryString["id"], out orderId))
            {
                // Only load data on initial page load
                if (!IsPostBack)
                {
                    LoadOrderData();
                }
            }
            else
            {
                Response.Redirect("Orders.aspx");
            }
        }
        else
        {
            Response.Redirect("Orders.aspx");
        }
    }

    private void LoadOrderData()
    {
        try
        {
            // Debug: Log the order ID being searched
            System.Diagnostics.Debug.WriteLine(string.Format("LoadOrderData - Searching for order ID: {0}", orderId));
            
            Order order = orderDAL.GetOrderById(orderId);
            
            // Debug: Log the result
            System.Diagnostics.Debug.WriteLine(string.Format("LoadOrderData - GetOrderById result: {0}", order != null ? "Found" : "Not Found"));
            
            if (order != null)
            {
                // Debug: Log order details
                System.Diagnostics.Debug.WriteLine(string.Format("LoadOrderData - Order found: ID={0}, Status={1}, Customer={2}", 
                    order.Id, order.Status, order.CustomerName));
                
                // Order basic info
                lblOrderId.Text = order.Id.ToString();
                lblCreatedDate.Text = order.CreatedDate.ToString("dd/MM/yyyy HH:mm");
                
                // Order status
                ddlStatus.SelectedValue = order.Status;
                ddlPaymentStatus.SelectedValue = order.PaymentStatus;
                txtTrackingNumber.Text = order.TrackingNumber;
                txtNotes.Text = order.Notes;
                
                // Customer info
                User customer = userDAL.GetUserById(order.UserId);
                if (customer != null)
                {
                    lblCustomerName.Text = customer.FullName;
                    lblCustomerEmail.Text = customer.Email;
                    lblCustomerPhone.Text = customer.Phone;
                }
                
                // Addresses
                lblShippingAddress.Text = order.ShippingAddress.Replace("\n", "<br/>");
                
                // Payment info
                lblPaymentMethod.Text = order.PaymentMethod;
                
                // Order totals
                decimal subtotal = order.Total - 30000 - (order.Total * 0.1m); // Subtract shipping and tax
                decimal tax = subtotal * 0.1m;
                
                lblSubtotal.Text = string.Format("{0:C}", subtotal);
                lblTax.Text = string.Format("{0:C}", tax);
                lblTotal.Text = string.Format("{0:C}", order.Total);
                
                // Load order items
                LoadOrderItems();
            }
            else
            {
                // Debug: Log that order was not found
                System.Diagnostics.Debug.WriteLine(string.Format("LoadOrderData - Order ID {0} not found in database", orderId));
                
                // Show error message instead of redirecting
                Response.Write(string.Format("<script>alert('Không tìm thấy đơn hàng với ID: {0}!');</script>", orderId));
                
                // Still redirect after showing the message
                Response.Redirect("Orders.aspx");
            }
        }
        catch (Exception ex)
        {
            // Debug: Log the exception
            System.Diagnostics.Debug.WriteLine(string.Format("LoadOrderData - Exception: {0}", ex.Message));
            Response.Write(string.Format("<script>alert('Lỗi tải dữ liệu đơn hàng: {0}');</script>", ex.Message));
        }
    }

    private void LoadOrderItems()
    {
        try
        {
            // Lấy dữ liệu thực từ database
            List<OrderItem> orderItems = orderDAL.GetOrderItemsByOrderId(orderId);
            
            if (orderItems != null && orderItems.Count > 0)
            {
                rptOrderItems.DataSource = orderItems;
                rptOrderItems.DataBind();
            }
            else
            {
                // Hiển thị thông báo không có sản phẩm
                rptOrderItems.Controls.Add(new LiteralControl("<div class='text-center text-muted py-4'><p>Không có sản phẩm nào trong đơn hàng này</p></div>"));
            }
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>alert('Lỗi tải chi tiết đơn hàng: {0}');</script>", ex.Message));
        }
    }

    protected void btnUpdateStatus_Click(object sender, EventArgs e)
    {
        try
        {
            // Test: Kiểm tra PostBack có hoạt động không
            Response.Write("<script>alert('PostBack hoạt động! Đang cập nhật...');</script>");
            
            // Debug: Log trước khi cập nhật
            System.Diagnostics.Debug.WriteLine(string.Format("Updating order {0} - Status: {1}, PaymentStatus: {2}", 
                orderId, ddlStatus.SelectedValue, ddlPaymentStatus.SelectedValue));
            
            Order order = orderDAL.GetOrderById(orderId);
            if (order != null)
            {
                // Debug: Log giá trị cũ
                System.Diagnostics.Debug.WriteLine(string.Format("Old values - Status: {0}, PaymentStatus: {1}", 
                    order.Status, order.PaymentStatus));
                
                order.Status = ddlStatus.SelectedValue;
                order.PaymentStatus = ddlPaymentStatus.SelectedValue;
                order.TrackingNumber = txtTrackingNumber.Text.Trim();
                order.Notes = txtNotes.Text.Trim();
                
                // Debug: Log giá trị mới
                System.Diagnostics.Debug.WriteLine(string.Format("New values - Status: {0}, PaymentStatus: {1}", 
                    order.Status, order.PaymentStatus));
                
                bool result = orderDAL.UpdateOrder(order);
                
                // Debug: Log kết quả
                System.Diagnostics.Debug.WriteLine(string.Format("Update result: {0}", result));
                
                if (result)
                {
                    Response.Write("<script>alert('Cập nhật trạng thái đơn hàng thành công!');</script>");
                    
                    // Send notification email to customer (if implemented)
                    // SendStatusUpdateEmail(order);
                    
                    // Refresh the page to show updated info
                    LoadOrderData();
                }
                else
                {
                    Response.Write("<script>alert('Cập nhật trạng thái thất bại!');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Không tìm thấy đơn hàng!');</script>");
            }
        }
        catch (Exception ex)
        {
            // Debug: Log lỗi
            System.Diagnostics.Debug.WriteLine(string.Format("Error updating order: {0}", ex.Message));
            Response.Write(string.Format("<script>alert('Lỗi cập nhật trạng thái: {0}');</script>", ex.Message));
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Orders.aspx");
    }

    // Helper method để xử lý đường dẫn ảnh sản phẩm
    protected string GetImageUrl(object imagePath)
    {
        if (imagePath == null || string.IsNullOrEmpty(imagePath.ToString()))
        {
            return ResolveUrl("~/Image/placeholder.jpg");
        }
        return ResolveUrl(imagePath.ToString());
    }

    // Helper method để xử lý đường dẫn thiết kế
    protected string GetDesignUrl(string designPath)
    {
        if (string.IsNullOrEmpty(designPath))
        {
            return "#";
        }
        
        // Kiểm tra nếu là Base64 data URL
        if (designPath.StartsWith("data:image/"))
        {
            return "#"; // Không tạo link cho Base64 data
        }
        
        // Nếu là đường dẫn tương đối, thêm prefix
        if (!designPath.StartsWith("http") && !designPath.StartsWith("/"))
        {
            return ResolveUrl("~/Upload/Designs/" + designPath);
        }
        
        return ResolveUrl(designPath);
    }

    // Helper method to send status update email (to be implemented)
    private void SendStatusUpdateEmail(Order order)
    {
        // This would send an email to the customer about the status update
        // Implementation would depend on your email service
    }
}