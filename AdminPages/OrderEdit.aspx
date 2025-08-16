<%@ Page Title="" Language="C#" MasterPageFile="../Admin.master" AutoEventWireup="true" CodeFile="OrderEdit.aspx.cs" Inherits="AdminPages_OrderEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .design-popup {
            display: none;
            position: fixed;
            z-index: 9999;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            background: linear-gradient(135deg, rgba(0,0,0,0.8) 0%, rgba(30,32,48,0.9) 100%);
            backdrop-filter: blur(8px);
            animation: fadeIn 0.3s ease-out;
        }
        
        @keyframes fadeIn {
            from { opacity: 0; }
            to { opacity: 1; }
        }
        
        .design-popup-content {
            background: linear-gradient(135deg, #ffffff 0%, #f8fafc 50%, #e0e7ff 100%);
            margin: 3% auto;
            padding: 0;
            border-radius: 24px;
            width: 90%;
            max-width: 1000px;
            max-height: 90vh;
            overflow: hidden;
            box-shadow: 
                0 20px 60px rgba(0,0,0,0.15),
                0 8px 32px rgba(99,102,241,0.1),
                0 0 0 1px rgba(255,255,255,0.1);
            border: 1px solid rgba(255,255,255,0.2);
            animation: slideInUp 0.4s cubic-bezier(0.16, 1, 0.3, 1);
            position: relative;
        }
        
        @keyframes slideInUp {
            from { 
                opacity: 0; 
                transform: translateY(60px) scale(0.95);
            }
            to { 
                opacity: 1; 
                transform: translateY(0) scale(1);
            }
        }
        
        .design-popup-header {
            background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%);
            padding: 32px 40px 24px 40px;
            text-align: center;
            position: relative;
            overflow: hidden;
        }
        
        .design-popup-header::before {
            content: '';
            position: absolute;
            top: -50%;
            left: -50%;
            width: 200%;
            height: 200%;
            background: radial-gradient(circle, rgba(255,255,255,0.1) 0%, transparent 70%);
            animation: rotate 20s linear infinite;
        }
        
        @keyframes rotate {
            from { transform: rotate(0deg); }
            to { transform: rotate(360deg); }
        }
        
        .design-popup-header-content {
            position: relative;
            z-index: 2;
        }
        
        .design-popup-icon {
            font-size: 4rem;
            color: #ffffff;
            filter: drop-shadow(0 4px 12px rgba(255,255,255,0.3));
            margin-bottom: 16px;
            animation: pulse 2s ease-in-out infinite;
        }
        
        @keyframes pulse {
            0%, 100% { transform: scale(1); }
            50% { transform: scale(1.05); }
        }
        
        .design-popup-title {
            margin: 0;
            font-weight: 800;
            color: #ffffff;
            font-size: 2rem;
            text-shadow: 0 2px 8px rgba(0,0,0,0.2);
            letter-spacing: -0.5px;
        }
        
        .design-popup-close {
            position: absolute;
            top: 24px;
            right: 32px;
            width: 40px;
            height: 40px;
            background: rgba(255,255,255,0.2);
            border: 1px solid rgba(255,255,255,0.3);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 20px;
            font-weight: bold;
            color: #ffffff;
            cursor: pointer;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            backdrop-filter: blur(10px);
            z-index: 10;
        }
        
        .design-popup-close:hover {
            background: rgba(255,255,255,0.3);
            transform: scale(1.1) rotate(90deg);
            box-shadow: 0 4px 16px rgba(0,0,0,0.2);
        }
        
        .design-popup-body {
            padding: 40px;
        }
        
        .design-info-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 32px;
            margin-bottom: 32px;
        }
        
        .design-section {
            background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%);
            padding: 28px;
            border-radius: 20px;
            border: 1px solid #e0e7ff;
            box-shadow: 
                0 4px 16px rgba(99,102,241,0.08),
                0 1px 4px rgba(0,0,0,0.05);
            transition: all 0.3s ease;
            position: relative;
            overflow: hidden;
        }
        
        .design-section::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            height: 4px;
            background: linear-gradient(90deg, #6366f1, #8b5cf6, #ec4899);
            border-radius: 20px 20px 0 0;
        }
        
        .design-section:hover {
            transform: translateY(-2px);
            box-shadow: 
                0 8px 24px rgba(99,102,241,0.12),
                0 2px 8px rgba(0,0,0,0.08);
        }
        
        .design-section h6 {
            color: #1e293b;
            font-weight: 700;
            font-size: 1.2rem;
            margin-bottom: 20px;
            display: flex;
            align-items: center;
            gap: 12px;
        }
        
        .design-section h6 i {
            color: #6366f1;
            font-size: 1.1em;
        }
        
        .design-image {
            max-width: 100%;
            max-height: 220px;
            border: 2px solid #e0e7ff;
            border-radius: 16px;
            box-shadow: 
                0 8px 24px rgba(99,102,241,0.15),
                0 2px 8px rgba(0,0,0,0.1);
            object-fit: cover;
            margin: 12px 0;
            transition: all 0.3s ease;
        }
        
        .design-image:hover {
            transform: scale(1.02);
            box-shadow: 
                0 12px 32px rgba(99,102,241,0.2),
                0 4px 12px rgba(0,0,0,0.15);
        }
        
        .design-data {
            background: linear-gradient(135deg, #f8fafc 0%, #ffffff 100%);
            padding: 16px;
            border-radius: 12px;
            font-size: 0.9rem;
            max-height: 220px;
            overflow: auto;
            border: 1px solid #e0e7ff;
            white-space: pre-wrap;
            word-wrap: break-word;
            color: #374151;
            font-family: 'Monaco', 'Menlo', 'Ubuntu Mono', monospace;
            line-height: 1.5;
            box-shadow: inset 0 2px 4px rgba(0,0,0,0.05);
        }
        
        .design-data::-webkit-scrollbar {
            width: 6px;
        }
        
        .design-data::-webkit-scrollbar-track {
            background: #f1f5f9;
            border-radius: 3px;
        }
        
        .design-data::-webkit-scrollbar-thumb {
            background: #cbd5e1;
            border-radius: 3px;
        }
        
        .design-data::-webkit-scrollbar-thumb:hover {
            background: #94a3b8;
        }
        
        .design-badge {
            background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%);
            color: white;
            padding: 6px 12px;
            border-radius: 20px;
            font-size: 0.9rem;
            font-weight: 600;
            box-shadow: 0 2px 8px rgba(99,102,241,0.3);
            border: 1px solid rgba(255,255,255,0.2);
        }
        
        .design-meta {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 12px;
            padding: 8px 0;
            border-bottom: 1px solid #f1f5f9;
        }
        
        .design-meta:last-child {
            border-bottom: none;
        }
        
        .design-meta span:first-child {
            font-weight: 600;
            color: #1e293b;
            font-size: 0.95rem;
        }
        
        .design-meta span:last-child {
            color: #64748b;
            font-size: 0.9rem;
        }
        
        .design-popup-footer {
            background: linear-gradient(135deg, #f8fafc 0%, #ffffff 100%);
            padding: 24px 40px;
            text-align: center;
            border-top: 1px solid #e0e7ff;
        }
        
        .design-popup-btn {
            background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%);
            color: white;
            border: none;
            padding: 14px 40px;
            font-size: 1.1rem;
            border-radius: 50px;
            font-weight: 600;
            box-shadow: 
                0 4px 16px rgba(99,102,241,0.3),
                0 2px 8px rgba(0,0,0,0.1);
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            cursor: pointer;
            display: inline-flex;
            align-items: center;
            gap: 8px;
        }
        
        .design-popup-btn:hover {
            transform: translateY(-2px);
            box-shadow: 
                0 8px 24px rgba(99,102,241,0.4),
                0 4px 12px rgba(0,0,0,0.15);
        }
        
        .design-popup-btn:active {
            transform: translateY(0);
        }
        
        .design-empty-state {
            color: #64748b;
            font-size: 0.95rem;
            text-align: center;
            padding: 40px 20px;
            background: linear-gradient(135deg, #f8fafc 0%, #f1f5f9 100%);
            border-radius: 12px;
            border: 2px dashed #cbd5e1;
            margin: 12px 0;
        }
        
        .design-error-state {
            color: #ef4444;
            font-size: 0.9rem;
            text-align: center;
            padding: 12px;
            background: #fef2f2;
            border: 1px solid #fecaca;
            border-radius: 8px;
            margin: 8px 0;
        }
        
        @media (max-width: 768px) {
            .design-info-grid {
                grid-template-columns: 1fr;
                gap: 24px;
            }
            
            .design-popup-content {
                margin: 5% auto;
                width: 95%;
                max-height: 95vh;
            }
            
            .design-popup-header {
                padding: 24px 20px 20px 20px;
            }
            
            .design-popup-body {
                padding: 24px 20px;
            }
            
            .design-popup-footer {
                padding: 20px;
            }
            
            .design-popup-title {
                font-size: 1.5rem;
            }
            
            .design-popup-icon {
                font-size: 3rem;
            }
            
            .design-section {
                padding: 20px;
            }
        }
        
        @media (max-width: 480px) {
            .design-popup-content {
                margin: 2% auto;
                width: 98%;
            }
            
            .design-popup-header {
                padding: 20px 16px 16px 16px;
            }
            
            .design-popup-body {
                padding: 20px 16px;
            }
            
            .design-popup-title {
                font-size: 1.3rem;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <h2>Chi tiết đơn hàng #<asp:Label ID="lblOrderId" runat="server" Text="" /></h2>
                
                <div class="row">
                    <div class="col-md-8">
                        <!-- Order Items -->
                        <div class="card mb-4">
                            <div class="card-header">
                                <h5>Sản phẩm trong đơn hàng</h5>
                            </div>
                            <div class="card-body">
                                <asp:Repeater ID="rptOrderItems" runat="server">
                                    <ItemTemplate>
                                        <div class="row align-items-center mb-3 pb-3 border-bottom">
                                            <div class="col-md-2">
                                                <img src='<%# GetImageUrl(Eval("ProductImage")) %>' alt="Product" class="img-fluid rounded" style="max-height: 80px;" />
                                            </div>
                                            <div class="col-md-6">
                                                <h6><%# Eval("ProductName") %></h6>
                                                <p class="text-muted mb-1">
                                                    <small>
                                                        Size: <%# Eval("Size") %> | Màu: <%# Eval("Color") %><%# Eval("CustomDesignPath") != null && !string.IsNullOrEmpty(Eval("CustomDesignPath").ToString()) ? " | Thiết kế tùy chỉnh" : "" %>
                                                    </small>
                                                </p>
                                                <%# Eval("DesignId") != null && Eval("DesignId") != DBNull.Value ? 
                                                    "<button type='button' class='btn btn-sm btn-outline-primary' onclick='showDesignPopup(\"" + 
                                                    Eval("DesignId") + "\", \"" + 
                                                    (string.IsNullOrEmpty(Eval("DesignName").ToString()) ? "Thiết kế" : Eval("DesignName").ToString()) + "\", \"" + 
                                                    (string.IsNullOrEmpty(Eval("DesignLogoPath").ToString()) ? "" : Eval("DesignLogoPath").ToString()) + "\", \"" + 
                                                    (string.IsNullOrEmpty(Eval("DesignPreviewPath").ToString()) ? "" : Eval("DesignPreviewPath").ToString()) + "\", \"" + 
                                                    (string.IsNullOrEmpty(Eval("DesignPositionData").ToString()) ? "" : HttpUtility.JavaScriptStringEncode(Eval("DesignPositionData").ToString())) + "\", \"" + 
                                                    (Eval("DesignCreatedAt") != DBNull.Value ? Convert.ToDateTime(Eval("DesignCreatedAt")).ToString("dd/MM/yyyy HH:mm") : "N/A") + "\")'>" +
                                                    "<i class='fas fa-eye'></i> Xem thiết kế</button>" : "" %>
                                            </div>
                                            <div class="col-md-2 text-center">
                                                <strong><%# Eval("Quantity") %></strong>
                                            </div>
                                            <div class="col-md-2 text-end">
                                                <strong><%# String.Format("{0:C}", Eval("Price")) %></strong>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        
                        <!-- Order History -->
                        <div class="card">
                            <div class="card-header">
                                <h5>Lịch sử đơn hàng</h5>
                            </div>
                            <div class="card-body">
                                <div class="timeline">
                                    <div class="timeline-item">
                                        <div class="timeline-marker bg-primary"></div>
                                        <div class="timeline-content">
                                            <h6>Đơn hàng được tạo</h6>
                                            <p class="text-muted">
                                                <asp:Label ID="lblCreatedDate" runat="server" Text="" />
                                            </p>
                                        </div>
                                    </div>
                                    <div class="timeline-item">
                                        <div class="timeline-marker bg-warning"></div>
                                        <div class="timeline-content">
                                            <h6>Đang xử lý</h6>
                                            <p class="text-muted">Đơn hàng đang được xử lý</p>
                                        </div>
                                    </div>
                                    <div class="timeline-item">
                                        <div class="timeline-marker bg-info"></div>
                                        <div class="timeline-content">
                                            <h6>Đang giao hàng</h6>
                                            <p class="text-muted">Đơn hàng đang được giao</p>
                                        </div>
                                    </div>
                                    <div class="timeline-item">
                                        <div class="timeline-marker bg-success"></div>
                                        <div class="timeline-content">
                                            <h6>Hoàn thành</h6>
                                            <p class="text-muted">Đơn hàng đã được giao thành công</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <div class="col-md-4">
                        <!-- Order Status -->
                        <div class="card mb-4">
                            <div class="card-header">
                                <h5>Trạng thái đơn hàng</h5>
                            </div>
                            <div class="card-body">
                                <div class="mb-3">
                                    <label class="form-label">Trạng thái</label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="Chờ xử lý" Value="Pending" />
                                        <asp:ListItem Text="Đang xử lý" Value="Processing" />
                                        <asp:ListItem Text="Đang giao hàng" Value="Shipping" />
                                        <asp:ListItem Text="Hoàn thành" Value="Completed" />
                                        <asp:ListItem Text="Đã hủy" Value="Cancelled" />
                                    </asp:DropDownList>
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Trạng thái thanh toán</label>
                                    <asp:DropDownList ID="ddlPaymentStatus" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="Chưa thanh toán" Value="Pending" />
                                        <asp:ListItem Text="Đã thanh toán" Value="Paid" />
                                        <asp:ListItem Text="Hoàn tiền" Value="Refunded" />
                                    </asp:DropDownList>
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Mã vận đơn</label>
                                    <asp:TextBox ID="txtTrackingNumber" runat="server" CssClass="form-control" />
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Ghi chú</label>
                                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                                </div>
                                
                                <asp:Button ID="btnUpdateStatus" runat="server" CssClass="btn btn-primary w-100" Text="Cập nhật trạng thái" OnClick="btnUpdateStatus_Click" />
                            </div>
                        </div>
                        
                        <!-- Customer Info -->
                        <div class="card mb-4">
                            <div class="card-header">
                                <h5>Thông tin khách hàng</h5>
                            </div>
                            <div class="card-body">
                                <p><strong>Tên:</strong> <asp:Label ID="lblCustomerName" runat="server" Text="" /></p>
                                <p><strong>Email:</strong> <asp:Label ID="lblCustomerEmail" runat="server" Text="" /></p>
                                <p><strong>Điện thoại:</strong> <asp:Label ID="lblCustomerPhone" runat="server" Text="" /></p>
                            </div>
                        </div>
                        
                        <!-- Shipping Info -->
                        <div class="card mb-4">
                            <div class="card-header">
                                <h5>Địa chỉ giao hàng</h5>
                            </div>
                            <div class="card-body">
                                <asp:Label ID="lblShippingAddress" runat="server" Text="" />
                            </div>
                        </div>
                        
                        <!-- Order Summary -->
                        <div class="card">
                            <div class="card-header">
                                <h5>Tổng kết đơn hàng</h5>
                            </div>
                            <div class="card-body">
                                <div class="d-flex justify-content-between mb-2">
                                    <span>Tạm tính:</span>
                                    <span><asp:Label ID="lblSubtotal" runat="server" Text="" /></span>
                                </div>
                                <div class="d-flex justify-content-between mb-2">
                                    <span>Phí vận chuyển:</span>
                                    <span><asp:Label ID="lblShippingFee" runat="server" Text="30,000đ" /></span>
                                </div>
                                <div class="d-flex justify-content-between mb-2">
                                    <span>Thuế VAT:</span>
                                    <span><asp:Label ID="lblTax" runat="server" Text="" /></span>
                                </div>
                                <hr />
                                <div class="d-flex justify-content-between">
                                    <strong>Tổng cộng:</strong>
                                    <strong><asp:Label ID="lblTotal" runat="server" Text="" /></strong>
                                </div>
                                <div class="mt-2">
                                    <small class="text-muted">Phương thức: <asp:Label ID="lblPaymentMethod" runat="server" Text="" /></small>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="row mt-4">
                    <div class="col-md-12">
                        <asp:Button ID="btnBack" runat="server" CssClass="btn btn-secondary" Text="Quay lại" OnClick="btnBack_Click" />
                        <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-outline-primary ms-2" Text="In đơn hàng" OnClientClick="window.print(); return false;" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <!-- Design Popup Modal -->
    <div id="designPopup" class="design-popup">
        <div class="design-popup-content">
            <div class="design-popup-header">
                <div class="design-popup-close" onclick="closeDesignPopup()">
                    <i class="fas fa-times"></i>
                </div>
                <div class="design-popup-header-content">
                    <div class="design-popup-icon">
                        <i class="fas fa-palette"></i>
                    </div>
                    <h3 class="design-popup-title" id="designPopupTitle">Thông tin thiết kế</h3>
                </div>
            </div>
            
            <div class="design-popup-body">
                <div class="design-info-grid">
                    <div class="design-section">
                        <h6><i class="fas fa-image"></i> Ảnh thiết kế</h6>
                        <div class="design-meta">
                            <span>Logo:</span>
                            <span id="designLogoInfo">Đang tải...</span>
                        </div>
                        <div id="designLogoContainer"></div>
                        
                        <div class="design-meta">
                            <span>Preview:</span>
                            <span id="designPreviewInfo">Đang tải...</span>
                        </div>
                        <div id="designPreviewContainer"></div>
                    </div>
                    
                    <div class="design-section">
                        <h6><i class="fas fa-cogs"></i> Thông số decal</h6>
                        <div class="design-meta">
                            <span>ID:</span>
                            <span class="design-badge" id="designIdBadge">-</span>
                        </div>
                        <div class="design-meta">
                            <span>Ngày tạo:</span>
                            <span id="designCreatedAt">-</span>
                        </div>
                        <div style="margin-top: 20px;">
                            <span style="font-weight:600;color:#1e293b;display:block;margin-bottom:12px;">Dữ liệu vị trí:</span>
                            <div class="design-data" id="designPositionData">Đang tải...</div>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="design-popup-footer">
                <button class="design-popup-btn" onclick="closeDesignPopup()">
                    <i class="fas fa-check"></i>
                    Đóng
                </button>
            </div>
        </div>
    </div>
    
    <style>
        .timeline {
            position: relative;
            padding-left: 30px;
        }
        
        .timeline::before {
            content: '';
            position: absolute;
            left: 15px;
            top: 0;
            bottom: 0;
            width: 2px;
            background: #e9ecef;
        }
        
        .timeline-item {
            position: relative;
            margin-bottom: 30px;
        }
        
        .timeline-marker {
            position: absolute;
            left: -23px;
            width: 16px;
            height: 16px;
            border-radius: 50%;
            border: 2px solid #fff;
            box-shadow: 0 0 0 2px #e9ecef;
        }
        
        .timeline-content h6 {
            margin-bottom: 5px;
            color: #333;
        }
        
        .timeline-content p {
            margin-bottom: 0;
            font-size: 0.9rem;
        }
    </style>
    
    <script>
        function showDesignPopup(designId, designName, logoPath, previewPath, positionData, createdAt) {
            // Set popup title
            document.getElementById('designPopupTitle').textContent = designName || 'Thông tin thiết kế';
            
            // Set design ID
            document.getElementById('designIdBadge').textContent = designId;
            
            // Set creation date
            document.getElementById('designCreatedAt').textContent = createdAt;
            
            // Handle logo image
            const logoContainer = document.getElementById('designLogoContainer');
            const logoInfo = document.getElementById('designLogoInfo');
            
            if (logoPath && logoPath.trim() !== '') {
                if (logoPath.startsWith('data:image/')) {
                    // Base64 image
                    logoContainer.innerHTML = '<img src="' + logoPath + '" class="design-image" alt="Logo" />';
                    logoInfo.textContent = 'Base64 Image';
                } else {
                    // File path
                    logoContainer.innerHTML = '<img src="' + logoPath + '" class="design-image" alt="Logo" onerror="this.style.display=\'none\'; this.nextElementSibling.style.display=\'block\';" /><div class="design-error-state" style="display:none;">Không thể tải ảnh</div>';
                    logoInfo.textContent = 'File Image';
                }
            } else {
                logoContainer.innerHTML = '<div class="design-empty-state"><i class="fas fa-image" style="font-size:2rem;color:#cbd5e1;margin-bottom:8px;"></i><br>Không có logo</div>';
                logoInfo.textContent = 'Không có';
            }
            
            // Handle preview image
            const previewContainer = document.getElementById('designPreviewContainer');
            const previewInfo = document.getElementById('designPreviewInfo');
            
            if (previewPath && previewPath.trim() !== '') {
                if (previewPath.startsWith('data:image/')) {
                    // Base64 image
                    previewContainer.innerHTML = '<img src="' + previewPath + '" class="design-image" alt="Preview" />';
                    previewInfo.textContent = 'Base64 Image';
                } else {
                    // File path
                    previewContainer.innerHTML = '<img src="' + previewPath + '" class="design-image" alt="Preview" onerror="this.style.display=\'none\'; this.nextElementSibling.style.display=\'block\';" /><div class="design-error-state" style="display:none;">Không thể tải ảnh</div>';
                    previewInfo.textContent = 'File Image';
                }
            } else {
                previewContainer.innerHTML = '<div class="design-empty-state"><i class="fas fa-eye" style="font-size:2rem;color:#cbd5e1;margin-bottom:8px;"></i><br>Không có preview</div>';
                previewInfo.textContent = 'Không có';
            }
            
            // Handle position data
            const positionDataElement = document.getElementById('designPositionData');
            if (positionData && positionData.trim() !== '') {
                try {
                    // Try to parse and format JSON
                    const parsedData = JSON.parse(positionData);
                    positionDataElement.textContent = JSON.stringify(parsedData, null, 2);
                } catch (e) {
                    // If not valid JSON, display as is
                    positionDataElement.textContent = positionData;
                }
            } else {
                positionDataElement.textContent = 'Không có dữ liệu vị trí';
            }
            
            // Show popup
            document.getElementById('designPopup').style.display = 'block';
        }
        
        function closeDesignPopup() {
            document.getElementById('designPopup').style.display = 'none';
        }
        
        // Close popup when clicking outside
        window.onclick = function(event) {
            const popup = document.getElementById('designPopup');
            if (event.target === popup) {
                closeDesignPopup();
            }
        }
        
        // Close popup with Escape key
        document.addEventListener('keydown', function(event) {
            if (event.key === 'Escape') {
                closeDesignPopup();
            }
        });
    </script>
</asp:Content>