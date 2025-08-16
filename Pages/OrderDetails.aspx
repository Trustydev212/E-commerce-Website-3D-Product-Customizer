<%@ Page Language="C#" MasterPageFile="../Public.Master" AutoEventWireup="true" CodeFile="OrderDetails.aspx.cs" Inherits="OrderDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chi tiết đơn hàng</title>
    <style>
        /* Custom style cho trang chi tiết đơn hàng */
        .order-details-card {
            border-radius: 18px;
            box-shadow: 0 4px 24px rgba(124,58,237,0.10);
            border: none;
            margin-bottom: 2rem;
        }
        .order-details-header {
            background: linear-gradient(90deg, #A78BFA 0%, #7C3AED 100%);
            color: #fff;
            border-radius: 18px 18px 0 0;
            padding: 1.5rem 2rem;
        }
        .order-details-body {
            padding: 2rem;
            font-size: 1.1rem;
        }
        .order-details-label {
            font-weight: 600;
            color: #7C3AED;
            min-width: 160px;
            display: inline-block;
        }
        .order-details-row {
            margin-bottom: 1rem;
        }
        @media (max-width: 600px) {
            .order-details-body { padding: 1rem; }
            .order-details-header { padding: 1rem; font-size: 1.2rem; }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-lg-7 col-md-10">
                <div class="card order-details-card">
                    <div class="order-details-header">
                        <h2 class="mb-0"><i class="fas fa-receipt me-2"></i>Chi tiết đơn hàng</h2>
                    </div>
                    <div class="order-details-body">
                        <asp:Label ID="lblOrderInfo" runat="server" Text="Thông tin đơn hàng sẽ hiển thị ở đây." />
                    </div>
                </div>
                <div class="text-center mt-4">
                    <a href="UserProfile.aspx#orders" class="btn btn-outline-primary">
                        <i class="fas fa-arrow-left me-2"></i>Quay lại danh sách đơn hàng
                    </a>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 