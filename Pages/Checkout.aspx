<%@ Page Language="C#" MasterPageFile="../Public.Master" AutoEventWireup="true" CodeFile="Checkout.aspx.cs" Inherits="Checkout" %>
<%@ Register TagPrefix="uc" TagName="ChatBot" Src="../UserControls/ChatBot.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Thanh toán - 3D T-Shirt Design Platform</title>
    <meta name="description" content="Trang thanh toán đơn hàng" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-12">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb">
                        <li class="breadcrumb-item"><a href="Default.aspx">Trang chủ</a></li>
                        <li class="breadcrumb-item"><a href="Cart.aspx">Giỏ hàng</a></li>
                        <li class="breadcrumb-item active" aria-current="page">Thanh toán</li>
                    </ol>
                </nav>
                
                <h1 class="h2 mb-4"><i class="fas fa-credit-card me-2"></i>Thanh toán đơn hàng</h1>
                
                <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </asp:Panel>
                
                <div class="row">
                    <div class="col-lg-8">
                        <!-- Delivery Information -->
                        <div class="card mb-4">
                            <div class="card-header">
                                <h5 class="mb-0"><i class="fas fa-shipping-fast me-2"></i>Thông tin giao hàng</h5>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label">Họ và tên <span class="text-danger">*</span></label>
                                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Vui lòng nhập họ và tên" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label">Số điện thoại <span class="text-danger">*</span></label>
                                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone" ErrorMessage="Vui lòng nhập số điện thoại" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Email <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="SingleLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Vui lòng nhập email" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Địa chỉ giao hàng <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress" ErrorMessage="Vui lòng nhập địa chỉ giao hàng" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Ghi chú</label>
                                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        
                        <!-- Payment Method -->
                        <div class="card mb-4">
                            <div class="card-header">
                                <h5 class="mb-0"><i class="fas fa-credit-card me-2"></i>Phương thức thanh toán</h5>
                            </div>
                            <div class="card-body">
                                <asp:RadioButtonList ID="rblPaymentMethod" runat="server" CssClass="list-unstyled">
                                    <asp:ListItem Value="COD" Selected="True">
                                        <div class="form-check">
                                            <input class="form-check-input" type="radio" name="payment" id="cod" checked>
                                            <label class="form-check-label" for="cod">
                                                <i class="fas fa-money-bill-wave me-2"></i>Thanh toán khi nhận hàng (COD)
                                            </label>
                                        </div>
                                    </asp:ListItem>
                                    <asp:ListItem Value="BANK">
                                        <div class="form-check">
                                            <input class="form-check-input" type="radio" name="payment" id="bank">
                                            <label class="form-check-label" for="bank">
                                                <i class="fas fa-university me-2"></i>Chuyển khoản ngân hàng
                                            </label>
                                        </div>
                                    </asp:ListItem>
                                    <asp:ListItem Value="MOMO">
                                        <div class="form-check">
                                            <input class="form-check-input" type="radio" name="payment" id="momo">
                                            <label class="form-check-label" for="momo">
                                                <i class="fas fa-mobile-alt me-2"></i>Ví MoMo
                                            </label>
                                        </div>
                                    </asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
                    
                    <div class="col-lg-4">
                        <!-- Order Summary -->
                        <div class="card mb-4">
                            <div class="card-header">
                                <h5 class="mb-0"><i class="fas fa-receipt me-2"></i>Tổng kết đơn hàng</h5>
                            </div>
                            <div class="card-body">
                                <asp:Repeater ID="rptOrderItems" runat="server">
                                    <ItemTemplate>
                                        <div class="d-flex justify-content-between align-items-center mb-2 pb-2 border-bottom">
                                            <div>
                                                <h6 class="mb-0"><%# Eval("ProductName") %></h6>
                                                <small class="text-muted">SL: <%# Eval("Quantity") %></small>
                                            </div>
                                            <span class="fw-bold"><%# String.Format("{0:N0} đ", Convert.ToDecimal(Eval("Price")) * Convert.ToInt32(Eval("Quantity"))) %></span>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                
                                <div class="d-flex justify-content-between mb-2">
                                    <span>Tạm tính:</span>
                                    <span><asp:Label ID="lblSubtotal" runat="server" Text="0 đ"></asp:Label></span>
                                </div>
                                <div class="d-flex justify-content-between mb-2">
                                    <span>Phí vận chuyển:</span>
                                    <span><asp:Label ID="lblShipping" runat="server" Text="0 đ"></asp:Label></span>
                                </div>
                                <div class="d-flex justify-content-between mb-2">
                                    <span>Thuế VAT:</span>
                                    <span><asp:Label ID="lblTax" runat="server" Text="0 đ"></asp:Label></span>
                                </div>
                                <hr />
                                <div class="d-flex justify-content-between mb-3">
                                    <strong>Tổng cộng:</strong>
                                    <strong class="text-primary"><asp:Label ID="lblTotal" runat="server" Text="0 đ"></asp:Label></strong>
                                </div>
                                
                                <div class="d-grid gap-2">
                                    <asp:Button ID="btnPlaceOrder" runat="server" Text="Đặt hàng" CssClass="btn btn-primary btn-lg" OnClick="btnPlaceOrder_Click" OnClientClick="return prepareOrderData();" />
                                    <a href="Cart.aspx" class="btn btn-outline-secondary">Quay lại giỏ hàng</a>
                                </div>
                            </div>
                        </div>
                        
                        <!-- Security Notice -->
                        <div class="card border-success">
                            <div class="card-body text-center">
                                <i class="fas fa-shield-alt text-success fa-2x mb-2"></i>
                                <h6 class="text-success">Bảo mật thông tin</h6>
                                <small class="text-muted">Thông tin của bạn được bảo mật tối đa</small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <style>
        .text-center {
            margin-top: 0px !important;
        }
    </style>
    <!-- Hidden field to store design data -->
    <asp:HiddenField ID="hdnDesignData" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDesignId" runat="server" ClientIDMode="Static" />
    
    <!-- ChatBot UserControl -->
    <uc:ChatBot runat="server" ID="ChatBot" />
    
    <script src="../JS/checkout.js"></script>
</asp:Content>

