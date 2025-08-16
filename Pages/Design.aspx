<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Design.aspx.cs" Inherits="Pages_Design" %>
<%@ Register Src="~/UserControls/Header.ascx" TagPrefix="uc" TagName="Header" %>

<link href="../Css/site.css" rel="stylesheet" />
<link href="../Css/design.css" rel="stylesheet" />
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
<link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />

<script src="https://cdnjs.cloudflare.com/ajax/libs/three.js/r128/three.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/three@0.128.0/examples/js/loaders/GLTFLoader.js"></script>
<script src="https://cdn.jsdelivr.net/npm/three@0.128.0/examples/js/controls/OrbitControls.js"></script>
<script src="https://cdn.jsdelivr.net/npm/three@0.128.0/examples/js/geometries/DecalGeometry.js"></script>
<script src="../JS/design.js"></script>

<form id="form1" runat="server">
    <uc:Header runat="server" ID="Header1" />

    <div class="main-layout">
        <div class="design-sidebar-popup">
            <div class="sidebar-section">
                <h3><i class="fas fa-magic"></i> Thiết kế của bạn</h3>
                <div id="drop-area" class="drop-area">
                    <label for="fileElem" class="drop-label">
                        <span class="drop-icon">🖼️</span><br />
                        Kéo & thả ảnh vào đây hoặc <span class="drop-upload">chọn file</span>
                    </label>
                    <asp:FileUpload ID="fuLogo" runat="server" CssClass="form-control mb-3" Style="display:none;" />
                    <input type="file" id="fileElem" accept="image/*" style="display:none;" />
                    <div id="previewThumb" class="preview-thumb" style="display:none;"></div>
                </div>
                <asp:Button ID="btnUpload" runat="server" Text="Tải logo lên" CssClass="btn btn-primary w-100 mb-2" OnClick="btnUpload_Click" />
                <asp:Label ID="lblUploadStatus" runat="server" CssClass="text-success" />
                
                <!-- Nút tạo ảnh test -->
                <button type="button" id="btnCreateTest" class="btn btn-outline-warning btn-sm w-100 mt-2">
                    <i class="fas fa-flask"></i> Tạo ảnh test
                </button>
                
                <!-- Controls để điều chỉnh ảnh -->
                <div id="imageControls" class="image-controls" style="display:none;">
                    <h4><i class="fas fa-sliders-h"></i> Điều chỉnh ảnh</h4>
                    
                    <!-- Kích thước -->
                    <div class="control-group">
                        <label>Kích thước:</label>
                        <input type="range" id="sizeSlider" min="0.1" max="3" step="0.1" value="1" class="form-range" />
                        <span id="sizeValue">100%</span>
                    </div>
                    
                    <!-- Vị trí X -->
                    <div class="control-group">
                        <label>Vị trí ngang:</label>
                        <input type="range" id="posXSlider" min="-2" max="2" step="0.1" value="0" class="form-range" />
                        <span id="posXValue">0</span>
                    </div>
                    
                    <!-- Vị trí Y -->
                    <div class="control-group">
                        <label>Vị trí dọc:</label>
                        <input type="range" id="posYSlider" min="-2" max="2" step="0.1" value="0" class="form-range" />
                        <span id="posYValue">0</span>
                    </div>
                    
                    <!-- Xoay -->
                    <div class="control-group">
                        <label>Xoay:</label>
                        <input type="range" id="rotationSlider" min="0" max="360" step="5" value="0" class="form-range" />
                        <span id="rotationValue">0°</span>
                    </div>
                    
                    <!-- Nút reset -->
                    <button type="button" id="btnResetImage" class="btn btn-outline-secondary btn-sm w-100 mt-2">
                        <i class="fas fa-undo"></i> Đặt lại
                    </button>
                    
                    <!-- Nút test để thử các vị trí khác nhau -->
                    <button type="button" id="btnTestImage" class="btn btn-outline-info btn-sm w-100 mt-2">
                        <i class="fas fa-search"></i> Test vị trí
                    </button>

                    <!-- Workflow Instructions -->
                    <div class="workflow-info mt-3 p-3 bg-light rounded">
                        <h6><i class="fas fa-info-circle"></i> Hướng dẫn sử dụng:</h6>
                        <ol class="small">
                            <li><strong>Click vào áo</strong> để đặt logo tại vị trí mong muốn</li>
                            <li><strong>Điều chỉnh</strong> kích thước, vị trí, xoay bằng thanh trượt</li>
                            <li><strong>Preview UV</strong> để xem vùng in an toàn</li>
                            <li><strong>Export PNG</strong> để xuất file in áo</li>
                        </ol>
                        <div class="alert alert-info mt-2 small">
                            <strong>💡 Lưu ý:</strong><br>
                            • <strong>3D Preview:</strong> Logo luôn hiển thị đầy đủ, không bị cắt<br>
                            • <strong>Export in:</strong> Logo có thể bị cắt nếu nằm ngoài vùng UV island<br>
                            • Dùng "Xem UV Layout" để kiểm tra vùng in an toàn
                        </div>
                    </div>

                    <!-- Export Section -->
                    <div class="export-section mt-3">
                        <h6><i class="fas fa-download"></i> Xuất file in:</h6>
                        <button type="button" id="btnPreviewUV" class="btn btn-info btn-sm w-100 mb-2">
                            <i class="fas fa-eye"></i> Xem UV Layout
                        </button>
                        <button type="button" id="btnExportPNG" class="btn btn-success btn-sm w-100">
                            <i class="fas fa-file-image"></i> Export Logo PNG
                        </button>
                    </div>
                </div>
            </div>
            <div class="design-actions">
                <asp:Button ID="btnCheckout" runat="server" Text="Thanh toán" CssClass="btn btn-primary" />
                <button type="button" id="btnAISuggest" class="btn btn-cta" onclick="openAIPopup()"><i class="fas fa-robot"></i></button>
            </div>
            <asp:Label ID="lblDesignStatus" runat="server" CssClass="text-info mt-2" />
        </div>
        <div id="modelViewer" class="mockup-3d-placeholder model-viewer"></div>
    </div>
    
    <!-- AI Suggestion Popup -->
    <div id="aiPopup" class="ai-popup" style="display:none;">
        <div class="ai-popup-content">
            <button type="button" class="ai-popup-close" onclick="closeAIPopup()">&times;</button>
            <h4><i class="fas fa-robot"></i> Gợi ý thiết kế với AI</h4>
            <input type="text" id="aiPrompt" class="form-control mb-2" placeholder="Nhập ý tưởng thiết kế (ví dụ: mèo cute, hoa văn Nhật Bản...)" />
            <button type="button" class="btn btn-primary mb-2" onclick="suggestAI()">Tạo gợi ý</button>
            <div id="aiPreview" class="ai-preview"></div>
        </div>
    </div>
    
    <asp:HiddenField ID="hdnModelPath" runat="server" />
    <asp:HiddenField ID="hdnProductId" runat="server" />
    <asp:HiddenField ID="hdnSize" runat="server" />
    <asp:HiddenField ID="hdnColor" runat="server" />
    <asp:HiddenField ID="hdnQuantity" runat="server" />
</form> 