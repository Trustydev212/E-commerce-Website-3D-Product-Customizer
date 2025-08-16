<%@ Page Language="C#" MasterPageFile="../Public.Master" AutoEventWireup="true" CodeFile="Blog.aspx.cs" Inherits="Blog" %>
<%@ Register TagPrefix="uc" TagName="ChatBot" Src="../UserControls/ChatBot.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Blog - 3D T-Shirt Design Platform</title>
    <meta name="description" content="Cập nhật tin tức, xu hướng thiết kế và mẹo hay về áo thun 3D" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-12">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb">
                        <li class="breadcrumb-item"><a href="Default.aspx">Trang chủ</a></li>
                        <li class="breadcrumb-item active" aria-current="page">Blog</li>
                    </ol>
                </nav>
                
                <div class="d-flex justify-content-between align-items-center mb-4">
                    <h1 class="h2 mb-0"><i class="fas fa-blog me-2"></i>Blog & Tin tức</h1>
                </div>
                
                <div class="row">
                    <div class="col-lg-9">
                        <!-- Search and Filter -->
                        <div class="card mb-4">
                            <div class="card-body">
                                <div class="row align-items-center">
                                    <div class="col-md-6">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:Button ID="btnSearch" runat="server" Text="Tìm kiếm" CssClass="btn btn-outline-primary" OnClick="btnSearch_Click" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="d-flex justify-content-end">
                                            <asp:DropDownList ID="ddlSortBy" runat="server" CssClass="form-select me-2" AutoPostBack="true" OnSelectedIndexChanged="ddlSortBy_SelectedIndexChanged">
                                                <asp:ListItem Text="Mới nhất" Value="newest" Selected="True" />
                                                <asp:ListItem Text="Cũ nhất" Value="oldest" />
                                                <asp:ListItem Text="Phổ biến" Value="popular" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <!-- Blog Posts -->
                        <div class="row">
                            <asp:Repeater ID="rptPosts" runat="server">
                                <ItemTemplate>
                                    <div class="col-lg-6 col-md-6 mb-4">
                                        <div class="card blog-post-card h-100">
                                            <div class="position-relative">
                                                <img src="<%# Eval("ImagePath") ?? "~/Image/placeholder.jpg" %>" class="card-img-top" alt="<%# Eval("Title") %>" style="height: 200px; object-fit: cover;" />
                                                <div class="position-absolute top-0 end-0 m-2">
                                                    <span class="badge bg-primary"><%# Eval("Category") %></span>
                                                </div>
                                            </div>
                                            <div class="card-body d-flex flex-column">
                                                <h5 class="card-title"><%# Eval("Title") %></h5>
                                                <p class="card-text text-muted"><%# Eval("Summary") %></p>
                                                <div class="mt-auto">
                                                    <div class="d-flex justify-content-between align-items-center">
                                                        <small class="text-muted">
                                                            <i class="fas fa-calendar-alt me-1"></i><%# String.Format("{0:dd/MM/yyyy}", Eval("PublishedDate")) %>
                                                        </small>
                                                        <small class="text-muted">
                                                            <i class="fas fa-eye me-1"></i><%# Eval("ViewCount") %> lượt xem
                                                        </small>
                                                    </div>
                                                    <div class="mt-2">
                                                        <a href="PostDetails.aspx?id=<%# Eval("Id") %>" class="btn btn-primary btn-sm">Xem chi tiết</a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        
                        <!-- Pagination -->
                        <div class="d-flex justify-content-center">
                            <nav aria-label="Page navigation">
                                <ul class="pagination">
                                    <li class="page-item">
                                        <asp:LinkButton ID="lnkPrevious" runat="server" CssClass="page-link" OnClick="lnkPrevious_Click">
                                            <i class="fas fa-chevron-left"></i> Trước
                                        </asp:LinkButton>
                                    </li>
                                    <li class="page-item">
                                        <span class="page-link">Trang <asp:Label ID="lblCurrentPage" runat="server" Text="1" /> / <asp:Label ID="lblTotalPages" runat="server" Text="1" /></span>
                                    </li>
                                    <li class="page-item">
                                        <asp:LinkButton ID="lnkNext" runat="server" CssClass="page-link" OnClick="lnkNext_Click">
                                            Tiếp <i class="fas fa-chevron-right"></i>
                                        </asp:LinkButton>
                                    </li>
                                </ul>
                            </nav>
                        </div>
                        
                        <!-- No Posts Message -->
                        <asp:Panel ID="pnlNoPosts" runat="server" Visible="false" CssClass="text-center py-5">
                            <div class="no-posts">
                                <i class="fas fa-newspaper fa-5x text-muted mb-3"></i>
                                <h3 class="text-muted">Không có bài viết nào</h3>
                                <p class="text-muted">Hiện tại chưa có bài viết nào được đăng</p>
                            </div>
                        </asp:Panel>
                    </div>
                    
                    <div class="col-lg-3">
                        <!-- Recent Posts -->
                        <div class="card mb-4">
                            <div class="card-header">
                                <h5 class="mb-0">Bài viết mới nhất</h5>
                            </div>
                            <div class="card-body">
                                <asp:Repeater ID="rptRecentPosts" runat="server">
                                    <ItemTemplate>
                                        <div class="d-flex mb-3">
                                            <img src="<%# Eval("ImagePath") ?? "~/Image/placeholder.jpg" %>" alt="<%# Eval("Title") %>" class="me-3 rounded" style="width: 50px; height: 50px; object-fit: cover;" />
                                            <div class="flex-grow-1">
                                                <h6 class="mb-1"><a href="PostDetails.aspx?id=<%# Eval("Id") %>" class="text-decoration-none"><%# Eval("Title") %></a></h6>
                                                <small class="text-muted"><%# String.Format("{0:dd/MM/yyyy}", Eval("PublishedDate")) %></small>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        
                        <!-- Popular Tags -->
                        <div class="card mb-4">
                            <div class="card-header">
                                <h5 class="mb-0">Thẻ phổ biến</h5>
                            </div>
                            <div class="card-body">
                                <div class="tag-cloud">
                                    <a href="#" class="badge bg-light text-dark me-1 mb-1">3D Design</a>
                                    <a href="#" class="badge bg-light text-dark me-1 mb-1">T-Shirt</a>
                                    <a href="#" class="badge bg-light text-dark me-1 mb-1">Fashion</a>
                                    <a href="#" class="badge bg-light text-dark me-1 mb-1">Custom</a>
                                    <a href="#" class="badge bg-light text-dark me-1 mb-1">Printing</a>
                                    <a href="#" class="badge bg-light text-dark me-1 mb-1">Trend</a>
                                    <a href="#" class="badge bg-light text-dark me-1 mb-1">Tutorial</a>
                                </div>
                            </div>
                        </div>
                        
                        <!-- Newsletter -->
                        <div class="card">
                            <div class="card-header">
                                <h5 class="mb-0">Theo dõi tin tức</h5>
                            </div>
                            <div class="card-body">
                                <p class="text-muted">Nhận thông báo về bài viết mới nhất</p>
                                <div class="input-group">
                                    <asp:TextBox ID="txtNewsletterEmail" runat="server" CssClass="form-control" TextMode="SingleLine"></asp:TextBox>
                                    <asp:Button ID="btnSubscribe" runat="server" Text="Đăng ký" CssClass="btn btn-primary" OnClick="btnSubscribe_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <style>
        .blog-post-card {
            transition: transform 0.3s ease;
        }
        .blog-post-card:hover {
            transform: translateY(-5px);
        }
        .tag-cloud a:hover {
            background-color: #007bff !important;
            color: white !important;
        }
    </style>
    
    <!-- ChatBot UserControl -->
    <uc:ChatBot runat="server" ID="ChatBot" />
</asp:Content>

