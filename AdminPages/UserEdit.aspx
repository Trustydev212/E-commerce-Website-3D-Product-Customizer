<%@ Page Title="" Language="C#" MasterPageFile="../Admin.master" AutoEventWireup="true" CodeFile="UserEdit.aspx.cs" Inherits="AdminPages_UserEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <h2><asp:Label ID="lblTitle" runat="server" Text="Thêm người dùng mới" /></h2>
                
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label class="form-label">Tên đăng nhập *</label>
                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" 
                                                              ErrorMessage="Vui lòng nhập tên đăng nhập" CssClass="text-danger" />
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Email *</label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="SingleLine" />
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" 
                                                              ErrorMessage="Vui lòng nhập email" CssClass="text-danger" />
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" 
                                                                  ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" 
                                                                  ErrorMessage="Email không hợp lệ" CssClass="text-danger" />
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Mật khẩu *</label>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" 
                                                              ErrorMessage="Vui lòng nhập mật khẩu" CssClass="text-danger" />
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Họ tên *</label>
                                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" 
                                                              ErrorMessage="Vui lòng nhập họ tên" CssClass="text-danger" />
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Điện thoại</label>
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label class="form-label">Địa chỉ</label>
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Vai trò</label>
                                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="Người dùng" Value="User" Selected="True" />
                                        <asp:ListItem Text="Quản trị viên" Value="Admin" />
                                    </asp:DropDownList>
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Avatar</label>
                                    <asp:FileUpload ID="fuAvatar" runat="server" CssClass="form-control" />
                                    <asp:Label ID="lblCurrentAvatar" runat="server" CssClass="form-text" />
                                </div>
                                
                                <div class="mb-3">
                                    <div class="form-check">
                                        <asp:CheckBox ID="cbIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                                        <label class="form-check-label">Kích hoạt tài khoản</label>
                                    </div>
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Xem trước Avatar</label>
                                    <div class="avatar-preview border p-3 text-center">
                                        <asp:Image ID="imgPreview" runat="server" CssClass="img-thumbnail rounded-circle" Width="100" Height="100" />
                                        <asp:Label ID="lblNoAvatar" runat="server" Text="Chưa có avatar" CssClass="text-muted" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row">
                            <div class="col-md-12">
                                <div class="mb-3">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Lưu" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-secondary ms-2" Text="Hủy" OnClick="btnCancel_Click" CausesValidation="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>