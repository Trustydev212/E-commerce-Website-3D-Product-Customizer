<%@ Page Language="C#" MasterPageFile="../Public.master" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Thông tin cá nhân - 3D T-Shirt Design Platform</title>
    <meta name="description" content="Trang cá nhân người dùng" />
    <link href="../Css/site.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Loading Overlay -->
    <div class="loading-overlay" id="loadingOverlay">
        <div class="loading-spinner">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Đang tải...</span>
            </div>
        </div>
    </div>

    <div class="user-profile-container">
        <div class="container">
            <div class="row">
                <!-- Sidebar -->
                <div class="col-lg-3 mb-4 mb-lg-0">
                    <div class="user-profile-sidebar">
                        <div class="card-body text-center p-4">
                            <asp:Image ID="imgAvatar" runat="server" CssClass="user-profile-avatar mb-3" Width="120" Height="120" ImageUrl="~/Images/avatar-default.png" />
                            <h5 class="mb-1 fw-bold" id="userName" runat="server"></h5>
                            <p class="text-muted small mb-3" id="userEmail" runat="server"></p>
                            <div class="mb-3">
                                <asp:FileUpload ID="fuAvatar" runat="server" CssClass="form-control form-control-sm" onchange="previewAvatar(this)" />
                            </div>
                            <asp:Button ID="btnChangeAvatar" runat="server" Text="Đổi ảnh đại diện" CssClass="user-profile-btn btn-sm w-100" OnClick="btnChangeAvatar_Click" OnClientClick="showLoading()" />
                        </div>
                    </div>
                    
                    <div class="user-profile-tabs list-group mt-4">
                        <a href="#profile" class="list-group-item list-group-item-action active" data-bs-toggle="tab">
                            <i class="fas fa-user me-2"></i>Thông tin cá nhân
                        </a>
                        <a href="#orders" class="list-group-item list-group-item-action" data-bs-toggle="tab">
                            <i class="fas fa-shopping-bag me-2"></i>Đơn hàng
                        </a>
                        <a href="#designs" class="list-group-item list-group-item-action" data-bs-toggle="tab">
                            <i class="fas fa-paint-brush me-2"></i>Thiết kế
                        </a>
                        <a href="#notifications" class="list-group-item list-group-item-action" data-bs-toggle="tab">
                            <i class="fas fa-bell me-2"></i>Thông báo
                        </a>
                        <a href="#settings" class="list-group-item list-group-item-action" data-bs-toggle="tab">
                            <i class="fas fa-cog me-2"></i>Cài đặt
                        </a>
                    </div>
                </div>
                
                <!-- Main Content -->
                <div class="col-lg-9">
                    <div class="tab-content">
                        <!-- Profile Tab -->
                        <div class="tab-pane fade show active" id="profile">
                            <div class="user-profile-card card mb-4">
                                <div class="card-header">
                                    <h5 class="mb-0"><i class="fas fa-user me-2"></i>Thông tin cá nhân</h5>
                                </div>
                                <div class="card-body p-4">
                                    <div class="user-profile-form">
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label">Họ và tên <span class="text-danger">*</span></label>
                                                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Nhập họ và tên" />
                                                <div class="invalid-feedback">Vui lòng nhập họ và tên</div>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label">Email</label>
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true" />
                                                <small class="text-muted">Email không thể thay đổi</small>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label">Số điện thoại</label>
                                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Nhập số điện thoại" />
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label">Ngày sinh</label>
                                                <asp:TextBox ID="txtBirthDate" runat="server" CssClass="form-control" TextMode="SingleLine" />
                                            </div>
                                        </div>
                                        <div class="mb-4">
                                            <label class="form-label">Địa chỉ</label>
                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Nhập địa chỉ" />
                                        </div>
                                        <asp:Button ID="btnUpdateProfile" runat="server" Text="Cập nhật thông tin" CssClass="user-profile-btn" OnClick="btnUpdateProfile_Click" OnClientClick="showLoading()" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <!-- Orders Tab -->
                        <div class="tab-pane fade" id="orders">
                            <div class="user-profile-card card mb-4">
                                <div class="card-header">
                                    <h5 class="mb-0"><i class="fas fa-shopping-bag me-2"></i>Đơn hàng của tôi</h5>
                                </div>
                                <div class="card-body p-4">
                                    <asp:UpdatePanel ID="UpdatePanelOrders" runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="rptOrders" runat="server">
                                                <ItemTemplate>
                                                    <div class="order-item">
                                                        <div class="row align-items-center">
                                                            <div class="col-md-2">
                                                                <strong class="text-primary">#<%# Eval("Id") %></strong>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <span class="text-muted"><i class="fas fa-calendar me-1"></i><%# Eval("CreatedDate", "{0:dd/MM/yyyy}") %></span>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <span class="status-badge status-<%# GetStatusColor(Eval("Status").ToString()) %>"><%# Eval("Status") %></span>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <strong class="text-primary"><%# Eval("Total", "{0:N0} đ") %></strong>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <a href='OrderDetails.aspx?id=<%# Eval("Id") %>' class="btn btn-sm btn-outline-primary">
                                                                    <i class="fas fa-eye me-1"></i>Chi tiết
                                                                </a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        
                        <!-- Designs Tab -->
                        <div class="tab-pane fade" id="designs">
                            <div class="user-profile-card card mb-4">
                                <div class="card-header">
                                    <h5 class="mb-0"><i class="fas fa-paint-brush me-2"></i>Thiết kế của tôi</h5>
                                </div>
                                <div class="card-body p-4">
                                    <asp:UpdatePanel ID="UpdatePanelDesigns" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <asp:Repeater ID="rptDesigns" runat="server">
                                                    <ItemTemplate>
                                                        <div class="col-md-6 col-lg-4 mb-4">
                                                            <div class="design-item">
                                                                <div class="design-preview mb-3">
                                                                    <img src='<%# Eval("PreviewPath", "{0}") ?? "/Images/design-placeholder.png" %>' class="img-fluid" alt="Design" />
                                                                </div>
                                                                <h6 class="fw-bold mb-2"><%# Eval("Name") %></h6>
                                                                <p class="text-muted small mb-3">
                                                                    <i class="fas fa-calendar me-1"></i><%# Eval("CreatedAt", "{0:dd/MM/yyyy}") %>
                                                                </p>
                                                                <div class="d-grid gap-2">
                                                                    <a href='DesignEditor.aspx?id=<%# Eval("Id") %>' class="btn btn-sm btn-outline-primary">
                                                                        <i class="fas fa-edit me-1"></i>Chỉnh sửa
                                                                    </a>
                                                                    <asp:Button ID="btnDeleteDesign" runat="server" Text="Xóa thiết kế" CssClass="btn btn-sm btn-outline-danger" CommandName="DeleteDesign" CommandArgument='<%# Eval("Id") %>' OnCommand="btnDeleteDesign_Command" OnClientClick="return confirm('Bạn có chắc muốn xóa thiết kế này?')" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        
                        <!-- Notifications Tab -->
                        <div class="tab-pane fade" id="notifications">
                            <div class="user-profile-card card mb-4">
                                <div class="card-header">
                                    <h5 class="mb-0"><i class="fas fa-bell me-2"></i>Thông báo</h5>
                                </div>
                                <div class="card-body p-4">
                                    <div class="text-center py-5">
                                        <i class="fas fa-bell fa-3x text-muted mb-3"></i>
                                        <h5 class="text-muted mb-3">Quản lý thông báo</h5>
                                        <p class="text-muted mb-4">Xem và quản lý tất cả thông báo của bạn</p>
                                        <a href="UserNotifications.aspx" class="user-profile-btn">
                                            <i class="fas fa-external-link-alt me-2"></i>Xem thông báo
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <!-- Settings Tab -->
                        <div class="tab-pane fade" id="settings">
                            <div class="user-profile-card card mb-4">
                                <div class="card-header">
                                    <h5 class="mb-0"><i class="fas fa-cog me-2"></i>Đổi mật khẩu</h5>
                                </div>
                                <div class="card-body p-4">
                                    <div class="user-profile-form">
                                        <div class="mb-3">
                                            <label class="form-label">Mật khẩu hiện tại <span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Nhập mật khẩu hiện tại" />
                                        </div>
                                        <div class="mb-3">
                                            <label class="form-label">Mật khẩu mới <span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Nhập mật khẩu mới (tối thiểu 6 ký tự)" />
                                            <small class="text-muted">Mật khẩu phải có ít nhất 6 ký tự</small>
                                        </div>
                                        <div class="mb-4">
                                            <label class="form-label">Xác nhận mật khẩu mới <span class="text-danger">*</span></label>
                                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Nhập lại mật khẩu mới" />
                                        </div>
                                        <asp:Button ID="btnChangePassword" runat="server" Text="Đổi mật khẩu" CssClass="user-profile-btn" OnClick="btnChangePassword_Click" OnClientClick="showLoading()" />
                                    </div>
                                </div>
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
    <script>
        // Loading overlay functions
        function showLoading() {
            document.getElementById('loadingOverlay').style.display = 'block';
        }

        function hideLoading() {
            document.getElementById('loadingOverlay').style.display = 'none';
        }

        // Avatar preview function
        function previewAvatar(input) {
            if (input.files && input.files[0]) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    const avatarImg = document.getElementById('<%= imgAvatar.ClientID %>');
                    avatarImg.src = e.target.result;
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        // Form validation
        function validateProfileForm() {
            const fullName = document.getElementById('<%= txtFullName.ClientID %>').value.trim();
            if (!fullName) {
                showNotification('Vui lòng nhập họ và tên', 'error');
                return false;
            }
            return true;
        }

        function validatePasswordForm() {
            const currentPassword = document.getElementById('<%= txtCurrentPassword.ClientID %>').value.trim();
            const newPassword = document.getElementById('<%= txtNewPassword.ClientID %>').value.trim();
            const confirmPassword = document.getElementById('<%= txtConfirmPassword.ClientID %>').value.trim();
            
            if (!currentPassword || !newPassword || !confirmPassword) {
                showNotification('Vui lòng điền đầy đủ thông tin', 'error');
                return false;
            }
            
            if (newPassword.length < 6) {
                showNotification('Mật khẩu mới phải có ít nhất 6 ký tự', 'error');
                return false;
            }
            
            if (newPassword !== confirmPassword) {
                showNotification('Mật khẩu mới và xác nhận mật khẩu không khớp', 'error');
                return false;
            }
            
            return true;
        }

        // Tab activation
        document.addEventListener('DOMContentLoaded', function() {
            var triggerTabList = [].slice.call(document.querySelectorAll('.list-group-item'));
            triggerTabList.forEach(function (triggerEl) {
                triggerEl.addEventListener('click', function (event) {
                    var tabTrigger = new bootstrap.Tab(triggerEl);
                    tabTrigger.show();
                });
            });

            // Hide loading after page load
            hideLoading();
        });

        // Auto-hide loading after AJAX requests
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function() {
            hideLoading();
        });
    </script>
</asp:Content>


