<%@ Page Language="C#" MasterPageFile="../Public.Master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<%@ Register TagPrefix="uc" TagName="ChatBot" Src="../UserControls/ChatBot.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Đăng nhập - 3D T-Shirt Design Platform</title>
    <meta name="description" content="Đăng nhập vào tài khoản 3D T-Shirt của bạn" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6 col-lg-4">
                <div class="card shadow-sm">
                    <div class="card-body p-4">
                        <div class="text-center mb-4">
                            <i class="fas fa-sign-in-alt fa-3x text-primary mb-3"></i>
                            <h2 class="card-title fw-bold">Đăng nhập</h2>
                            <p class="text-muted">Đăng nhập để truy cập tài khoản của bạn</p>
                        </div>
                        
                        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-danger">
                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        </asp:Panel>
                        
                        <div class="mb-3">
                            <label for="txtEmail" class="form-label">Email</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="SingleLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Vui lòng nhập email" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email không hợp lệ" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" CssClass="text-danger small" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                        
                        <div class="mb-3">
                            <label for="txtPassword" class="form-label">Mật khẩu</label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Vui lòng nhập mật khẩu" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        
                        <div class="mb-3 form-check">
                            <asp:CheckBox ID="chkRememberMe" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label" for="chkRememberMe">Ghi nhớ đăng nhập</label>
                        </div>
                        
                        <div class="d-grid mb-3">
                            <asp:Button ID="btnLogin" runat="server" Text="Đăng nhập" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
                        </div>
                        
                        <div class="text-center">
                            <p class="mb-0">Chưa có tài khoản? <a href="Register.aspx" class="text-primary text-decoration-none">Đăng ký ngay</a></p>
                            <p class="mt-2 mb-0"><a href="#" class="text-muted text-decoration-none small">Quên mật khẩu?</a></p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <!-- ChatBot UserControl -->
    <uc:ChatBot runat="server" ID="ChatBot" />
</asp:Content>

