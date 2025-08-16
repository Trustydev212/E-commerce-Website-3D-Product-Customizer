// Notification System JavaScript
class NotificationManager {
    constructor() {
        this.userId = this.getUserId();
        this.unreadCount = 0;
        this.notifications = [];
        this.isInitialized = false;
        
        if (this.userId) {
            this.init();
        }
    }

    getUserId() {
        // Lấy userId từ session hoặc hidden field
        const userIdElement = document.getElementById('hdnUserId');
        return userIdElement ? parseInt(userIdElement.value) : null;
    }

    init() {
        this.createNotificationUI();
        this.loadNotifications();
        this.startAutoRefresh();
        this.isInitialized = true;
    }

    createNotificationUI() {
        // Tạo notification bell icon với badge
        const navbar = document.querySelector('.navbar-nav');
        if (!navbar) return;

        const notificationHtml = `
            <li class="nav-item dropdown">
                <a class="nav-link dropdown-toggle position-relative" href="#" id="notificationDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i class="fas fa-bell"></i>
                    <span class="badge badge-danger badge-pill position-absolute" id="notificationBadge" style="top: 0; right: 0; display: none;">0</span>
                </a>
                <div class="dropdown-menu dropdown-menu-right" id="notificationDropdownMenu" style="width: 350px; max-height: 400px; overflow-y: auto;">
                    <div class="dropdown-header d-flex justify-content-between align-items-center">
                        <span>Thông báo</span>
                        <button class="btn btn-sm btn-outline-primary" onclick="notificationManager.markAllAsRead()">Đánh dấu tất cả đã đọc</button>
                    </div>
                    <div class="dropdown-divider"></div>
                    <div id="notificationList">
                        <div class="text-center py-3">
                            <i class="fas fa-spinner fa-spin"></i> Đang tải...
                        </div>
                    </div>
                    <div class="dropdown-divider"></div>
                    <div class="dropdown-item text-center">
                        <a href="UserNotifications.aspx" class="text-primary">Xem tất cả thông báo</a>
                    </div>
                </div>
            </li>
        `;

        // Thêm vào navbar (trước login/logout)
        const loginItem = navbar.querySelector('.nav-item:last-child');
        if (loginItem) {
            loginItem.insertAdjacentHTML('beforebegin', notificationHtml);
        } else {
            navbar.insertAdjacentHTML('beforeend', notificationHtml);
        }

        // Thêm CSS cho notification
        this.addNotificationStyles();
    }

    addNotificationStyles() {
        const style = document.createElement('style');
        style.textContent = `
            .notification-item {
                padding: 10px;
                border-bottom: 1px solid #eee;
                cursor: pointer;
                transition: background-color 0.2s;
            }
            .notification-item:hover {
                background-color: #f8f9fa;
            }
            .notification-item.unread {
                background-color: #e3f2fd;
                border-left: 3px solid #2196f3;
            }
            .notification-item.unread:hover {
                background-color: #bbdefb;
            }
            .notification-title {
                font-weight: bold;
                font-size: 14px;
                margin-bottom: 5px;
            }
            .notification-message {
                font-size: 12px;
                color: #666;
                margin-bottom: 5px;
            }
            .notification-meta {
                font-size: 11px;
                color: #999;
            }
            .notification-type {
                padding: 2px 6px;
                border-radius: 10px;
                font-size: 10px;
                font-weight: bold;
                margin-left: 5px;
            }
            .type-info { background: #d1ecf1; color: #0c5460; }
            .type-success { background: #d4edda; color: #155724; }
            .type-warning { background: #fff3cd; color: #856404; }
            .type-error { background: #f8d7da; color: #721c24; }
            .notification-badge {
                animation: pulse 2s infinite;
            }
            @keyframes pulse {
                0% { transform: scale(1); }
                50% { transform: scale(1.1); }
                100% { transform: scale(1); }
            }
        `;
        document.head.appendChild(style);
    }

    async loadNotifications() {
        try {
            const response = await fetch(`NotificationService.asmx/GetNotifications?userId=${this.userId}&limit=10`);
            const result = await response.json();
            
            if (result.d && result.d.success) {
                this.notifications = result.d.data;
                this.renderNotifications();
                this.updateUnreadCount();
            }
        } catch (error) {
            console.error('Error loading notifications:', error);
        }
    }

    async updateUnreadCount() {
        try {
            const response = await fetch(`NotificationService.asmx/GetUnreadCount?userId=${this.userId}`);
            const result = await response.json();
            
            if (result.d && result.d.success) {
                this.unreadCount = result.d.count;
                this.updateBadge();
            }
        } catch (error) {
            console.error('Error updating unread count:', error);
        }
    }

    renderNotifications() {
        const notificationList = document.getElementById('notificationList');
        if (!notificationList) return;

        if (this.notifications.length === 0) {
            notificationList.innerHTML = `
                <div class="text-center py-3">
                    <i class="fas fa-bell-slash text-muted"></i>
                    <p class="text-muted mb-0">Không có thông báo nào</p>
                </div>
            `;
            return;
        }

        const notificationsHtml = this.notifications.map(notification => {
            const isUnread = !notification.IsRead;
            const typeClass = `type-${notification.Type || 'info'}`;
            const typeText = this.getTypeDisplayName(notification.Type);
            const timeAgo = this.getTimeAgo(notification.CreatedAt);
            
            return `
                <div class="notification-item ${isUnread ? 'unread' : ''}" onclick="notificationManager.handleNotificationClick(${notification.Id}, '${notification.ActionUrl || ''}')">
                    <div class="d-flex align-items-start">
                        <div class="flex-grow-1">
                            <div class="notification-title">
                                ${notification.Title}
                                <span class="notification-type ${typeClass}">${typeText}</span>
                            </div>
                            <div class="notification-message">${notification.Message}</div>
                            <div class="notification-meta">
                                <i class="fas fa-clock"></i> ${timeAgo}
                                ${notification.IsGlobal ? '<span class="badge badge-success badge-sm ml-1">Toàn hệ thống</span>' : ''}
                            </div>
                        </div>
                        ${isUnread ? '<div class="ml-2"><i class="fas fa-circle text-primary" style="font-size: 8px;"></i></div>' : ''}
                    </div>
                </div>
            `;
        }).join('');

        notificationList.innerHTML = notificationsHtml;
    }

    updateBadge() {
        const badge = document.getElementById('notificationBadge');
        if (!badge) return;

        if (this.unreadCount > 0) {
            badge.textContent = this.unreadCount > 99 ? '99+' : this.unreadCount;
            badge.style.display = 'block';
            badge.classList.add('notification-badge');
        } else {
            badge.style.display = 'none';
            badge.classList.remove('notification-badge');
        }
    }

    async handleNotificationClick(notificationId, actionUrl) {
        // Đánh dấu đã đọc
        await this.markAsRead(notificationId);
        
        // Nếu có action URL, chuyển hướng
        if (actionUrl) {
            window.location.href = actionUrl;
        }
    }

    async markAsRead(notificationId) {
        try {
            const response = await fetch('NotificationService.asmx/MarkAsRead', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    notificationId: notificationId,
                    userId: this.userId
                })
            });
            
            const result = await response.json();
            if (result.d && result.d.success) {
                // Cập nhật lại danh sách và badge
                await this.loadNotifications();
            }
        } catch (error) {
            console.error('Error marking notification as read:', error);
        }
    }

    async markAllAsRead() {
        try {
            const response = await fetch('NotificationService.asmx/MarkAllAsRead', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    userId: this.userId
                })
            });
            
            const result = await response.json();
            if (result.d && result.d.success) {
                await this.loadNotifications();
            }
        } catch (error) {
            console.error('Error marking all notifications as read:', error);
        }
    }

    startAutoRefresh() {
        // Tự động refresh thông báo mỗi 30 giây
        setInterval(() => {
            if (this.isInitialized) {
                this.loadNotifications();
            }
        }, 30000);
    }

    getTypeDisplayName(type) {
        switch (type) {
            case 'info': return 'Thông tin';
            case 'success': return 'Thành công';
            case 'warning': return 'Cảnh báo';
            case 'error': return 'Lỗi';
            default: return type;
        }
    }

    getTimeAgo(dateString) {
        const date = new Date(dateString);
        const now = new Date();
        const diffInSeconds = Math.floor((now - date) / 1000);

        if (diffInSeconds < 60) {
            return 'Vừa xong';
        } else if (diffInSeconds < 3600) {
            const minutes = Math.floor(diffInSeconds / 60);
            return `${minutes} phút trước`;
        } else if (diffInSeconds < 86400) {
            const hours = Math.floor(diffInSeconds / 3600);
            return `${hours} giờ trước`;
        } else {
            const days = Math.floor(diffInSeconds / 86400);
            return `${days} ngày trước`;
        }
    }

    // Method để tạo thông báo toast
    showToast(message, type = 'info', duration = 5000) {
        const toastContainer = this.getOrCreateToastContainer();
        
        const toast = document.createElement('div');
        toast.className = `toast show bg-${type} text-white`;
        toast.innerHTML = `
            <div class="toast-body">
                <i class="fas fa-${this.getToastIcon(type)}"></i>
                ${message}
                <button type="button" class="ml-2 mb-1 close text-white" onclick="this.parentElement.parentElement.remove()">
                    <span>&times;</span>
                </button>
            </div>
        `;
        
        toastContainer.appendChild(toast);
        
        // Tự động xóa sau duration
        setTimeout(() => {
            if (toast.parentElement) {
                toast.remove();
            }
        }, duration);
    }

    getOrCreateToastContainer() {
        let container = document.getElementById('toastContainer');
        if (!container) {
            container = document.createElement('div');
            container.id = 'toastContainer';
            container.style.cssText = `
                position: fixed;
                top: 20px;
                right: 20px;
                z-index: 9999;
                max-width: 350px;
            `;
            document.body.appendChild(container);
        }
        return container;
    }

    getToastIcon(type) {
        switch (type) {
            case 'success': return 'check-circle';
            case 'warning': return 'exclamation-triangle';
            case 'error': return 'times-circle';
            default: return 'info-circle';
        }
    }
}

// Khởi tạo notification manager khi trang load
let notificationManager;
// document.addEventListener('DOMContentLoaded', function() {
//     notificationManager = new NotificationManager();
// });

// Export để sử dụng ở các file khác
window.notificationManager = notificationManager; 