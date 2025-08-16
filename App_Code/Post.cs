using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
    public string ImagePath { get; set; }
    public string Category { get; set; }
    public DateTime PublishedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Author { get; set; }
    public int ViewCount { get; set; }
    public bool IsPublished { get; set; }
    public bool IsActive { get; set; }
} 