<%@ Page Title="Thông báo" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeFile="UserNotifications.aspx.cs" Inherits="UserNotifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .notification-container {
            max-width: 800px;
            margin: 0 auto;
        }
        .notification-item {
            border: 1px solid #ddd;
            border-radius: 8px;
            padding: 15px;
            margin-bottom: 15px;
            background: white;
            transition: all 0.3s ease;
        }
        .notification-item:hover {
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }
        .notification-item.unread {
            border-left: 4px solid #007bff;
            background: #f8f9ff;
        }
        .notification-item.global {
            border-left: 4px solid #28a745;
        }
        .notification-header {
            display: flex;
            justify-content: between;
            align-items: flex-start;
            margin-bottom: 10px;
        }
        .notification-title {
            font-weight: bold;
            margin-bottom: 5px;
        }
        .notification-type {
            padding: 2px 8px;
            border-radius: 12px;
            font-size: 12px;
            font-weight: bold;
            margin-left: 10px;
        }
        .type-info { background: #d1ecf1; color: #0c5460; }
        .type-success { background: #d4edda; color: #155724; }
        .type-warning { background: #fff3cd; color: #856404; }
        .type-error { background: #f8d7da; color: #721c24; }
        .notification-meta {
            font-size: 12px;
            color: #666;
            margin-top: 10px;
        }
        .notification-actions {
            margin-top: 10px;
        }
        .btn-sm {
            padding: 0.25rem 0.5rem;
            font-size: 0.875rem;
        }
        .empty-state {
            text-align: center;
            padding: 40px 20px;
            color: #666;
        }
        .empty-state i {
            font-size: 4rem;
            margin-bottom: 20px;
            opacity: 0.5;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="notification-container">
            <div class="d-flex justify-content-between align-items-center mb-4">
                <h2><i class="fas fa-bell"></i> Thông báo</h2>
                <div>
                    <asp:Button ID="btnRefresh" runat="server" Text="Làm mới" 
                        CssClass="btn btn-outline-primary btn-sm" OnClick="btnRefresh_Click" />
                    <asp:Button ID="btnMarkAllRead" runat="server" Text="Đánh dấu tất cả đã đọc" 
                        CssClass="btn btn-outline-success btn-sm ml-2" OnClick="btnMarkAllRead_Click" />
                </div>
            </div>

            <!-- Thông báo kết quả -->
            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert mb-3">
                <asp:Label ID="lblMessage" runat="server" />
            </asp:Panel>

            <!-- Bộ lọc -->
            <div class="card mb-3">
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-4">
                            <label>Lọc theo loại:</label>
                            <asp:DropDownList ID="ddlFilterType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterType_SelectedIndexChanged">
                                <asp:ListItem Text="Tất cả" Value="" />
                                <asp:ListItem Text="Thông tin" Value="info" />
                                <asp:ListItem Text="Thành công" Value="success" />
                                <asp:ListItem Text="Cảnh báo" Value="warning" />
                                <asp:ListItem Text="Lỗi" Value="error" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label>Trạng thái:</label>
                            <asp:DropDownList ID="ddlFilterStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterStatus_SelectedIndexChanged">
                                <asp:ListItem Text="Tất cả" Value="" />
                                <asp:ListItem Text="Chưa đọc" Value="unread" />
                                <asp:ListItem Text="Đã đọc" Value="read" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label>Phạm vi:</label>
                            <asp:DropDownList ID="ddlFilterScope" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterScope_SelectedIndexChanged">
                                <asp:ListItem Text="Tất cả" Value="" />
                                <asp:ListItem Text="Cá nhân" Value="personal" />
                                <asp:ListItem Text="Toàn hệ thống" Value="global" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Danh sách thông báo -->
            <asp:Repeater ID="rptNotifications" runat="server" OnItemCommand="rptNotifications_ItemCommand">
                <ItemTemplate>
                    <div class="notification-item <%# Convert.ToBoolean(Eval("IsRead")) ? "" : "unread" %> <%# Convert.ToBoolean(Eval("IsGlobal")) ? "global" : "" %>">
                        <div class="notification-header">
                            <div class="flex-grow-1">
                                <div class="d-flex align-items-center">
                                    <div class="notification-title"><%# Eval("Title") %></div>
                                    <span class="notification-type type-<%# Eval("Type") %>"><%# GetTypeDisplayName(Eval("Type") != null ? Eval("Type").ToString() : "") %></span>
                                    <%# Convert.ToBoolean(Eval("IsGlobal")) ? "<span class='badge badge-success ml-2'>Toàn hệ thống</span>" : "<span class='badge badge-info ml-2'>Cá nhân</span>" %>
                                </div>
                                <div class="notification-message mt-2">
                                    <%# Eval("Message") %>
                                </div>
                                <div class="notification-meta">
                                    <i class="fas fa-clock"></i> <%# Convert.ToDateTime(Eval("CreatedAt")).ToString("dd/MM/yyyy HH:mm") %>
                                    <%# !string.IsNullOrEmpty(Eval("CreatedBy") != null ? Eval("CreatedBy").ToString() : "") ? " - Bởi: " + Eval("CreatedBy") : "" %>
                                    <%# Eval("ExpiresAt") != DBNull.Value ? " - Hết hạn: " + Convert.ToDateTime(Eval("ExpiresAt")).ToString("dd/MM/yyyy HH:mm") : "" %>
                                </div>
                            </div>
                        </div>
                        
                        <div class="notification-actions">
                            <%# !string.IsNullOrEmpty(Eval("ActionUrl") != null ? Eval("ActionUrl").ToString() : "") ? "<a href='" + Eval("ActionUrl") + "' class='btn btn-primary btn-sm mr-2'><i class='fas fa-external-link-alt'></i> Xem chi tiết</a>" : "" %>
                            
                            <asp:LinkButton ID="btnMarkRead" runat="server" CommandName="MarkRead" 
                                CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-outline-primary btn-sm mr-2"
                                ToolTip="Đánh dấu đã đọc" Visible='<%# !Convert.ToBoolean(Eval("IsRead")) %>'>
                                <i class="fas fa-check"></i> Đã đọc
                            </asp:LinkButton>
                            
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" 
                                CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-outline-danger btn-sm"
                                ToolTip="Xóa" OnClientClick="return confirm('Bạn có chắc muốn xóa thông báo này?');">
                                <i class="fas fa-trash"></i> Xóa
                            </asp:LinkButton>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <!-- Trạng thái trống -->
            <asp:Panel ID="pnlNoData" runat="server" Visible="false" CssClass="empty-state">
                <i class="fas fa-bell-slash"></i>
                <h4>Không có thông báo nào</h4>
                <p>Bạn chưa có thông báo nào hoặc đã xem hết tất cả thông báo.</p>
            </asp:Panel>

            <!-- Phân trang -->
            <div class="d-flex justify-content-center mt-4" id="paginationContainer" runat="server" visible="false">
                <nav>
                    <ul class="pagination">
                        <li class="page-item">
                            <asp:LinkButton ID="btnPrev" runat="server" CssClass="page-link" OnClick="btnPrev_Click">
                                <i class="fas fa-chevron-left"></i> Trước
                            </asp:LinkButton>
                        </li>
                        <li class="page-item">
                            <span class="page-link">
                                Trang <asp:Literal ID="litCurrentPage" runat="server" /> / <asp:Literal ID="litTotalPages" runat="server" />
                            </span>
                        </li>
                        <li class="page-item">
                            <asp:LinkButton ID="btnNext" runat="server" CssClass="page-link" OnClick="btnNext_Click">
                                Sau <i class="fas fa-chevron-right"></i>
                            </asp:LinkButton>
                        </li>
                    </ul>
                </nav>
            </div>
        </div>
    </div>
</asp:Content> 