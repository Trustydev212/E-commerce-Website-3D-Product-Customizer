<%@ Page Title="" Language="C#" MasterPageFile="../Public.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>
<%@ Register TagPrefix="uc" TagName="ChatBot" Src="../UserControls/ChatBot.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>3D T-Shirt Design Platform - Trang chủ</title>
    <meta name="description" content="Nền tảng thiết kế áo thun 3D tùy chỉnh hàng đầu Việt Nam" />
    <style>
        /* ===== HERO SECTION STYLES ===== */
        .hero-section {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 6rem 0;
            position: relative;
            overflow: hidden;
        }
        .hero-section::before {
        
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1000 1000"><polygon fill="rgba(255,255,255,0.05)" points="0,1000 1000,0 1000,1000"/></svg>');
            background-size: cover;
        }

        .hero-section .container {
            margin-top: 100px;
            position: relative;
            z-index: 2;
        }

        /* Hero Text Content Styling */
        .hero-section .col-lg-6:first-child {
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: flex-start;
        }

        /* Hero Text Styling */
        .hero-section h1 {
            color: #ffffff;
            font-size: 3.5rem;
            font-weight: 800;
            line-height: 1.2;
            margin-bottom: 1.5rem;
            text-shadow: 0 4px 8px rgba(0,0,0,0.3);
            background: linear-gradient(135deg, #ffffff 0%, #f0f4ff 100%);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            background-clip: text;
        }
        .text-center{
            margin-top: 0px !important;
        }
        .hero-section .lead {
            color: #e8f2ff;
            font-size: 1.3rem;
            font-weight: 400;
            line-height: 1.6;
            margin-bottom: 2.5rem;
            text-shadow: 0 2px 4px rgba(0,0,0,0.2);
        }

        /* Hero Buttons */
        .hero-buttons {
            display: flex;
            gap: 1rem;
            flex-wrap: wrap;
        }

        .hero-btn-primary {
            background: linear-gradient(135deg, #7c3aed 0%, #a78bfa 100%);
            border: none;
            border-radius: 50px;
            padding: 1rem 2.5rem;
            font-size: 1.1rem;
            font-weight: 600;
            color: white;
            text-decoration: none;
            transition: all 0.3s ease;
            box-shadow: 0 8px 25px rgba(124, 58, 237, 0.4);
            position: relative;
            overflow: hidden;
        }

        .hero-btn-primary::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.2), transparent);
            transition: left 0.5s;
        }

        .hero-btn-primary:hover {
            transform: translateY(-3px);
            box-shadow: 0 12px 35px rgba(124, 58, 237, 0.6);
            color: white;
            text-decoration: none;
        }

        .hero-btn-primary:hover::before {
            left: 100%;
        }

        .hero-btn-primary:active {
            transform: translateY(-1px);
        }

        /* Hero Image/Video */
        .hero-image {
            position: relative;
            border-radius: 25px;
            overflow: hidden;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            transform: perspective(1000px) rotateY(-5deg);
            transition: all 0.3s ease;
        }

        .hero-image:hover {
            transform: perspective(1000px) rotateY(0deg) scale(1.02);
            box-shadow: 0 25px 80px rgba(0,0,0,0.4);
        }

        .hero-image video {
            width: 100%;
            height: 450px;
            border-radius: 25px;
            object-fit: cover;
            box-shadow: 0 10px 30px rgba(0,0,0,0.2);
        }

        /* Responsive Design */
        @media (max-width: 768px) {
            .hero-section {
                padding: 4rem 0;
            }
            
            .hero-section h1 {
                font-size: 2.5rem;
            }
            
            .hero-section .lead {
                font-size: 1.1rem;
            }
            
            .hero-buttons {
                justify-content: center;
            }
            
            .hero-image {
                transform: none;
                margin-top: 2rem;
            }
            
            .hero-image:hover {
                transform: scale(1.02);
            }
        }

        @media (max-width: 576px) {
            .hero-section h1 {
                font-size: 2rem;
            }
            
            .hero-section .lead {
                font-size: 1rem;
            }
            
            .hero-btn-primary {
                padding: 0.8rem 2rem;
                font-size: 1rem;
            }
            
            .hero-image video {
                height: 300px;
            }
        }

        /* Animation for hero elements */
        .hero-section h1,
        .hero-section .lead,
        .hero-buttons,
        .hero-image {
            animation: fadeInUp 1s ease-out;
        }

        .hero-section .lead {
            animation-delay: 0.2s;
        }

        .hero-buttons {
            animation-delay: 0.4s;
        }

        .hero-image {
            animation-delay: 0.6s;
        }

        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(30px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Hero Section -->
    <section class="hero-section">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-lg-6">
                    <h1 class="display-4 fw-bold">Thiết kế áo thun 3D độc đáo</h1>
                    <p class="lead">Tạo ra những chiếc áo thun unique với công nghệ 3D tiên tiến. Thiết kế, xem trước và đặt hàng ngay!</p>
                    <div class="hero-buttons">
                        <a href="Products.aspx" class="hero-btn-primary">
                            <i class="fas fa-shopping-bag me-2"></i>Khám phá sản phẩm
                        </a>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="hero-image">
                        <video
    style="width: 100%; height: 400px; border-radius: 20px; object-fit: cover;"
    autoplay
    loop
    muted
    playsinline
>
    <source src="https://cdn.prod.website-files.com/65f6776adcbc7d17dbd30416%2F68282bdef6ea2287d9ec5873_livereneder8-transcode.webm" type="video/webm" />
    Trình duyệt của bạn không hỗ trợ video.
</video>
                    </div>
                </div>
            </div>
        </div>
    </section>
    
    <!-- Categories Section -->
    <section class="py-5">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center mb-5">
                    <h2 class="display-5 fw-bold">Danh mục sản phẩm</h2>
                    <p class="lead">Lựa chọn từ nhiều loại áo thun chất lượng cao</p>
                </div>
            </div>
            <div class="row">
                <asp:Repeater ID="rptCategories" runat="server">
                    <ItemTemplate>
                        <div class="col-lg-3 col-md-6 mb-4">
                            <div class="card category-card h-100">
                                <div class="card-body text-center">
                                    <img src="<%# Eval("ImagePath") ?? "~/Image/placeholder.jpg" %>" alt="<%# Eval("Name") %>" class="img-fluid mb-3" style="height: 150px; object-fit: cover;" />
                                    <h5 class="card-title"><%# Eval("Name") %></h5>
                                    <p class="card-text"><%# Eval("Description") %></p>
                                    <a href="Products.aspx?category=<%# Eval("Id") %>" class="btn btn-primary">Xem sản phẩm</a>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
    
    <!-- Featured Products Section -->
    <section class="py-5 bg-light">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center mb-5">
                    <h2 class="display-5 fw-bold">Sản phẩm nổi bật</h2>
                    <p class="lead">Những thiết kế được yêu thích nhất</p>
                </div>
            </div>
            <div class="row">
                <asp:Repeater ID="rptFeaturedProducts" runat="server">
                    <ItemTemplate>
                        <div class="col-lg-4 col-md-6 mb-4">
                            <div class="card product-card h-100">
                                <img src="<%# Eval("ImagePath") ?? "~/Image/placeholder.jpg" %>" class="card-img-top product-image" alt="<%# Eval("Name") %>" style="height: 250px; object-fit: cover;" />
                                <div class="card-body">
                                    <h5 class="card-title"><%# Eval("Name") %></h5>
                                    <p class="card-text"><%# Eval("Description") %></p>
                                    <div class="d-flex justify-content-between align-items-center">
                                        <div class="price">
                                            <span class="h5 text-primary"><%# String.Format("{0:C}", Eval("Price")) %></span>
                                        </div>
                                        <div class="buttons">
                                            <a href="ProductDetails.aspx?id=<%# Eval("Id") %>" class="btn btn-outline-primary btn-sm">Chi tiết</a>
                                            <button class="btn btn-primary btn-sm" data-id="<%# Eval("Id") %>" onclick="addToCart(this)">Mua ngay</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
    
    <!-- Introduction Section -->
    <section id="introduction-section" class="py-5">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center mb-5">
                    <h2 class="display-5 fw-bold">Giới thiệu về 3D T-Shirt</h2>
                    <p class="lead">Khám phá công nghệ thiết kế 3D tiên tiến</p>
                </div>
            </div>
            
            <!-- Single Video Demo -->
            <div class="row justify-content-center mb-5">
                <div class="col-lg-8 col-md-10">
                    <div class="video-demo-card">
                        <div class="video-container">
                            <video autoplay muted loop class="demo-video">
                                <source src="https://cdn.prod.website-files.com/65f6776adcbc7d17dbd30416%2F68281335d591a236c7c0fd24_walkingtshirtv3-transcode.webm" type="video/webm">
                                Trình duyệt của bạn không hỗ trợ video.
                            </video>
                            <div class="video-overlay">
                                <div class="play-icon">
                                    <i class="fas fa-play"></i>
                                </div>
                            </div>
                        </div>
                        <div class="video-content">
                            <h4>Mô phỏng 3D thực tế</h4>
                            <p>Xem mô phỏng 3D chân thực của sản phẩm với công nghệ tiên tiến</p>
                        </div>
                    </div>
                </div>
            </div>
            
            <!-- Features -->
            <div class="row">
                <div class="col-lg-3 col-md-6 mb-4">
                    <div class="feature-card text-center">
                        <div class="feature-icon">
                            <i class="fas fa-cube fa-3x"></i>
                        </div>
                        <h5 class="mt-3">Thiết kế 3D</h5>
                        <p class="text-muted">Tạo thiết kế 3D chân thực với công cụ tiên tiến</p>
                    </div>
                </div>
                
                <div class="col-lg-3 col-md-6 mb-4">
                    <div class="feature-card text-center">
                        <div class="feature-icon">
                            <i class="fas fa-palette fa-3x"></i>
                        </div>
                        <h5 class="mt-3">Tùy chỉnh màu sắc</h5>
                        <p class="text-muted">Thay đổi màu sắc và chất liệu dễ dàng</p>
                    </div>
                </div>
                
                <div class="col-lg-3 col-md-6 mb-4">
                    <div class="feature-card text-center">
                        <div class="feature-icon">
                            <i class="fas fa-upload fa-3x"></i>
                        </div>
                        <h5 class="mt-3">Upload logo</h5>
                        <p class="text-muted">Thêm logo và hình ảnh của riêng bạn</p>
                    </div>
                </div>
                
                <div class="col-lg-3 col-md-6 mb-4">
                    <div class="feature-card text-center">
                        <div class="feature-icon">
                            <i class="fas fa-eye fa-3x"></i>
                        </div>
                        <h5 class="mt-3">Xem trước thực tế</h5>
                        <p class="text-muted">Xem trước sản phẩm từ mọi góc độ</p>
                    </div>
                </div>
            </div>
            
            <!-- CTA Button -->
            <div class="row mt-5">
                <div class="col-12 text-center">
                    <a href="Design.aspx" class="btn btn-primary btn-lg">
                        <i class="fas fa-magic me-2"></i>
                        Bắt đầu thiết kế ngay
                    </a>
                </div>
            </div>
        </div>
    </section>
    
    <!-- Blog Section -->
    <section class="py-5 bg-light">
        <div class="container">
            <div class="row">
                <div class="col-12 text-center mb-5">
                    <h2 class="display-5 fw-bold">Blog & Tin tức</h2>
                    <p class="lead">Cập nhật xu hướng thiết kế mới nhất</p>
                </div>
            </div>
            <div class="row">
                <asp:Repeater ID="rptLatestPosts" runat="server">
                    <ItemTemplate>
                        <div class="col-lg-4 col-md-6 mb-4">
                            <div class="card blog-card h-100">
                                <img src="<%# Eval("ImagePath") ?? "~/Image/placeholder.jpg" %>" class="card-img-top blog-image" alt="<%# Eval("Title") %>" style="height: 200px; object-fit: cover;" />
                                <div class="card-body">
                                    <h5 class="card-title"><%# Eval("Title") %></h5>
                                    <p class="card-text"><%# Eval("Summary") %></p>
                                    <div class="d-flex justify-content-between align-items-center">
                                        <small class="text-muted"><%# String.Format("{0:dd/MM/yyyy}", Eval("PublishedDate")) %></small>
                                        <a href="PostDetails.aspx?id=<%# Eval("Id") %>" class="btn btn-outline-primary btn-sm">Đọc thêm</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div class="row">
                <div class="col-12 text-center">
                    <a href="Blog.aspx" class="btn btn-primary">Xem tất cả bài viết</a>
                </div>
            </div>
        </div>
    </section>
    
    <!-- ChatBot UserControl -->
    <uc:ChatBot runat="server" ID="ChatBot" />
    
    <script type="text/javascript">
    function addToCart(btn) {
        var id = btn.getAttribute('data-id');
        // Thực hiện logic thêm vào giỏ hàng với id này
        alert('Thêm sản phẩm có ID: ' + id + ' vào giỏ hàng!');
    }
    </script>
</asp:Content>


