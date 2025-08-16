using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string CustomDesignPath { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    // Navigation properties for display
    public string ProductName { get; set; }
    public string ProductImagePath { get; set; }
    public string CustomDesign { get; set; }
} 