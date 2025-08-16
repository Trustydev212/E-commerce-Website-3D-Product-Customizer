using System;

[Serializable]
public class Notification
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string Type { get; set; } // "info", "success", "warning", "error"
    public string Icon { get; set; }
    public string ActionUrl { get; set; }
    public bool? IsRead { get; set; }
    public bool? IsGlobal { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public string CreatedBy { get; set; }
} 