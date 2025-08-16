<%@ Page Title="" Language="C#" MasterPageFile="../Admin.master" AutoEventWireup="true" CodeFile="Categories.aspx.cs" Inherits="AdminPages_Categories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../Css/admin-pages.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid">
        <div class="admin-page-container">
            <div class="page-header">
                <h2>
                    <div class="header-icon">
                        <i class="fas fa-tags"></i>
                    </div>
                    Quản lý danh mục
                </h2>
            </div>
            
            <div class="mb-3">
                <asp:Button ID="btnAddNew" runat="server" Text="➕ Thêm danh mục mới" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
            </div>
            
            <div class="table-responsive">
                <asp:GridView ID="gvCategories" runat="server" CssClass="table table-hover" 
                              AutoGenerateColumns="false" DataKeyNames="Id" 
                              OnRowCommand="gvCategories_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-CssClass="order-id" />
                        <asp:BoundField DataField="Name" HeaderText="Tên danh mục" ItemStyle-CssClass="customer-name" />
                        <asp:BoundField DataField="Description" HeaderText="Mô tả" />
                        <asp:BoundField DataField="DisplayOrder" HeaderText="Thứ tự" />
                        <asp:TemplateField HeaderText="Trạng thái">
                            <ItemTemplate>
                                <span class="status-badge <%# Convert.ToBoolean(Eval("IsActive")) ? "status-delivered" : "status-cancelled" %>">
                                    <%# Convert.ToBoolean(Eval("IsActive")) ? "Hoạt động" : "Tạm dừng" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CreatedDate" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="order-date" />
                        <asp:TemplateField HeaderText="Hành động">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-primary btn-sm" 
                                               CommandName="EditCategory" CommandArgument='<%# Eval("Id") %>' 
                                               ToolTip="Chỉnh sửa danh mục">
                                    <i class="fas fa-edit"></i> Sửa
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-sm" 
                                               CommandName="DeleteCategory" CommandArgument='<%# Eval("Id") %>' 
                                               OnClientClick="return confirm('Bạn có chắc chắn muốn xóa danh mục này?');"
                                               ToolTip="Xóa danh mục">
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

