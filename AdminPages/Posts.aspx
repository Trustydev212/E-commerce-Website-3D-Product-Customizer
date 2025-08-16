<%@ Page Title="" Language="C#" MasterPageFile="../Admin.master" AutoEventWireup="true" CodeFile="Posts.aspx.cs" Inherits="AdminPages_Posts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Quản lý bài viết - Admin</title>
    <link href="../Css/admin-pages.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid">
        <div class="admin-page-container">
            <div class="page-header">
                <h2>
                    <div class="header-icon">
                        <i class="fas fa-newspaper"></i>
                    </div>
                    Quản lý bài viết
                </h2>
            </div>
            
            <div class="d-flex justify-content-between align-items-center mb-4">
                <a href="PostEdit.aspx" class="btn btn-success">
                    <i class="fas fa-plus me-2"></i> Thêm bài viết mới
                </a>
            </div>

            <!-- Search and Filter -->
            <div class="card mb-4">
                <div class="card-body">
                    <div class="row align-items-center">
                        <div class="col-md-4">
                            <div class="input-group">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Tìm kiếm bài viết..."></asp:TextBox>
                                <asp:Button ID="btnSearch" runat="server" Text="🔍 Tìm kiếm" CssClass="btn btn-outline-primary" OnClick="btnSearch_Click" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                <asp:ListItem Text="Tất cả danh mục" Value="" />
                                <asp:ListItem Text="Thiết kế" Value="Thiết kế" />
                                <asp:ListItem Text="Xu hướng" Value="Xu hướng" />
                                <asp:ListItem Text="Hướng dẫn" Value="Hướng dẫn" />
                                <asp:ListItem Text="Tin tức" Value="Tin tức" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                <asp:ListItem Text="Tất cả trạng thái" Value="" />
                                <asp:ListItem Text="Đã xuất bản" Value="1" />
                                <asp:ListItem Text="Chưa xuất bản" Value="0" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Button ID="btnClear" runat="server" Text="🗑️ Xóa bộ lọc" CssClass="btn btn-outline-secondary" OnClick="btnClear_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Posts Grid -->
            <div class="card">
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvPosts" runat="server" CssClass="table table-hover" 
                                      AutoGenerateColumns="False" DataKeyNames="Id" 
                                      OnRowCommand="gvPosts_RowCommand" OnRowDataBound="gvPosts_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="Id" ItemStyle-CssClass="order-id" />
                                <asp:TemplateField HeaderText="Ảnh">
                                    <ItemTemplate>
                                        <img src='<%# Eval("ImagePath") ?? "~/Image/placeholder.jpg" %>' alt="Post Image" 
                                             style="width: 60px; height: 40px; object-fit: cover; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Title" HeaderText="Tiêu đề" SortExpression="Title" ItemStyle-CssClass="customer-name" />
                                <asp:BoundField DataField="Category" HeaderText="Danh mục" SortExpression="Category" />
                                <asp:BoundField DataField="Author" HeaderText="Tác giả" SortExpression="Author" />
                                <asp:BoundField DataField="PublishedDate" HeaderText="Ngày xuất bản" SortExpression="PublishedDate" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-CssClass="order-date" />
                                <asp:BoundField DataField="ViewCount" HeaderText="Lượt xem" SortExpression="ViewCount" />
                                <asp:TemplateField HeaderText="Trạng thái">
                                    <ItemTemplate>
                                        <span class='status-badge <%# Convert.ToBoolean(Eval("IsPublished")) ? "status-delivered" : "status-pending" %>'>
                                            <%# Convert.ToBoolean(Eval("IsPublished")) ? "Đã xuất bản" : "Chưa xuất bản" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Thao tác">
                                    <ItemTemplate>
                                        <div class="btn-group" role="group">
                                            <a href='PostEdit.aspx?id=<%# Eval("Id") %>' class="btn btn-sm btn-outline-primary" ToolTip="Chỉnh sửa bài viết">
                                                <i class="fas fa-edit"></i> Sửa
                                            </a>
                                            <asp:LinkButton ID="btnTogglePublish" runat="server" CssClass="btn btn-sm btn-outline-success" 
                                                          CommandName="TogglePublish" CommandArgument='<%# Eval("Id") %>'
                                                          ToolTip='<%# Convert.ToBoolean(Eval("IsPublished")) ? "Ẩn bài viết" : "Xuất bản" %>'>
                                                <i class='<%# Convert.ToBoolean(Eval("IsPublished")) ? "fas fa-eye-slash" : "fas fa-eye" %>'></i>
                                                <%# Convert.ToBoolean(Eval("IsPublished")) ? "Ẩn" : "Xuất bản" %>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-outline-danger" 
                                                          CommandName="DeletePost" CommandArgument='<%# Eval("Id") %>'
                                                          OnClientClick="return confirm('Bạn có chắc muốn xóa bài viết này?');"
                                                          ToolTip="Xóa bài viết">
                                                <i class="fas fa-trash"></i> Xóa
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                    <!-- Pagination -->
                    <div class="d-flex justify-content-between align-items-center mt-3">
                        <div>
                            <asp:Label ID="lblPageInfo" runat="server" Text=""></asp:Label>
                        </div>
                        <div>
                            <asp:Button ID="btnPrevious" runat="server" Text="⬅️ Trước" CssClass="btn btn-outline-primary" OnClick="btnPrevious_Click" />
                            <asp:Button ID="btnNext" runat="server" Text="Sau ➡️" CssClass="btn btn-outline-primary" OnClick="btnNext_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 