<%@ Page Title="" Language="C#" MasterPageFile="../Public.Master" AutoEventWireup="true" CodeFile="Cart.aspx.cs" Inherits="Cart" %>
<%@ Register TagPrefix="uc" TagName="ChatBot" Src="../UserControls/ChatBot.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Giỏ hàng - 3D T-Shirt Design Platform</title>
    <meta name="description" content="Giỏ hàng mua sắm của bạn" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-12">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb">
                        <li class="breadcrumb-item"><a href="Default.aspx">Trang chủ</a></li>
                        <li class="breadcrumb-item active" aria-current="page">Giỏ hàng</li>
                    </ol>
                </nav>
                
                <div class="d-flex justify-content-between align-items-center mb-4">
                    <h1 class="h2 mb-0"><i class="fas fa-shopping-cart me-2"></i>Giỏ hàng của bạn</h1>
                    <asp:Button ID="btnClearCart" runat="server" Text="Xóa toàn bộ" CssClass="btn btn-outline-danger" OnClick="btnClearCart_Click" OnClientClick="return confirm('Bạn có chắc chắn muốn xóa toàn bộ giỏ hàng?');" />
                </div>
                
                <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </asp:Panel>
                
                <asp:Panel ID="pnlEmptyCart" runat="server" Visible="false" CssClass="text-center py-5">
                    <div class="empty-cart">
                        <i class="fas fa-shopping-cart fa-5x text-muted mb-3"></i>
                        <h3 class="text-muted">Giỏ hàng trống</h3>
                        <p class="text-muted">Bạn chưa thêm sản phẩm nào vào giỏ hàng</p>
                        <a href="Products.aspx" class="btn btn-primary">Tiếp tục mua sắm</a>
                    </div>
                </asp:Panel>
                
                <asp:Panel ID="pnlCartItems" runat="server" Visible="true">
                    <div class="row">
                        <div class="col-lg-8">
                            <div class="card">
                                <div class="card-header">
                                    <h5 class="mb-0">Sản phẩm trong giỏ hàng</h5>
                                </div>
                                <div class="card-body p-0">
                                    <asp:Repeater ID="rptCartItems" runat="server" OnItemCommand="rptCartItems_ItemCommand">
                                        <ItemTemplate>
                                            <div class="cart-item border-bottom p-3">
                                                <div class="row align-items-center">
                                                    <div class="col-md-2">
                                                        <img src="<%# Eval("ProductImagePath") ?? "~/Image/placeholder.jpg" %>" alt="<%# Eval("ProductName") %>" class="img-fluid rounded" style="height: 80px; object-fit: cover;" />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <h6 class="mb-1"><%# Eval("ProductName") %></h6>
                                                        <small class="text-muted">Màu sắc: <%# Eval("Color") %></small><br />
                                                        <small class="text-muted">Kích thước: <%# Eval("Size") %></small>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="input-group input-group-sm">
                                                            <asp:Button ID="btnDecrease" runat="server" Text="-" CssClass="btn btn-outline-secondary" CommandName="UpdateQuantity" CommandArgument='<%# Eval("Id") + "," + (Convert.ToInt32(Eval("Quantity")) - 1) %>' />
                                                            <span class="input-group-text"><%# Eval("Quantity") %></span>
                                                            <asp:Button ID="btnIncrease" runat="server" Text="+" CssClass="btn btn-outline-secondary" CommandName="UpdateQuantity" CommandArgument='<%# Eval("Id") + "," + (Convert.ToInt32(Eval("Quantity")) + 1) %>' />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2 text-center">
                                                        <span class="fw-bold text-primary"><%# String.Format("{0:C}", Eval("Price")) %></span>
                                                    </div>
                                                    <div class="col-md-2 text-center">
                                                        <span class="fw-bold"><%# String.Format("{0:C}", Convert.ToDecimal(Eval("Price")) * Convert.ToInt32(Eval("Quantity"))) %></span>
                                                    </div>
                                                    <div class="col-md-1 text-center">
                                                        <asp:Button ID="btnRemove" runat="server" Text="🗑️" CssClass="btn btn-outline-danger btn-sm" CommandName="RemoveItem" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('Bạn có chắc chắn muốn xóa sản phẩm này?');" />
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                            
                            <div class="mt-3">
                                <a href="Products.aspx" class="btn btn-outline-primary"><i class="fas fa-arrow-left me-2"></i>Tiếp tục mua sắm</a>
                            </div>
                        </div>
                        
                        <div class="col-lg-4">
                            <div class="card">
                                <div class="card-header">
                                    <h5 class="mb-0">Tổng kết đơn hàng</h5>
                                </div>
                                <div class="card-body">
                                    <div class="d-flex justify-content-between mb-2">
                                        <span>Tạm tính:</span>
                                        <span><asp:Label ID="lblSubtotal" runat="server" Text="0 đ"></asp:Label></span>
                                    </div>
                                    <div class="d-flex justify-content-between mb-2">
                                        <span>Phí vận chuyển:</span>
                                        <span><asp:Label ID="lblShipping" runat="server" Text="30,000 đ"></asp:Label></span>
                                    </div>
                                    <div class="d-flex justify-content-between mb-2">
                                        <span>Thuế VAT (10%):</span>
                                        <span><asp:Label ID="lblTax" runat="server" Text="0 đ"></asp:Label></span>
                                    </div>
                                    <hr />
                                    <div class="d-flex justify-content-between mb-3">
                                        <strong>Tổng cộng:</strong>
                                        <strong class="text-primary"><asp:Label ID="lblTotal" runat="server" Text="0 đ"></asp:Label></strong>
                                    </div>
                                    
                                    <div class="mb-3">
                                        <label class="form-label">Mã giảm giá</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtCouponCode" runat="server" CssClass="form-control" placeholder="Nhập mã giảm giá"></asp:TextBox>
                                            <asp:Button ID="btnApplyCoupon" runat="server" Text="Áp dụng" CssClass="btn btn-outline-primary" OnClick="btnApplyCoupon_Click" />
                                        </div>
                                    </div>
                                    
                                    <div class="d-grid">
                                        <asp:Button ID="btnCheckout" runat="server" Text="Tiến hành thanh toán" CssClass="btn btn-primary btn-lg" OnClick="btnCheckout_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    
    <style>
        .cart-item:hover {
            background-color: #f8f9fa;
        }
        .empty-cart {
            padding: 3rem 0;
        }
    </style>
    
    <!-- ChatBot UserControl -->
    <uc:ChatBot runat="server" ID="ChatBot" />
</asp:Content>

