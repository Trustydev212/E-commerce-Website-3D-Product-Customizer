<%@ Page Title="" Language="C#" MasterPageFile="../Admin.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="AdminPages_Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../Css/admin-pages.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid">
        <div class="admin-page-container">
            <div class="page-header">
                <h2>
                    <div class="header-icon">
                        <i class="fas fa-users"></i>
                    </div>
                    Quản lý người dùng
                </h2>
            </div>
            
            <div class="mb-3">
                <asp:Button ID="btnAddNew" runat="server" Text="➕ Thêm người dùng mới" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
            </div>
            
            <div class="table-responsive">
                <asp:GridView ID="gvUsers" runat="server" CssClass="table table-hover" 
                              AutoGenerateColumns="false" DataKeyNames="Id" 
                              OnRowCommand="gvUsers_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-CssClass="order-id" />
                        <asp:BoundField DataField="Username" HeaderText="Tên đăng nhập" ItemStyle-CssClass="customer-name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="FullName" HeaderText="Họ tên" />
                        <asp:BoundField DataField="Phone" HeaderText="Điện thoại" />
                        <asp:BoundField DataField="Role" HeaderText="Vai trò" />
                        <asp:TemplateField HeaderText="Trạng thái">
                            <ItemTemplate>
                                <span class="status-badge <%# Convert.ToBoolean(Eval("IsActive")) ? "status-delivered" : "status-cancelled" %>">
                                    <%# Convert.ToBoolean(Eval("IsActive")) ? "Hoạt động" : "Tạm dừng" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CreatedAt" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="order-date" />
                        <asp:BoundField DataField="LastLogin" HeaderText="Đăng nhập cuối" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                        <asp:TemplateField HeaderText="Hành động">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-primary btn-sm" 
                                               CommandName="EditUser" CommandArgument='<%# Eval("Id") %>' 
                                               ToolTip="Chỉnh sửa người dùng">
                                    <i class="fas fa-edit"></i> Sửa
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-sm" 
                                               CommandName="DeleteUser" CommandArgument='<%# Eval("Id") %>' 
                                               OnClientClick="return confirm('Bạn có chắc chắn muốn xóa người dùng này?');"
                                               ToolTip="Xóa người dùng">
                                    <i class="fas fa-trash"></i> Xóa
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

