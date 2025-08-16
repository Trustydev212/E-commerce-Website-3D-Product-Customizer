using System;

namespace App_Code
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Color { get; set; }
        public string ImagePath { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 