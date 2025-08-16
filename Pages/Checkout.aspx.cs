using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Script.Serialization;

public partial class Checkout : System.Web.UI.Page
{
    private List<CartItem> cartItems;
    private decimal subtotal = 0;
    private decimal shipping = 0;
    private decimal tax = 0;
    private decimal total = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Luôn load cart items
        LoadCartItems();

        if (!IsPostBack)
        {
            txtFullName.Attributes.Add("placeholder", "Nhập họ và tên");
            txtPhone.Attributes.Add("placeholder", "Nhập số điện thoại");
            txtEmail.Attributes.Add("placeholder", "Nhập email");
            txtAddress.Attributes.Add("placeholder", "Nhập địa chỉ giao hàng");
            txtNotes.Attributes.Add("placeholder", "Ghi chú thêm (tùy chọn)");
            
            // Check if user is logged in
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx?ReturnUrl=" + Request.RawUrl);
                return;
            }

            // Check if cart is empty
            if (cartItems == null || cartItems.Count == 0)
            {
                Response.Redirect("Cart.aspx");
                return;
            }

            LoadUserInfo();
            LoadOrderSummary();
        }
    }

    private void LoadCartItems()
    {
        try
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            CartDAL cartDAL = new CartDAL();
            var carts = cartDAL.GetCartByUserId(userId);
            cartItems = carts.Select(c => new CartItem {
                Id = c.Id,
                UserId = c.UserId,
                ProductId = c.ProductId,
                ProductName = c.ProductName,
                ProductImagePath = c.ProductImagePath,
                Color = c.Color,
                Size = c.Size,
                Price = c.Price,
                Quantity = c.Quantity,
                CreatedDate = c.CreatedDate,
                CustomDesign = c.CustomDesign
            }).ToList();
        }
        catch (Exception ex)
        {
            ShowMessage("Có lỗi xảy ra khi tải thông tin giỏ hàng: " + ex.Message, "danger");
        }
    }

    private void LoadUserInfo()
    {
        try
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            UserDAL userDAL = new UserDAL();
            User user = userDAL.GetUserById(userId);
            
            if (user != null)
            {
                txtFullName.Text = user.FullName;
                txtPhone.Text = user.Phone;
                txtEmail.Text = user.Email;
                txtAddress.Text = user.Address;
            }
        }
        catch (Exception ex)
        {
            // If error loading user info, continue with empty fields
        }
    }

    private void LoadOrderSummary()
    {
        try
        {
            // Luôn tính lại tổng tiền từ cartItems
            subtotal = cartItems.Sum(item => item.Price * item.Quantity);
            shipping = 30000; // Fixed shipping cost
            tax = subtotal * 0.1m; // 10% VAT
            total = subtotal + shipping + tax;

            // Bind order items
            rptOrderItems.DataSource = cartItems;
            rptOrderItems.DataBind();

            // Update labels
            lblSubtotal.Text = String.Format("{0:N0} đ", subtotal);
            lblShipping.Text = String.Format("{0:N0} đ", shipping);
            lblTax.Text = String.Format("{0:N0} đ", tax);
            lblTotal.Text = String.Format("{0:N0} đ", total);
        }
        catch (Exception ex)
        {
            ShowMessage("Có lỗi xảy ra khi tải thông tin đơn hàng: " + ex.Message, "danger");
        }
    }

    protected void btnPlaceOrder_Click(object sender, EventArgs e)
    {
        try
        {
            // Lấy designId từ hidden field (nếu có)
            int designIdFromClient = 0;
            if (!string.IsNullOrEmpty(hdnDesignId.Value))
                int.TryParse(hdnDesignId.Value, out designIdFromClient);

            // Validate form
            if (!Page.IsValid)
            {
                return;
            }

            // Debug: Check cartItems
            if (cartItems == null || cartItems.Count == 0)
            {
                ShowMessage("Giỏ hàng trống hoặc chưa được tải!", "danger");
                return;
            }

            // Debug: Log cart items
            System.Diagnostics.Debug.WriteLine(string.Format("Cart Debug - Items count: {0}", cartItems.Count));
            foreach (var item in cartItems)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Cart Item - Product: {0}, Price: {1}, Quantity: {2}, Total: {3}", 
                    item.ProductName, item.Price, item.Quantity, item.Price * item.Quantity));
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            string paymentMethod = rblPaymentMethod.SelectedValue;

            // Recalculate totals to ensure they are correct
            subtotal = cartItems.Sum(item => item.Price * item.Quantity);
            shipping = 30000; // Fixed shipping cost
            tax = subtotal * 0.1m; // 10% VAT
            total = subtotal + shipping + tax;

            // Debug: Log calculated values
            System.Diagnostics.Debug.WriteLine(string.Format("Calculated Debug - Subtotal: {0}, Shipping: {1}, Tax: {2}, Total: {3}", 
                subtotal, shipping, tax, total));

            // Create order
            Order order = new Order
            {
                UserId = userId,
                CustomerName = txtFullName.Text.Trim(),
                CustomerPhone = txtPhone.Text.Trim(),
                CustomerEmail = txtEmail.Text.Trim(),
                ShippingAddress = txtAddress.Text.Trim(),
                Notes = txtNotes.Text.Trim(),
                PaymentMethod = paymentMethod,
                PaymentStatus = paymentMethod == "COD" ? "Pending" : "Pending",
                Status = "Pending",
                Subtotal = subtotal,
                ShippingFee = shipping,
                Tax = tax,
                Total = total,
                CreatedDate = DateTime.Now
            };

            // Debug: Log order values
            System.Diagnostics.Debug.WriteLine(string.Format("Order Debug - Subtotal: {0}, ShippingFee: {1}, Tax: {2}, Total: {3}", 
                order.Subtotal, order.ShippingFee, order.Tax, order.Total));
            
            // Log to admin logs for debugging
            OrderDAL orderDAL = new OrderDAL();
            orderDAL.LogAdminAction(userId, "Order Creation Debug", 
                string.Format("Subtotal: {0}, ShippingFee: {1}, Tax: {2}, Total: {3}", 
                order.Subtotal, order.ShippingFee, order.Tax, order.Total));

            // Insert order
            int orderId = orderDAL.InsertOrder(order);

            // Sau khi tạo order, lưu payment
            if (orderId > 0)
            {
                Payment payment = new Payment
                {
                    OrderId = orderId,
                    PaymentMethod = order.PaymentMethod,
                    Amount = order.Total,
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };
                PaymentDAL paymentDAL = new PaymentDAL();
                paymentDAL.InsertPayment(payment);

                // Debug: Log order creation success
                orderDAL.LogAdminAction(userId, "Order Created", 
                    string.Format("Order ID: {0}, Items count: {1}", orderId, cartItems.Count));

                // Insert order items and handle design data
                int successCount = 0;
                foreach (var item in cartItems)
                {
                    int? designId = null;
                    
                    // Nếu có custom design JSON, tạo mới như cũ
                    if (!string.IsNullOrEmpty(item.CustomDesign) && item.CustomDesign != "Custom Design" && item.CustomDesign != "true")
                    {
                        try
                        {
                            var serializer = new JavaScriptSerializer();
                            var designData = serializer.Deserialize<dynamic>(item.CustomDesign);
                            
                            // Tạo thiết kế mới
                            Design design = new Design
                            {
                                UserId = userId,
                                ProductId = item.ProductId,
                                Name = "Custom Design - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                                LogoPath = designData["logoImage"] != null ? designData["logoImage"].ToString() : null,
                                PositionData = item.CustomDesign, // Lưu toàn bộ JSON data
                                PreviewPath = designData["screenshotDataUrl"] != null ? designData["screenshotDataUrl"].ToString() : null,
                                IsPublic = false,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            };

                            DesignDAL designDAL = new DesignDAL();
                            designId = designDAL.InsertDesign(design);
                            
                            // Log thành công
                            orderDAL.LogAdminAction(userId, "Design Created", 
                                string.Format("Design ID: {0}, Product: {1}", designId, item.ProductName));
                        }
                        catch (Exception ex)
                        {
                            orderDAL.LogAdminAction(userId, "Design Error", 
                                string.Format("Error creating design for product {0}: {1}", item.ProductName, ex.Message));
                        }
                    }
                    else if (!string.IsNullOrEmpty(item.CustomDesign) && item.CustomDesign == "true" && designIdFromClient > 0)
                    {
                        // Nếu là sản phẩm custom (CustomDesign == "true"), lấy designId từ client
                        designId = designIdFromClient;
                    }

                    // Tạo OrderItem với DesignId
                    OrderItem orderItem = new OrderItem
                    {
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Color = item.Color,
                        Size = item.Size,
                        DesignId = designId // Liên kết với Design nếu có
                    };

                    orderDAL.InsertOrderItem(orderItem);
                    successCount++;
                    
                    // Debug: Log success
                    orderDAL.LogAdminAction(userId, "OrderItem Success", 
                        string.Format("Item {0} saved successfully, DesignId: {1}", successCount, designId));
                }

                // Debug: Log final result
                orderDAL.LogAdminAction(userId, "OrderItems Complete", 
                    string.Format("Total items: {0}, Success: {1}", cartItems.Count, successCount));

                // Clear cart
                CartDAL cartDAL = new CartDAL();
                cartDAL.ClearCart(userId);

                // Clear session
                Session.Remove("CartSubtotal");
                Session.Remove("CartShipping");
                Session.Remove("CartTax");
                Session.Remove("CartTotal");

                // Tạo thông báo đặt hàng thành công
                NotificationHelper.CreateOrderSuccessNotification(userId, orderId, total);

                // Redirect to success page or show success message
                Session["OrderId"] = orderId;
                ShowMessage("Đặt hàng thành công! Mã đơn hàng: " + orderId, "success");
                
                // Auto redirect after 3 seconds
                Response.AddHeader("REFRESH", "3;URL=UserProfile.aspx");
            }
            else
            {
                ShowMessage("Có lỗi xảy ra khi đặt hàng. Vui lòng thử lại.", "danger");
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Có lỗi xảy ra khi đặt hàng: " + ex.Message, "danger");
            // Log error
            LogError(ex);
        }
    }

    private void ShowMessage(string message, string type = "info")
    {
        lblMessage.Text = message;
        pnlMessage.CssClass = "alert alert-" + type;
        pnlMessage.Visible = true;
    }

    private void LogError(Exception ex)
    {
        try
        {
            OrderDAL orderDAL = new OrderDAL();
            int userId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;
            orderDAL.LogAdminAction(userId, "Checkout Error", ex.Message + " - " + ex.StackTrace);
        }
        catch
        {
            // Ignore logging errors
        }
    }
}