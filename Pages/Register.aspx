<%@ Page Language="C#" MasterPageFile="../Public.Master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Đăng ký - 3D T-Shirt Design Platform</title>
    <meta name="description" content="Tạo tài khoản mới cho 3D T-Shirt Design Platform" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-8 col-lg-6">
                <div class="card shadow-sm">
                    <div class="card-body p-4">
                        <div class="text-center mb-4">
                            <i class="fas fa-user-plus fa-3x text-primary mb-3"></i>
                            <h2 class="card-title fw-bold">Đăng ký tài khoản</h2>
                            <p class="text-muted">Tạo tài khoản mới để sử dụng dịch vụ của chúng tôi</p>
                        </div>
                        
                        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        </asp:Panel>
                        
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label for="txtUsername" class="form-label">Tên đăng nhập <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="Vui lòng nhập tên đăng nhập" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="Tên đăng nhập chỉ chứa chữ cái, số và dấu gạch dưới (3-20 ký tự)" ValidationExpression="^[a-zA-Z0-9_]{3,20}$" CssClass="text-danger small" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label for="txtEmail" class="form-label">Email <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="SingleLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Vui lòng nhập email" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email không hợp lệ" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" CssClass="text-danger small" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        
                        <div class="mb-3">
                            <label for="txtFullName" class="form-label">Họ và tên <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Vui lòng nhập họ và tên" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        
                        <div class="mb-3">
                            <label for="txtPhone" class="form-label">Số điện thoại</label>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" TextMode="SingleLine"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone" ErrorMessage="Số điện thoại không hợp lệ" ValidationExpression="^[0-9]{10,11}$" CssClass="text-danger small" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                        
                        <div class="mb-3">
                            <label for="txtAddress" class="form-label">Địa chỉ</label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                        </div>
                        
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label for="txtPassword" class="form-label">Mật khẩu <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Vui lòng nhập mật khẩu" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Mật khẩu phải có ít nhất 6 ký tự" ValidationExpression="^.{6,}$" CssClass="text-danger small" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label for="txtConfirmPassword" class="form-label">Xác nhận mật khẩu <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="Vui lòng xác nhận mật khẩu" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" ErrorMessage="Mật khẩu xác nhận không khớp" CssClass="text-danger small" Display="Dynamic"></asp:CompareValidator>
                            </div>
                        </div>
                        
                        <div class="mb-3 form-check">
                            <asp:CheckBox ID="chkAgreeTerms" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label" for="chkAgreeTerms">
                                Tôi đồng ý với <a href="#" class="text-primary text-decoration-none">điều khoản sử dụng</a> và <a href="#" class="text-primary text-decoration-none">chính sách bảo mật</a>
                            </label>
                            <asp:CustomValidator ID="cvAgreeTerms" runat="server"
                                ErrorMessage="Vui lòng đồng ý với điều khoản sử dụng"
                                ClientValidationFunction="validateAgreeTerms"
                                Display="Dynamic"
                                CssClass="text-danger small" />
                        </div>
                        
                        <div class="d-grid mb-3">
                            <asp:Button ID="btnRegister" runat="server" Text="Đăng ký" CssClass="btn btn-primary" OnClick="btnRegister_Click" />
                        </div>
                        
                        <div class="text-center">
                            <p class="mb-0">Đã có tài khoản? <a href="Login.aspx" class="text-primary text-decoration-none">Đăng nhập ngay</a></p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
    function validateAgreeTerms(sender, args) {
        args.IsValid = document.getElementById('<%= chkAgreeTerms.ClientID %>').checked;
    }
    </script>
    <script src='<%: ResolveUrl("~/JS/site.js") %>'></script>
</asp:Content>

