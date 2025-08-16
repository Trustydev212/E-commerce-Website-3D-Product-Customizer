// Site JavaScript Functions

// Initialize site functions when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    initializeSiteFunctions();
});

function initializeSiteFunctions() {
    // Initialize 3D viewer
    initialize3DViewer();
    
    // Initialize product interactions
    initializeProductInteractions();
    
    // Initialize cart functionality
    initializeCartFunctionality();
    
    // Initialize design tools
    initializeDesignTools();
    
    // Initialize chatbot - Commented out to avoid conflicts with separate chatbot.js
    // initializeChatbot();
    
    // Initialize animations
    initializeAnimations();
    
    // Initialize lazy loading
    initializeLazyLoading();
}

// 3D Viewer initialization
function initialize3DViewer() {
    const viewerContainer = document.getElementById('3d-viewer');
    if (!viewerContainer) return;
    
    // Three.js setup
    const scene = new THREE.Scene();
    const camera = new THREE.PerspectiveCamera(75, viewerContainer.offsetWidth / viewerContainer.offsetHeight, 0.1, 1000);
    const renderer = new THREE.WebGLRenderer({ antialias: true });
    
    renderer.setSize(viewerContainer.offsetWidth, viewerContainer.offsetHeight);
    renderer.setClearColor(0xf0f0f0);
    viewerContainer.appendChild(renderer.domElement);
    
    // Add lights
    const ambientLight = new THREE.AmbientLight(0x404040, 0.6);
    scene.add(ambientLight);
    
    const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
    directionalLight.position.set(1, 1, 1);
    scene.add(directionalLight);
    
    // Load 3D model
    const loader = new THREE.GLTFLoader();
    let currentModel = null;
    
    window.load3DModel = function(modelPath) {
        if (currentModel) {
            scene.remove(currentModel);
        }
        
        loader.load(modelPath, function(gltf) {
            currentModel = gltf.scene;
            scene.add(currentModel);
            
            // Center the model
            const box = new THREE.Box3().setFromObject(currentModel);
            const center = box.getCenter(new THREE.Vector3());
            currentModel.position.sub(center);
            
            // Scale the model
            const size = box.getSize(new THREE.Vector3());
            const maxSize = Math.max(size.x, size.y, size.z);
            const scale = 2 / maxSize;
            currentModel.scale.setScalar(scale);
            
            // Position camera
            camera.position.z = 3;
            camera.lookAt(0, 0, 0);
        });
    };
    
    // Animation loop
    function animate() {
        requestAnimationFrame(animate);
        
        if (currentModel) {
            currentModel.rotation.y += 0.01;
        }
        
        renderer.render(scene, camera);
    }
    animate();
    
    // Handle window resize
    window.addEventListener('resize', function() {
        camera.aspect = viewerContainer.offsetWidth / viewerContainer.offsetHeight;
        camera.updateProjectionMatrix();
        renderer.setSize(viewerContainer.offsetWidth, viewerContainer.offsetHeight);
    });
}

// Product interactions
function initializeProductInteractions() {
    // Product image hover effects
    const productImages = document.querySelectorAll('.product-image');
    productImages.forEach(img => {
        img.addEventListener('mouseenter', function() {
            this.style.transform = 'scale(1.05)';
        });
        
        img.addEventListener('mouseleave', function() {
            this.style.transform = 'scale(1)';
        });
    });
    
    // Product quick view
    const quickViewButtons = document.querySelectorAll('.btn-quick-view');
    quickViewButtons.forEach(btn => {
        btn.addEventListener('click', function(e) {
            e.preventDefault();
            const productId = this.getAttribute('data-product-id');
            showProductQuickView(productId);
        });
    });
    
    // Size and color selection
    const sizeOptions = document.querySelectorAll('.size-option');
    sizeOptions.forEach(option => {
        option.addEventListener('click', function() {
            sizeOptions.forEach(o => o.classList.remove('selected'));
            this.classList.add('selected');
        });
    });
    
    const colorOptions = document.querySelectorAll('.color-option');
    colorOptions.forEach(option => {
        option.addEventListener('click', function() {
            colorOptions.forEach(o => o.classList.remove('selected'));
            this.classList.add('selected');
            
            // Update product image based on color
            const color = this.getAttribute('data-color');
            updateProductImage(color);
        });
    });
}

function showProductQuickView(productId) {
    // Fetch product data
    fetch(`/api/products/${productId}`)
        .then(response => response.json())
        .then(product => {
            const modalContent = `
                <div class="row">
                    <div class="col-md-6">
                        <img src="${product.imagePath}" class="img-fluid" alt="${product.name}">
                    </div>
                    <div class="col-md-6">
                        <h3>${product.name}</h3>
                        <p class="product-price">${formatCurrency(product.price)}</p>
                        <p>${product.description}</p>
                        <div class="product-options">
                            <div class="size-selection mb-3">
                                <label>Kích thước:</label>
                                <div class="size-options">
                                    <button class="btn btn-outline-primary size-option" data-size="S">S</button>
                                    <button class="btn btn-outline-primary size-option" data-size="M">M</button>
                                    <button class="btn btn-outline-primary size-option" data-size="L">L</button>
                                    <button class="btn btn-outline-primary size-option" data-size="XL">XL</button>
                                </div>
                            </div>
                            <div class="color-selection mb-3">
                                <label>Màu sắc:</label>
                                <div class="color-options">
                                    <div class="color-option" data-color="white" style="background-color: white;"></div>
                                    <div class="color-option" data-color="black" style="background-color: black;"></div>
                                    <div class="color-option" data-color="red" style="background-color: red;"></div>
                                    <div class="color-option" data-color="blue" style="background-color: blue;"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `;
            
            const actions = `
                <button type="button" class="btn btn-primary" onclick="addToCart(${productId})">Thêm vào giỏ hàng</button>
                <button type="button" class="btn btn-success" onclick="startDesign(${productId})">Thiết kế 3D</button>
            `;
            
            showModal('Chi tiết sản phẩm', modalContent, actions);
        })
        .catch(error => {
            console.error('Error loading product:', error);
            showNotification('Lỗi tải thông tin sản phẩm', 'error');
        });
}

function updateProductImage(color) {
    const productImage = document.querySelector('.product-image');
    if (productImage) {
        const basePath = productImage.src.split('_')[0];
        productImage.src = `${basePath}_${color}.jpg`;
    }
}

// Cart functionality
function initializeCartFunctionality() {
    // Add to cart buttons
    const addToCartButtons = document.querySelectorAll('.btn-add-cart');
    addToCartButtons.forEach(btn => {
        btn.addEventListener('click', function(e) {
            e.preventDefault();
            const productId = this.getAttribute('data-product-id');
            addToCart(productId);
        });
    });
    
    // Quantity controls
    const quantityControls = document.querySelectorAll('.quantity-controls');
    quantityControls.forEach(control => {
        const minusBtn = control.querySelector('.quantity-minus');
        const plusBtn = control.querySelector('.quantity-plus');
        const input = control.querySelector('.quantity-input');
        
        if (minusBtn && plusBtn && input) {
            minusBtn.addEventListener('click', function() {
                const currentValue = parseInt(input.value) || 1;
                if (currentValue > 1) {
                    input.value = currentValue - 1;
                    updateCartItemQuantity(input.getAttribute('data-cart-id'), currentValue - 1);
                }
            });
            
            plusBtn.addEventListener('click', function() {
                const currentValue = parseInt(input.value) || 1;
                input.value = currentValue + 1;
                updateCartItemQuantity(input.getAttribute('data-cart-id'), currentValue + 1);
            });
            
            input.addEventListener('change', function() {
                const newValue = parseInt(this.value) || 1;
                updateCartItemQuantity(this.getAttribute('data-cart-id'), newValue);
            });
        }
    });
    
    // Remove from cart
    const removeButtons = document.querySelectorAll('.btn-remove-cart');
    removeButtons.forEach(btn => {
        if (btn) {
            btn.addEventListener('click', function(e) {
                e.preventDefault();
                const cartId = this.getAttribute('data-cart-id');
                removeFromCart(cartId);
            });
        }
    });
}

function addToCart(productId) {
    const selectedSize = document.querySelector('.size-option.selected')?.getAttribute('data-size') || 'M';
    const selectedColor = document.querySelector('.color-option.selected')?.getAttribute('data-color') || 'white';
    
    const formData = new FormData();
    formData.append('productId', productId);
    formData.append('size', selectedSize);
    formData.append('color', selectedColor);
    formData.append('quantity', '1');
    formData.append('customDesign', 'false');
    
    fetch('AddToCart.aspx', {
        method: 'POST',
        body: formData
    })
    .then(response => response.text())
    .then(result => {
        if (result.includes('SUCCESS')) {
            // Parse the new cart count from response
            const parts = result.split('|');
            const message = parts[0].replace('SUCCESS: ', '');
            const newCartCount = parts.length > 1 ? parts[1] : '0';
            
            showNotification(message, 'success');
            
            // Update cart count in navigation
            const cartCountElement = document.getElementById('cartCount');
            if (cartCountElement) {
                cartCountElement.textContent = newCartCount;
            }
            
            console.log('Cart count updated to:', newCartCount);
        } else {
            const errorMessage = result.replace('ERROR: ', '');
            showNotification('Lỗi thêm vào giỏ hàng: ' + errorMessage, 'error');
        }
    })
    .catch(error => {
        console.error('Error adding to cart:', error);
        showNotification('Lỗi thêm vào giỏ hàng', 'error');
    });
}

function updateCartItemQuantity(cartId, quantity) {
    fetch('/api/cart/update', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            cartId: cartId,
            quantity: quantity
        })
    })
    .then(response => response.json())
    .then(result => {
        if (result.success) {
            updateCartTotal();
        } else {
            showNotification('Lỗi cập nhật số lượng', 'error');
        }
    })
    .catch(error => {
        console.error('Error updating cart:', error);
    });
}

function removeFromCart(cartId) {
    if (confirm('Bạn có chắc chắn muốn xóa sản phẩm này khỏi giỏ hàng?')) {
        fetch('/api/cart/remove', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ cartId: cartId })
        })
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                location.reload();
            } else {
                showNotification('Lỗi xóa sản phẩm', 'error');
            }
        })
        .catch(error => {
            console.error('Error removing from cart:', error);
        });
    }
}

function updateCartCount() {
    const userId = document.getElementById('hdnUserId')?.value;
    if (!userId) {
        console.log('User not logged in, cannot update cart count');
        return;
    }
    
    const formData = new FormData();
    formData.append('userId', userId);
    
    fetch('GetCartCount.aspx', {
        method: 'POST',
        body: formData
    })
    .then(response => response.text())
    .then(result => {
        try {
            const data = JSON.parse(result);
            if (data.success) {
                const cartCountElements = document.querySelectorAll('#cartCount');
                cartCountElements.forEach(element => {
                    element.textContent = data.count;
                });
                console.log('Cart count updated to:', data.count);
            } else {
                console.log('Failed to update cart count:', data.message);
            }
        } catch (e) {
            console.error('Error parsing cart count response:', e);
        }
    })
    .catch(error => {
        console.error('Error updating cart count:', error);
    });
}

function updateCartTotal() {
    fetch('/api/cart/total')
        .then(response => response.json())
        .then(data => {
            const totalElement = document.querySelector('.cart-total');
            if (totalElement) {
                totalElement.textContent = formatCurrency(data.total);
            }
        })
        .catch(error => {
            console.error('Error updating cart total:', error);
        });
}

// Design tools
function initializeDesignTools() {
    const designCanvas = document.getElementById('design-canvas');
    if (!designCanvas) return;
    
    // Initialize design interface
    const designTools = {
        selectedColor: '#000000',
        selectedSize: 'M',
        selectedFont: 'Arial',
        currentText: '',
        currentLogo: null
    };
    
    // Color picker
    const colorPicker = document.getElementById('color-picker');
    if (colorPicker) {
        colorPicker.addEventListener('change', function() {
            designTools.selectedColor = this.value;
            updateDesignPreview();
        });
    }
    
    // Text input
    const textInput = document.getElementById('design-text');
    if (textInput) {
        textInput.addEventListener('input', function() {
            designTools.currentText = this.value;
            updateDesignPreview();
        });
    }
    
    // Logo upload
    const logoUpload = document.getElementById('logo-upload');
    if (logoUpload) {
        logoUpload.addEventListener('change', function() {
            const file = this.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    designTools.currentLogo = e.target.result;
                    updateDesignPreview();
                };
                reader.readAsDataURL(file);
            }
        });
    }
    
    // Save design
    const saveDesignBtn = document.getElementById('save-design');
    if (saveDesignBtn) {
        saveDesignBtn.addEventListener('click', function() {
            saveDesign(designTools);
        });
    }
    
    window.designTools = designTools;
}

function updateDesignPreview() {
    const preview = document.getElementById('design-preview');
    if (!preview) return;
    
    const tools = window.designTools;
    
    // Update preview based on current design settings
    preview.style.backgroundColor = tools.selectedColor;
    
    // Update text
    const textElement = preview.querySelector('.design-text');
    if (textElement) {
        textElement.textContent = tools.currentText;
        textElement.style.fontFamily = tools.selectedFont;
    }
    
    // Update logo
    const logoElement = preview.querySelector('.design-logo');
    if (logoElement && tools.currentLogo) {
        logoElement.src = tools.currentLogo;
        logoElement.style.display = 'block';
    }
}

function startDesign(productId) {
    window.location.href = `/Pages/Design3D.aspx?productId=${productId}`;
}

function saveDesign(designData) {
    fetch('/api/design/save', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(designData)
    })
    .then(response => response.json())
    .then(result => {
        if (result.success) {
            showNotification('Thiết kế đã được lưu', 'success');
        } else {
            showNotification('Lỗi lưu thiết kế', 'error');
        }
    })
    .catch(error => {
        console.error('Error saving design:', error);
        showNotification('Lỗi lưu thiết kế', 'error');
    });
}

// Chatbot functionality - Commented out to avoid conflicts with separate chatbot.js
/*
function initializeChatbot() {
    const chatToggle = document.getElementById('chatbot-toggle');
    const chatWindow = document.getElementById('chatbot-window');
    const chatClose = document.querySelector('.chatbot-close');
    
    if (chatToggle) {
        chatToggle.addEventListener('click', function() {
            chatWindow.style.display = 'block';
            chatToggle.style.display = 'none';
        });
    }
    
    if (chatClose) {
        chatClose.addEventListener('click', function() {
            chatWindow.style.display = 'none';
            chatToggle.style.display = 'block';
        });
    }
}
*/

// Animations
function initializeAnimations() {
    // Fade in animation for cards
    const cards = document.querySelectorAll('.card, .product-card, .blog-card');
    
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };
    
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
            }
        });
    }, observerOptions);
    
    cards.forEach(card => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(20px)';
        card.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
        observer.observe(card);
    });
    
    // Counter animation
    const counters = document.querySelectorAll('.counter');
    counters.forEach(counter => {
        const target = parseInt(counter.textContent);
        let count = 0;
        const increment = target / 100;
        
        const updateCount = () => {
            if (count < target) {
                count += increment;
                counter.textContent = Math.floor(count);
                requestAnimationFrame(updateCount);
            } else {
                counter.textContent = target;
            }
        };
        
        observer.observe(counter);
        counter.addEventListener('animationstart', updateCount);
    });
}

// Lazy loading
function initializeLazyLoading() {
    const images = document.querySelectorAll('img[data-src]');
    
    const imageObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const img = entry.target;
                img.src = img.dataset.src;
                img.classList.remove('lazy');
                imageObserver.unobserve(img);
            }
        });
    });
    
    images.forEach(img => {
        imageObserver.observe(img);
    });
}

// Utility functions
function formatCurrency(amount) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
}

function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
    notification.style.top = '20px';
    notification.style.right = '20px';
    notification.style.zIndex = '9999';
    notification.style.minWidth = '300px';
    
    notification.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    document.body.appendChild(notification);
    
    // Auto remove after 5 seconds
    setTimeout(() => {
        if (notification.parentNode) {
            notification.parentNode.removeChild(notification);
        }
    }, 5000);
}

function showModal(title, content, actions = '') {
    const modal = document.createElement('div');
    modal.className = 'modal fade';
    modal.innerHTML = `
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">${title}</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    ${content}
                </div>
                <div class="modal-footer">
                    ${actions}
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
                </div>
            </div>
        </div>
    `;
    
    document.body.appendChild(modal);
    const bootstrapModal = new bootstrap.Modal(modal);
    bootstrapModal.show();
    
    // Remove modal from DOM when hidden
    modal.addEventListener('hidden.bs.modal', function() {
        document.body.removeChild(modal);
    });
    
    return bootstrapModal;
}

// Search functionality
function initializeSearch() {
    const searchForm = document.getElementById('search-form');
    const searchInput = document.getElementById('search-input');
    
    if (searchForm) {
        searchForm.addEventListener('submit', function(e) {
            e.preventDefault();
            const searchTerm = searchInput.value.trim();
            if (searchTerm) {
                window.location.href = `/Pages/Search.aspx?q=${encodeURIComponent(searchTerm)}`;
            }
        });
    }
}

// Initialize search when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    initializeSearch();
});

// --- Shirt Color Picker Custom ---
document.addEventListener('DOMContentLoaded', function() {
    const colorPicker = document.getElementById('colorPicker');
    const customColorRadio = document.getElementById('customColorRadio');
    const colorRadios = document.querySelectorAll('input[name="shirtColor"]');
    const shirtColorHidden = document.getElementById('ShirtColor');

    function updateShirtColorHidden() {
        var val = document.querySelector('input[name="shirtColor"]:checked')?.value || '';
        if (shirtColorHidden) shirtColorHidden.value = val;
    }

    if (colorPicker && customColorRadio && colorRadios.length) {
        colorPicker.addEventListener('input', function(e) {
            customColorRadio.value = this.value;
            customColorRadio.checked = true;
            updateShirtColorHidden();
        });
        colorRadios.forEach(radio => {
            radio.addEventListener('change', function() {
                if (this !== customColorRadio && this.checked) {
                    colorPicker.value = this.value;
                }
                updateShirtColorHidden();
            });
        });
        // Gọi 1 lần khi load
        updateShirtColorHidden();
    }
});
// --- End Shirt Color Picker Custom ---