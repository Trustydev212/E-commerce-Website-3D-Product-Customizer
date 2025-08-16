<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Footer.ascx.cs" Inherits="Footer" %>
<link href="~/Css/global-fonts.css" rel="stylesheet" type="text/css" />

<!-- Đồng bộ footer với Public.Master -->
<footer class="bg-dark text-light py-5 mt-5">
    <div class="container">
        <div class="row">
            <div class="col-md-4 mb-4">
                <h5><i class="fas fa-tshirt me-2"></i>3D T-Shirt</h5>
                <p>Nền tảng thiết kế áo thun 3D tùy chỉnh hàng đầu Việt Nam</p>
            </div>
            <div class="col-md-2 mb-4">
                <h6>Sản phẩm</h6>
                <ul class="list-unstyled">
                    <li><a href="<%= ResolveUrl("~/Pages/Products.aspx") %>" class="text-light text-decoration-none">Áo thun</a></li>
                    <li><a href="<%= ResolveUrl("~/Pages/Products.aspx") %>?type=hoodie" class="text-light text-decoration-none">Hoodie</a></li>
                    <li><a href="<%= ResolveUrl("~/Pages/Products.aspx") %>?type=polo" class="text-light text-decoration-none">Polo</a></li>
                </ul>
            </div>
            <div class="col-md-2 mb-4">
                <h6>Hỗ trợ</h6>
                <ul class="list-unstyled">
                    <li><a href="<%= ResolveUrl("~/Pages/Contact.aspx") %>" class="text-light text-decoration-none">Liên hệ</a></li>
                    <li><a href="<%= ResolveUrl("~/Pages/Guide.aspx") %>" class="text-light text-decoration-none">Hướng dẫn</a></li>
                    <li><a href="<%= ResolveUrl("~/Pages/FAQ.aspx") %>" class="text-light text-decoration-none">FAQ</a></li>
                </ul>
            </div>
            <div class="col-md-4 mb-4">
                <h6>Theo dõi chúng tôi</h6>
                <div class="social-links">
                    <a href="https://facebook.com/" class="text-light me-3" target="_blank"><i class="fab fa-facebook-f"></i></a>
                    <a href="https://instagram.com/" class="text-light me-3" target="_blank"><i class="fab fa-instagram"></i></a>
                    <a href="https://youtube.com/" class="text-light me-3" target="_blank"><i class="fab fa-youtube"></i></a>
                </div>
                <div class="newsletter mt-3">
                    <h6>Đăng ký nhận tin</h6>
                    <div class="input-group">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Nhập email của bạn..." />
                        <asp:Button ID="btnSubscribe" runat="server" CssClass="btn btn-primary" Text="Đăng ký" OnClick="btnSubscribe_Click" />
                    </div>
                </div>
            </div>
        </div>
        <hr class="my-4" />
        <div class="text-center">
            <p>&copy; 2024 3D T-Shirt. Tất cả quyền được bảo lưu.</p>
        </div>
    </div>
</footer>
<style>
    .site-footer, .site-footer * { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; }
</style>