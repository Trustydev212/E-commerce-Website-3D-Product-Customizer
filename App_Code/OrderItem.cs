using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int? DesignId { get; set; } // Link to Designs table
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice { get; set; }
    public string CustomDesignPath { get; set; }
    public string CustomDesign { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public DateTime CreatedDate { get; set; }
    
    // Additional properties for display
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public decimal ProductPrice { get; set; }
    
    // Design information properties
    public string DesignName { get; set; }
    public string DesignLogoPath { get; set; }
    public string DesignPositionData { get; set; }
    public string DesignPreviewPath { get; set; }
    public DateTime? DesignCreatedAt { get; set; }
    
    // Additional property for database compatibility
    public DateTime CreatedAt 
    { 
        get { return CreatedDate; } 
        set { CreatedDate = value; } 
    }
} 