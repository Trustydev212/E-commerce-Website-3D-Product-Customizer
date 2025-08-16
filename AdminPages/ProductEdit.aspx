<%@ Page Title="" Language="C#" MasterPageFile="../Admin.master" AutoEventWireup="true" CodeFile="ProductEdit.aspx.cs" Inherits="AdminPages_ProductEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .image-upload-section {
            background: #f8f9fa;
            border-radius: 10px;
            padding: 20px;
            margin-bottom: 20px;
        }
        
        .color-tabs {
            margin-bottom: 20px;
        }
        
        .color-tab {
            padding: 10px 20px;
            margin-right: 10px;
            border: 2px solid #ddd;
            border-radius: 25px;
            cursor: pointer;
            transition: all 0.3s ease;
            display: inline-block;
        }
        
        .color-tab.active {
            border-color: #007bff !important;
            background: #007bff !important;
            color: white !important;
            transform: scale(1.05);
        }
        
        .color-tab:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        
        .image-preview-container {
            border: 2px dashed #ddd;
            border-radius: 10px;
            padding: 15px;
            text-align: center;
            background: white;
            margin-bottom: 15px;
        }
        
        .main-image-preview {
            height: 400px;
            object-fit: cover;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }
        
        .thumbnail-preview {
            height: 80px;
            object-fit: cover;
            border-radius: 8px;
            margin: 5px;
            cursor: pointer;
            transition: all 0.3s ease;
        }
        
        .thumbnail-preview:hover {
            transform: scale(1.05);
            box-shadow: 0 4px 8px rgba(0,0,0,0.2);
        }
        
        .gallery-preview {
            height: 200px;
            object-fit: cover;
            border-radius: 10px;
            margin: 5px;
            cursor: pointer;
            transition: all 0.3s ease;
        }
        
        .gallery-preview:hover {
            transform: scale(1.02);
            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        }
        
        .upload-btn {
            background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
            color: white;
            border: none;
            padding: 8px 16px;
            border-radius: 20px;
            cursor: pointer;
            transition: all 0.3s ease;
            margin: 5px;
        }
        
        .upload-btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0,123,255,0.3);
        }
        
        .remove-btn {
            background: #dc3545;
            color: white;
            border: none;
            padding: 4px 8px;
            border-radius: 50%;
            cursor: pointer;
            position: absolute;
            top: -5px;
            right: -5px;
            font-size: 12px;
        }
        
        .image-container {
            position: relative;
            display: inline-block;
        }

        .color-status {
            display: flex;
            align-items: center;
            gap: 8px;
            padding: 8px;
            border-radius: 8px;
            background: #f8f9fa;
            margin-bottom: 8px;
        }

        .color-indicator {
            width: 20px;
            height: 20px;
            border-radius: 50%;
            border: 2px solid #ddd;
        }

        .status-text {
            font-size: 12px;
            font-weight: 500;
        }

        .status {
            color: #dc3545;
            font-weight: 600;
        }

        .status.has-image {
            color: #28a745;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <h2><asp:Label ID="lblTitle" runat="server" Text="Thêm sản phẩm mới" /></h2>
                
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label class="form-label">Tên sản phẩm *</label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" 
                                                              ErrorMessage="Vui lòng nhập tên sản phẩm" CssClass="text-danger" />
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Mô tả</label>
                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Danh mục *</label>
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="Chọn danh mục" Value="" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="ddlCategory" 
                                                              ErrorMessage="Vui lòng chọn danh mục" CssClass="text-danger" />
                                </div>
                                
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="mb-3">
                                            <label class="form-label">Giá gốc *</label>
                                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtPrice" 
                                                                      ErrorMessage="Vui lòng nhập giá" CssClass="text-danger" />
                                            <asp:CompareValidator ID="cvPrice" runat="server" ControlToValidate="txtPrice" 
                                                                Type="Double" Operator="DataTypeCheck" ErrorMessage="Giá phải là số" CssClass="text-danger" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="mb-3">
                                            <label class="form-label">Giá khuyến mãi</label>
                                            <asp:TextBox ID="txtSalePrice" runat="server" CssClass="form-control" />
                                            <asp:CompareValidator ID="cvSalePrice" runat="server" ControlToValidate="txtSalePrice" 
                                                                Type="Double" Operator="DataTypeCheck" ErrorMessage="Giá KM phải là số" CssClass="text-danger" />
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="mb-3">
                                    <label class="form-label">Số lượng tồn kho</label>
                                    <asp:TextBox ID="txtStockQuantity" runat="server" CssClass="form-control" Text="0" />
                                    <asp:CompareValidator ID="cvStockQuantity" runat="server" ControlToValidate="txtStockQuantity" 
                                                        Type="Integer" Operator="DataTypeCheck" ErrorMessage="Số lượng phải là số nguyên" CssClass="text-danger" />
                                </div>
                                
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="mb-3">
                                            <label class="form-label">Kích thước</label>
                                            <asp:TextBox ID="txtSize" runat="server" CssClass="form-control" placeholder="S, M, L, XL" />
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="mb-3">
                                            <label class="form-label">Màu sắc</label>
                                            <asp:TextBox ID="txtColor" runat="server" CssClass="form-control" placeholder="Trắng, Đen, ..." />
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="mb-3">
                                            <label class="form-label">Chất liệu</label>
                                            <asp:TextBox ID="txtMaterial" runat="server" CssClass="form-control" placeholder="Cotton, Polyester, ..." />
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="mb-3">
                                    <div class="form-check">
                                        <asp:CheckBox ID="cbIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                                        <label class="form-check-label">Kích hoạt</label>
                                    </div>
                                </div>
                                
                                <!-- Main Product Image Upload Section -->
                                <div class="mb-3">
                                    <label class="form-label fw-bold">Ảnh chính sản phẩm *</label>
                                    <div class="image-preview-container">
                                        <asp:FileUpload ID="fuMainImage" runat="server" CssClass="form-control mb-2" accept="image/*" />
                                        <div id="mainImagePreview" class="mt-2">
                                            <img id="mainImageDisplay" class="main-image-preview" style="display: none; max-width: 100%;" />
                                            <div id="mainImagePlaceholder" class="text-muted">
                                                <i class="fas fa-image fa-3x mb-2"></i><br/>
                                                Chưa có ảnh chính
                                            </div>
                                        </div>
                                    </div>
                                    <small class="text-muted">Ảnh này sẽ hiển thị ở trang danh sách sản phẩm và trang chủ. Chỉ chấp nhận file ảnh.</small>
                                    <asp:CustomValidator ID="cvMainImage" runat="server" ControlToValidate="fuMainImage" 
                                                       ErrorMessage="Vui lòng chọn ảnh chính cho sản phẩm" CssClass="text-danger" 
                                                       OnServerValidate="cvMainImage_ServerValidate" />
                                </div>
                                
                                <!-- 3D Model Upload Section -->
                                <div class="mb-3">
                                    <label class="form-label fw-bold">File 3D Model (GLB)</label>
                                    <div class="image-preview-container">
                                        <asp:FileUpload ID="fu3DModel" runat="server" CssClass="form-control mb-2" accept=".glb" />
                                        <div id="model3dPreview" class="mt-2">
                                            <div id="model3dPlaceholder" class="text-muted">
                                                <i class="fas fa-cube fa-3x mb-2"></i><br/>
                                                Chưa có file 3D model
                                            </div>
                                            <div id="model3dInfo" class="text-success" style="display: none;">
                                                <i class="fas fa-check-circle fa-2x mb-2"></i><br/>
                                                <span id="model3dFileName"></span>
                                            </div>
                                        </div>
                                    </div>
                                    <small class="text-muted">Chỉ chấp nhận file .glb. File sẽ được lưu vào /Upload/Models/3D/[Danh mục]/</small>
                                </div>
                            </div>
                            
                            <div class="col-md-6">
                                <!-- Simplified Image Upload Section -->
                                <div class="image-upload-section">
                                    <h5><i class="fas fa-images me-2"></i>Upload ảnh sản phẩm theo màu sắc</h5>
                                    <p class="text-muted mb-3">Upload ảnh cho từng màu sắc. Tất cả ảnh sẽ được lưu cùng lúc khi bạn click "Lưu".</p>
                                    
                                    <!-- Multiple Image Upload for All Colors -->
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">Upload ảnh cho tất cả màu sắc:</label>
                                        
                                        <!-- White Color Upload -->
                                        <div class="mb-3">
                                            <label class="form-label">Màu Trắng:</label>
                                            <asp:FileUpload ID="fuWhiteImage" runat="server" CssClass="form-control mb-2" accept="image/*" />
                                            <div id="whiteImagePreview" class="mt-2">
                                                <img id="whiteImageDisplay" class="main-image-preview" style="display: none; max-width: 100%;" />
                                                <div id="whiteImagePlaceholder" class="text-muted">
                                                    <i class="fas fa-image fa-2x mb-2"></i><br/>
                                                    Chưa có ảnh cho màu Trắng
                                                </div>
                                            </div>
                                        </div>
                                        
                                        <!-- Black Color Upload -->
                                        <div class="mb-3">
                                            <label class="form-label">Màu Đen:</label>
                                            <asp:FileUpload ID="fuBlackImage" runat="server" CssClass="form-control mb-2" accept="image/*" />
                                            <div id="blackImagePreview" class="mt-2">
                                                <img id="blackImageDisplay" class="main-image-preview" style="display: none; max-width: 100%;" />
                                                <div id="blackImagePlaceholder" class="text-muted">
                                                    <i class="fas fa-image fa-2x mb-2"></i><br/>
                                                    Chưa có ảnh cho màu Đen
                                                </div>
                                            </div>
                                        </div>
                                        
                                        <!-- Red Color Upload -->
                                        <div class="mb-3">
                                            <label class="form-label">Màu Đỏ:</label>
                                            <asp:FileUpload ID="fuRedImage" runat="server" CssClass="form-control mb-2" accept="image/*" />
                                            <div id="redImagePreview" class="mt-2">
                                                <img id="redImageDisplay" class="main-image-preview" style="display: none; max-width: 100%;" />
                                                <div id="redImagePlaceholder" class="text-muted">
                                                    <i class="fas fa-image fa-2x mb-2"></i><br/>
                                                    Chưa có ảnh cho màu Đỏ
                                                </div>
                                            </div>
                                        </div>
                                        
                                        <!-- Blue Color Upload -->
                                        <div class="mb-3">
                                            <label class="form-label">Màu Xanh:</label>
                                            <asp:FileUpload ID="fuBlueImage" runat="server" CssClass="form-control mb-2" accept="image/*" />
                                            <div id="blueImagePreview" class="mt-2">
                                                <img id="blueImageDisplay" class="main-image-preview" style="display: none; max-width: 100%;" />
                                                <div id="blueImagePlaceholder" class="text-muted">
                                                    <i class="fas fa-image fa-2x mb-2"></i><br/>
                                                    Chưa có ảnh cho màu Xanh
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <!-- Image Status for All Colors -->
                                    <div class="mb-3">
                                        <label class="form-label fw-bold">Trạng thái ảnh theo màu:</label>
                                        <div class="row">
                                            <div class="col-3">
                                                <div class="color-status" data-color="white">
                                                    <div class="color-indicator" style="background-color: #ffffff; border: 2px solid #ddd;"></div>
                                                    <span class="status-text">Trắng: <span class="status">Chưa có</span></span>
                                                </div>
                                            </div>
                                            <div class="col-3">
                                                <div class="color-status" data-color="black">
                                                    <div class="color-indicator" style="background-color: #000000;"></div>
                                                    <span class="status-text">Đen: <span class="status">Chưa có</span></span>
                                                </div>
                                            </div>
                                            <div class="col-3">
                                                <div class="color-status" data-color="red">
                                                    <div class="color-indicator" style="background-color: #ff0000;"></div>
                                                    <span class="status-text">Đỏ: <span class="status">Chưa có</span></span>
                                                </div>
                                            </div>
                                            <div class="col-3">
                                                <div class="color-status" data-color="blue">
                                                    <div class="color-indicator" style="background-color: #0000ff;"></div>
                                                    <span class="status-text">Xanh: <span class="status">Chưa có</span></span>
                                                </div>
                                            </div>
                                        </div>
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
    
    <script>
        // Wait for DOM to be ready
        document.addEventListener('DOMContentLoaded', function() {
            // Main image upload handler
            const mainImageInput = document.getElementById('<%= fuMainImage.ClientID %>');
            if (mainImageInput) {
                mainImageInput.addEventListener('change', function(e) {
                    handleMainImageUpload(e.target.files[0]);
                });
            }

            // File upload handlers for all colors
            const whiteImageInput = document.getElementById('<%= fuWhiteImage.ClientID %>');
            const blackImageInput = document.getElementById('<%= fuBlackImage.ClientID %>');
            const redImageInput = document.getElementById('<%= fuRedImage.ClientID %>');
            const blueImageInput = document.getElementById('<%= fuBlueImage.ClientID %>');

            if (whiteImageInput) {
                whiteImageInput.addEventListener('change', function(e) {
                    handleColorFileUpload(e.target.files[0], 'white');
                });
            }

            if (blackImageInput) {
                blackImageInput.addEventListener('change', function(e) {
                    handleColorFileUpload(e.target.files[0], 'black');
                });
            }

            if (redImageInput) {
                redImageInput.addEventListener('change', function(e) {
                    handleColorFileUpload(e.target.files[0], 'red');
                });
            }

            if (blueImageInput) {
                blueImageInput.addEventListener('change', function(e) {
                    handleColorFileUpload(e.target.files[0], 'blue');
                });
            }

            // 3D Model file upload handler
            const model3dInput = document.getElementById('<%= fu3DModel.ClientID %>');
            if (model3dInput) {
                model3dInput.addEventListener('change', function(e) {
                    handle3DModelUpload(e.target.files[0]);
                });
            }
        });

        function handle3DModelUpload(file) {
            if (!file) return;

            // Check file extension
            const fileExtension = file.name.toLowerCase().split('.').pop();
            if (fileExtension !== 'glb') {
                alert('Chỉ chấp nhận file .glb');
                const model3dInput = document.getElementById('<%= fu3DModel.ClientID %>');
                if (model3dInput) model3dInput.value = '';
                return;
            }

            // Display file info
            const placeholder = document.getElementById('model3dPlaceholder');
            const info = document.getElementById('model3dInfo');
            const fileName = document.getElementById('model3dFileName');
            
            if (placeholder) placeholder.style.display = 'none';
            if (info) info.style.display = 'block';
            if (fileName) fileName.textContent = file.name;
        }

        function handleColorFileUpload(file, color) {
            if (!file) return;

            // Validate file type
            if (!file.type.startsWith('image/')) {
                alert('Vui lòng chọn file ảnh hợp lệ!');
                return;
            }

            // Validate file size (max 5MB)
            if (file.size > 5 * 1024 * 1024) {
                alert('File ảnh không được lớn hơn 5MB!');
                return;
            }

            const reader = new FileReader();
            reader.onload = function(e) {
                const imageUrl = e.target.result;
                
                // Display the image preview for the specific color
                displayColorImage(imageUrl, color);
                
                // Update status for this color
                updateColorStatus(color, true);
            };
            reader.readAsDataURL(file);
        }

        function handleMainImageUpload(file) {
            if (!file) return;

            // Validate file type
            if (!file.type.startsWith('image/')) {
                alert('Vui lòng chọn file ảnh hợp lệ!');
                return;
            }

            // Validate file size (max 5MB)
            if (file.size > 5 * 1024 * 1024) {
                alert('File ảnh không được lớn hơn 5MB!');
                return;
            }

            const reader = new FileReader();
            reader.onload = function(e) {
                const imageUrl = e.target.result;
                
                // Display the main image preview
                const mainImageDisplay = document.getElementById('mainImageDisplay');
                const mainImagePlaceholder = document.getElementById('mainImagePlaceholder');
                
                if (mainImageDisplay) {
                    mainImageDisplay.src = imageUrl;
                    mainImageDisplay.style.display = 'block';
                }
                if (mainImagePlaceholder) mainImagePlaceholder.style.display = 'none';
            };
            reader.readAsDataURL(file);
        }

        function displayColorImage(imageUrl, color) {
            const colorNames = {
                'white': 'Trắng',
                'black': 'Đen', 
                'red': 'Đỏ',
                'blue': 'Xanh'
            };
            
            const display = document.getElementById(color + 'ImageDisplay');
            const placeholder = document.getElementById(color + 'ImagePlaceholder');
            
            if (display) {
                display.src = imageUrl;
                display.style.display = 'block';
            }
            if (placeholder) placeholder.style.display = 'none';
        }

        function updateColorStatus(color, hasImage) {
            const statusElement = document.querySelector(`[data-color="${color}"] .status`);
            if (statusElement) {
                statusElement.textContent = hasImage ? 'Đã có' : 'Chưa có';
                statusElement.className = hasImage ? 'status has-image' : 'status';
            }
        }

        function clearAllFileInputs() {
            const mainImageInput = document.getElementById('<%= fuMainImage.ClientID %>');
            const whiteImageInput = document.getElementById('<%= fuWhiteImage.ClientID %>');
            const blackImageInput = document.getElementById('<%= fuBlackImage.ClientID %>');
            const redImageInput = document.getElementById('<%= fuRedImage.ClientID %>');
            const blueImageInput = document.getElementById('<%= fuBlueImage.ClientID %>');
            const model3dInput = document.getElementById('<%= fu3DModel.ClientID %>');
            
            if (mainImageInput) mainImageInput.value = '';
            if (whiteImageInput) whiteImageInput.value = '';
            if (blackImageInput) blackImageInput.value = '';
            if (redImageInput) redImageInput.value = '';
            if (blueImageInput) blueImageInput.value = '';
            if (model3dInput) model3dInput.value = '';
        }

        function resetAllImagePreviews() {
            // Reset all color image previews
            const colors = ['white', 'black', 'red', 'blue'];
            colors.forEach(color => {
                const display = document.getElementById(color + 'ImageDisplay');
                const placeholder = document.getElementById(color + 'ImagePlaceholder');
                
                if (display) display.style.display = 'none';
                if (placeholder) placeholder.style.display = 'block';
            });
            
            // Reset main image preview
            const mainImageDisplay = document.getElementById('mainImageDisplay');
            const mainImagePlaceholder = document.getElementById('mainImagePlaceholder');
            
            if (mainImageDisplay) mainImageDisplay.style.display = 'none';
            if (mainImagePlaceholder) mainImagePlaceholder.style.display = 'block';

            // Reset 3D model preview
            const model3dPlaceholder = document.getElementById('model3dPlaceholder');
            const model3dInfo = document.getElementById('model3dInfo');
            
            if (model3dPlaceholder) model3dPlaceholder.style.display = 'block';
            if (model3dInfo) model3dInfo.style.display = 'none';
        }
    </script>
</asp:Content>