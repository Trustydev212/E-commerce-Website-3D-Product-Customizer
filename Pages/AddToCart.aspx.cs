using System;
using System.Web;
using App_Code;

public partial class AddToCart : System.Web.UI.Page
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
                    Response.Write("ERROR: User not logged in");
                    return;
                }

                int userId = Convert.ToInt32(Session["UserId"]);
                
                // Lấy form data
                string productIdStr = Request.Form["productId"];
                string size = Request.Form["size"];
                string color = Request.Form["color"];
                string quantityStr = Request.Form["quantity"];
                bool isCustomDesign = Request.Form["customDesign"] == "true";
                
                // Lấy thông tin thiết kế từ localStorage (nếu có)
                string designData = Request.Form["designData"] ?? "";
                
                // Validate input
                if (string.IsNullOrEmpty(productIdStr))
                {
                    Response.Write("ERROR: Product ID is required");
                    return;
                }
                
                int productId = Convert.ToInt32(productIdStr);
                int quantity = Convert.ToInt32(quantityStr);

                // Lấy thông tin sản phẩm
                ProductDAL productDAL = new ProductDAL();
                Product product = productDAL.GetProductById(productId);

                if (product == null)
                {
                    Response.Write("ERROR: Product not found");
                    return;
                }
                
                // Tạo cart item
                CartItem cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    ProductName = product.Name,
                    ProductImagePath = product.ImagePath,
                    Size = size,
                    Color = color,
                    Price = product.Price,
                    Quantity = quantity,
                    CustomDesign = isCustomDesign ? designData : "", // Lưu JSON data thực tế
                    CreatedDate = DateTime.Now
                };
                
                // Thêm vào giỏ hàng
                CartDAL cartDAL = new CartDAL();
                bool success = cartDAL.AddToCart(cartItem);
                
                if (success)
                {
                    // Lấy số lượng giỏ hàng mới
                    int newCartCount = cartDAL.GetCartItemCount(userId);
                    Response.Write(String.Format("SUCCESS: Product added to cart|{0}", newCartCount));
                }
                else
                {
                    Response.Write("ERROR: Failed to add product to cart");
                }
            }
            catch (Exception ex)
            {
                Response.Write("ERROR: " + ex.Message);
            }
        }
        else
        {
            Response.Write("ERROR: Invalid request method");
        }
    }
} 