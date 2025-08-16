using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public int UserId { get; set; }
    public string Status { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public string CustomerEmail { get; set; }
    public string ShippingAddress { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public string Notes { get; set; }
    public string TrackingNumber { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    
    // Additional properties for compatibility
    public string OrderStatus 
    { 
        get { return Status; } 
        set { Status = value; } 
    }
    
    public decimal ShippingCost 
    { 
        get { return ShippingFee; } 
        set { ShippingFee = value; } 
    }
    
    public DateTime OrderDate 
    { 
        get { return CreatedDate; } 
        set { CreatedDate = value; } 
    }
    
    // Backward compatibility properties
    public decimal TotalAmount 
    { 
        get { return Total; } 
        set { Total = value; } 
    }
    
    public string BillingAddress 
    { 
        get { return ShippingAddress; } 
        set { ShippingAddress = value; } 
    }
} 