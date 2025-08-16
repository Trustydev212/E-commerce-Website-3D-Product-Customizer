<%@ Page Title="Quản lý thông báo" Language="C#" MasterPageFile="~/Admin.master" AutoEventWireup="true" CodeFile="AdminNotifications.aspx.cs" Inherits="AdminPages_AdminNotifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Css/admin-pages.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="admin-page-container">
            <div class="page-header">
                <h2>
                    <div class="header-icon">
                        <i class="fas fa-bell"></i>
                    </div>
                    Quản lý thông báo
                </h2>
            </div>
            
            <!-- Form tạo thông báo mới -->
            <div class="notification-form">
                <div class="d-flex align-items-center mb-3">
                    <h4 class="mb-0">
                        <i class="fas fa-plus-circle text-primary me-2"></i>
                        Tạo thông báo mới
                    </h4>
                </div>
                
                <div class="form-section">
                    <h5><i class="fas fa-info-circle text-info"></i> Thông tin cơ bản</h5>
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label class="form-label fw-semibold">Tiêu đề thông báo</label>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="Nhập tiêu đề thông báo..." />
                            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" 
                                ErrorMessage="Tiêu đề không được để trống" CssClass="text-danger small" Display="Dynamic" />
                        </div>
                        <div class="col-md-6 mb-3">
                            <label class="form-label fw-semibold">Loại thông báo</label>
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select">
                                <asp:ListItem Text="📢 Thông tin" Value="info" />
                                <asp:ListItem Text="✅ Thành công" Value="success" />
                                <asp:ListItem Text="⚠️ Cảnh báo" Value="warning" />
                                <asp:ListItem Text="❌ Lỗi" Value="error" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    
                    <div class="mb-3">
                        <label class="form-label fw-semibold">Nội dung thông báo</label>
                        <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" 
                            Rows="4" placeholder="Nhập nội dung chi tiết của thông báo..." />
                        <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage" 
                            ErrorMessage="Nội dung không được để trống" CssClass="text-danger small" Display="Dynamic" />
                    </div>
                </div>
                
                <div class="form-section">
                    <h5><i class="fas fa-cog text-warning"></i> Tùy chọn nâng cao</h5>
                    <div class="row">
                        <div class="col-md-4 mb-3">
                            <label class="form-label fw-semibold">Icon (tùy chọn)</label>
                            <asp:TextBox ID="txtIcon" runat="server" CssClass="form-control" placeholder="fas fa-info-circle" />
                            <small class="text-muted">Sử dụng FontAwesome icon class</small>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label fw-semibold">URL hành động</label>
                            <asp:TextBox ID="txtActionUrl" runat="server" CssClass="form-control" placeholder="/Pages/SomePage.aspx" />
                            <small class="text-muted">Link khi click vào thông báo</small>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label class="form-label fw-semibold">Thời gian hết hạn</label>
                            <asp:TextBox ID="txtExpiresAt" runat="server" CssClass="form-control" placeholder="yyyy-MM-dd HH:mm" />
                            <small class="text-muted">Để trống nếu không có hạn</small>
                        </div>
                    </div>
                </div>
                
                <div class="form-section">
                    <h5><i class="fas fa-users text-success"></i> Đối tượng nhận</h5>
                    <div class="mb-3">
                        <div class="form-check">
                            <asp:CheckBox ID="chkIsGlobal" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label fw-semibold" for="<%= chkIsGlobal.ClientID %>">
                                <i class="fas fa-globe text-success me-2"></i>
                                Thông báo toàn hệ thống (gửi cho tất cả người dùng)
                            </label>
                        </div>
                    </div>
                    
                    <div class="mb-3" id="userSelection" runat="server">
                        <label class="form-label fw-semibold">Chọn người dùng cụ thể</label>
                        <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-select">
                            <asp:ListItem Text="-- Chọn người dùng --" Value="" />
                        </asp:DropDownList>
                        <small class="text-muted">Chỉ hiển thị khi không chọn thông báo toàn hệ thống</small>
                    </div>
                </div>
                
                <div class="d-flex gap-2">
                    <asp:Button ID="btnCreateNotification" runat="server" Text="🚀 Tạo thông báo" 
                        CssClass="btn btn-primary" OnClick="btnCreateNotification_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="🗑️ Xóa form" 
                        CssClass="btn btn-secondary" OnClick="btnClear_Click" />
                </div>
            </div>
            
            <!-- Thông báo kết quả -->
            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
                <asp:Label ID="lblMessage" runat="server" />
            </asp:Panel>
            
            <!-- Danh sách thông báo -->
            <div class="notification-list">
                <div class="list-header">
                    <h4>
                        <i class="fas fa-list text-primary me-2"></i>
                        Danh sách thông báo
                    </h4>
                    <div class="d-flex gap-2">
                        <asp:Button ID="btnRefresh" runat="server" Text="🔄 Làm mới" 
                            CssClass="btn btn-outline-primary btn-sm" OnClick="btnRefresh_Click" />
                        <asp:Button ID="btnMarkAllRead" runat="server" Text="✅ Đánh dấu tất cả đã đọc" 
                            CssClass="btn btn-outline-success btn-sm" OnClick="btnMarkAllRead_Click" />
                    </div>
                </div>
                
                <asp:Repeater ID="rptNotifications" runat="server" OnItemCommand="rptNotifications_ItemCommand">
                    <ItemTemplate>
                        <div class="notification-item <%# Convert.ToBoolean(Eval("IsRead")) ? "" : "unread" %> <%# Convert.ToBoolean(Eval("IsGlobal")) ? "global" : "" %>">
                            <div class="d-flex justify-content-between align-items-start">
                                <div class="flex-grow-1">
                                    <div class="d-flex align-items-center mb-2">
                                        <h6 class="mb-0 me-2 fw-semibold"><%# Eval("Title") %></h6>
                                        <span class="notification-type type-<%# Eval("Type") %>"><%# GetTypeDisplayName(Eval("Type").ToString()) %></span>
                                        <%# Convert.ToBoolean(Eval("IsGlobal")) ? "<span class='badge badge-success ms-2'>🌍 Toàn hệ thống</span>" : "" %>
                                    </div>
                                    <p class="mb-2 text-muted"><%# Eval("Message") %></p>
                                    <div class="notification-meta">
                                        <span><i class="fas fa-clock"></i> <%# Convert.ToDateTime(Eval("CreatedAt")).ToString("dd/MM/yyyy HH:mm") %></span>
                                        <%# !string.IsNullOrEmpty(Eval("CreatedBy").ToString()) ? "<span><i class='fas fa-user'></i> " + Eval("CreatedBy") + "</span>" : "" %>
                                        <%# Eval("ExpiresAt") != DBNull.Value ? "<span><i class='fas fa-hourglass-end'></i> Hết hạn: " + Convert.ToDateTime(Eval("ExpiresAt")).ToString("dd/MM/yyyy HH:mm") + "</span>" : "" %>
                                    </div>
                                </div>
                                <div class="btn-group btn-group-sm">
                                    <asp:LinkButton ID="btnMarkRead" runat="server" CommandName="MarkRead" 
                                        CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-outline-primary"
                                        ToolTip="Đánh dấu đã đọc">
                                        <i class="fas fa-check"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" 
                                        CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-outline-warning"
                                        ToolTip="Chỉnh sửa">
                                        <i class="fas fa-edit"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" 
                                        CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-outline-danger"
                                        ToolTip="Xóa" OnClientClick="return confirm('Bạn có chắc muốn xóa thông báo này?');">
                                        <i class="fas fa-trash"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                
                <asp:Panel ID="pnlNoData" runat="server" Visible="false" CssClass="no-data">
                    <i class="fas fa-bell-slash"></i>
                    <h5 class="text-muted">Chưa có thông báo nào</h5>
                    <p class="text-muted">Tạo thông báo đầu tiên để bắt đầu</p>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content> 