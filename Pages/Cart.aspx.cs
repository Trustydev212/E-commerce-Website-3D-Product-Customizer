using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

public partial class Cart : System.Web.UI.Page
{
    private List<CartItem> cartItems;
    private decimal subtotal = 0;
    private decimal shipping = 30000; // Fixed shipping cost
    private decimal tax = 0;
    private decimal total = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine(String.Format("[DEBUG] Page_Load - IsPostBack: {0}", IsPostBack));
        
        if (!IsPostBack)
        {
            // Check if user is logged in
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx?ReturnUrl=" + Request.RawUrl);
                return;
            }

            LoadCartItems();
            CalculateTotals();
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("[DEBUG] Page_Load - PostBack detected, cart will be reloaded after command execution");
        }
    }

    private void LoadCartItems()
    {
        try
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            System.Diagnostics.Debug.WriteLine(String.Format("[DEBUG] LoadCartItems - Loading cart for user: {0}", userId));
            
            CartDAL cartDAL = new CartDAL();
            var carts = cartDAL.GetCartByUserId(userId);
            int cartCount = (carts != null) ? carts.Count : 0;
            System.Diagnostics.Debug.WriteLine(String.Format("[DEBUG] LoadCartItems - Found {0} items in database", cartCount));
            
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

            int itemCount = (cartItems != null) ? cartItems.Count : 0;
            System.Diagnostics.Debug.WriteLine(String.Format("[DEBUG] LoadCartItems - Converted to {0} CartItem objects", itemCount));

            if (cartItems != null && cartItems.Count > 0)
            {
                rptCartItems.DataSource = cartItems;
                rptCartItems.DataBind();
                pnlCartItems.Visible = true;
                pnlEmptyCart.Visible = false;
                System.Diagnostics.Debug.WriteLine("[DEBUG] LoadCartItems - Cart items displayed");
            }
            else
            {
                pnlCartItems.Visible = false;
                pnlEmptyCart.Visible = true;
                System.Diagnostics.Debug.WriteLine("[DEBUG] LoadCartItems - Empty cart displayed");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("[ERROR] LoadCartItems: {0}", ex.Message));
            ShowMessage("Có lỗi xảy ra khi tải giỏ hàng: " + ex.Message, "danger");
        }
    }

    private void CalculateTotals()
    {
        if (cartItems != null && cartItems.Count > 0)
        {
            subtotal = cartItems.Sum(item => item.Price * item.Quantity);
            tax = subtotal * 0.1m; // 10% VAT
            total = subtotal + shipping + tax;
        }
        else
        {
            subtotal = 0;
            tax = 0;
            total = 0;
            shipping = 0;
        }

        // Update labels
        lblSubtotal.Text = String.Format("{0:N0} đ", subtotal);
        lblShipping.Text = String.Format("{0:N0} đ", shipping);
        lblTax.Text = String.Format("{0:N0} đ", tax);
        lblTotal.Text = String.Format("{0:N0} đ", total);
    }

    protected void rptCartItems_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            CartDAL cartDAL = new CartDAL();

            if (e.CommandName == "UpdateQuantity")
            {
                string[] args = e.CommandArgument.ToString().Split(',');
                int cartId = Convert.ToInt32(args[0]);
                int newQuantity = Convert.ToInt32(args[1]);

                if (newQuantity <= 0)
                {
                    // Remove item if quantity is 0 or less
                    cartDAL.RemoveFromCart(cartId);
                    ShowMessage("Đã xóa sản phẩm khỏi giỏ hàng", "success");
                }
                else
                {
                    cartDAL.UpdateCartQuantity(cartId, newQuantity);
                    ShowMessage("Đã cập nhật số lượng sản phẩm", "success");
                }
            }
            else if (e.CommandName == "RemoveItem")
            {
                int cartId = Convert.ToInt32(e.CommandArgument);
                System.Diagnostics.Debug.WriteLine(String.Format("[DEBUG] Removing cart item with ID: {0}", cartId));
                
                // Kiểm tra xem item có tồn tại không trước khi xóa
                var existingItems = cartDAL.GetCartByUserId(userId);
                var itemToRemove = existingItems.FirstOrDefault(item => item.Id == cartId);
                
                if (itemToRemove != null)
                {
                    cartDAL.RemoveFromCart(cartId);
                    ShowMessage("Đã xóa sản phẩm khỏi giỏ hàng", "success");
                    System.Diagnostics.Debug.WriteLine(String.Format("[DEBUG] Successfully removed cart item: {0}", cartId));
                }
                else
                {
                    ShowMessage("Không tìm thấy sản phẩm để xóa", "warning");
                    System.Diagnostics.Debug.WriteLine(String.Format("[DEBUG] Cart item not found: {0}", cartId));
                }
            }

            // Reload cart
            LoadCartItems();
            CalculateTotals();
            
            // Cập nhật số lượng giỏ hàng trong navigation
            UpdateMasterPageCartCount();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("[ERROR] rptCartItems_ItemCommand: {0}", ex.Message));
            ShowMessage("Có lỗi xảy ra: " + ex.Message, "danger");
        }
    }
    
    private void UpdateMasterPageCartCount()
    {
        try
        {
            // Gọi JavaScript function để cập nhật cart count
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateCartCount", 
                "updateCartCount();", true);
            System.Diagnostics.Debug.WriteLine("[DEBUG] Cart count update script registered");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("[ERROR] UpdateMasterPageCartCount: {0}", ex.Message));
        }
    }

    protected void btnClearCart_Click(object sender, EventArgs e)
    {
        try
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            CartDAL cartDAL = new CartDAL();
            cartDAL.ClearCart(userId);
            
            ShowMessage("Đã xóa toàn bộ giỏ hàng", "success");
            LoadCartItems();
            CalculateTotals();
            
            // Cập nhật số lượng giỏ hàng trong navigation
            UpdateMasterPageCartCount();
        }
        catch (Exception ex)
        {
            ShowMessage("Có lỗi xảy ra khi xóa giỏ hàng: " + ex.Message, "danger");
        }
    }

    protected void btnApplyCoupon_Click(object sender, EventArgs e)
    {
        string couponCode = txtCouponCode.Text.Trim();
        
        if (string.IsNullOrEmpty(couponCode))
        {
            ShowMessage("Vui lòng nhập mã giảm giá", "warning");
            return;
        }

        // Check coupon validity (simplified example)
        if (couponCode.ToUpper() == "DISCOUNT10")
        {
            decimal discount = subtotal * 0.1m; // 10% discount
            total = subtotal + shipping + tax - discount;
            lblTotal.Text = String.Format("{0:N0} đ", total);
            ShowMessage("Áp dụng mã giảm giá thành công! Giảm " + String.Format("{0:N0} đ", discount), "success");
        }
        else
        {
            ShowMessage("Mã giảm giá không hợp lệ", "danger");
        }
    }

    protected void btnCheckout_Click(object sender, EventArgs e)
    {
        try
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            CartDAL cartDAL = new CartDAL();
            var items = cartDAL.GetCartByUserId(userId);

            if (items == null || items.Count == 0)
            {
                ShowMessage("Giỏ hàng trống. Vui lòng thêm sản phẩm trước khi thanh toán", "warning");
                return;
            }

            // Store cart total in session for checkout
            Session["CartTotal"] = total;
            Session["CartSubtotal"] = subtotal;
            Session["CartShipping"] = shipping;
            Session["CartTax"] = tax;
            
            Response.Redirect("Checkout.aspx");
        }
        catch (Exception ex)
        {
            ShowMessage("Có lỗi xảy ra khi chuyển đến trang thanh toán: " + ex.Message, "danger");
        }
    }

    private void ShowMessage(string message, string type = "info")
    {
        lblMessage.Text = message;
        pnlMessage.CssClass = "alert alert-" + type;
        pnlMessage.Visible = true;
    }
} 