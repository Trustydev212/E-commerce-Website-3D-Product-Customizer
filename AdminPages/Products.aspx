<%@ Page Title="" Language="C#" MasterPageFile="../Admin.master" AutoEventWireup="true" CodeFile="Products.aspx.cs" Inherits="AdminPages_Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../Css/admin-pages.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid">
        <div class="admin-page-container">
            <div class="page-header">
                <h2>
                    <div class="header-icon">
                        <i class="fas fa-tshirt"></i>
                    </div>
                    Quản lý sản phẩm
                </h2>
            </div>
            
            <div class="mb-3">
                <asp:Button ID="btnAddNew" runat="server" Text="➕ Thêm sản phẩm mới" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
            </div>
            
            <div class="table-responsive">
                <asp:GridView ID="gvProducts" runat="server" CssClass="table table-hover" 
                              AutoGenerateColumns="false" DataKeyNames="Id" 
                              OnRowCommand="gvProducts_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-CssClass="order-id" />
                        <asp:BoundField DataField="Name" HeaderText="Tên sản phẩm" ItemStyle-CssClass="customer-name" />
                        <asp:BoundField DataField="Price" HeaderText="Giá" DataFormatString="{0:C}" ItemStyle-CssClass="order-total" />
                        <asp:BoundField DataField="StockQuantity" HeaderText="Tồn kho" />
                        <asp:BoundField DataField="CategoryId" HeaderText="Danh mục ID" />
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
                                               CommandName="EditProduct" CommandArgument='<%# Eval("Id") %>' 
                                               ToolTip="Chỉnh sửa sản phẩm">
                                    <i class="fas fa-edit"></i> Sửa
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-sm" 
                                               CommandName="DeleteProduct" CommandArgument='<%# Eval("Id") %>' 
                                               OnClientClick="return confirm('Bạn có chắc chắn muốn xóa sản phẩm này?');"
                                               ToolTip="Xóa sản phẩm">
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

