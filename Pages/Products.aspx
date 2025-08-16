<%@ Page Title="" Language="C#" MasterPageFile="../Public.Master" AutoEventWireup="true" CodeFile="Products.aspx.cs" Inherits="Products" %>
<%@ Register TagPrefix="uc" TagName="ChatBot" Src="../UserControls/ChatBot.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Sản phẩm - 3D T-Shirt Design Platform</title>
    <meta name="description" content="Khám phá bộ sưu tập áo thun 3D độc đáo với nhiều thiết kế và tùy chọn tùy chỉnh" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Page Header -->
    <section class="page-header py-5">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center">
                    <h1 class="display-4 fw-bold">Sản phẩm</h1>
                    <p class="lead">Khám phá bộ sưu tập áo thun 3D độc đáo của chúng tôi</p>
                    <p class="mb-0">
                        <span id="lblProductCount" runat="server" class="badge bg-primary fs-6">0</span> sản phẩm
                    </p>
                </div>
            </div>
        </div>
    </section>

    <!-- Filters Section -->
    <section class="bg-white border-bottom">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-lg-3 col-md-6 mb-3">
                    <label class="form-label fw-bold">Danh mục:</label>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" 
                        AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                        <asp:ListItem Text="Tất cả danh mục" Value="" />
                    </asp:DropDownList>
                </div>
                <div class="col-lg-3 col-md-6 mb-3">
                    <label class="form-label fw-bold">Sắp xếp:</label>
                    <asp:DropDownList ID="ddlSort" runat="server" CssClass="form-select" 
                        AutoPostBack="true" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
                        <asp:ListItem Text="Mới nhất" Value="newest" />
                        <asp:ListItem Text="Giá tăng dần" Value="price_asc" />
                        <asp:ListItem Text="Giá giảm dần" Value="price_desc" />
                        <asp:ListItem Text="Tên A-Z" Value="name_asc" />
                        <asp:ListItem Text="Tên Z-A" Value="name_desc" />
                    </asp:DropDownList>
                </div>
                <div class="col-lg-4 col-md-6 mb-3">
                    <label class="form-label fw-bold">Tìm kiếm:</label>
                    <div class="input-group">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" 
                            placeholder="Nhập tên sản phẩm..." />
                        <asp:Button ID="btnSearch" runat="server" Text="Tìm" CssClass="btn btn-outline-secondary" 
                            OnClick="btnSearch_Click" />
                    </div>
                </div>
                <div class="col-lg-2 col-md-6 mb-3">
                    <label class="form-label fw-bold">Chế độ xem:</label>
                    <div class="view-toggle">
                        <asp:Button ID="btnGridView" runat="server" Text="Grid" CssClass="btn btn-outline-secondary active" 
                            OnClick="btnGridView_Click" />
                        <asp:Button ID="btnListView" runat="server" Text="List" CssClass="btn btn-outline-secondary" 
                            OnClick="btnListView_Click" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <asp:Button ID="btnClearFilter" runat="server" Text="Xóa bộ lọc" CssClass="btn btn-outline-secondary" 
                        OnClick="btnClearFilter_Click" />
                    <asp:Button ID="btnShowAll" runat="server" Text="Hiển thị tất cả" CssClass="btn btn-primary" 
                        OnClick="btnShowAll_Click" />
                </div>
            </div>
        </div>
    </section>

    <!-- Products Grid View -->
    <asp:Panel ID="pnlGridView" runat="server" CssClass="py-5">
        <div class="container">
            <div class="row">
                <asp:Repeater ID="rptProducts" runat="server">
                    <ItemTemplate>
                        <div class="col-lg-4 col-md-6 mb-4">
                            <div class="card product-card h-100">
                                <!-- Product Badges -->
                                <div class="product-badges">
                                    <%-- <%# Eval("IsNew") != null && (bool)Eval("IsNew") ? "<span class='badge bg-primary'>Mới</span>" : "" %> --%>
                                    <%# Eval("SalePrice") != null && (decimal)Eval("SalePrice") < (decimal)Eval("Price") ? "<span class='badge bg-danger'>Giảm giá</span>" : "" %>
                                </div>
                                
                                <!-- Product Image -->
                                <img src='<%# Eval("ImagePath") ?? "~/Image/placeholder.jpg" %>' 
                                     class="card-img-top product-image" 
                                     alt='<%# Eval("Name") %>' />
                                
                                <!-- Card Body -->
                                <div class="card-body d-flex flex-column">
                                    <h5 class="card-title"><%# Eval("Name") %></h5>
                                    <p class="card-text text-muted flex-grow-1"><%# Eval("Description") %></p>
                                    
                                    <!-- Price -->
                                    <div class="price">
                                        <asp:Literal ID="litOriginalPrice" runat="server" 
                                            Text='<%# (Eval("SalePrice") != null && (decimal)Eval("SalePrice") < (decimal)Eval("Price")) ? 
                                                "<span class=\"text-decoration-line-through\">" + String.Format("{0:C}", Eval("Price")) + "</span>" : "" %>' />
                                        <span class="h5 text-primary"><%# String.Format("{0:C}", Eval("SalePrice") ?? Eval("Price")) %></span>
                                    </div>
                                    
                                    <!-- Rating -->
                                    <div class="rating">
                                        <i class="fas fa-star"></i>
                                        <i class="fas fa-star"></i>
                                        <i class="fas fa-star"></i>
                                        <i class="fas fa-star"></i>
                                        <i class="fas fa-star-half-alt"></i>
                                        <small>(4.5)</small>
                                    </div>
                                    
                                    <!-- Product Meta -->
                                    <div class="product-meta">
                                        <span class="badge bg-light">Áo thun</span>
                                        <span class="badge bg-light">Còn hàng</span>
                                    </div>
                                    
                                    <!-- Action Buttons -->
                                    <div class="mt-3">
                                        <a href='ProductDetails.aspx?id=<%# Eval("Id") %>' class="btn btn-primary btn-sm w-100">
                                            <i class="fas fa-eye me-1"></i>Xem chi tiết
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </asp:Panel>

    <!-- Products List View -->
    <asp:Panel ID="pnlListView" runat="server" CssClass="py-5" Visible="false">
        <div class="container">
            <asp:Repeater ID="rptProductsList" runat="server">
                <ItemTemplate>
                    <div class="card mb-3">
                        <div class="row g-0">
                            <div class="col-md-3">
                                <img src='<%# Eval("ImagePath") ?? "~/Image/placeholder.jpg" %>' 
                                     class="img-fluid rounded-start" 
                                     alt='<%# Eval("Name") %>' 
                                     style="height: 200px; object-fit: cover;" />
                            </div>
                            <div class="col-md-9">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-8">
                                            <h5 class="card-title"><%# Eval("Name") %></h5>
                                            <p class="card-text"><%# Eval("Description") %></p>
                                            <div class="price mb-2">
                                                <asp:Literal ID="litOriginalPriceList" runat="server" 
                                                    Text='<%# (Eval("SalePrice") != null && (decimal)Eval("SalePrice") < (decimal)Eval("Price")) ? 
                                                        "<span class=\"text-decoration-line-through\">" + String.Format("{0:C}", Eval("Price")) + "</span>" : "" %>' />
                                                <span class="h5 text-primary"><%# String.Format("{0:C}", Eval("SalePrice") ?? Eval("Price")) %></span>
                                            </div>
                                            <div class="rating mb-2">
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star-half-alt"></i>
                                                <small>(4.5)</small>
                                            </div>
                                            <div class="product-meta">
                                                <span class="badge bg-light">Áo thun</span>
                                                <span class="badge bg-light">Còn hàng</span>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="btn-group-vertical w-100">
                                                <a href='ProductDetails.aspx?id=<%# Eval("Id") %>' class="btn btn-primary">
                                                    <i class="fas fa-eye me-2"></i>Xem chi tiết
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:Panel>

    <!-- No Products Message -->
    <asp:Panel ID="pnlNoProducts" runat="server" CssClass="text-center py-5" Visible="false">
        <div class="container">
            <i class="fas fa-search fa-4x mb-4"></i>
            <h4>Không tìm thấy sản phẩm</h4>
            <p class="text-muted">Hãy thử thay đổi bộ lọc hoặc từ khóa tìm kiếm</p>
            <asp:Button ID="btnResetSearch" runat="server" Text="Xem tất cả sản phẩm" 
                CssClass="btn btn-primary" OnClick="btnShowAll_Click" />
        </div>
    </asp:Panel>

    <!-- Pagination -->
    <section class="py-4">
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <nav aria-label="Product pagination">
                        <ul class="pagination justify-content-center">
                            <asp:Repeater ID="rptPagination" runat="server">
                                <ItemTemplate>
                                    <li class="page-item <%# IsActivePage(Eval("PageNumber")) %>">
                                        <asp:LinkButton ID="lnkPage" runat="server" 
                                            CssClass="page-link" 
                                            CommandName="Page" 
                                            CommandArgument='<%# Eval("PageNumber") %>' 
                                            OnCommand="lnkPage_Command">
                                            <%# Eval("PageNumber") %>
                                        </asp:LinkButton>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </nav>
                </div>
            </div>
        </div>
    </section>

    <!-- ChatBot UserControl -->
    <uc:ChatBot runat="server" ID="ChatBot" />

    <script type="text/javascript">
        function addToCart(productId) {
            // Thực hiện logic thêm vào giỏ hàng
            console.log('Thêm sản phẩm ID:', productId, 'vào giỏ hàng');
            
            // Gọi AJAX để thêm vào giỏ hàng
            $.ajax({
                type: "POST",
                url: "Products.aspx/AddToCart",
                data: JSON.stringify({ productId: productId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(response) {
                    if (response.d.success) {
                        // Hiển thị thông báo thành công
                        showNotification('Đã thêm sản phẩm vào giỏ hàng!', 'success');
                        
                        // Cập nhật số lượng giỏ hàng nếu có
                        if (response.d.cartCount !== undefined) {
                            $('#cartCount').text(response.d.cartCount);
                        }
                    } else {
                        showNotification('Có lỗi xảy ra: ' + response.d.message, 'error');
                    }
                },
                error: function() {
                    showNotification('Có lỗi xảy ra khi thêm vào giỏ hàng', 'error');
                }
            });
        }

        function buyNow(productId) {
            // Chuyển hướng đến trang chi tiết sản phẩm với tham số mua ngay
            window.location.href = 'ProductDetails.aspx?id=' + productId + '&buynow=true';
        }

        function showNotification(message, type) {
            // Tạo thông báo tạm thời
            const notification = $('<div class="alert alert-' + (type === 'success' ? 'success' : 'danger') + ' alert-dismissible fade show position-fixed" style="top: 20px; right: 20px; z-index: 9999;">' +
                message +
                '<button type="button" class="btn-close" data-bs-dismiss="alert"></button>' +
                '</div>');
            
            $('body').append(notification);
            
            // Tự động ẩn sau 3 giây
            setTimeout(function() {
                notification.alert('close');
            }, 3000);
        }

        // Khởi tạo tooltips và popovers
        $(document).ready(function() {
            // Khởi tạo tooltips
            var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
            var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl);
            });

            // Khởi tạo popovers
            var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
            var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
                return new bootstrap.Popover(popoverTriggerEl);
            });
        });
    </script>
</asp:Content>