<%@ Page Title="" Language="C#" MasterPageFile="../Admin.master" AutoEventWireup="true" CodeFile="CategoryEdit.aspx.cs" Inherits="AdminPages_CategoryEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <h2><asp:Label ID="lblTitle" runat="server" Text="Thêm danh mục mới" /></h2>
                
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label class="form-label">Tên danh mục *</label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" 
                                                              ErrorMessage="Vui lòng nhập tên danh mục" CssClass="text-danger" />
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Mô tả</label>
                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Thứ tự hiển thị</label>
                                    <asp:TextBox ID="txtDisplayOrder" runat="server" CssClass="form-control" Text="1" />
                                    <asp:CompareValidator ID="cvDisplayOrder" runat="server" ControlToValidate="txtDisplayOrder" 
                                                        Type="Integer" Operator="DataTypeCheck" ErrorMessage="Thứ tự phải là số nguyên" CssClass="text-danger" />
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Hình ảnh</label>
                                    <asp:FileUpload ID="fuImage" runat="server" CssClass="form-control" />
                                    <asp:Label ID="lblCurrentImage" runat="server" CssClass="form-text" />
                                </div>
                                
                                <div class="mb-3">
                                    <div class="form-check">
                                        <asp:CheckBox ID="cbIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                                        <label class="form-check-label">Kích hoạt</label>
                                    </div>
                                </div>
                                
                                <div class="mb-3">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Lưu" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-secondary ms-2" Text="Hủy" OnClick="btnCancel_Click" CausesValidation="false" />
                                </div>
                            </div>
                            
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label class="form-label">Xem trước hình ảnh</label>
                                    <div class="image-preview border p-3 text-center">
                                        <asp:Image ID="imgPreview" runat="server" CssClass="img-thumbnail" Width="200" Height="200" />
                                        <asp:Label ID="lblNoImage" runat="server" Text="Chưa có hình ảnh" CssClass="text-muted" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>