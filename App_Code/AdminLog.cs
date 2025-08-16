using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AdminLog
{
    public int Id { get; set; }
    public string Action { get; set; }
    public string ErrorMessage { get; set; }
    public int? UserId { get; set; }
    public string IpAddress { get; set; }
    public DateTime CreatedDate { get; set; }
} 