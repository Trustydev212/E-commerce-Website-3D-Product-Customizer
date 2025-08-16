<%@ Page Language="C#" MasterPageFile="../Public.Master" AutoEventWireup="true" CodeFile="ProductDetails.aspx.cs" Inherits="ProductDetails" %>
<%@ Register TagPrefix="uc" TagName="ChatBot" Src="../UserControls/ChatBot.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        /* ===== PRODUCT DETAILS PAGE STYLES ===== */
        .product-details-container {
            background: linear-gradient(135deg, #f8f9ff 0%, #e8f2ff 100%);
            min-height: 100vh;
            padding: 2rem 0;
        }

        /* Main Product Image */
        .product-image { 
            height: 450px; 
            object-fit: cover; 
            border-radius: 20px;
            box-shadow: 0 8px 32px rgba(124, 58, 237, 0.15);
            transition: all 0.3s ease;
        }
        
        .product-image:hover {
            transform: scale(1.02);
            box-shadow: 0 12px 40px rgba(124, 58, 237, 0.25);
        }

        /* Thumbnail Images */
        .thumbnail-container {
            background: white;
            border-radius: 15px;
            padding: 1rem;
            box-shadow: 0 4px 20px rgba(124, 58, 237, 0.1);
            margin-top: 1rem;
        }

        .thumbnail-image { 
            height: 80px; 
            object-fit: cover; 
            cursor: pointer; 
            transition: all 0.3s ease;
            border-radius: 10px;
            border: 2px solid transparent;
        }
        
        .thumbnail-image:hover { 
            opacity: 0.8; 
            transform: translateY(-2px);
            border-color: #7c3aed;
        }

        /* Color Selection */
        .color-btn { 
            width: 45px; 
            height: 45px; 
            border-radius: 50%; 
            border: 3px solid #e9ecef;
            transition: all 0.3s ease;
            position: relative;
            overflow: hidden;
        }
        
        .color-btn:hover {
            transform: scale(1.1);
            border-color: #7c3aed;
        }
        
        .color-btn.selected { 
            border: 3px solid #7c3aed; 
            box-shadow: 0 0 15px rgba(124, 58, 237, 0.4);
            transform: scale(1.1);
        }

        .color-btn.selected::after {
            content: '✓';
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            color: white;
            font-weight: bold;
            text-shadow: 1px 1px 2px rgba(0,0,0,0.5);
        }

        /* Size Buttons */
        .size-btn { 
            width: 60px; 
            height: 45px; 
            border-radius: 12px;
            font-weight: 600;
            transition: all 0.3s ease;
        }
        
        .size-btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(124, 58, 237, 0.2);
        }

        /* Quantity Controls - Simple and Clean */
        .quantity-section {
            background: #f8f9fa;
            border-radius: 10px;
            padding: 1rem;
            margin-bottom: 1rem;
        }

        .quantity-label {
            font-weight: 600;
            color: #333;
            margin-bottom: 0.75rem;
            display: block;
        }

        .quantity-label i {
            color: #7c3aed;
            margin-right: 0.5rem;
        }

        .quantity-controls {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 0.5rem;
        }

        .quantity-input { 
            width: 70px !important;
            height: 38px !important;
            text-align: center !important;
            border-radius: 6px !important;
            border: 1px solid #ddd !important;
            font-weight: 600 !important;
            font-size: 14px !important;
            padding: 6px !important;
            background: white !important;
            margin: 0 !important;
        }
        
        .quantity-input:focus {
            border-color: #7c3aed !important;
            outline: none !important;
            box-shadow: none !important;
        }

        .quantity-btn {
            width: 38px !important;
            height: 38px !important;
            border-radius: 6px !important;
            border: 1px solid #ddd !important;
            background: white !important;
            font-size: 14px !important;
            display: flex !important;
            align-items: center !important;
            justify-content: center !important;
            padding: 0 !important;
            margin: 0 !important;
            min-width: 38px !important;
        }
        
        .quantity-btn:hover {
            border-color: #7c3aed !important;
            background: #7c3aed !important;
            color: white !important;
        }

        /* Product Info */
        .product-details {
            background: white;
            border-radius: 20px;
            padding: 2rem;
            box-shadow: 0 8px 32px rgba(124, 58, 237, 0.1);
            height: fit-content;
        }

        .product-details h1 {
            color: #1f2937;
            font-weight: 700;
            font-size: 2.2rem;
            margin-bottom: 1rem;
        }

        .product-price {
            background: linear-gradient(135deg, #7c3aed 0%, #a78bfa 100%);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            background-clip: text;
            font-size: 2.5rem;
            font-weight: 800;
        }

        .product-description {
            color: #6b7280;
            font-size: 1.1rem;
            line-height: 1.7;
            margin-bottom: 2rem;
        }

        /* Form Labels */
        .form-label {
            font-weight: 600;
            color: #374151;
            margin-bottom: 0.75rem;
            font-size: 1rem;
        }

        /* Add to Cart Button */
        .add-to-cart-btn {
            background: linear-gradient(135deg, #7c3aed 0%, #a78bfa 100%);
            border: none;
            border-radius: 15px;
            padding: 1rem 2rem;
            font-size: 1.1rem;
            font-weight: 600;
            color: white;
            transition: all 0.3s ease;
            box-shadow: 0 4px 16px rgba(124, 58, 237, 0.3);
        }
        
        .add-to-cart-btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 8px 25px rgba(124, 58, 237, 0.4);
            background: linear-gradient(135deg, #6d28d9 0%, #8b5cf6 100%);
        }

        /* Tabs */
        .nav-tabs {
            border: none;
            background: white;
            border-radius: 15px;
            padding: 0.5rem;
            box-shadow: 0 4px 20px rgba(124, 58, 237, 0.1);
        }

        .nav-tabs .nav-link {
            border: none;
            border-radius: 10px;
            color: #6b7280;
            font-weight: 500;
            padding: 0.75rem 1.5rem;
            transition: all 0.3s ease;
        }
        
        .nav-tabs .nav-link:hover {
            background: #f3f4f6;
            color: #7c3aed;
        }
        
        .nav-tabs .nav-link.active {
            background: linear-gradient(135deg, #7c3aed 0%, #a78bfa 100%);
            color: white;
            box-shadow: 0 4px 16px rgba(124, 58, 237, 0.3);
        }

        /* Related Products */
        .related-products {
            background: white;
            border-radius: 20px;
            padding: 2rem;
            box-shadow: 0 8px 32px rgba(124, 58, 237, 0.1);
            margin-top: 3rem;
        }

        .related-product-card {
            border: none;
            border-radius: 15px;
            overflow: hidden;
            transition: all 0.3s ease;
            box-shadow: 0 4px 20px rgba(124, 58, 237, 0.1);
        }
        
        .related-product-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 12px 40px rgba(124, 58, 237, 0.2);
        }

        /* Responsive Design */
        @media (max-width: 768px) {
            .product-image { height: 350px; }
            .product-details h1 { font-size: 1.8rem; }
            .product-price { font-size: 2rem; }
            .product-details { padding: 1.5rem; }
            .gallery-image { height: 150px; }
        }

        @media (max-width: 576px) {
            .product-image { height: 300px; }
            .product-details h1 { font-size: 1.5rem; }
            .product-price { font-size: 1.8rem; }
            .color-btn { width: 40px; height: 40px; }
            .size-btn { width: 50px; height: 40px; }
        }

        /* Loading Animation */
        .loading {
            opacity: 0.7;
            pointer-events: none;
        }

        .loading::after {
            content: '';
            position: absolute;
            top: 50%;
            left: 50%;
            width: 20px;
            height: 20px;
            margin: -10px 0 0 -10px;
            border: 2px solid #7c3aed;
            border-top: 2px solid transparent;
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }

        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="product-details-container">
        <div class="container">
        <div class="row">
            <div class="col-lg-6">
                <div class="product-images">
                    <div class="main-image mb-3">
                        <img id="mainImage" runat="server" class="img-fluid rounded product-image w-100" alt="Product Image" />
                    </div>
                    <div class="thumbnail-container">
                        <div class="row">
                            <div class="col-3">
                                <img id="thumb1" runat="server" class="img-fluid rounded thumbnail-image" />
                            </div>
                            <div class="col-3">
                                <img id="thumb2" runat="server" class="img-fluid rounded thumbnail-image" />
                            </div>
                            <div class="col-3">
                                <img id="thumb3" runat="server" class="img-fluid rounded thumbnail-image" />
                            </div>
                            <div class="col-3">
                                <img id="thumb4" runat="server" class="img-fluid rounded thumbnail-image" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="col-lg-6">
                <div class="product-details">
                    <h1 id="productName" runat="server"></h1>
                    <div class="price mb-3">
                            <span class="product-price" id="productPrice" runat="server"></span>
                    </div>
                    <div class="description mb-4">
                            <p class="product-description" id="productDescription" runat="server"></p>
                    </div>
                    
                    <div class="product-options">
                            <div class="mb-4">
                                <label class="form-label"><i class="fas fa-ruler me-2"></i>Kích thước:</label>
                            <div class="btn-group" role="group">
                                <input type="radio" class="btn-check" name="size" id="sizeS" value="S" checked />
                                <label class="btn btn-outline-primary size-btn" for="sizeS">S</label>
                                <input type="radio" class="btn-check" name="size" id="sizeM" value="M" />
                                <label class="btn btn-outline-primary size-btn" for="sizeM">M</label>
                                <input type="radio" class="btn-check" name="size" id="sizeL" value="L" />
                                <label class="btn btn-outline-primary size-btn" for="sizeL">L</label>
                                <input type="radio" class="btn-check" name="size" id="sizeXL" value="XL" />
                                <label class="btn btn-outline-primary size-btn" for="sizeXL">XL</label>
                            </div>
                        </div>
                        
                            <div class="mb-4">
                                <label class="form-label"><i class="fas fa-palette me-2"></i>Màu sắc:</label>
                                <div class="d-flex gap-3">
                                    <button type="button" class="btn color-btn" style="background-color: #ffffff;" data-color="white"></button>
                                <button type="button" class="btn color-btn" style="background-color: #000000;" data-color="black"></button>
                                <button type="button" class="btn color-btn" style="background-color: #ff0000;" data-color="red"></button>
                                <button type="button" class="btn color-btn" style="background-color: #0000ff;" data-color="blue"></button>
                            </div>
                        </div>
                        
                        <div class="quantity-section">
                            <label class="quantity-label">
                                <i class="fas fa-sort-numeric-up"></i>Số lượng:
                            </label>
                            <div class="quantity-controls">
                                <button type="button" class="btn quantity-btn" onclick="decreaseQuantity()">
                                    <i class="fas fa-minus"></i>
                                </button>
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control text-center quantity-input" Text="1" />
                                <button type="button" class="btn quantity-btn" onclick="increaseQuantity()">
                                    <i class="fas fa-plus"></i>
                                </button>
                            </div>
                        </div>
                        
                        <div class="d-grid gap-2 d-md-flex">
                            <button type="button" class="btn add-to-cart-btn flex-fill" onclick="goToDesign()">
                                <i class="fas fa-cube me-2"></i>Thiết kế 3D
                            </button>
                        </div>
                        
                        <!-- Hidden field to store image data -->
                        <asp:HiddenField ID="hdnImageData" runat="server" />
                        
                        <!-- Hidden fields to store selected options -->
                        <asp:HiddenField ID="hdnSelectedSize" runat="server" />
                        <asp:HiddenField ID="hdnSelectedColor" runat="server" />
                        <asp:HiddenField ID="hdnSelectedQuantity" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Product Details Tabs -->
        <div class="row mt-5">
            <div class="col-12">
                <ul class="nav nav-tabs" id="productTabs" role="tablist">
                    <li class="nav-item" role="presentation">
                        <button class="nav-link active" id="description-tab" data-bs-toggle="tab" data-bs-target="#description" type="button" role="tab">
                            <i class="fas fa-info-circle me-2"></i>Mô tả chi tiết
                        </button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" id="specifications-tab" data-bs-toggle="tab" data-bs-target="#specifications" type="button" role="tab">
                            <i class="fas fa-cogs me-2"></i>Thông số
                        </button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" id="reviews-tab" data-bs-toggle="tab" data-bs-target="#reviews" type="button" role="tab">
                            <i class="fas fa-star me-2"></i>Đánh giá
                        </button>
                    </li>
                </ul>
                <div class="tab-content" id="productTabsContent">
                    <div class="tab-pane fade show active" id="description" role="tabpanel">
                        <div class="p-4 bg-white rounded-bottom">
                            <asp:Literal ID="litDetailedDescription" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="tab-pane fade" id="specifications" role="tabpanel">
                        <div class="p-4 bg-white rounded-bottom">
                            <table class="table table-borderless">
                                <tr class="border-bottom">
                                    <td class="fw-bold text-dark">Chất liệu</td>
                                    <td class="text-muted">100% Cotton</td>
                                </tr>
                                <tr class="border-bottom">
                                    <td class="fw-bold text-dark">Kích thước</td>
                                    <td class="text-muted">S, M, L, XL</td>
                                </tr>
                                <tr class="border-bottom">
                                    <td class="fw-bold text-dark">Màu sắc</td>
                                    <td class="text-muted">Trắng, Đen, Đỏ, Xanh</td>
                                </tr>
                                <tr>
                                    <td class="fw-bold text-dark">Hướng dẫn giặt</td>
                                    <td class="text-muted">Giặt máy ở nhiệt độ thường</td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="tab-pane fade" id="reviews" role="tabpanel">
                        <div class="p-4 bg-white rounded-bottom text-center">
                            <i class="fas fa-star text-warning" style="font-size: 3rem;"></i>
                            <h6 class="mt-3">Chưa có đánh giá nào</h6>
                            <p class="text-muted">Hãy là người đầu tiên đánh giá sản phẩm này!</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Related Products -->
        <div class="related-products">
            <h4 class="mb-4"><i class="fas fa-th-large me-2"></i>Sản phẩm liên quan</h4>
                <div class="row">
                    <asp:Repeater ID="rptRelatedProducts" runat="server">
                        <ItemTemplate>
                            <div class="col-md-3 mb-4">
                            <div class="card related-product-card h-100">
                                    <img src='<%# Eval("ImageUrl") %>' class="card-img-top" alt="Product" style="height: 200px; object-fit: cover;" />
                                    <div class="card-body">
                                    <h6 class="card-title fw-bold"><%# Eval("Name") %></h6>
                                    <p class="card-text product-price mb-3"><%# Eval("Price", "{0:N0} VNĐ") %></p>
                                    <a href='ProductDetails.aspx?id=<%# Eval("Id") %>' class="btn add-to-cart-btn btn-sm w-100">
                                        <i class="fas fa-eye me-2"></i>Xem chi tiết
                                    </a>
                                </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
            </div>
        </div>
    </div>
    
    <script>
        // Lưu trữ ảnh theo màu sắc
        var productImages = {
            white: { main: '', thumb1: '', thumb2: '', thumb3: '', thumb4: '' },
            black: { main: '', thumb1: '', thumb2: '', thumb3: '', thumb4: '' },
            red: { main: '', thumb1: '', thumb2: '', thumb3: '', thumb4: '' },
            blue: { main: '', thumb1: '', thumb2: '', thumb3: '', thumb4: '' }
        };

        // Fallback image nếu không có ảnh
        var fallbackImage = '';
        
        // Load image data from hidden field
        function loadImageData() {
            try {
                var imageDataField = document.getElementById('<%= hdnImageData.ClientID %>');
                if (imageDataField && imageDataField.value) {
                    var imageData = JSON.parse(imageDataField.value);
                    
                    // Set fallback image
                    fallbackImage = imageData.fallbackImage || '';
                    
                    // Set product images for each color
                    if (imageData.colors) {
                        Object.keys(imageData.colors).forEach(function(color) {
                            if (productImages[color]) {
                                productImages[color] = imageData.colors[color];
                            }
                        });
                    }
                }
            } catch (ex) {
                console.error('Error loading image data:', ex);
            }
        }

        function increaseQuantity() {
            var qty = document.getElementById('ContentPlaceHolder1_txtQuantity');
            if (qty) {
                qty.value = parseInt(qty.value) + 1;
            }
        }
        
        function decreaseQuantity() {
            var qty = document.getElementById('ContentPlaceHolder1_txtQuantity');
            if (qty && parseInt(qty.value) > 1) {
                qty.value = parseInt(qty.value) - 1;
            }
        }
        
        // Thay đổi ảnh theo màu sắc với hiệu ứng loading
        function changeProductImages(color) {
            // Thêm hiệu ứng loading
            var mainImg = document.getElementById('ContentPlaceHolder1_mainImage');
            if (mainImg) {
                mainImg.classList.add('loading');
                mainImg.style.position = 'relative';
            }
            
            if (productImages[color]) {
                var images = productImages[color];
                
                // Thay đổi ảnh chính
                if (mainImg && images.main && images.main.trim() !== '') {
                    mainImg.src = images.main;
                } else if (mainImg && fallbackImage && fallbackImage.trim() !== '') {
                    mainImg.src = fallbackImage;
                } else {
                    console.warn('Không có ảnh nào để hiển thị cho màu:', color);
                }
                
                // Thay đổi ảnh thumbnail
                var thumb1 = document.getElementById('ContentPlaceHolder1_thumb1');
                var thumb2 = document.getElementById('ContentPlaceHolder1_thumb2');
                var thumb3 = document.getElementById('ContentPlaceHolder1_thumb3');
                var thumb4 = document.getElementById('ContentPlaceHolder1_thumb4');
                
                if (thumb1) thumb1.src = (images.thumb1 && images.thumb1.trim() !== '') ? images.thumb1 : fallbackImage;
                if (thumb2) thumb2.src = (images.thumb2 && images.thumb2.trim() !== '') ? images.thumb2 : fallbackImage;
                if (thumb3) thumb3.src = (images.thumb3 && images.thumb3.trim() !== '') ? images.thumb3 : fallbackImage;
                if (thumb4) thumb4.src = (images.thumb4 && images.thumb4.trim() !== '') ? images.thumb4 : fallbackImage;
            } else {
                console.warn('Không tìm thấy ảnh cho màu:', color);
            }
            
            // Xóa hiệu ứng loading sau 500ms
            setTimeout(function() {
                if (mainImg) {
                    mainImg.classList.remove('loading');
                }
            }, 500);
        }
        
        // Color selection với animation
        document.addEventListener('DOMContentLoaded', function() {
            // Load image data from hidden field
            loadImageData();

            // Thêm event listener cho color buttons
            var colorButtons = document.querySelectorAll('.color-btn');
            if (colorButtons) {
                colorButtons.forEach(function(btn) {
                    btn.addEventListener('click', function() {
                        // Loại bỏ selected class từ tất cả buttons
                        document.querySelectorAll('.color-btn').forEach(function(b) {
                            b.classList.remove('selected');
                        });
                        
                        // Thêm selected class cho button được click
                        this.classList.add('selected');
                        
                        // Lấy màu sắc được chọn
                        var selectedColor = this.getAttribute('data-color');
                        
                        // Cập nhật hidden field
                        document.getElementById('<%= hdnSelectedColor.ClientID %>').value = selectedColor;
                        
                        // Thay đổi ảnh
                        changeProductImages(selectedColor);
                    });
                });
            }
            
            // Thêm event listener cho size buttons
            var sizeButtons = document.querySelectorAll('input[name="size"]');
            if (sizeButtons) {
                sizeButtons.forEach(function(btn) {
                    btn.addEventListener('change', function() {
                        var selectedSize = this.value;
                        document.getElementById('<%= hdnSelectedSize.ClientID %>').value = selectedSize;
                    });
                });
            }
            
            // Thêm event listener cho quantity input
            var quantityInput = document.getElementById('ContentPlaceHolder1_txtQuantity');
            if (quantityInput) {
                quantityInput.addEventListener('change', function() {
                    var quantity = this.value;
                    document.getElementById('<%= hdnSelectedQuantity.ClientID %>').value = quantity;
                });
            }
            
            // Mặc định chọn màu trắng
            var firstColorBtn = document.querySelector('.color-btn');
            if (firstColorBtn) {
                firstColorBtn.classList.add('selected');
                document.getElementById('<%= hdnSelectedColor.ClientID %>').value = firstColorBtn.getAttribute('data-color');
            }
            
            // Mặc định chọn size M
            var sizeM = document.getElementById('sizeM');
            if (sizeM) {
                sizeM.checked = true;
                document.getElementById('<%= hdnSelectedSize.ClientID %>').value = 'M';
            }
            
            // Mặc định quantity = 1
            document.getElementById('<%= hdnSelectedQuantity.ClientID %>').value = '1';
            
            // Thêm hiệu ứng hover cho thumbnail images
            var thumbnailImages = document.querySelectorAll('.thumbnail-image');
            if (thumbnailImages) {
                thumbnailImages.forEach(function(thumb) {
                    thumb.addEventListener('click', function() {
                        var mainImg = document.getElementById('ContentPlaceHolder1_mainImage');
                        if (mainImg && this.src) {
                            mainImg.src = this.src;
                        }
                    });
                });
            }
        });

        function goToDesign() {
            // Lấy thông tin size được chọn
            var sizeElement = document.querySelector('input[name="size"]:checked');
            var size = sizeElement ? sizeElement.value : 'M';
            
            // Lấy thông tin color được chọn
            var colorElement = document.querySelector('.color-btn.selected');
            var color = colorElement ? colorElement.getAttribute('data-color') : 'white';
            
            // Lấy thông tin quantity
            var quantityElement = document.getElementById('ContentPlaceHolder1_txtQuantity');
            var quantity = quantityElement ? quantityElement.value : '1';
            
            // Lưu các giá trị vào hidden fields
            document.getElementById('<%= hdnSelectedSize.ClientID %>').value = size;
            document.getElementById('<%= hdnSelectedColor.ClientID %>').value = color;
            document.getElementById('<%= hdnSelectedQuantity.ClientID %>').value = quantity;
            
            // Lấy product ID từ URL hiện tại
            var currentUrl = window.location.href;
            var urlParams = new URLSearchParams(window.location.search);
            var productId = urlParams.get('id');
            
            if (!productId) {
                alert('Lỗi: Không tìm thấy ID sản phẩm!');
                return;
            }
            
            // Tạo URL cho Design.aspx với đầy đủ thông tin
            var designUrl = 'Design.aspx?productId=' + productId + '&size=' + encodeURIComponent(size) + '&color=' + encodeURIComponent(color) + '&quantity=' + encodeURIComponent(quantity);
            
            // Chuyển hướng
            window.location.href = designUrl;
        }
    </script>
    
    <!-- ChatBot UserControl -->
    <uc:ChatBot runat="server" ID="ChatBot" />
</asp:Content>
