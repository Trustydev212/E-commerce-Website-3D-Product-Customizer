<%@ Page Title="" Language="C#" MasterPageFile="../Admin.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="AdminPages_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <link href="../Css/dashboard.css" rel="stylesheet" type="text/css" />
    <link href="../Css/admin-pages.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid">
        <div class="admin-page-container">
            <!-- Header -->
            <div class="page-header">
                <h2>
                    <div class="header-icon">
                        <i class="fas fa-tachometer-alt"></i>
                    </div>
                    Dashboard
                </h2>
                <p class="text-muted mb-0">Tổng quan hệ thống quản lý</p>
                <small class="text-muted">Cập nhật lần cuối: <asp:Label ID="lblLastUpdate" runat="server" /></small>
            </div>

            <!-- Statistics Cards -->
            <div class="row mb-4">
                <div class="col-xl-3 col-md-6 mb-3">
                    <div class="card stats-card text-white bg-primary">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center">
                                <div>
                                    <div class="stats-number"><asp:Label ID="lblTotalOrders" runat="server" Text="0" /></div>
                                    <div class="stats-label">Tổng đơn hàng</div>
                                </div>
                                <div class="stats-icon">
                                    <i class="fas fa-shopping-cart"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-md-6 mb-3">
                    <div class="card stats-card text-white bg-success">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center">
                                <div>
                                    <div class="stats-number"><asp:Label ID="lblTotalRevenue" runat="server" Text="0" /></div>
                                    <div class="stats-label">Tổng doanh thu</div>
                                </div>
                                <div class="stats-icon">
                                    <i class="fas fa-dollar-sign"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-md-6 mb-3">
                    <div class="card stats-card text-white bg-warning">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center">
                                <div>
                                    <div class="stats-number"><asp:Label ID="lblTotalUsers" runat="server" Text="0" /></div>
                                    <div class="stats-label">Tổng người dùng</div>
                                </div>
                                <div class="stats-icon">
                                    <i class="fas fa-users"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-md-6 mb-3">
                    <div class="card stats-card text-white bg-info">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center">
                                <div>
                                    <div class="stats-number"><asp:Label ID="lblTotalProducts" runat="server" Text="0" /></div>
                                    <div class="stats-label">Tổng sản phẩm</div>
                                </div>
                                <div class="stats-icon">
                                    <i class="fas fa-tshirt"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Charts Row -->
            <div class="row mb-4">
                <div class="col-xl-8 col-lg-7">
                    <div class="card">
                        <div class="card-header">
                            <h5><i class="fas fa-chart-line"></i> Biểu đồ doanh thu 7 ngày qua</h5>
                        </div>
                        <div class="card-body">
                            <div class="chart-container">
                                <canvas id="revenueChart"></canvas>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-4 col-lg-5">
                    <div class="card">
                        <div class="card-header">
                            <h5><i class="fas fa-chart-pie"></i> Phân bố đơn hàng theo trạng thái</h5>
                        </div>
                        <div class="card-body">
                            <div class="chart-container">
                                <canvas id="orderStatusChart"></canvas>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Recent Data & Quick Actions -->
            <div class="row">
                <div class="col-xl-8">
                    <div class="row">
                        <div class="col-md-6 mb-4">
                            <div class="card">
                                <div class="card-header">
                                    <h5><i class="fas fa-clock"></i> Đơn hàng gần đây</h5>
                                </div>
                                <div class="card-body p-0">
                                    <asp:Repeater ID="rptRecentOrders" runat="server">
                                        <ItemTemplate>
                                            <div class="recent-item">
                                                <div class="d-flex justify-content-between align-items-center">
                                                    <div>
                                                        <strong>#<%# Eval("Id") %></strong>
                                                        <br />
                                                        <small class="text-muted"><%# Eval("CustomerName") %></small>
                                                    </div>
                                                    <div class="text-end">
                                                        <div class="fw-bold"><%# String.Format("{0:C}", Eval("TotalAmount")) %></div>
                                                        <span class="status-badge status-<%# Eval("Status").ToString().ToLower() %>">
                                                            <%# GetStatusDisplayName(Eval("Status").ToString()) %>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 mb-4">
                            <div class="card">
                                <div class="card-header">
                                    <h5><i class="fas fa-star"></i> Sản phẩm bán chạy</h5>
                                </div>
                                <div class="card-body p-0">
                                    <asp:Repeater ID="rptTopProducts" runat="server">
                                        <ItemTemplate>
                                            <div class="recent-item">
                                                <div class="d-flex justify-content-between align-items-center">
                                                    <div>
                                                        <strong><%# Eval("Name") %></strong>
                                                        <br />
                                                        <small class="text-muted">Đã bán: <%# Eval("SoldCount") %> cái</small>
                                                    </div>
                                                    <div class="text-end">
                                                        <div class="fw-bold"><%# String.Format("{0:C}", Eval("Price")) %></div>
                                                        <small class="text-success">+<%# Eval("Revenue") %></small>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-4">
                    <div class="quick-actions">
                        <h5 class="mb-3"><i class="fas fa-bolt"></i> Thao tác nhanh</h5>
                        <div class="row">
                            <div class="col-6 mb-3">
                                <a href="Orders.aspx" class="quick-action-btn text-center">
                                    <i class="fas fa-shopping-cart fa-2x mb-2"></i>
                                    <div>Quản lý đơn hàng</div>
                                </a>
                            </div>
                            <div class="col-6 mb-3">
                                <a href="Products.aspx" class="quick-action-btn text-center">
                                    <i class="fas fa-tshirt fa-2x mb-2"></i>
                                    <div>Quản lý sản phẩm</div>
                                </a>
                            </div>
                            <div class="col-6 mb-3">
                                <a href="Users.aspx" class="quick-action-btn text-center">
                                    <i class="fas fa-users fa-2x mb-2"></i>
                                    <div>Quản lý người dùng</div>
                                </a>
                            </div>
                            <div class="col-6 mb-3">
                                <a href="Categories.aspx" class="quick-action-btn text-center">
                                    <i class="fas fa-tags fa-2x mb-2"></i>
                                    <div>Quản lý danh mục</div>
                                </a>
                            </div>
                            <div class="col-6 mb-3">
                                <a href="AdminNotifications.aspx" class="quick-action-btn text-center">
                                    <i class="fas fa-bell fa-2x mb-2"></i>
                                    <div>Quản lý thông báo</div>
                                </a>
                            </div>
                            <div class="col-6 mb-3">
                                <a href="Posts.aspx" class="quick-action-btn text-center">
                                    <i class="fas fa-newspaper fa-2x mb-2"></i>
                                    <div>Quản lý bài viết</div>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        // Revenue Chart
        const revenueCtx = document.getElementById('revenueChart').getContext('2d');
        const revenueChart = new Chart(revenueCtx, {
            type: 'line',
            data: {
                labels: <asp:Literal ID="litRevenueLabels" runat="server" />,
                datasets: [{
                    label: 'Doanh thu (VNĐ)',
                    data: <asp:Literal ID="litRevenueData" runat="server" />,
                    borderColor: 'rgb(75, 192, 192)',
                    backgroundColor: 'rgba(75, 192, 192, 0.2)',
                    tension: 0.4,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            callback: function(value) {
                                return new Intl.NumberFormat('vi-VN', {
                                    style: 'currency',
                                    currency: 'VND'
                                }).format(value);
                            }
                        }
                    }
                }
            }
        });

        // Order Status Chart
        const statusCtx = document.getElementById('orderStatusChart').getContext('2d');
        const statusChart = new Chart(statusCtx, {
            type: 'doughnut',
            data: {
                labels: <asp:Literal ID="litStatusLabels" runat="server" />,
                datasets: [{
                    data: <asp:Literal ID="litStatusData" runat="server" />,
                    backgroundColor: [
                        '#ffc107', // Pending
                        '#17a2b8', // Processing
                        '#28a745', // Shipping
                        '#20c997', // Completed
                        '#dc3545'  // Cancelled
                    ]
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'bottom'
                    }
                }
            }
        });
    </script>
</asp:Content>