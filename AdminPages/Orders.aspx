<%@ Page Title="" Language="C#" MasterPageFile="../Admin.master" AutoEventWireup="true" CodeFile="Orders.aspx.cs" Inherits="AdminPages_Orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../Css/admin-pages.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid">
        <div class="admin-page-container">
            <div class="page-header">
                <h2>
                    <div class="header-icon">
                        <i class="fas fa-shopping-cart"></i>
                    </div>
                    Quản lý đơn hàng
                </h2>
            </div>
            
            <div class="table-responsive">
                <asp:GridView ID="gvOrders" runat="server" CssClass="table table-hover" 
                              AutoGenerateColumns="false" DataKeyNames="Id" 
                              OnRowCommand="gvOrders_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-CssClass="order-id" />
                        <asp:BoundField DataField="OrderNumber" HeaderText="Mã đơn hàng" ItemStyle-CssClass="order-id" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Khách hàng" ItemStyle-CssClass="customer-name" />
                        <asp:TemplateField HeaderText="Trạng thái">
                            <ItemTemplate>
                                <span class="status-badge status-<%# Eval("Status").ToString().ToLower() %>">
                                    <%# GetStatusDisplayName(Eval("Status").ToString()) %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Total" HeaderText="Tổng tiền" DataFormatString="{0:C}" ItemStyle-CssClass="order-total" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="order-date" />
                        <asp:TemplateField HeaderText="Thanh toán">
                            <ItemTemplate>
                                <span class="payment-status payment-<%# Eval("PaymentStatus").ToString().ToLower() %>">
                                    <%# GetPaymentStatusDisplayName(Eval("PaymentStatus").ToString()) %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hành động">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-primary btn-sm" 
                                               CommandName="EditOrder" CommandArgument='<%# Eval("Id") %>' 
                                               ToolTip="Chỉnh sửa đơn hàng">
                                    <i class="fas fa-edit"></i> Sửa
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-sm" 
                                               CommandName="DeleteOrder" CommandArgument='<%# Eval("Id") %>' 
                                               OnClientClick="return confirm('Bạn có chắc chắn muốn xóa đơn hàng này?');"
                                               ToolTip="Xóa đơn hàng">
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

