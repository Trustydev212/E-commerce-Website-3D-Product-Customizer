// Products page functionality
$(document).ready(function() {
    // Initialize tooltips
    $('[data-bs-toggle="tooltip"]').tooltip();
    
    // Initialize loading states
    $('.btn-loading').on('click', function() {
        var $btn = $(this);
        var originalText = $btn.text();
        $btn.prop('disabled', true).text('Đang xử lý...');
        
        // Re-enable after 3 seconds if no response
        setTimeout(function() {
            $btn.prop('disabled', false).text(originalText);
        }, 3000);
    });
});

// Quick View functionality
function quickView(btn) {
    var productId = $(btn).data('id');
    var $btn = $(btn);
    var originalText = $btn.text();
    
    // Show loading state
    $btn.prop('disabled', true).text('Đang tải...');
    
    // Call Web Service
    $.ajax({
        type: "POST",
        url: "App_Code/ProductService.cs/GetProductQuickView",
        data: JSON.stringify({ productId: productId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(response) {
            $btn.prop('disabled', false).text(originalText);
            
            if (response.d.success) {
                showQuickViewModal(response.d.product);
            } else {
                showNotification('error', response.d.message);
            }
        },
        error: function(xhr, status, error) {
            $btn.prop('disabled', false).text(originalText);
            showNotification('error', 'Có lỗi xảy ra khi tải thông tin sản phẩm');
            console.error('Quick view error:', error);
        }
    });
}

// Add to Cart functionality
function addToCart(btn) {
    var productId = $(btn).data('id');
    var $btn = $(btn);
    var originalText = $btn.text();
    
    // Show loading state
    $btn.prop('disabled', true).text('Đang thêm...');
    
    // Call Web Service
    $.ajax({
        type: "POST",
        url: "App_Code/ProductService.cs/AddToCart",
        data: JSON.stringify({ productId: productId, quantity: 1 }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(response) {
            $btn.prop('disabled', false).text(originalText);
            
            if (response.d.success) {
                showNotification('success', response.d.message);
                updateCartCount(response.d.cartCount);
            } else {
                showNotification('error', response.d.message);
            }
        },
        error: function(xhr, status, error) {
            $btn.prop('disabled', false).text(originalText);
            showNotification('error', 'Có lỗi xảy ra khi thêm vào giỏ hàng');
            console.error('Add to cart error:', error);
        }
    });
}

// Start 3D Design functionality
function startDesign(btn) {
    var productId = $(btn).data('id');
    var $btn = $(btn);
    var originalText = $btn.text();
    
    // Show loading state
    $btn.prop('disabled', true).text('Đang tải...');
    
    // Call Web Service
    $.ajax({
        type: "POST",
        url: "App_Code/ProductService.cs/GetDesignInfo",
        data: JSON.stringify({ productId: productId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(response) {
            $btn.prop('disabled', false).text(originalText);
            
            if (response.d.success) {
                // Redirect to design page with product info
                var designUrl = 'Design.aspx?productId=' + productId;
                window.location.href = designUrl;
            } else {
                showNotification('error', response.d.message);
            }
        },
        error: function(xhr, status, error) {
            $btn.prop('disabled', false).text(originalText);
            showNotification('error', 'Có lỗi xảy ra khi tải thông tin thiết kế');
            console.error('Design error:', error);
        }
    });
}

// Show Quick View Modal
function showQuickViewModal(product) {
    var modalHtml = `
        <div class="modal fade" id="quickViewModal" tabindex="-1" aria-labelledby="quickViewModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="quickViewModalLabel">${product.name}</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-6">
                                <img src="${product.imagePath}" class="img-fluid rounded" alt="${product.name}" />
                            </div>
                            <div class="col-md-6">
                                <h6 class="text-muted mb-2">${product.categoryName}</h6>
                                <p class="mb-3">${product.description}</p>
                                
                                <div class="price-section mb-3">
                                    ${product.salePrice ? 
                                        `<span class="h4 text-primary">${formatCurrency(product.salePrice)}</span>
                                         <span class="text-muted text-decoration-line-through ms-2">${formatCurrency(product.price)}</span>` :
                                        `<span class="h4 text-primary">${formatCurrency(product.price)}</span>`
                                    }
                                </div>
                                
                                <div class="product-details mb-3">
                                    <div class="row">
                                        <div class="col-6">
                                            <small class="text-muted">Tồn kho:</small>
                                            <div class="fw-bold">${product.stockQuantity} sản phẩm</div>
                                        </div>
                                        <div class="col-6">
                                            <small class="text-muted">Màu sắc:</small>
                                            <div class="fw-bold">${product.color || 'N/A'}</div>
                                        </div>
                                    </div>
                                    <div class="row mt-2">
                                        <div class="col-6">
                                            <small class="text-muted">Kích thước:</small>
                                            <div class="fw-bold">${product.size || 'N/A'}</div>
                                        </div>
                                        <div class="col-6">
                                            <small class="text-muted">Chất liệu:</small>
                                            <div class="fw-bold">${product.material || 'N/A'}</div>
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="d-grid gap-2">
                                    <button class="btn btn-primary" onclick="addToCartFromModal(${product.id})">
                                        <i class="fas fa-shopping-cart me-2"></i>Thêm vào giỏ hàng
                                    </button>
                                    <a href="ProductDetails.aspx?id=${product.id}" class="btn btn-outline-primary">
                                        <i class="fas fa-eye me-2"></i>Xem chi tiết
                                    </a>
                                    <button class="btn btn-success" onclick="startDesignFromModal(${product.id})">
                                        <i class="fas fa-palette me-2"></i>Thiết kế 3D
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;
    
    // Remove existing modal if any
    $('#quickViewModal').remove();
    
    // Add new modal to body
    $('body').append(modalHtml);
    
    // Show modal
    var modal = new bootstrap.Modal(document.getElementById('quickViewModal'));
    modal.show();
}

// Add to cart from modal
function addToCartFromModal(productId) {
    // Close modal first
    $('#quickViewModal').modal('hide');
    
    // Find the original button and trigger add to cart
    var $originalBtn = $('button[data-id="' + productId + '"]').filter(function() {
        return $(this).text().includes('Thêm vào giỏ');
    });
    
    if ($originalBtn.length > 0) {
        addToCart($originalBtn[0]);
    } else {
        // Fallback: direct AJAX call
        $.ajax({
            type: "POST",
            url: "App_Code/ProductService.cs/AddToCart",
            data: JSON.stringify({ productId: productId, quantity: 1 }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(response) {
                if (response.d.success) {
                    showNotification('success', response.d.message);
                    updateCartCount(response.d.cartCount);
                } else {
                    showNotification('error', response.d.message);
                }
            },
            error: function() {
                showNotification('error', 'Có lỗi xảy ra khi thêm vào giỏ hàng');
            }
        });
    }
}

// Start design from modal
function startDesignFromModal(productId) {
    // Close modal first
    $('#quickViewModal').modal('hide');
    
    // Redirect to design page
    window.location.href = 'Design.aspx?productId=' + productId;
}

// Update cart count in header
function updateCartCount(count) {
    var $cartBadge = $('.cart-count-badge');
    if ($cartBadge.length > 0) {
        $cartBadge.text(count);
        if (count > 0) {
            $cartBadge.removeClass('d-none');
        } else {
            $cartBadge.addClass('d-none');
        }
    }
}

// Show notification
function showNotification(type, message) {
    var alertClass = type === 'success' ? 'alert-success' : 'alert-danger';
    var icon = type === 'success' ? 'fas fa-check-circle' : 'fas fa-exclamation-circle';
    
    var notificationHtml = `
        <div class="alert ${alertClass} alert-dismissible fade show position-fixed" 
             style="top: 20px; right: 20px; z-index: 9999; min-width: 300px;" role="alert">
            <i class="${icon} me-2"></i>
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    `;
    
    // Remove existing notifications
    $('.alert.position-fixed').remove();
    
    // Add new notification
    $('body').append(notificationHtml);
    
    // Auto remove after 5 seconds
    setTimeout(function() {
        $('.alert.position-fixed').fadeOut(function() {
            $(this).remove();
        });
    }, 5000);
}

// Format currency
function formatCurrency(amount) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
}

// Product card hover effects
$(document).ready(function() {
    $('.product-card').hover(
        function() {
            $(this).find('.product-overlay').fadeIn(200);
        },
        function() {
            $(this).find('.product-overlay').fadeOut(200);
        }
    );
    
    // Add loading class to buttons
    $('button[onclick*="quickView"], button[onclick*="addToCart"], button[onclick*="startDesign"]').addClass('btn-loading');
}); 