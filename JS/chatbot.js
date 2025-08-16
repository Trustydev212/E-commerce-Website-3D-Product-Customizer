// Chatbot toggle logic
function toggleChatbot() {
    var chatWindow = document.getElementById('chatbot-window');
    var chatToggle = document.getElementById('chatbot-toggle');
    if (chatWindow.style.display === 'none' || chatWindow.style.display === '') {
        chatWindow.style.display = 'block';
        chatToggle.style.display = 'none';
    } else {
        chatWindow.style.display = 'none';
        chatToggle.style.display = 'flex';
    }
}

// Hàm riêng để đóng popup (chỉ gọi khi bấm nút đóng)
function closeChatbot() {
    var chatWindow = document.getElementById('chatbot-window');
    var chatToggle = document.getElementById('chatbot-toggle');
    chatWindow.style.display = 'none';
    chatToggle.style.display = 'flex';
}

// Gửi tin nhắn nhanh
function sendQuickMessage(message) {
    document.getElementById('txtMessage').value = message;
    document.getElementById('btnSend').click();
}

// Thêm tin nhắn vào khung chat (nếu muốn dùng JS client)
function addMessage(message, isBot) {
    var messagesContainer = document.getElementById('chatbot-messages');
    var messageDiv = document.createElement('div');
    messageDiv.className = 'message ' + (isBot ? 'bot-message' : 'user-message');
    messageDiv.innerHTML = '<div class="message-content">' + message + '</div>';
    messagesContainer.appendChild(messageDiv);
    messagesContainer.scrollTop = messagesContainer.scrollHeight;
}

// Khởi tạo sự kiện toggle khi DOM ready
window.addEventListener('DOMContentLoaded', function() {
    var toggleBtn = document.getElementById('chatbot-toggle');
    if (toggleBtn) toggleBtn.addEventListener('click', toggleChatbot);
}); 