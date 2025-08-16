/*
 * 3D T-shirt Design Platform - Enhanced Version
 * 
 * WORKFLOW:
 * 1. 3D PREVIEW: Uses DecalGeometry for smooth 3D preview - NO CUTTING
 * 2. EXPORT: Uses UV mapping only for print-ready PNG export
 * 
 * IMPORTANT: This version ensures logos are NEVER cut in 3D preview
 * UV mapping is ONLY used during export to generate print-ready files
 */

// Global variables for 3D scene
let scene, camera, renderer, controls, model, imageMesh;
let currentImageUrl = null;
let originalMaterials = new Map(); // Store original materials
let decalProjector = null; // For advanced decal projection
// Thêm biến toàn cục cho decal
let decalMesh = null;
let decalMeshes = []; // Thêm biến lưu danh sách decal
// Thêm biến lưu thông tin decal vị trí cuối cùng
let lastDecalPosition = null;
let lastDecalNormal = null;
// Thêm biến toàn cục để lưu mesh target của decal
let lastDecalMeshTarget = null;

// Projective Texture Mapping variables
let projectorCamera = null;
let projectorTexture = null;
let projectorMaterial = null;
let projectorTargetMesh = null;

// UV Mapping variables (ONLY for export, not for 3D preview)
let uvCanvas = null;
let uvCtx = null;
let uvBaseImage = null;
let uvLogoImage = null;
let uvTexture = null;
let uvMaterial = null;
let uvTargetMesh = null;
let uvLogoParams = { x: 0.5, y: 0.5, scale: 0.3, rotation: 0 };
let uvClick = null; // Lưu UV khi user click

// Export workflow variables
let exportLogoInfo = null;
let uvIslandPath = null; // Path của UV island để mask
let isExportMode = false; // Flag để biết đang ở chế độ export

// Color mapping for 3D models
const colorMap = {
    'white': 0xffffff,
    'black': 0x333333,  // Changed from 0x000000 to 0x333333 (lighter black)
    'red': 0xff0000,
    'blue': 0x0000ff,
    'green': 0x00ff00,
    'yellow': 0xffff00,
    'pink': 0xffc0cb,
    'purple': 0x800080,
    'orange': 0xffa500,
    'gray': 0x808080,
    'brown': 0xa52a2a,
    'navy': 0x000080,
    'maroon': 0x800000,
    'olive': 0x808000,
    'lime': 0x00ff00,
    'aqua': 0x00ffff,
    'teal': 0x008080,
    'silver': 0xc0c0c0,
    'gold': 0xffd700
};

// Initialize when DOM is loaded
window.addEventListener('DOMContentLoaded', function() {
    console.log('[DEBUG] DOMContentLoaded - Starting initialization...');
    
    // Debug URL parameters
    const urlParams = new URLSearchParams(window.location.search);
    console.log('[DEBUG] URL Parameters:', {
        productId: urlParams.get('productId'),
        size: urlParams.get('size'),
        color: urlParams.get('color'),
        quantity: urlParams.get('quantity')
    });
    
    // Try to get model path from hidden field
    const hdnModelPath = document.getElementById('hdnModelPath');
    let modelPath = '';
    
    if (hdnModelPath) {
        modelPath = hdnModelPath.value;
    }
    
    console.log('Model Path:', modelPath);
    
    if (modelPath) {
        console.log('Loading 3D model...');
        myInit3DViewer(modelPath);
    } else {
        console.log('No model path found!');
        const modelViewer = document.getElementById('modelViewer');
        if (modelViewer) {
            modelViewer.innerHTML = '<div class="text-warning p-3">Không tìm thấy đường dẫn mô hình 3D!</div>';
        }
    }
    
    // Initialize event listeners
    initializeEventListeners();

    // Thêm nút Export vào UI nếu chưa có
    if (!document.getElementById('btnExportPNG')) {
        const exportBtn = document.createElement('button');
        exportBtn.id = 'btnExportPNG';
        exportBtn.textContent = 'Export Logo PNG';
        exportBtn.className = 'btn btn-success mt-2';
        exportBtn.title = 'Xuất file PNG để in áo';
        const controls = document.getElementById('imageControls') || document.body;
        controls.appendChild(exportBtn);
    }

    // Thêm nút Preview UV
    if (!document.getElementById('btnPreviewUV')) {
        const previewBtn = document.createElement('button');
        previewBtn.id = 'btnPreviewUV';
        previewBtn.textContent = 'Preview UV Layout';
        previewBtn.className = 'btn btn-info mt-2';
        previewBtn.title = 'Xem layout UV để biết vùng in an toàn';
        const controls = document.getElementById('imageControls') || document.body;
        controls.appendChild(previewBtn);
    }
    
    // Thêm thông báo workflow
    addWorkflowInstructions();
});

// Initialize all event listeners
function initializeEventListeners() {
    // Test image button
    const btnCreateTest = document.getElementById('btnCreateTest');
    if (btnCreateTest) {
        btnCreateTest.addEventListener('click', function() {
            const testImageUrl = createTestImage();
            showImagePreview(testImageUrl);
            addImageToModel(testImageUrl);
        });
    }
    
    // Drag & Drop upload
    const dropArea = document.getElementById('drop-area');
    const fileElem = document.getElementById('fileElem');
    
    if (dropArea && fileElem) {
        dropArea.addEventListener('click', () => fileElem.click());
        dropArea.addEventListener('dragover', e => { 
            e.preventDefault(); 
            dropArea.classList.add('dragover'); 
        });
        dropArea.addEventListener('dragleave', e => { 
            dropArea.classList.remove('dragover'); 
        });
        dropArea.addEventListener('drop', e => {
            e.preventDefault(); 
            dropArea.classList.remove('dragover');
            const file = e.dataTransfer.files[0];
            if (file && file.type.startsWith('image/')) {
                showPreview(file);
                fileElem.files = e.dataTransfer.files;
            }
        });
        
        fileElem.addEventListener('change', e => {
            const file = e.target.files[0];
            if (file && file.type.startsWith('image/')) {
                showPreview(file);
            }
        });
    }

    // Export PNG button
    setTimeout(() => {
        const btnExportPNG = document.getElementById('btnExportPNG');
        if (btnExportPNG) {
            btnExportPNG.addEventListener('click', function() {
                exportLogoToPNG();
            });
        }

        // Preview UV button
        const btnPreviewUV = document.getElementById('btnPreviewUV');
        if (btnPreviewUV) {
            btnPreviewUV.addEventListener('click', function() {
                showUVLayoutPreview();
            });
        }
    }, 1000);
}

// Show image preview
function showPreview(file) {
    const reader = new FileReader();
    reader.onload = e => {
        showImagePreview(e.target.result);
        addImageToModel(e.target.result);
    };
    reader.readAsDataURL(file);
}

// Show image preview in UI
function showImagePreview(imageUrl) {
    const previewThumb = document.getElementById('previewThumb');
    if (previewThumb) {
        previewThumb.style.display = 'block';
        previewThumb.innerHTML = `<img src="${imageUrl}" alt="preview" style="max-width:100%;max-height:80px;border-radius:12px;box-shadow:0 2px 8px #a78bfa33;" />`;
    }
    
    // Show image controls
    const imageControls = document.getElementById('imageControls');
    if (imageControls) {
        imageControls.style.display = 'block';
    }
}

// Create test image
function createTestImage() {
    const canvas = document.createElement('canvas');
    canvas.width = 256;
    canvas.height = 256;
    const ctx = canvas.getContext('2d');
    
    // Draw red circle
    ctx.fillStyle = '#ff0000';
    ctx.beginPath();
    ctx.arc(128, 128, 100, 0, 2 * Math.PI);
    ctx.fill();
    
    // Draw "TEST" text
    ctx.fillStyle = '#ffffff';
    ctx.font = '48px Arial';
    ctx.textAlign = 'center';
    ctx.fillText('TEST', 128, 140);
    
    return canvas.toDataURL();
}

// Enhanced addImageToModel - chỉ chuẩn bị cho decal 3D, không dùng UV mapping
function addImageToModel(imageUrl) {
    console.log('🖼️ Adding image to model for 3D decal placement...');
    
    // Reset toàn bộ trạng thái decal khi upload ảnh mới
    lastDecalPosition = null;
    lastDecalNormal = null;
    lastDecalMeshTarget = null;
    // Xóa hết decal cũ trên áo
    if (decalMeshes && decalMeshes.length > 0) {
        for (const d of decalMeshes) {
            scene.remove(d);
            d.geometry?.dispose();
            d.material?.dispose();
        }
        decalMeshes = [];
        decalMesh = null;
    }
    
    if (!scene || !model) {
        console.log('❌ Scene or model not ready yet');
        return;
    }
    currentImageUrl = imageUrl;
    
    // Lấy mesh lớn nhất có UV (chỉ để lưu thông tin cho export)
    const shirtMesh = findShirtFrontMesh(model) || model;
    
    // Kiểm tra xem mesh có geometry và UV attributes không
    if (!shirtMesh || !shirtMesh.geometry || !shirtMesh.geometry.attributes || !shirtMesh.geometry.attributes.uv) {
        console.log('⚠️ Model không có UV mapping - chỉ có thể xem 3D, không export được');
        // Removed debug alert - Model không có UV mapping! Chỉ có thể xem 3D preview, không thể export PNG.
        return;
    }
    
    console.log('✅ Found shirt mesh with UV coordinates');
    uvTargetMesh = shirtMesh;
    
    // Lấy texture gốc (chỉ để lưu thông tin cho export)
    let baseMap = shirtMesh.material.map;
    if (!baseMap || !baseMap.image) {
        // Nếu không có, tạo canvas trắng
        uvBaseImage = document.createElement('canvas');
        uvBaseImage.width = 1024;
        uvBaseImage.height = 1024;
        let ctx = uvBaseImage.getContext('2d');
        ctx.fillStyle = '#ffffff';
        ctx.fillRect(0, 0, uvBaseImage.width, uvBaseImage.height);
        console.log('📄 Created white base image for export');
    } else {
        // Copy texture gốc
        uvBaseImage = document.createElement('canvas');
        uvBaseImage.width = baseMap.image.width;
        uvBaseImage.height = baseMap.image.height;
        let ctx = uvBaseImage.getContext('2d');
        ctx.drawImage(baseMap.image, 0, 0);
        console.log('📄 Copied original texture for export');
    }
    
    // Tạo canvas để vẽ logo (chỉ cho export)
    uvCanvas = document.createElement('canvas');
    uvCanvas.width = uvBaseImage.width;
    uvCanvas.height = uvBaseImage.height;
    uvCtx = uvCanvas.getContext('2d');
    
    console.log('🎨 Created UV canvas for export preparation');
    
    uvLogoImage = new window.Image();
    uvLogoImage.onload = function() {
        // KHÔNG vẽ UV logo ngay - chỉ lưu thông tin
        createUVIslandPath();
        console.log('✅ Logo prepared for export - click on shirt to place 3D decal');
    };
    uvLogoImage.src = imageUrl;
    
    // KHÔNG gọi bất kỳ hàm UV mapping nào - chỉ chuẩn bị cho export
    console.log('🎯 Image prepared for 3D decal placement - NO UV mapping applied to preview');
}

// Create UV island path for masking
function createUVIslandPath() {
    console.log('📏 Creating UV island path for export masking...');
    
    if (!uvTargetMesh || !uvTargetMesh.geometry.attributes.uv) {
        console.log('❌ Cannot create UV path - missing mesh or UV coordinates');
        return;
    }
    
    const uv = uvTargetMesh.geometry.attributes.uv;
    const indices = uvTargetMesh.geometry.index;
    
    if (!indices) {
        console.log('❌ Cannot create UV path - missing indices');
        return;
    }
    
    console.log('📊 UV coordinates found - vertices:', uv.count, 'indices:', indices.count);
    
    // Tạo path từ UV coordinates
    uvIslandPath = new Path2D();
    let firstPoint = true;
    
    for (let i = 0; i < indices.count; i += 3) {
        const u1 = uv.getX(indices.getX(i)) * uvCanvas.width;
        const v1 = (1 - uv.getY(indices.getX(i))) * uvCanvas.height;
        const u2 = uv.getX(indices.getX(i + 1)) * uvCanvas.width;
        const v2 = (1 - uv.getY(indices.getX(i + 1))) * uvCanvas.height;
        const u3 = uv.getX(indices.getX(i + 2)) * uvCanvas.width;
        const v3 = (1 - uv.getY(indices.getX(i + 2))) * uvCanvas.height;
        
        if (firstPoint) {
            uvIslandPath.moveTo(u1, v1);
            firstPoint = false;
        }
        
        uvIslandPath.lineTo(u2, v2);
        uvIslandPath.lineTo(u3, v3);
        uvIslandPath.lineTo(u1, v1);
    }
    
    console.log('✅ UV island path created successfully');
}

// Enhanced showUVLayoutPreview - hiển thị UV layout để user hiểu vùng in
function showUVLayoutPreview() {
    if (!uvCanvas || !uvCtx) {
        // Removed debug alert - Chưa có logo để xem UV layout!
        return;
    }
    
    // Vẽ logo lên UV canvas trước khi preview
    drawUVLogo();
    
    // Tạo popup để hiển thị UV layout
    const popup = document.createElement('div');
    popup.style.cssText = `
        position: fixed; top: 0; left: 0; width: 100%; height: 100%;
        background: rgba(0,0,0,0.8); z-index: 9999; display: flex;
        align-items: center; justify-content: center;
    `;
    
    const content = document.createElement('div');
    content.style.cssText = `
        background: white; padding: 20px; border-radius: 10px;
        max-width: 90%; max-height: 90%; overflow: auto;
    `;
    
    const canvas = document.createElement('canvas');
    canvas.width = uvCanvas.width;
    canvas.height = uvCanvas.height;
    const ctx = canvas.getContext('2d');
    
    // Vẽ nền trắng
    ctx.fillStyle = '#ffffff';
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    
    // Vẽ UV island outline
    if (uvIslandPath) {
        ctx.strokeStyle = '#ff0000';
        ctx.lineWidth = 3;
        ctx.stroke(uvIslandPath);
        
        // Vẽ vùng safe area (nhỏ hơn UV island 10%)
        ctx.strokeStyle = '#00ff00';
        ctx.lineWidth = 2;
        ctx.setLineDash([5, 5]);
        ctx.stroke(uvIslandPath);
        ctx.setLineDash([]);
    }
    
    // Vẽ logo hiện tại nếu có
    if (uvLogoImage && exportLogoInfo) {
        const { uv, scale, rotation } = exportLogoInfo;
        const posX = uv.x * canvas.width;
        const posY = (1 - uv.y) * canvas.height;
        const logoW = uvLogoImage.width * scale;
        const logoH = uvLogoImage.height * scale;
        
        ctx.save();
        ctx.translate(posX, posY);
        ctx.rotate(rotation * Math.PI / 180);
        ctx.drawImage(uvLogoImage, -logoW/2, -logoH/2, logoW, logoH);
        ctx.restore();
    }
    
    content.innerHTML = `
        <h4>UV Layout Preview</h4>
        <p><strong>Đỏ:</strong> Vùng UV island (vùng in được)<br>
        <strong>Xanh:</strong> Safe area (vùng an toàn)</p>
        <p class="text-warning"><strong>Lưu ý:</strong> Logo nằm ngoài vùng đỏ sẽ bị cắt khi in!</p>
        <button onclick="this.parentElement.parentElement.remove()" class="btn btn-primary">Đóng</button>
    `;
    content.appendChild(canvas);
    
    popup.appendChild(content);
    document.body.appendChild(popup);
}

// Enhanced export function - sử dụng UV mapping chỉ khi export
function exportLogoToPNG() {
    console.log('📤 Starting PNG export process...');
    
    if (!exportLogoInfo || !uvLogoImage) {
        // Removed debug alert - Bạn cần đặt logo lên áo trước khi export!
        console.log('❌ Export failed - missing logo info or image');
        return;
    }
    
    console.log('✅ Export info found:', exportLogoInfo);
    
    // Bước 1: Lấy thông tin từ 3D decal
    const { uv, scale, rotation } = exportLogoInfo;
    
    console.log('📊 Export parameters - UV:', uv, 'Scale:', scale, 'Rotation:', rotation);
    
    // Bước 2: Vẽ logo lên UV canvas (chỉ cho export)
    console.log('🎨 Drawing logo on UV canvas for export...');
    drawUVLogo();
    
    // Bước 3: Tạo canvas với kích thước chuẩn cho in
    const canvasSize = 2048; // Kích thước cao cho chất lượng in tốt
    const canvas = document.createElement('canvas');
    canvas.width = canvas.height = canvasSize;
    const ctx = canvas.getContext('2d');
    
    console.log('📐 Created export canvas with size:', canvasSize);
    
    // Bước 4: Vẽ nền trắng
    ctx.fillStyle = '#ffffff';
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    
    // Bước 5: Vẽ UV island outline (tùy chọn)
    if (uvIslandPath) {
        ctx.strokeStyle = '#cccccc';
        ctx.lineWidth = 2;
        ctx.stroke(uvIslandPath);
        console.log('📏 UV island outline drawn');
    }
    
    // Bước 6: Render logo tại vị trí UV đúng scale và rotation
    const posX = uv.x * canvas.width;
    const posY = (1 - uv.y) * canvas.height;
    const logoW = uvLogoImage.width * scale;
    const logoH = uvLogoImage.height * scale;
    
    console.log('📍 Logo position on canvas - X:', posX, 'Y:', posY, 'W:', logoW, 'H:', logoH);
    
    ctx.save();
    ctx.translate(posX, posY);
    ctx.rotate(rotation * Math.PI / 180);
    ctx.drawImage(uvLogoImage, -logoW/2, -logoH/2, logoW, logoH);
    ctx.restore();
    
    // Bước 7: Mask theo UV island nếu cần
    if (uvIslandPath) {
        console.log('✂️ Applying UV island mask (logo may be cut if outside UV area)');
        ctx.globalCompositeOperation = 'destination-in';
        ctx.fill(uvIslandPath);
        ctx.globalCompositeOperation = 'source-over';
    }
    
    // Bước 8: Export PNG
    const dataURL = canvas.toDataURL('image/png');
    const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
    downloadImage(dataURL, `logo_export_${timestamp}.png`);
    
    console.log('✅ Export completed successfully!');
    console.log('Export details:', {
        uv: uv,
        scale: scale,
        rotation: rotation,
        canvasSize: canvasSize,
        note: 'Logo may be cut if outside UV island - this is normal for printing'
    });
}

// Method 1: Try UV mapping if model has UV coordinates
function tryUVMapping(texture) {
    const shirtMesh = findShirtFrontMesh(model);
    if (!shirtMesh || !shirtMesh.geometry.attributes.uv) {
        console.log('No UV coordinates found, skipping UV mapping');
        return false;
    }
    
    try {
        // Create a new material with the image texture
        const material = shirtMesh.material.clone();
        material.map = texture;
        material.needsUpdate = true;
        
        // Store original material
        if (!originalMaterials.has(shirtMesh)) {
            originalMaterials.set(shirtMesh, shirtMesh.material);
        }
        
        // Apply new material
        shirtMesh.material = material;
        
        // Create a visual indicator for the image area
        createImageIndicator(shirtMesh, texture);
        
        console.log('UV mapping applied successfully');
        return true;
    } catch (error) {
        console.error('UV mapping failed:', error);
        return false;
    }
}

// Method 2: Try decal projection
function tryDecalProjection(texture) {
    const shirtMesh = findShirtFrontMesh(model);
    if (!shirtMesh) {
        console.log('No shirt mesh found for decal projection');
        return false;
    }
    
    try {
        // Create decal projector
        const projector = new THREE.Vector3(0, 0, 1); // Project from front
        const decalGeometry = createDecalGeometryFromProjection(shirtMesh, projector, 0.5);
        
        if (decalGeometry) {
            const decalMaterial = new THREE.MeshBasicMaterial({
                map: texture,
                transparent: true,
                alphaTest: 0.1,
                side: THREE.DoubleSide
            });
            
            imageMesh = new THREE.Mesh(decalGeometry, decalMaterial);
            scene.add(imageMesh);
            console.log('Decal projection created successfully');
            return true;
        }
        
        console.log('Could not create decal geometry from projection');
        return false;
    } catch (error) {
        console.error('Decal projection failed:', error);
        return false;
    }
}

// Method 3: Try surface decal
function trySurfaceDecal(texture) {
    const shirtMesh = findShirtFrontMesh(model);
    if (!shirtMesh) {
        console.log('No shirt mesh found for surface decal');
        return false;
    }
    
    try {
        createDecalOnShirt(shirtMesh, texture);
        console.log('Surface decal created successfully');
        return true;
    } catch (error) {
        console.error('Surface decal failed:', error);
        return false;
    }
}

// Create decal geometry from projection
function createDecalGeometryFromProjection(mesh, projector, size) {
    const geometry = mesh.geometry;
    const positions = geometry.attributes.position;
    const normals = geometry.attributes.normal;
    
    if (!positions || !normals) {
        console.log('No position or normal attributes found');
        return null;
    }
    
    // Find vertices that face the projector
    const projectedVertices = [];
    const projectorNormal = projector.clone().normalize();
    
    for (let i = 0; i < positions.count; i++) {
        const normal = new THREE.Vector3();
        normal.fromBufferAttribute(normals, i);
        
        // Check if vertex faces the projector
        const dotProduct = normal.dot(projectorNormal);
        if (dotProduct > 0.5) { // Threshold for facing direction
            const position = new THREE.Vector3();
            position.fromBufferAttribute(positions, i);
            projectedVertices.push(position);
        }
    }
    
    if (projectedVertices.length < 3) {
        console.log('Not enough projected vertices found');
        return null;
    }
    
    // Create a plane geometry based on projected vertices
    const center = new THREE.Vector3();
    projectedVertices.forEach(v => center.add(v));
    center.divideScalar(projectedVertices.length);
    
    // Create plane geometry
    const decalGeometry = new THREE.PlaneGeometry(size, size);
    decalGeometry.translate(center.x, center.y, center.z);
    
    return decalGeometry;
}

// Create image indicator for UV mapped textures
function createImageIndicator(mesh, texture) {
    // Create a small visual indicator to show where the image is applied
    const indicatorGeometry = new THREE.SphereGeometry(0.02, 8, 8);
    const indicatorMaterial = new THREE.MeshBasicMaterial({ color: 0xff0000 });
    const indicator = new THREE.Mesh(indicatorGeometry, indicatorMaterial);
    
    // Position indicator at the center of the mesh
    const box = new THREE.Box3().setFromObject(mesh);
    const center = box.getCenter(new THREE.Vector3());
    indicator.position.copy(center);
    
    scene.add(indicator);
    
    // Remove indicator after 3 seconds
    setTimeout(() => {
        scene.remove(indicator);
    }, 3000);
}

// Find the front part of the shirt mesh
function findShirtFrontMesh(model) {
    let largestMesh = null;
    let maxArea = 0;
    
    model.traverse((child) => {
        if (child.isMesh && child.geometry) {
            // Calculate surface area
            const geometry = child.geometry;
            const area = calculateMeshArea(geometry);
            
            // Check if this is likely the front of the shirt
            if (area > maxArea && isLikelyShirtFront(child)) {
                maxArea = area;
                largestMesh = child;
            }
        }
    });
    
    if (largestMesh) {
        console.log('Found shirt front mesh with area:', maxArea);
    } else {
        console.log('No suitable shirt front mesh found');
    }
    
    return largestMesh;
}

// Calculate mesh surface area
function calculateMeshArea(geometry) {
    let area = 0;
    const positions = geometry.attributes.position;
    const indices = geometry.index;
    
    if (indices) {
        for (let i = 0; i < indices.count; i += 3) {
            const a = new THREE.Vector3();
            const b = new THREE.Vector3();
            const c = new THREE.Vector3();
            
            a.fromBufferAttribute(positions, indices.getX(i));
            b.fromBufferAttribute(positions, indices.getX(i + 1));
            c.fromBufferAttribute(positions, indices.getX(i + 2));
            
            area += calculateTriangleArea(a, b, c);
        }
    }
    
    return area;
}

// Calculate triangle area
function calculateTriangleArea(a, b, c) {
    const ab = new THREE.Vector3().subVectors(b, a);
    const ac = new THREE.Vector3().subVectors(c, a);
    const cross = new THREE.Vector3().crossVectors(ab, ac);
    return cross.length() * 0.5;
}

// Check if mesh is likely the front of the shirt
function isLikelyShirtFront(mesh) {
    // Check if mesh has a reasonable size and position
    const box = new THREE.Box3().setFromObject(mesh);
    const size = box.getSize(new THREE.Vector3());
    
    // Front of shirt should be relatively flat and large
    return size.x > 0.5 && size.y > 0.5 && size.z < 0.3;
}

// Create decal on shirt surface
function createDecalOnShirt(shirtMesh, texture) {
    // Create a decal geometry that follows the shirt surface
    const decalGeometry = createDecalGeometry(shirtMesh);
    
    // Create material with the image texture
    const decalMaterial = new THREE.MeshBasicMaterial({
        map: texture,
        transparent: true,
        alphaTest: 0.1,
        side: THREE.DoubleSide
    });
    
    // Create decal mesh
    imageMesh = new THREE.Mesh(decalGeometry, decalMaterial);
    
    // Position the decal on the shirt
    imageMesh.position.copy(shirtMesh.position);
    imageMesh.rotation.copy(shirtMesh.rotation);
    imageMesh.scale.copy(shirtMesh.scale);
    
    // Slightly offset to prevent z-fighting
    imageMesh.position.z += 0.001;
    
    scene.add(imageMesh);
    console.log('Decal created on shirt surface');
}

// Create decal geometry that follows the shirt surface
function createDecalGeometry(shirtMesh) {
    const shirtGeometry = shirtMesh.geometry;
    const decalSize = 0.5; // Size of the decal
    
    // Create a plane geometry for the decal
    const decalGeometry = new THREE.PlaneGeometry(decalSize, decalSize);
    
    // Apply the shirt's transformation to the decal
    decalGeometry.applyMatrix4(shirtMesh.matrixWorld);
    
    return decalGeometry;
}

// Fallback method: create plane image
function createPlaneImage(texture) {
    // Create plane geometry for image
    const geometry = new THREE.PlaneGeometry(1, 1);
    const material = new THREE.MeshBasicMaterial({ 
        map: texture, 
        transparent: true,
        side: THREE.DoubleSide,
        alphaTest: 0.1
    });
    
    imageMesh = new THREE.Mesh(geometry, material);
    
    // Position image in front of model (shirt front)
    imageMesh.position.set(0, 0, 0.1);
    imageMesh.rotation.x = -Math.PI / 2; // Rotate to horizontal
    
    scene.add(imageMesh);
    console.log('Plane image created as fallback');
}

// Alternative method: Apply texture directly to shirt material
function applyTextureToShirt(imageUrl) {
    if (!model) return;
    
    const textureLoader = new THREE.TextureLoader();
    textureLoader.load(imageUrl, function(texture) {
        // Find shirt mesh
        model.traverse((child) => {
            if (child.isMesh && child.material) {
                // Store original material if not already stored
                if (!originalMaterials.has(child)) {
                    originalMaterials.set(child, child.material.clone());
                }
                
                // Create new material with image texture
                const newMaterial = child.material.clone();
                newMaterial.map = texture;
                newMaterial.needsUpdate = true;
                
                child.material = newMaterial;
            }
        });
        
        console.log('Texture applied directly to shirt material');
    });
}

// Reset image controls to default
function resetImageControls() {
    const sizeSlider = document.getElementById('sizeSlider');
    const posXSlider = document.getElementById('posXSlider');
    const posYSlider = document.getElementById('posYSlider');
    const rotationSlider = document.getElementById('rotationSlider');
    
    if (sizeSlider) sizeSlider.value = 1;
    if (posXSlider) posXSlider.value = 0;
    if (posYSlider) posYSlider.value = 0;
    if (rotationSlider) rotationSlider.value = 0;
    
    updateImageTransform();
}

// Enhanced updateImageTransform - chỉ cập nhật decal 3D, không dùng UV mapping
function updateImageTransform() {
    const sizeSlider = document.getElementById('sizeSlider');
    const posXSlider = document.getElementById('posXSlider');
    const posYSlider = document.getElementById('posYSlider');
    const rotationSlider = document.getElementById('rotationSlider');
    
    if (!sizeSlider || !posXSlider || !posYSlider || !rotationSlider) return;
    
    // Cập nhật thông số
    const size = parseFloat(sizeSlider.value);
    const rotation = parseFloat(rotationSlider.value);
    
    console.log('🔄 Updating 3D decal transform - Size:', size, 'Rotation:', rotation);
    
    // Cập nhật export info nếu có UV click
    if (exportLogoInfo) {
        exportLogoInfo.scale = size * 0.3;
        exportLogoInfo.rotation = rotation;
        console.log('📊 Export info updated:', exportLogoInfo);
    }
    
    // Cập nhật decal 3D nếu có
    if (lastDecalMeshTarget && lastDecalPosition && lastDecalNormal) {
        console.log('🎨 Updating 3D decal with new parameters...');
        
        // Tạo orientation mới với rotation mới
        const orientation = new THREE.Euler().setFromVector3(lastDecalNormal);
        orientation.z += THREE.MathUtils.degToRad(rotation);
        
        // Tạo lại decal với thông số mới
        placeDecal(lastDecalMeshTarget, lastDecalPosition, orientation, size, currentImageUrl);
    } else {
        console.log('⚠️ No decal to update - need to place decal first');
    }
    
    // Update display values
    updateControlValues(size, parseFloat(posXSlider.value), parseFloat(posYSlider.value), rotation);
}

// Update control value displays
function updateControlValues(size, posX, posY, rotation) {
    const sizeValue = document.getElementById('sizeValue');
    const posXValue = document.getElementById('posXValue');
    const posYValue = document.getElementById('posYValue');
    const rotationValue = document.getElementById('rotationValue');
    
    if (sizeValue) sizeValue.textContent = Math.round(size * 100) + '%';
    if (posXValue) posXValue.textContent = posX.toFixed(1);
    if (posYValue) posYValue.textContent = posY.toFixed(1);
    if (rotationValue) rotationValue.textContent = rotation + '°';
}

// Initialize 3D viewer
function myInit3DViewer(modelPath) {
    const viewerContainer = document.getElementById('modelViewer');
    if (!viewerContainer) return;
    
    // Clear old content
    viewerContainer.innerHTML = "";
    
    // Create scene, camera, renderer
    scene = new THREE.Scene();
    camera = new THREE.PerspectiveCamera(75, viewerContainer.offsetWidth / viewerContainer.offsetHeight, 0.1, 1000);
    renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
    renderer.setSize(viewerContainer.offsetWidth, viewerContainer.offsetHeight);
    renderer.setClearColor(0xf8fafc, 1);
    viewerContainer.appendChild(renderer.domElement);

    // Add OrbitControls for user interaction
    controls = new THREE.OrbitControls(camera, renderer.domElement);
    controls.enableDamping = true;
    controls.dampingFactor = 0.05;
    controls.screenSpacePanning = false;
    controls.minDistance = 1;
    controls.maxDistance = 10;
    controls.maxPolarAngle = Math.PI;

    // Add multi-directional lighting
    const ambientLight = new THREE.AmbientLight(0xffffff, 0.3);
    scene.add(ambientLight);
    
    const directionalLight1 = new THREE.DirectionalLight(0xffffff, 0.8);
    directionalLight1.position.set(1, 1, 1);
    scene.add(directionalLight1);
    
    const directionalLight2 = new THREE.DirectionalLight(0xffffff, 0.4);
    directionalLight2.position.set(-1, 0.5, -1);
    scene.add(directionalLight2);
    
    const directionalLight3 = new THREE.DirectionalLight(0xffffff, 0.3);
    directionalLight3.position.set(0, -1, 0);
    scene.add(directionalLight3);

    // Load model
    const loader = new THREE.GLTFLoader();
    loader.load(modelPath, function(gltf) {
        model = gltf.scene;
        scene.add(model);
        
        // Apply color to model if selected
        applyColorToModel();
        
        // Calculate initial bounding box
        const box = new THREE.Box3().setFromObject(model);
        const center = box.getCenter(new THREE.Vector3());
        const size = box.getSize(new THREE.Vector3());
        
        // Center model at origin
        model.position.sub(center);
        
        // Calculate appropriate scale based on model size
        const maxSize = Math.max(size.x, size.y, size.z);
        let scale = 6.0 / maxSize;
        
        // Adjust scale based on model type
        if (maxSize < 1) {
            scale = 8.0 / maxSize;
        } else if (maxSize > 5) {
            scale = 5.0 / maxSize;
        }
        
        model.scale.setScalar(scale);
        
        // Recalculate bounding box after scaling
        const scaledBox = new THREE.Box3().setFromObject(model);
        const scaledSize = scaledBox.getSize(new THREE.Vector3());
        
        // Center model completely on all axes
        const scaledCenter = scaledBox.getCenter(new THREE.Vector3());
        model.position.sub(scaledCenter);
        
        // Adjust camera to see entire model
        const distance = Math.max(scaledSize.x, scaledSize.y, scaledSize.z) * 2.0;
        camera.position.set(0, 0, distance);
        camera.lookAt(0, 0, 0);
        
        // Add event listeners for image controls
        addImageControlListeners();
        
        // Start animation loop
        animate();
        
        // Handle resize
        window.addEventListener('resize', function() {
            camera.aspect = viewerContainer.offsetWidth / viewerContainer.offsetHeight;
            camera.updateProjectionMatrix();
            renderer.setSize(viewerContainer.offsetWidth, viewerContainer.offsetHeight);
        });
        
        // Kích hoạt click để dán decal
        enableDecalPlacementOnClick();
        
        // KHÔNG kích hoạt projector placement - chỉ dùng decal 3D
        
    }, undefined, function(error) {
        viewerContainer.innerHTML = '<div class="text-danger p-3">Không thể tải mô hình 3D!</div>';
        console.error(error);
    });
}

// Animation loop
function animate() {
    requestAnimationFrame(animate);
    controls.update();
    renderer.render(scene, camera);
}

// Add image control event listeners
function addImageControlListeners() {
    // Event listeners for sliders
    const sizeSlider = document.getElementById('sizeSlider');
    const posXSlider = document.getElementById('posXSlider');
    const posYSlider = document.getElementById('posYSlider');
    const rotationSlider = document.getElementById('rotationSlider');
    
    if (sizeSlider) sizeSlider.addEventListener('input', updateImageTransform);
    if (posXSlider) posXSlider.addEventListener('input', updateImageTransform);
    if (posYSlider) posYSlider.addEventListener('input', updateImageTransform);
    if (rotationSlider) rotationSlider.addEventListener('input', updateImageTransform);
    
    // Reset button
    const btnResetImage = document.getElementById('btnResetImage');
    if (btnResetImage) {
        btnResetImage.addEventListener('click', resetImageControls);
    }
    
    // Test position button
    const btnTestImage = document.getElementById('btnTestImage');
    if (btnTestImage) {
        btnTestImage.addEventListener('click', testImagePositions);
    }
}

// Test different image positions
function testImagePositions() {
    const posXSlider = document.getElementById('posXSlider');
    const posYSlider = document.getElementById('posYSlider');
    const rotationSlider = document.getElementById('rotationSlider');
    
    if (!posXSlider || !posYSlider || !rotationSlider) return;
    
    const currentPosX = parseFloat(posXSlider.value);
    const currentPosY = parseFloat(posYSlider.value);
    const currentRotation = parseFloat(rotationSlider.value);

    // Test X position
    posXSlider.value = (currentPosX + 0.1).toFixed(1);
    updateImageTransform();
    console.log('Tested X+: Position X =', posXSlider.value);

    posXSlider.value = (currentPosX - 0.1).toFixed(1);
    updateImageTransform();
    console.log('Tested X-: Position X =', posXSlider.value);

    // Test Y position
    posYSlider.value = (currentPosY + 0.1).toFixed(1);
    updateImageTransform();
    console.log('Tested Y+: Position Y =', posYSlider.value);

    posYSlider.value = (currentPosY - 0.1).toFixed(1);
    updateImageTransform();
    console.log('Tested Y-: Position Y =', posYSlider.value);

    // Test rotation
    rotationSlider.value = (currentRotation + 5).toFixed(0);
    updateImageTransform();
    console.log('Tested Rot+: Rotation =', rotationSlider.value);

    rotationSlider.value = (currentRotation - 5).toFixed(0);
    updateImageTransform();
    console.log('Tested Rot-: Rotation =', rotationSlider.value);
}

// AI Suggestion popup functions
function openAIPopup() { 
    const aiPopup = document.getElementById('aiPopup');
    if (aiPopup) aiPopup.style.display = 'block'; 
}

function closeAIPopup() { 
    const aiPopup = document.getElementById('aiPopup');
    if (aiPopup) aiPopup.style.display = 'none'; 
}

function suggestAI() {
    const aiPrompt = document.getElementById('aiPrompt');
    const aiPreview = document.getElementById('aiPreview');
    
    if (!aiPrompt || !aiPreview) return;
    
    const prompt = aiPrompt.value;
    aiPreview.innerHTML = '<div class="ai-loading">Đang tạo gợi ý...</div>';
    
    // Simulate AI call, return demo image after 2s
    setTimeout(() => {
        aiPreview.innerHTML = `<img src='https://placehold.co/300x200?text=${encodeURIComponent(prompt||'AI Design')}' style='max-width:100%;border-radius:16px;box-shadow:0 2px 8px #a78bfa33;' />`;
    }, 2000);
} 

// Enhanced decal placement - chỉ dùng Decal 3D, không dùng UV mapping cho preview
function enableDecalPlacementOnClick() {
    console.log('🎯 Enabling 3D decal placement (NO UV mapping for preview)');
    
    const canvas = renderer.domElement;
    const raycaster = new THREE.Raycaster();
    const mouse = new THREE.Vector2();
    
    canvas.addEventListener('click', function(event) {
        if (!currentImageUrl) {
            console.log('❌ No image selected for placement');
            return;
        }
        
        console.log('🖱️ Click detected - placing 3D decal...');
        
        const rect = canvas.getBoundingClientRect();
        mouse.x = ((event.clientX - rect.left) / rect.width) * 2 - 1;
        mouse.y = -((event.clientY - rect.top) / rect.height) * 2 + 1;
        raycaster.setFromCamera(mouse, camera);
        
        // Tìm tất cả mesh trong model
        const meshes = [];
        model.traverse(child => { if (child.isMesh) meshes.push(child); });
        const intersects = raycaster.intersectObjects(meshes, true);
        
        if (intersects.length > 0 && currentImageUrl) {
            const intersect = intersects[0];
            console.log('🎯 Hit detected on mesh:', intersect.object.name || 'unnamed');
            
            // Bước 1: Lưu thông tin 3D decal
            lastDecalPosition = intersect.point.clone();
            lastDecalNormal = intersect.face.normal.clone().transformDirection(intersect.object.matrixWorld);
            lastDecalMeshTarget = intersect.object; // Lưu lại mesh target
            
            console.log('📍 3D Position:', lastDecalPosition);
            console.log('📐 Normal:', lastDecalNormal);
            
            // Bước 2: Lấy thông tin UV nếu có (chỉ để export)
            if (intersect.uv) {
                uvClick = intersect.uv.clone();
                
                // Lấy giá trị điều chỉnh từ sliders
                const sizeSlider = document.getElementById('sizeSlider');
                const rotationSlider = document.getElementById('rotationSlider');
                const size = parseFloat(sizeSlider?.value || 1);
                const rotation = parseFloat(rotationSlider?.value || 0);
                
                // Lưu thông tin cho export
                exportLogoInfo = {
                    uv: intersect.uv.clone(),
                    scale: size * 0.3, // Convert slider value to actual scale
                    rotation: rotation
                };
                
                console.log('📊 UV info saved for export:', exportLogoInfo);
            }
            
            // Bước 3: Tạo decal 3D cho preview (KHÔNG dùng UV mapping)
            const orientation = new THREE.Euler().setFromVector3(lastDecalNormal);
            const size = parseFloat(document.getElementById('sizeSlider')?.value || 1);
            const rotation = parseFloat(document.getElementById('rotationSlider')?.value || 0);
            orientation.z += THREE.MathUtils.degToRad(rotation);
            
            console.log('🎨 Creating 3D decal with size:', size, 'rotation:', rotation);
            placeDecal(intersect.object, lastDecalPosition, orientation, size, currentImageUrl);
            
            // KHÔNG gọi drawUVLogo() - để tránh xung đột với decal 3D
            console.log('✅ 3D decal placement completed - NO UV mapping applied to preview');
        } else {
            console.log('❌ No valid intersection found');
        }
    });
}

// Enhanced placeDecal - tạo decal 3D mượt mà, không bị cắt
function placeDecal(mesh, position, orientation, size, imageUrl) {
    console.log('=== PLACING 3D DECAL ===');
    console.log('Position:', position);
    console.log('Orientation:', orientation);
    console.log('Size:', size);
    console.log('Image URL:', imageUrl);
    
    // Xóa tất cả decal cũ
    if (decalMeshes.length > 0) {
        console.log('Removing all old decals...');
        for (const d of decalMeshes) {
            scene.remove(d);
            d.geometry.dispose();
            d.material.dispose();
        }
        decalMeshes = [];
        decalMesh = null;
    }
    
    const textureLoader = new THREE.TextureLoader();
    textureLoader.load(imageUrl, function(texture) {
        // Tăng kích thước decal để tránh bị cắt
        const decalSize = new THREE.Vector3(size * 1.5, size * 1.5, 1.0);
        
        console.log('Creating DecalGeometry with size:', decalSize);
        
        // Tạo DecalGeometry với kích thước lớn hơn để tránh cắt
        const decalGeometry = new THREE.DecalGeometry(mesh, position, orientation, decalSize);
        
        // Tạo material với alpha blending tốt và không bị cắt
        const decalMaterial = new THREE.MeshBasicMaterial({
            map: texture,
            transparent: true,
            alphaTest: 0.01, // Giảm alpha test để tránh cắt
            depthTest: true,
            depthWrite: false,
            polygonOffset: true,
            polygonOffsetFactor: -4,
            polygonOffsetUnits: -4,
            side: THREE.DoubleSide, // Hiển thị cả 2 mặt
        });
        
        decalMesh = new THREE.Mesh(decalGeometry, decalMaterial);
        scene.add(decalMesh);
        decalMeshes.push(decalMesh); // Lưu vào mảng
        
        console.log('✅ 3D Decal placed successfully - logo will NOT be cut');
        console.log('Decal size:', decalSize, 'Original size:', size);
        console.log('Decal geometry vertices:', decalGeometry.attributes.position.count);
        renderer.render(scene, camera);
    });
}

// Enhanced drawUVLogo - chỉ dùng cho export, không dùng cho preview 3D
function drawUVLogo() {
    console.log('🎨 Drawing UV logo for export only...');
    
    if (!uvCtx || !uvBaseImage || !uvLogoImage) {
        console.log('❌ UV drawing failed - missing context or images');
        return;
    }
    
    // Vẽ lại nền
    uvCtx.clearRect(0, 0, uvCanvas.width, uvCanvas.height);
    uvCtx.drawImage(uvBaseImage, 0, 0, uvCanvas.width, uvCanvas.height);
    
    // Vẽ UV island outline
    if (uvIslandPath) {
        uvCtx.strokeStyle = '#ff0000';
        uvCtx.lineWidth = 2;
        uvCtx.stroke(uvIslandPath);
        console.log('📏 UV island outline drawn on canvas');
    }
    
    // Tính toán vị trí, scale, xoay
    const { scale, rotation } = uvLogoParams;
    
    // Sử dụng UV click nếu có, ngược lại dùng giá trị mặc định
    let uvx = uvClick ? uvClick.x : uvLogoParams.x;
    let uvy = uvClick ? 1 - uvClick.y : uvLogoParams.y; // đảo trục Y
    
    const logoW = uvLogoImage.width * scale;
    const logoH = uvLogoImage.height * scale;
    const posX = uvCanvas.width * uvx;
    const posY = uvCanvas.height * uvy;
    
    console.log('📍 UV logo position - X:', posX, 'Y:', posY, 'W:', logoW, 'H:', logoH);
    
    uvCtx.save();
    uvCtx.translate(posX, posY);
    uvCtx.rotate(rotation * Math.PI / 180);
    uvCtx.drawImage(uvLogoImage, -logoW/2, -logoH/2, logoW, logoH);
    uvCtx.restore();
    
    // Tạo lại texture (chỉ cho export, không áp dụng lên 3D)
    if (uvTexture) uvTexture.dispose();
    uvTexture = new THREE.CanvasTexture(uvCanvas);
    uvTexture.needsUpdate = true;
    
    // KHÔNG gán lại material cho 3D mesh - để tránh xung đột với decal
    // uvMaterial chỉ dùng cho export
    if (!uvMaterial) {
        uvMaterial = new THREE.MeshBasicMaterial({ map: uvTexture });
    } else {
        uvMaterial.map = uvTexture;
        uvMaterial.needsUpdate = true;
    }
    
    console.log('✅ UV logo drawn successfully - texture created for export only');
}

// Hàm tạo projector camera
function createProjectorCamera(position, lookAt) {
    const cam = new THREE.PerspectiveCamera(45, 1, 0.1, 100);
    cam.position.copy(position);
    cam.lookAt(lookAt);
    cam.updateMatrixWorld();
    cam.updateProjectionMatrix();
    return cam;
}

// Hàm tạo material projective mapping
function createProjectiveMaterial(baseMaterial, projector, texture) {
    const mat = baseMaterial.clone();
    mat.onBeforeCompile = shader => {
        shader.uniforms.projectorMatrix = { value: new THREE.Matrix4().multiplyMatrices(projector.projectionMatrix, projector.matrixWorldInverse) };
        shader.uniforms.projectorTexture = { value: texture };
        shader.fragmentShader = `
            uniform sampler2D projectorTexture;
            uniform mat4 projectorMatrix;
            varying vec3 vWorldPosition;
        ` + shader.fragmentShader;
        shader.fragmentShader = shader.fragmentShader.replace(
            '#include <map_fragment>',
            `
            // Projective texture mapping
            vec4 projCoord = projectorMatrix * vec4(vWorldPosition, 1.0);
            projCoord.xyz /= projCoord.w;
            vec2 uvProj = projCoord.xy * 0.5 + 0.5;
            if(uvProj.x >= 0.0 && uvProj.x <= 1.0 && uvProj.y >= 0.0 && uvProj.y <= 1.0) {
                vec4 projColor = texture2D(projectorTexture, uvProj);
                diffuseColor.rgb = mix(diffuseColor.rgb, projColor.rgb, projColor.a);
            }
            `
        );
        shader.vertexShader = shader.vertexShader.replace(
            '#include <worldpos_vertex>',
            `#include <worldpos_vertex>
            vWorldPosition = worldPosition.xyz;
            `
        );
    };
    mat.defines = mat.defines || {};
    mat.defines['USE_UV'] = '';
    mat.needsUpdate = true;
    return mat;
}

// Hàm export logo ra PNG đúng vị trí UV
function exportLogoToTexture(logoImg, uvInfo, canvasSize = 1024) {
    const canvas = document.createElement('canvas');
    canvas.width = canvas.height = canvasSize;
    const ctx = canvas.getContext('2d');
    ctx.fillStyle = '#fff';
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    // Tính vị trí logo trên canvas UV
    const posX = uvInfo.uv.x * canvas.width;
    const posY = (1 - uvInfo.uv.y) * canvas.height;
    // Tính kích thước logo
    const logoW = logoImg.width * uvInfo.scale;
    const logoH = logoImg.height * uvInfo.scale;
    ctx.save();
    ctx.translate(posX, posY);
    ctx.rotate(uvInfo.rotation * Math.PI / 180);
    ctx.drawImage(logoImg, -logoW/2, -logoH/2, logoW, logoH);
    ctx.restore();
    // Xuất file PNG
    const dataURL = canvas.toDataURL('image/png');
    downloadImage(dataURL, 'logo_export.png');
}

function downloadImage(dataUrl, filename) {
    const a = document.createElement('a');
    a.href = dataUrl;
    a.download = filename;
    a.click();
}

// Hàm xuất thông tin đơn hàng: logo, thông số decal, ảnh preview 3D
function exportOrderInfo() {
    // 1. Lấy thông tin logo
    const logoImage = currentImageUrl;
    // 2. Lấy thông số decal
    const decalInfo = {
        position: lastDecalPosition ? lastDecalPosition.toArray() : null,
        normal: lastDecalNormal ? lastDecalNormal.toArray() : null,
        meshName: lastDecalMeshTarget ? (lastDecalMeshTarget.name || '(unnamed)') : null,
        scale: document.getElementById('sizeSlider')?.value || null,
        rotation: document.getElementById('rotationSlider')?.value || null
    };
    // 3. Chụp lại ảnh preview 3D
    const screenshotDataUrl = captureScreenshot();

    // 4. Hiển thị popup cho user xem lại
    const popup = document.createElement('div');
    popup.style.cssText = `position:fixed;top:0;left:0;width:100vw;height:100vh;z-index:99999;background:rgba(0,0,0,0.7);display:flex;align-items:center;justify-content:center;`;
    const content = document.createElement('div');
    content.style.cssText = 'background:#fff;padding:24px;border-radius:12px;max-width:90vw;max-height:90vh;overflow:auto;box-shadow:0 4px 32px #0002;';
    content.innerHTML = `
        <h4>Thông tin đơn hàng</h4>
        <b>Ảnh logo user upload:</b><br><img src="${logoImage}" style="max-width:200px;max-height:120px;border:1px solid #ccc;border-radius:8px;"><br><br>
        <b>Thông số decal:</b><br>
        <pre style="background:#f8f8f8;padding:8px;border-radius:6px;">${JSON.stringify(decalInfo, null, 2)}</pre>
        <b>Ảnh chụp mẫu 3D (giống preview):</b><br><img src="${screenshotDataUrl}" style="max-width:400px;max-height:300px;border:1px solid #ccc;border-radius:8px;">
        <br><br>
        <button class="btn btn-primary" onclick="this.parentElement.parentElement.remove()">Đóng</button>
    `;
    popup.appendChild(content);
    document.body.appendChild(popup);

    // 5. Nếu muốn gửi về server, có thể dùng AJAX tại đây
    // sendOrderToServer({logoImage, decalInfo, screenshotDataUrl});
}

// Hàm lưu thông tin thiết kế và chuyển sang trang thanh toán
function saveDesignAndCheckout() {
    console.log('[DEBUG] saveDesignAndCheckout called');
    
    // Kiểm tra session trước
    console.log('[DEBUG] Checking session...');
    
    // Lấy thông tin như đã làm ở exportOrderInfo
    const logoImage = currentImageUrl;
    const decalInfo = {
        position: lastDecalPosition ? lastDecalPosition.toArray() : null,
        normal: lastDecalNormal ? lastDecalNormal.toArray() : null,
        meshName: lastDecalMeshTarget ? (lastDecalMeshTarget.name || '(unnamed)') : null,
        scale: document.getElementById('sizeSlider')?.value || null,
        rotation: document.getElementById('rotationSlider')?.value || null
    };
    const screenshotDataUrl = captureScreenshot();
    
    console.log('[DEBUG] Design info prepared:', { logoImage, decalInfo, screenshotDataUrl });
    
    // Lưu vào localStorage
    localStorage.setItem('orderDesign', JSON.stringify({
        logoImage, decalInfo, screenshotDataUrl
    }));
    
    // Lấy thông tin sản phẩm từ URL parameters
    const urlParams = new URLSearchParams(window.location.search);
    const productId = urlParams.get('productId');
    const size = urlParams.get('size');
    const color = urlParams.get('color');
    const quantity = urlParams.get('quantity') || '1';
    
    console.log('[DEBUG] Product info from URL:', { productId, size, color, quantity });
    
    // Kiểm tra dữ liệu đầu vào
    if (!productId) {
        // Removed debug alert - Lỗi: Không tìm thấy thông tin sản phẩm. Vui lòng thử lại.
        return;
    }
    
    // Lưu design vào database trước
    saveDesignToDatabase(logoImage, decalInfo, screenshotDataUrl, productId, () => {
        // Sau khi lưu design thành công, thêm vào giỏ hàng
        addToCartAndCheckout(productId, size, color, quantity);
    });
}

// Hàm mới: Lưu design vào database với đầy đủ thông tin
function saveDesignToDatabase(logoImage, decalInfo, screenshotDataUrl, productId, callback) {
    console.log('[DEBUG] Saving design to database...');
    
    // Tạo tên design tự động
    const designName = `Design_${new Date().toISOString().slice(0, 19).replace(/:/g, '-')}`;
    
    // Lưu ảnh preview trước
    savePreviewImage(screenshotDataUrl, designName, (previewPath) => {
        // Lưu logo vào file trước
        saveLogoImage(logoImage, designName, (logoPath) => {
            // Chuẩn bị dữ liệu design theo cấu trúc bảng Designs
            const designData = {
                name: designName,
                logoPath: logoPath,
                positionData: JSON.stringify(decalInfo),
                previewPath: previewPath || '', // Handle null screenshot
                isPublic: false, // Mặc định không công khai
                createdAt: new Date().toISOString(),
                updatedAt: new Date().toISOString()
            };
            
            console.log('[DEBUG] Design data to save:', designData);
            
            // Tạo FormData để gửi lên server
            const formData = new FormData();
            formData.append('action', 'saveDesign');
            formData.append('productId', productId);
            formData.append('designData', JSON.stringify(designData));
            
            // Hiển thị loading
            const btnCheckout = document.getElementById('btnCheckout');
            if (btnCheckout) {
                btnCheckout.disabled = true;
                btnCheckout.textContent = 'Đang lưu thiết kế...';
            }
            
            // Gửi request lưu design
            fetch('Design.aspx', {
                method: 'POST',
                body: formData
            })
            .then(response => response.text())
            .then(result => {
                console.log('[DEBUG] Save design result (raw):', result);
                
                try {
                    // Tách JSON từ response nếu có HTML
                    let jsonStart = result.indexOf('{');
                    let jsonEnd = result.lastIndexOf('}') + 1;
                    let jsonString = result;
                    
                    if (jsonStart >= 0 && jsonEnd > jsonStart) {
                        jsonString = result.substring(jsonStart, jsonEnd);
                    }
                    
                    console.log('[DEBUG] Parsed JSON string:', jsonString);
                    const response = JSON.parse(jsonString);
                    console.log('[DEBUG] Parsed response object:', response);
                    
                    if (response.success) {
                        console.log('[SUCCESS] Design saved successfully, DesignId:', response.designId);
                        console.log('[DEBUG] DesignId nhận được từ backend:', response.designId);
                        
                        // Lưu DesignId vào localStorage để dùng sau
                        localStorage.setItem('savedDesignId', response.designId);
                        
                        // Hiển thị thông tin design đã lưu
                        showDesignInfo(response.designId, designData);
                        
                        // Reset button
                        if (btnCheckout) {
                            btnCheckout.disabled = false;
                            btnCheckout.textContent = 'Thanh toán';
                        }
                        
                        // Gọi callback để tiếp tục
                        if (callback) callback();
                        
                    } else {
                        console.error('[ERROR] Failed to save design:', response.message);
                        // Removed debug alert - Lỗi lưu thiết kế: ' + response.message
                        
                        // Reset button
                        if (btnCheckout) {
                            btnCheckout.disabled = false;
                            btnCheckout.textContent = 'Thanh toán';
                        }
                    }
                } catch (e) {
                    console.error('[ERROR] Invalid JSON response:', result);
                    console.error('[ERROR] JSON parse error:', e);
                    // Removed debug alert - Lỗi phản hồi từ server: ' + result
                    
                    // Reset button
                    if (btnCheckout) {
                        btnCheckout.disabled = false;
                        btnCheckout.textContent = 'Thanh toán';
                    }
                }
            })
            .catch(error => {
                console.error('[ERROR] Network error saving design:', error);
                // Removed debug alert - Lỗi kết nối khi lưu thiết kế: ' + error.message
                
                // Reset button
                if (btnCheckout) {
                    btnCheckout.disabled = false;
                    btnCheckout.textContent = 'Thanh toán';
                }
            });
        });
    });
}

// Hàm mới: Lưu logo vào server
function saveLogoImage(dataUrl, designName, callback) {
    console.log('[DEBUG] Saving logo image...');
    
    // Tạo tên file
    const timestamp = new Date().toISOString().slice(0, 19).replace(/:/g, '-');
    const fileName = `logo_${designName}_${timestamp}.png`;
    
    // Tạo FormData để gửi ảnh
    const formData = new FormData();
    formData.append('action', 'saveLogoImage');
    formData.append('imageData', dataUrl);
    formData.append('fileName', fileName);
    
    // Gửi request lưu ảnh
    fetch('Design.aspx', {
        method: 'POST',
        body: formData
    })
    .then(response => response.text())
    .then(result => {
        console.log('[DEBUG] Save logo image result:', result);
        
        try {
            // Tách JSON từ response nếu có HTML
            let jsonStart = result.indexOf('{');
            let jsonEnd = result.lastIndexOf('}') + 1;
            let jsonString = result;
            
            if (jsonStart >= 0 && jsonEnd > jsonStart) {
                jsonString = result.substring(jsonStart, jsonEnd);
            }
            
            const response = JSON.parse(jsonString);
            
            if (response.success) {
                console.log('[SUCCESS] Logo image saved:', response.imagePath);
                callback(response.imagePath);
            } else {
                console.error('[ERROR] Failed to save logo image:', response.message);
                // Fallback: sử dụng dataUrl nếu không lưu được
                callback(dataUrl);
            }
        } catch (e) {
            console.error('[ERROR] Invalid JSON response for logo image:', result);
            // Fallback: sử dụng dataUrl nếu không parse được
            callback(dataUrl);
        }
    })
    .catch(error => {
        console.error('[ERROR] Network error saving logo image:', error);
        // Fallback: sử dụng dataUrl nếu có lỗi network
        callback(dataUrl);
    });
}

// Hàm mới: Lưu ảnh preview vào server
function savePreviewImage(dataUrl, designName, callback) {
    console.log('[DEBUG] Saving preview image...');
    
    // Check if screenshot data is valid
    if (!dataUrl || dataUrl === 'data:,' || dataUrl === 'data:image/png;base64,') {
        console.log('[WARNING] Invalid screenshot data, skipping preview image save');
        callback(''); // Return empty string for invalid screenshot
        return;
    }
    
    // Tạo tên file
    const timestamp = new Date().toISOString().slice(0, 19).replace(/:/g, '-');
    const fileName = `preview_${designName}_${timestamp}.png`;
    
    // Tạo FormData để gửi ảnh
    const formData = new FormData();
    formData.append('action', 'savePreviewImage');
    formData.append('imageData', dataUrl);
    formData.append('fileName', fileName);
    
    // Gửi request lưu ảnh
    fetch('Design.aspx', {
        method: 'POST',
        body: formData
    })
    .then(response => response.text())
    .then(result => {
        console.log('[DEBUG] Save preview image result:', result);
        
        try {
            // Tách JSON từ response nếu có HTML
            let jsonStart = result.indexOf('{');
            let jsonEnd = result.lastIndexOf('}') + 1;
            let jsonString = result;
            
            if (jsonStart >= 0 && jsonEnd > jsonStart) {
                jsonString = result.substring(jsonStart, jsonEnd);
            }
            
            const response = JSON.parse(jsonString);
            
            if (response.success) {
                console.log('[SUCCESS] Preview image saved:', response.imagePath);
                callback(response.imagePath);
            } else {
                console.error('[ERROR] Failed to save preview image:', response.message);
                // Fallback: sử dụng dataUrl nếu không lưu được
                callback(dataUrl);
            }
        } catch (e) {
            console.error('[ERROR] Invalid JSON response for preview image:', result);
            // Fallback: sử dụng dataUrl nếu không parse được
            callback(dataUrl);
        }
    })
    .catch(error => {
        console.error('[ERROR] Network error saving preview image:', error);
        // Fallback: sử dụng dataUrl nếu có lỗi network
        callback(dataUrl);
    });
}

// Hàm thêm vào giỏ hàng và chuyển sang checkout
function addToCartAndCheckout(productId, size, color, quantity) {
    // Kiểm tra dữ liệu đầu vào
    if (!productId) {
        // Removed debug alert - Lỗi: Không tìm thấy thông tin sản phẩm. Vui lòng thử lại.
        return;
    }
    // Lấy thông tin thiết kế từ localStorage
    const orderDesign = localStorage.getItem('orderDesign');
    const savedDesignId = localStorage.getItem('savedDesignId');
    // Kiểm tra DesignId hợp lệ
    if (!savedDesignId || savedDesignId === 'null' || savedDesignId === 'undefined' || isNaN(Number(savedDesignId))) {
        // Removed debug alert - Lỗi: Không tìm thấy DesignId hợp lệ. Vui lòng lưu thiết kế trước khi đặt hàng!
        return;
    }
    // Tạo form data để gửi lên server
    const formData = new FormData();
    formData.append('productId', productId);
    formData.append('size', size || 'M');
    formData.append('color', color || 'white');
    formData.append('quantity', quantity || '1');
    formData.append('customDesign', 'true'); // Đánh dấu là sản phẩm có thiết kế custom
    // Thêm DesignId nếu đã lưu thành công
    formData.append('designId', savedDesignId);
    // Thêm thông tin thiết kế nếu có
    if (orderDesign) {
        formData.append('designData', orderDesign);
    }
    // Hiển thị loading
    const btnCheckout = document.getElementById('btnCheckout');
    if (btnCheckout) {
        btnCheckout.disabled = true;
        btnCheckout.textContent = 'Đang thêm vào giỏ hàng...';
    }
    // Gửi request thêm vào giỏ hàng
    fetch('AddToCart.aspx', {
        method: 'POST',
        body: formData
    })
    .then(response => response.text())
    .then(result => {
        // Kiểm tra kết quả
        if (result.includes('SUCCESS')) {
            // Delay 3 giây trước khi chuyển trang
            setTimeout(() => {
                // Lấy lại savedDesignId từ localStorage
                const savedDesignId = localStorage.getItem('savedDesignId');
                window.location.href = 'Checkout.aspx?designId=' + encodeURIComponent(savedDesignId);
            }, 3000);
        } else if (result.includes('ERROR')) {
            // Kiểm tra nếu lỗi là do user chưa đăng nhập
            if (result.includes('not logged in')) {
                // Removed debug alert - Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng. Vui lòng đăng nhập trước.
                // Chuyển đến trang login
                window.location.href = 'Login.aspx';
            } else {
                // Removed debug alert - Lỗi thêm vào giỏ hàng: ' + result
            }
            // Reset button
            if (btnCheckout) {
                btnCheckout.disabled = false;
                btnCheckout.textContent = 'Thanh toán';
            }
        } else {
            // Vẫn chuyển sang checkout nếu không rõ lỗi
            setTimeout(() => {
                window.location.href = 'Checkout.aspx';
            }, 3000);
        }
    })
    .catch(error => {
        // Reset button
        if (btnCheckout) {
            btnCheckout.disabled = false;
            btnCheckout.textContent = 'Thanh toán';
        }
    });
}

// Gắn vào nút Thanh toán (btnCheckout)
window.addEventListener('DOMContentLoaded', function() {
    console.log('[DEBUG] DOMContentLoaded - Starting event binding...');
    
    setTimeout(() => {
        const btnCheckout = document.getElementById('btnCheckout');
        console.log('[DEBUG] Looking for btnCheckout:', btnCheckout);
        
        if (btnCheckout) {
            btnCheckout.type = 'button';
            console.log('[Bind] Gắn event Thanh toán cho nút:', btnCheckout.id);
            btnCheckout.addEventListener('click', function(e) {
                e.preventDefault();
                e.stopPropagation();
                console.log('[Event] Checkout button clicked!');
                saveDesignAndCheckout();
            });
        } else {
            console.warn('[Bind] Không tìm thấy nút Thanh toán!');
        }
        
        // Nếu các nút nằm trong form, chặn submit form toàn cục
        const forms = document.querySelectorAll('form');
        forms.forEach(f => {
            f.addEventListener('submit', function(ev) {
                ev.preventDefault();
                ev.stopPropagation();
                console.log('[Form] Chặn submit form để không reload trang!');
            });
        });
    }, 1000);
});

// Add workflow instructions to UI
function addWorkflowInstructions() {
    const instructions = document.createElement('div');
    instructions.className = 'alert alert-info mt-3';
    instructions.innerHTML = `
        <h6>🎯 Workflow 3D T-shirt Design:</h6>
        <ol>
            <li><strong>3D Preview:</strong> Click vào áo để đặt logo - logo sẽ KHÔNG bị cắt</li>
            <li><strong>Adjust:</strong> Dùng sliders để điều chỉnh kích thước, xoay</li>
            <li><strong>Export:</strong> Click "Export Logo PNG" để xuất file in áo</li>
        </ol>
        <small class="text-muted">💡 Logo chỉ bị cắt khi export PNG, không bị cắt trong 3D preview</small>
    `;
    
    const container = document.getElementById('imageControls') || document.body;
    container.appendChild(instructions);
}

// Hàm mới: Hiển thị thông tin design đã lưu
function showDesignInfo(designId, designData) {
    console.log('[DEBUG] Showing design info:', { designId, designData });
    
    const popup = document.createElement('div');
    popup.style.cssText = `
        position: fixed; top: 0; left: 0; width: 100vw; height: 100vh;
        z-index: 99999; background: rgba(30,32,48,0.85);
        display: flex; align-items: center; justify-content: center;
        backdrop-filter: blur(2px);
        padding: 20px;
        box-sizing: border-box;
    `;
    
    const content = document.createElement('div');
    content.style.cssText = `
        background: linear-gradient(135deg, #f8fafc 60%, #e0e7ff 100%);
        padding: 36px 32px 28px 32px; border-radius: 22px;
        width: 100%; max-width: 800px; max-height: 90vh; 
        overflow-y: auto; overflow-x: hidden;
        box-shadow: 0 8px 40px #0003, 0 1.5px 8px #6366f180;
        border: 1.5px solid #e0e7ff;
        animation: popup-fade-in 0.4s cubic-bezier(.4,2,.6,1);
        margin: auto;
        position: relative;
    `;
    
    content.innerHTML = `
        <div style="text-align:center;">
            <i class="fas fa-check-circle" style="font-size:3.2rem;color:#22c55e;filter:drop-shadow(0 2px 8px #22c55e44);"></i>
            <h3 style="margin:18px 0 8px 0;font-weight:700;color:#1e293b;">Thiết kế đã lưu thành công!</h3>
            <div class="alert alert-success mt-2" style="font-size:1.1rem;border-radius:12px;">
                <i class="fas fa-info-circle"></i>
                Thiết kế đã được lưu vào database!<br>
                Bây giờ bạn có thể thêm vào giỏ hàng và thanh toán.
            </div>
        </div>
        <div class="row mt-3" style="gap:12px;margin:0;">
            <div class="col-md-6" style="min-width:220px;flex:1;">
                <h6 class="mb-3" style="color:#6366f1;font-weight:600;font-size:1.1rem;border-bottom:2px solid #e0e7ff;padding-bottom:8px;">
                    <i class="fas fa-info-circle me-2"></i> Thông tin thiết kế
                </h6>
                <div style="background:#f8fafc;padding:16px;border-radius:12px;border:1px solid #e0e7ff;">
                    <div class="mb-3">
                        <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:8px;">
                            <span style="font-weight:600;color:#374151;">ID:</span>
                            <span style="background:#6366f1;color:white;padding:4px 8px;border-radius:6px;font-size:0.9rem;font-weight:600;">${designId}</span>
                        </div>
                        <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:8px;">
                            <span style="font-weight:600;color:#374151;">Tên:</span>
                            <span style="color:#6b7280;font-size:0.95rem;">${designData.name}</span>
                        </div>
                        <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:8px;">
                            <span style="font-weight:600;color:#374151;">Ngày tạo:</span>
                            <span style="color:#6b7280;font-size:0.95rem;">${new Date(designData.createdAt).toLocaleString('vi-VN')}</span>
                        </div>
                    </div>
                    <div class="mb-3">
                        <div style="margin-bottom:12px;">
                            <span style="font-weight:600;color:#374151;display:block;margin-bottom:6px;">Logo:</span>
                            <img src="${designData.logoPath}" style="max-width:120px;max-height:80px;border:2px solid #e0e7ff;border-radius:10px;box-shadow:0 4px 12px #6366f133;object-fit:cover;">
                        </div>
                        <div>
                            <span style="font-weight:600;color:#374151;display:block;margin-bottom:6px;">Preview:</span>
                            <img src="${designData.previewPath}" style="max-width:180px;max-height:120px;border:2px solid #e0e7ff;border-radius:10px;box-shadow:0 4px 12px #6366f133;object-fit:cover;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6" style="min-width:220px;flex:1;">
                <h6 class="mb-3" style="color:#6366f1;font-weight:600;font-size:1.1rem;border-bottom:2px solid #e0e7ff;padding-bottom:8px;">
                    <i class="fas fa-paint-brush me-2"></i> Thông số decal
                </h6>
                <div style="background:#f1f5f9;padding:16px;border-radius:12px;border:1px solid #e0e7ff;">
                    <pre style="background:#ffffff;padding:12px;border-radius:8px;font-size:0.9rem;max-height:200px;overflow:auto;border:1px solid #e0e7ff;margin:0;white-space:pre-wrap;word-wrap:break-word;color:#374151;">${designData.positionData}</pre>
                </div>
            </div>
        </div>
        <div class="text-center mt-4">
            <button class="btn btn-primary" style="padding:12px 36px;font-size:1.15rem;border-radius:24px;font-weight:600;box-shadow:0 2px 12px #6366f133;transition:all .2s;" onclick="this.parentElement.parentElement.parentElement.remove()">
                <i class="fas fa-check"></i> Tiếp tục
            </button>
        </div>
    `;
    
    // Add fade-in animation
    const style = document.createElement('style');
    style.innerHTML = `
    @keyframes popup-fade-in {
        from { 
            opacity: 0; 
            transform: translateY(40px) scale(0.98);
        }
        to { 
            opacity: 1; 
            transform: none;
        }
    }
    
    @media (max-width: 768px) {
        .popup-content {
            margin: 10px;
            padding: 20px 16px;
            max-height: 95vh;
        }
    }
    `;
    document.head.appendChild(style);
    
    popup.appendChild(content);
    document.body.appendChild(popup);
    
    // Thêm class cho responsive
    content.classList.add('popup-content');
}

// Function to apply color to 3D model
function applyColorToModel() {
    if (!model) return;
    
    // Get selected color from URL parameters or hidden field
    const urlParams = new URLSearchParams(window.location.search);
    let selectedColor = urlParams.get('color');
    
    // If not in URL, try to get from hidden field
    if (!selectedColor) {
        const hdnColor = document.getElementById('hdnColor');
        if (hdnColor) {
            selectedColor = hdnColor.value;
        }
    }
    
    // If still no color, try window variable
    if (!selectedColor && window.selectedColor) {
        selectedColor = window.selectedColor;
    }
    
    console.log('[DEBUG] Selected color:', selectedColor);
    
    if (!selectedColor) {
        console.log('[DEBUG] No color selected, using default');
        return;
    }
    
    // Convert color name to hex value
    const colorHex = colorMap[selectedColor.toLowerCase()];
    if (!colorHex) {
        console.log('[DEBUG] Color not found in map:', selectedColor);
        return;
    }
    
    console.log('[DEBUG] Applying color:', selectedColor, '->', colorHex.toString(16));
    
    // Apply color to all meshes in the model
    model.traverse(function(child) {
        if (child.isMesh && child.material) {
            // Store original material if not already stored
            if (!originalMaterials.has(child)) {
                originalMaterials.set(child, child.material.clone());
            }
            
            // Create new material with the selected color
            const newMaterial = child.material.clone();
            
            // Remove any existing textures to ensure color shows properly
            newMaterial.map = null;
            newMaterial.normalMap = null;
            newMaterial.roughnessMap = null;
            newMaterial.metalnessMap = null;
            newMaterial.emissiveMap = null;
            
            if (newMaterial.isMeshStandardMaterial || newMaterial.isMeshPhongMaterial) {
                newMaterial.color.setHex(colorHex);
                // Set material properties for better color display
                newMaterial.roughness = 0.7;
                newMaterial.metalness = 0.0;
                newMaterial.emissive.setHex(0x000000);
                newMaterial.emissiveIntensity = 0.0;
            } else if (newMaterial.isMeshBasicMaterial) {
                newMaterial.color.setHex(colorHex);
            } else if (newMaterial.isMeshLambertMaterial) {
                newMaterial.color.setHex(colorHex);
            }
            
            // Force material update
            newMaterial.needsUpdate = true;
            
            child.material = newMaterial;
        }
    });
    
    console.log('[DEBUG] Color applied successfully');
}

// Function to reset model to original colors
function resetModelColor() {
    if (!model) return;
    
    console.log('[DEBUG] Resetting model to original colors');
    
    model.traverse(function(child) {
        if (child.isMesh && originalMaterials.has(child)) {
            child.material = originalMaterials.get(child);
        }
    });
}

// Function to change model color dynamically
function changeModelColor(colorName) {
    if (!model) return;
    
    const colorHex = colorMap[colorName.toLowerCase()];
    if (!colorHex) {
        console.log('[DEBUG] Color not found:', colorName);
        return;
    }
    
    console.log('[DEBUG] Changing model color to:', colorName, '->', colorHex.toString(16));
    
    model.traverse(function(child) {
        if (child.isMesh && child.material) {
            // Remove any existing textures to ensure color shows properly
            child.material.map = null;
            child.material.normalMap = null;
            child.material.roughnessMap = null;
            child.material.metalnessMap = null;
            child.material.emissiveMap = null;
            
            if (child.material.isMeshStandardMaterial || child.material.isMeshPhongMaterial) {
                child.material.color.setHex(colorHex);
                // Set material properties for better color display
                child.material.roughness = 0.7;
                child.material.metalness = 0.0;
                child.material.emissive.setHex(0x000000);
                child.material.emissiveIntensity = 0.0;
            } else if (child.material.isMeshBasicMaterial) {
                child.material.color.setHex(colorHex);
            } else if (child.material.isMeshLambertMaterial) {
                child.material.color.setHex(colorHex);
            }
            
            // Force material update
            child.material.needsUpdate = true;
        }
    });
}

// Function to capture screenshot of 3D scene
function captureScreenshot() {
    console.log('[DEBUG] Capturing screenshot...');
    
    if (!renderer || !scene || !camera) {
        console.error('[ERROR] Cannot capture screenshot - renderer, scene, or camera not available');
        return null;
    }
    
    try {
        // Force render the scene before capturing
        renderer.render(scene, camera);
        
        // Try to capture screenshot
        const screenshotDataUrl = renderer.domElement.toDataURL('image/png');
        
        if (screenshotDataUrl && screenshotDataUrl !== 'data:,') {
            console.log('[SUCCESS] Screenshot captured successfully');
            return screenshotDataUrl;
        } else {
            console.error('[ERROR] Screenshot capture returned empty data');
            return null;
        }
    } catch (error) {
        console.error('[ERROR] Failed to capture screenshot:', error);
        
        // Fallback: try alternative method
        try {
            // Create a temporary canvas and copy the renderer content
            const canvas = document.createElement('canvas');
            const ctx = canvas.getContext('2d');
            canvas.width = renderer.domElement.width;
            canvas.height = renderer.domElement.height;
            
            // Draw the renderer content to the canvas
            ctx.drawImage(renderer.domElement, 0, 0);
            
            const fallbackDataUrl = canvas.toDataURL('image/png');
            console.log('[SUCCESS] Screenshot captured using fallback method');
            return fallbackDataUrl;
        } catch (fallbackError) {
            console.error('[ERROR] Fallback screenshot method also failed:', fallbackError);
            return null;
        }
    }
}