// Function to prepare order data before submission
function prepareOrderData() {
    try {
        // Check if there's design data in localStorage
        const designData = localStorage.getItem('orderDesign');
        if (designData) {
            console.log('Found design data in localStorage:', designData);
            // Set the design data to hidden field
            var hdn = document.getElementById('hdnDesignData');
            if (hdn) hdn.value = designData;
            // KHÔNG xóa localStorage nữa để giữ lại dữ liệu sau reload
            // localStorage.removeItem('orderDesign');
            console.log('Design data prepared for submission');
        } else {
            console.log('No design data found in localStorage');
        }
        return true; // Allow form submission to continue
    } catch (error) {
        console.error('Error preparing order data:', error);
        return true; // Still allow form submission even if there's an error
    }
}

// Lấy designId từ query string khi vào trang Checkout
window.addEventListener('DOMContentLoaded', function() {
    const urlParams = new URLSearchParams(window.location.search);
    const designId = urlParams.get('designId');
    if (designId) {
        console.log('DesignId from query:', designId);
        // Gán vào hidden field riêng cho designId
        var hdnId = document.getElementById('hdnDesignId');
        if (hdnId) {
            hdnId.value = designId;
        } else {
            console.warn('Không tìm thấy hidden field hdnDesignId để lưu designId!');
        }
    } else {
        console.warn('No designId found in query string!');
    }
}); 