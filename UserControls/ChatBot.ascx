<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChatBot.ascx.cs" Inherits="ChatBot" %>
<link href="~/Css/global-fonts.css" rel="stylesheet" type="text/css" />
<link href="/Css/chatbot.css" rel="stylesheet" />
<script src="/JS/chatbot.js"></script>

<div id="chatbot-container" class="chatbot-container">
    <div id="chatbot-toggle" class="chatbot-toggle">
        <i class="fas fa-comment"></i>
    </div>
    
    <div id="chatbot-window" class="chatbot-window" style="display: none;">
        <div class="chatbot-header">
            <div class="chatbot-title">
                <i class="fas fa-robot me-2"></i>
                Trợ lý AI
            </div>
            <button type="button" class="chatbot-close" onclick="closeChatbot()">
                <i class="fas fa-times"></i>
            </button>
        </div>
        
        <div class="chatbot-messages" id="chatbot-messages">
            <div class="message bot-message">
                <div class="message-content">
                    Xin chào! Tôi là trợ lý AI của 3D T-Shirt Design. Tôi có thể giúp bạn:
                    <ul>
                        <li>Tìm hiểu về sản phẩm</li>
                        <li>Hướng dẫn thiết kế 3D</li>
                        <li>Hỗ trợ đặt hàng</li>
                        <li>Giải đáp thắc mắc</li>
                    </ul>
                    Bạn cần hỗ trợ gì không?
                </div>
            </div>
        </div>
        
        <div class="chatbot-input">
            <div class="input-group">
                <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" placeholder="Nhập tin nhắn..." onkeypress="return handleChatKeyPress(event)" />
                <asp:Button ID="btnSend" runat="server" CssClass="btn btn-primary" Text="Gửi" OnClientClick="return sendChatMessage();" />
            </div>
        </div>
        
        <div class="chatbot-suggestions">
            <button type="button" class="btn btn-outline-secondary btn-sm me-2" onclick="sendQuickMessage('Sản phẩm nào bán chạy nhất?')">
                Sản phẩm hot
            </button>
            <button type="button" class="btn btn-outline-secondary btn-sm me-2" onclick="sendQuickMessage('Hướng dẫn thiết kế 3D')">
                Thiết kế 3D
            </button>
            <button type="button" class="btn btn-outline-secondary btn-sm" onclick="sendQuickMessage('Chính sách đổi trả')">
                Đổi trả
            </button>
        </div>
    </div>
</div>

<style>
    .chatbot-container, .chatbot-container * { 
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif !important; 
    }
</style>

<script>
    function toggleChatbot() {
        var chatWindow = document.getElementById('chatbot-window');
        var chatToggle = document.getElementById('chatbot-toggle');
        
        // Chỉ mở popup, không đóng
        chatWindow.style.display = 'block';
        chatToggle.style.display = 'none';
    }
    
    function closeChatbot() {
        var chatWindow = document.getElementById('chatbot-window');
        var chatToggle = document.getElementById('chatbot-toggle');
        chatWindow.style.display = 'none';
        chatToggle.style.display = 'flex';
    }
    
    function sendQuickMessage(message) {
        document.getElementById('<%= txtMessage.ClientID %>').value = message;
        document.getElementById('<%= btnSend.ClientID %>').click();
    }
    
    function addMessage(message, isBot) {
        var messagesContainer = document.getElementById('chatbot-messages');
        var messageDiv = document.createElement('div');
        messageDiv.className = 'message ' + (isBot ? 'bot-message' : 'user-message');
        messageDiv.innerHTML = '<div class="message-content">' + message + '</div>';
        messagesContainer.appendChild(messageDiv);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }
    
    // Hàm gửi tin nhắn bằng AJAX
    function sendChatMessage() {
        var messageInput = document.getElementById('<%= txtMessage.ClientID %>');
        var message = messageInput.value.trim();
        
        if (message === '') {
            return false;
        }
        
        // Thêm tin nhắn người dùng ngay lập tức
        addMessage(message, false);
        messageInput.value = '';
        
        // Gửi tin nhắn bằng AJAX
        var xhr = new XMLHttpRequest();
        xhr.open('POST', window.location.href, true);
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
        
        xhr.onreadystatechange = function() {
            if (xhr.readyState === 4 && xhr.status === 200) {
                // Xử lý phản hồi từ server
                try {
                    var response = JSON.parse(xhr.responseText);
                    if (response.botResponse) {
                        addMessage(response.botResponse, true);
                    }
                } catch (e) {
                    // Fallback: thêm phản hồi mặc định
                    addMessage('Cảm ơn tin nhắn của bạn! Tôi sẽ phản hồi sớm nhất có thể.', true);
                }
            }
        };
        
        // Tạo form data
        var formData = new FormData();
        formData.append('<%= txtMessage.UniqueID %>', message);
        formData.append('__EVENTTARGET', '<%= btnSend.UniqueID %>');
        formData.append('__EVENTARGUMENT', '');
        formData.append('__VIEWSTATE', document.getElementById('__VIEWSTATE').value);
        formData.append('__VIEWSTATEGENERATOR', document.getElementById('__VIEWSTATEGENERATOR').value);
        formData.append('__EVENTVALIDATION', document.getElementById('__EVENTVALIDATION').value);
        
        xhr.send(new URLSearchParams(formData));
        
        return false; // Ngăn postback
    }
    
    // Xử lý phím Enter
    function handleChatKeyPress(event) {
        if (event.keyCode === 13) {
            sendChatMessage();
            return false;
        }
        return true;
    }
    
    // Initialize chatbot toggle
    document.addEventListener('DOMContentLoaded', function() {
        document.getElementById('chatbot-toggle').addEventListener('click', toggleChatbot);
    });
</script>