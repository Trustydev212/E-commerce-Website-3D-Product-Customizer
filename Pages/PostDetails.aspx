<%@ Page Language="C#" MasterPageFile="../Public.Master" AutoEventWireup="true" CodeFile="PostDetails.aspx.cs" Inherits="PostDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Chi tiết bài viết - 3D T-Shirt Design Platform</title>
    <meta name="description" content="Đọc chi tiết bài viết về thiết kế áo thun 3D và xu hướng thời trang" />
    <style>
        .blog-post {
            max-width: 800px;
            margin: 0 auto;
        }
        .post-content {
            line-height: 1.8;
            font-size: 1.1rem;
        }
        .post-content img {
            max-width: 100%;
            height: auto;
            border-radius: 8px;
            margin: 20px 0;
        }
        .post-content h2, .post-content h3 {
            margin-top: 30px;
            margin-bottom: 15px;
            color: #333;
        }
        .post-content p {
            margin-bottom: 20px;
        }
        .post-meta {
            background: #f8f9fa;
            padding: 15px;
            border-radius: 8px;
            margin-bottom: 30px;
        }
        .related-posts {
            margin-top: 50px;
            padding-top: 30px;
            border-top: 2px solid #eee;
        }
        .social-share {
            margin: 30px 0;
            padding: 20px;
            background: #f8f9fa;
            border-radius: 8px;
        }
        .social-share a {
            display: inline-block;
            margin: 0 10px;
            padding: 10px 15px;
            border-radius: 5px;
            text-decoration: none;
            color: white;
            font-weight: bold;
        }
        .social-share .facebook { background: #3b5998; }
        .social-share .twitter { background: #1da1f2; }
        .social-share .linkedin { background: #0077b5; }
        .social-share .email { background: #6c757d; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-12">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb">
                        <li class="breadcrumb-item"><a href="Default.aspx">Trang chủ</a></li>
                        <li class="breadcrumb-item"><a href="Blog.aspx">Blog</a></li>
                        <li class="breadcrumb-item active" aria-current="page">Chi tiết bài viết</li>
                    </ol>
                </nav>
            </div>
        </div>
        
        <div class="row">
            <div class="col-lg-8">
                <article class="blog-post">
                    <h1 class="mb-3" id="postTitle" runat="server"></h1>
                    
                    <div class="post-meta">
                        <div class="row">
                            <div class="col-md-6">
                                <i class="far fa-calendar-alt me-2"></i>
                                <span id="postDate" runat="server"></span>
                            </div>
                            <div class="col-md-6">
                                <i class="far fa-user me-2"></i>
                                <span id="postAuthor" runat="server"></span>
                            </div>
                        </div>
                    </div>
                    
                    <div class="mb-4">
                        <img id="postImage" runat="server" class="img-fluid rounded" alt="Post Image" />
                    </div>
                    
                    <div class="post-content">
                        <asp:Literal ID="litContent" runat="server"></asp:Literal>
                    </div>
                    
                    <!-- Social Share -->
                    <div class="social-share">
                        <h5>Chia sẻ bài viết:</h5>
                        <a href="#" class="facebook" onclick="shareOnFacebook()">
                            <i class="fab fa-facebook-f me-2"></i>Facebook
                        </a>
                        <a href="#" class="twitter" onclick="shareOnTwitter()">
                            <i class="fab fa-twitter me-2"></i>Twitter
                        </a>
                        <a href="#" class="linkedin" onclick="shareOnLinkedIn()">
                            <i class="fab fa-linkedin-in me-2"></i>LinkedIn
                        </a>
                        <a href="#" class="email" onclick="shareViaEmail()">
                            <i class="fas fa-envelope me-2"></i>Email
                        </a>
                    </div>
                </article>
                
                <!-- Related Posts -->
                <asp:Panel ID="pnlRelatedPosts" runat="server" CssClass="related-posts">
                    <h3>Bài viết liên quan</h3>
                    <div class="row">
                        <asp:Repeater ID="rptRelatedPosts" runat="server">
                            <ItemTemplate>
                                <div class="col-md-4 mb-3">
                                    <div class="card h-100">
                                        <img src="<%# Eval("ImagePath") ?? "~/Image/placeholder.jpg" %>" class="card-img-top" alt="<%# Eval("Title") %>" style="height: 150px; object-fit: cover;" />
                                        <div class="card-body">
                                            <h6 class="card-title"><%# Eval("Title") %></h6>
                                            <p class="card-text small text-muted"><%# Eval("Summary") %></p>
                                            <a href="PostDetails.aspx?id=<%# Eval("Id") %>" class="btn btn-outline-primary btn-sm">Đọc thêm</a>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>
            </div>
            
            <div class="col-lg-4">
                <!-- Newsletter Signup -->
                <div class="card mb-4">
                    <div class="card-header bg-primary text-white">
                        <h5 class="mb-0"><i class="fas fa-envelope me-2"></i>Đăng ký nhận tin</h5>
                    </div>
                    <div class="card-body">
                        <p class="card-text">Nhận thông báo về bài viết mới và ưu đãi đặc biệt!</p>
                        <div class="input-group">
                            <input type="email" class="form-control" placeholder="Email của bạn" id="newsletterEmail" />
                            <button class="btn btn-primary" type="button" onclick="subscribeNewsletter()">Đăng ký</button>
                        </div>
                    </div>
                </div>
                
                <!-- Popular Posts -->
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fas fa-fire me-2"></i>Bài viết phổ biến</h5>
                    </div>
                    <div class="card-body">
                        <asp:Repeater ID="rptPopularPosts" runat="server">
                            <ItemTemplate>
                                <div class="d-flex mb-3">
                                    <img src="<%# Eval("ImagePath") ?? "~/Image/placeholder.jpg" %>" alt="<%# Eval("Title") %>" class="me-3 rounded" style="width: 60px; height: 60px; object-fit: cover;" />
                                    <div class="flex-grow-1">
                                        <h6 class="mb-1"><a href="PostDetails.aspx?id=<%# Eval("Id") %>" class="text-decoration-none"><%# Eval("Title") %></a></h6>
                                        <small class="text-muted">
                                            <i class="fas fa-eye me-1"></i><%# Eval("ViewCount") %> lượt xem
                                        </small>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function shareOnFacebook() {
            var url = encodeURIComponent(window.location.href);
            var title = encodeURIComponent(document.title);
            window.open('https://www.facebook.com/sharer/sharer.php?u=' + url, '_blank');
        }
        
        function shareOnTwitter() {
            var url = encodeURIComponent(window.location.href);
            var title = encodeURIComponent(document.title);
            window.open('https://twitter.com/intent/tweet?url=' + url + '&text=' + title, '_blank');
        }
        
        function shareOnLinkedIn() {
            var url = encodeURIComponent(window.location.href);
            var title = encodeURIComponent(document.title);
            window.open('https://www.linkedin.com/sharing/share-offsite/?url=' + url, '_blank');
        }
        
        function shareViaEmail() {
            var url = encodeURIComponent(window.location.href);
            var title = encodeURIComponent(document.title);
            window.open('mailto:?subject=' + title + '&body=Kiểm tra bài viết này: ' + url, '_blank');
        }
        
        function subscribeNewsletter() {
            var email = document.getElementById('newsletterEmail').value;
            if (email) {
                // TODO: Implement newsletter subscription
                alert('Cảm ơn bạn đã đăng ký nhận tin!');
                document.getElementById('newsletterEmail').value = '';
            } else {
                alert('Vui lòng nhập email!');
            }
        }
    </script>
</asp:Content>

