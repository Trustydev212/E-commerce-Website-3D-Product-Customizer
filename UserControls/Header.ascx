<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Header" %>
<!-- Navigation (Header) -->
<nav class="navbar navbar-expand-lg navbar-light custom-header-navbar">
    <div class="header-container">
        <a class="navbar-brand fw-bold" href="Default.aspx">
            <i class="fas fa-tshirt me-2"></i>3D T-Shirt
        </a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav me-auto">
                <li class="nav-item">
                    <a class="nav-link" href="Default.aspx">Trang chủ</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="Products.aspx">Sản phẩm</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="Blog.aspx">Blog</a>
                </li>
            </ul>
            <ul class="navbar-nav">
                <li class="nav-item" runat="server" id="liCart" visible="false">
                    <a class="nav-link" href="Cart.aspx">
                        <i class="fas fa-shopping-cart me-1"></i>Giỏ hàng
                        <span class="badge bg-warning text-dark ms-1" id="cartCount" runat="server">0</span>
                    </a>
                </li>
                <li class="nav-item" runat="server" id="liProfile" visible="false">
                    <a class="nav-link" href="UserProfile.aspx">
                        <i class="fas fa-user me-1"></i>Hồ sơ
                    </a>
                </li>
                <li class="nav-item" runat="server" id="liLogin" visible="true">
                    <a class="nav-link" href="Login.aspx">
                        <i class="fas fa-sign-in-alt me-1"></i>Đăng nhập
                    </a>
                </li>
                <li class="nav-item" runat="server" id="liRegister" visible="true">
                    <a class="nav-link" href="Register.aspx">
                        <i class="fas fa-user-plus me-1"></i>Đăng ký
                    </a>
                </li>
                <li class="nav-item" runat="server" id="liLogout" visible="false">
                    <asp:LinkButton ID="btnLogout" runat="server" CssClass="nav-link" OnClick="btnLogout_Click">
                        <i class="fas fa-sign-out-alt me-1"></i>Đăng xuất
                    </asp:LinkButton>
                </li>
            </ul>
        </div>
    </div>
</nav> 