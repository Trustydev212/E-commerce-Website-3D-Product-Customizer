<%@ Page Title="" Language="C#" MasterPageFile="../Admin.master" AutoEventWireup="true" CodeFile="PostEdit.aspx.cs" Inherits="AdminPages_PostEdit" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Chỉnh sửa bài viết - Admin</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <h2><asp:Label ID="lblTitle" runat="server" Text="Thêm bài viết mới" /></h2>
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-8">
                                <div class="mb-3">
                                    <label class="form-label">Tiêu đề *</label>
                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" ErrorMessage="Vui lòng nhập tiêu đề" CssClass="text-danger" />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Tóm tắt</label>
                                    <asp:TextBox ID="txtSummary" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Nội dung *</label>
                                    <asp:TextBox ID="txtContent" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="8" />
                                    <asp:RequiredFieldValidator ID="rfvContent" runat="server" ControlToValidate="txtContent" ErrorMessage="Vui lòng nhập nội dung" CssClass="text-danger" />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Danh mục</label>
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="Chọn danh mục" Value="" />
                                        <asp:ListItem Text="Thiết kế" Value="Thiết kế" />
                                        <asp:ListItem Text="Xu hướng" Value="Xu hướng" />
                                        <asp:ListItem Text="Hướng dẫn" Value="Hướng dẫn" />
                                        <asp:ListItem Text="Tin tức" Value="Tin tức" />
                                    </asp:DropDownList>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Tác giả</label>
                                    <asp:TextBox ID="txtAuthor" runat="server" CssClass="form-control" />
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="mb-3">
                                            <label class="form-label">Ngày xuất bản</label>
                                            <asp:TextBox ID="txtPublishedDate" runat="server" CssClass="form-control" />
                                            <asp:RegularExpressionValidator ID="revPublishedDate" runat="server" ControlToValidate="txtPublishedDate" ValidationExpression="^\d{4}-\d{2}-\d{2}$" ErrorMessage="Định dạng: yyyy-MM-dd" CssClass="text-danger" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="mb-3">
                                            <label class="form-label">Trạng thái</label>
                                            <asp:CheckBox ID="cbIsPublished" runat="server" CssClass="form-check-input" />
                                            <span class="form-check-label ms-2">Đã xuất bản</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="mb-3">
                                    <label class="form-label">Ảnh đại diện</label>
                                    <asp:FileUpload ID="fuImage" runat="server" CssClass="form-control" />
                                    <asp:Label ID="lblCurrentImage" runat="server" CssClass="form-text" />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Xem trước ảnh</label>
                                    <div class="image-preview border p-3 text-center">
                                        <asp:Image ID="imgPreview" runat="server" CssClass="img-thumbnail" Width="200" Height="120" />
                                        <asp:Label ID="lblNoImage" runat="server" Text="Chưa có ảnh" CssClass="text-muted" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="mb-3">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Lưu" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-secondary ms-2" Text="Hủy" OnClick="btnCancel_Click" CausesValidation="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 