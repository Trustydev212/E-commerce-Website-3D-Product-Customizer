using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal? SalePrice { get; set; }
    public int CategoryId { get; set; }
    public string ImagePath { get; set; }
    public string ModelPath { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int StockQuantity { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public string Material { get; set; }
} 