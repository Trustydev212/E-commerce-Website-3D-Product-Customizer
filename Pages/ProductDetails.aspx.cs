using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using App_Code;

public partial class ProductDetails : System.Web.UI.Page
{
    private ProductDAL productDAL = new ProductDAL();
    private ProductImageDAL productImageDAL = new ProductImageDAL();
    private Product currentProduct;

    // Public property để truy cập từ ASPX
    public Product CurrentProduct
    {
        get { return currentProduct; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadProductDetails();
        }
    }

    private void LoadProductDetails()
    {
        try
        {
            int productId = Convert.ToInt32(Request.QueryString["id"]);
            currentProduct = productDAL.GetProductById(productId);
            
            if (currentProduct != null)
            {
                // Set product information
                productName.InnerText = currentProduct.Name;
                productPrice.InnerText = String.Format("{0:N0} VNĐ", currentProduct.Price);
                productDescription.InnerText = currentProduct.Description;
                
                // Fix wrong image paths in database
                FixImagePaths();
                
                // Load images from ProductImages table
                LoadProductImages(productId);
                
                // Set detailed description
                litDetailedDescription.Text = currentProduct.Description;
                
                // Populate JavaScript variables with image data
                PopulateJavaScriptImageData(productId);
            }
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>console.error('Error loading product details: {0}');</script>", ex.Message));
        }
    }

    private void PopulateJavaScriptImageData(int productId)
    {
        try
        {
            // Get all product images
            List<ProductImage> allImages = productImageDAL.GetProductImagesByProductId(productId);
            Response.Write(string.Format("<script>console.log('Total images found for product {0}: {1}');</script>", productId, allImages.Count));
            
            // Create image data object using Dictionary for easier serialization
            var imageData = new Dictionary<string, object>();
            imageData["fallbackImage"] = currentProduct.ImagePath ?? "";
            imageData["colors"] = new Dictionary<string, object>();
            
            // Populate image data for each color
            string[] colors = { "white", "black", "red", "blue" };
            foreach (string color in colors)
            {
                var colorImages = allImages.Where(img => img.Color.ToLower() == color).ToList();
                Response.Write(string.Format("<script>console.log('Images for color {0}: {1}');</script>", color, colorImages.Count));
                
                string mainImage = colorImages.Count > 0 ? colorImages[0].ImagePath : (string)imageData["fallbackImage"];
                Response.Write(string.Format("<script>console.log('{0} color - main image path: {1}');</script>", color, mainImage));
                
                // Fix path to be absolute from root
                if (!string.IsNullOrEmpty(mainImage) && !mainImage.StartsWith("~/") && !mainImage.StartsWith("/"))
                {
                    mainImage = "~/" + mainImage;
                }
                
                // Resolve the URL to absolute path
                if (mainImage.StartsWith("~/"))
                {
                    mainImage = ResolveUrl(mainImage);
                }
                
                // Get images for different colors for thumbnails
                string[] thumbnailColors = { "white", "black", "red", "blue" };
                string thumb1Image = "";
                string thumb2Image = "";
                string thumb3Image = "";
                string thumb4Image = "";
                
                // Get image for each thumbnail color
                for (int i = 0; i < thumbnailColors.Length; i++)
                {
                    var thumbColorImages = allImages.Where(img => img.Color.ToLower() == thumbnailColors[i]).ToList();
                    string thumbImage = thumbColorImages.Count > 0 ? thumbColorImages[0].ImagePath : mainImage;
                    
                    // Fix path and resolve URL
                    if (!string.IsNullOrEmpty(thumbImage) && !thumbImage.StartsWith("~/") && !thumbImage.StartsWith("/"))
                    {
                        thumbImage = "~/" + thumbImage;
                    }
                    if (thumbImage.StartsWith("~/"))
                    {
                        thumbImage = ResolveUrl(thumbImage);
                    }
                    
                    switch (i)
                    {
                        case 0: thumb1Image = thumbImage; break;
                        case 1: thumb2Image = thumbImage; break;
                        case 2: thumb3Image = thumbImage; break;
                        case 3: thumb4Image = thumbImage; break;
                    }
                }
                
                var colorData = new Dictionary<string, string>();
                colorData["main"] = mainImage;
                colorData["thumb1"] = thumb1Image;
                colorData["thumb2"] = thumb2Image;
                colorData["thumb3"] = thumb3Image;
                colorData["thumb4"] = thumb4Image;
                
                ((Dictionary<string, object>)imageData["colors"])[color] = colorData;
                
                Response.Write(string.Format("<script>console.log('{0} color - main: {1}');</script>", color, mainImage));
            }
            
            // Convert to JSON and store in hidden field
            string jsonData = new JavaScriptSerializer().Serialize(imageData);
            hdnImageData.Value = jsonData;
            
            Response.Write(string.Format("<script>console.log('Image data JSON: {0}');</script>", jsonData));
            
            // Debug: Log the structure of imageData
            Response.Write(string.Format("<script>console.log('Fallback image from data: {0}');</script>", imageData["fallbackImage"]));
            var colorsDict = (Dictionary<string, object>)imageData["colors"];
            foreach (string color in colors)
            {
                if (colorsDict.ContainsKey(color))
                {
                    var colorData = (Dictionary<string, string>)colorsDict[color];
                    Response.Write(string.Format("<script>console.log('{0} color data: {1}');</script>", color, colorData["main"]));
                }
            }
            
            // Also create direct script as backup
            var directScript = new System.Text.StringBuilder();
            directScript.AppendLine(string.Format("fallbackImage = '{0}';", imageData["fallbackImage"]));
            foreach (string color in colors)
            {
                if (colorsDict.ContainsKey(color))
                {
                    var colorData = (Dictionary<string, string>)colorsDict[color];
                    directScript.AppendLine(string.Format("productImages.{0}.main = '{1}';", color, colorData["main"]));
                    directScript.AppendLine(string.Format("productImages.{0}.thumb1 = '{1}';", color, colorData["thumb1"]));
                    directScript.AppendLine(string.Format("productImages.{0}.thumb2 = '{1}';", color, colorData["thumb2"]));
                    directScript.AppendLine(string.Format("productImages.{0}.thumb3 = '{1}';", color, colorData["thumb3"]));
                    directScript.AppendLine(string.Format("productImages.{0}.thumb4 = '{1}';", color, colorData["thumb4"]));
                }
            }
            Response.Write(string.Format("<script>{0}</script>", directScript.ToString()));
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>console.error('Error populating image data: {0}');</script>", ex.Message));
        }
    }

    private void LoadProductImages(int productId)
    {
        try
        {
            // Get all product images
            List<ProductImage> allImages = productImageDAL.GetProductImagesByProductId(productId);
            Response.Write(string.Format("<script>console.log('Total images found for product {0}: {1}');</script>", productId, allImages.Count));
            
            // Debug: Log all image paths
            foreach (var img in allImages)
            {
                Response.Write(string.Format("<script>console.log('Image: Color={0}, Path={1}, SortOrder={2}');</script>", 
                    img.Color, img.ImagePath, img.SortOrder));
            }
            
            // Get default color (white) images
            string defaultColor = "white";
            List<ProductImage> defaultImages = allImages.Where(img => img.Color.ToLower() == defaultColor).ToList();
            Response.Write(string.Format("<script>console.log('White color images found: {0}');</script>", defaultImages.Count));
            
            // Set main image - try to get from ProductImages first, fallback to Product.ImagePath
            string mainImagePath = GetMainImagePath(defaultImages, currentProduct.ImagePath);
            Response.Write(string.Format("<script>console.log('Main image path: {0}');</script>", mainImagePath));
            
            // Check if image file exists
            string physicalPath = Server.MapPath("~/" + mainImagePath.Replace("~/", ""));
            bool fileExists = System.IO.File.Exists(physicalPath);
            Response.Write(string.Format("<script>console.log('Physical path: {0}');</script>", physicalPath));
            Response.Write(string.Format("<script>console.log('File exists: {0}');</script>", fileExists));
            
            mainImage.Src = mainImagePath;
            mainImage.Alt = currentProduct.Name;
            
            // Set thumbnail images - each thumbnail shows a different color
            string[] thumbnailColors = { "white", "black", "red", "blue" };
            HtmlImage[] thumbnails = { thumb1, thumb2, thumb3, thumb4 };
            
            for (int i = 0; i < thumbnailColors.Length; i++)
            {
                var thumbColorImages = allImages.Where(img => img.Color.ToLower() == thumbnailColors[i]).ToList();
                string thumbImagePath = thumbColorImages.Count > 0 ? thumbColorImages[0].ImagePath : mainImagePath;
                
                // Fix path and resolve URL
                if (!string.IsNullOrEmpty(thumbImagePath) && !thumbImagePath.StartsWith("~/") && !thumbImagePath.StartsWith("/"))
                {
                    thumbImagePath = "~/" + thumbImagePath;
                }
                if (thumbImagePath.StartsWith("~/"))
                {
                    thumbImagePath = ResolveUrl(thumbImagePath);
                }
                
                SetThumbnailImage(thumbnails[i], thumbImagePath);
            }
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>console.error('Error loading product images: {0}');</script>", ex.Message));
            // Fallback to original image
            mainImage.Src = currentProduct.ImagePath;
            mainImage.Alt = currentProduct.Name;
            SetThumbnailImage(thumb1, currentProduct.ImagePath);
            SetThumbnailImage(thumb2, currentProduct.ImagePath);
            SetThumbnailImage(thumb3, currentProduct.ImagePath);
            SetThumbnailImage(thumb4, currentProduct.ImagePath);
        }
    }

    private string GetMainImagePath(List<ProductImage> images, string fallbackPath)
    {
        var mainImage = images.FirstOrDefault();
        string imagePath = mainImage != null ? mainImage.ImagePath : fallbackPath;
        
        // Fix path to be absolute from root
        if (!string.IsNullOrEmpty(imagePath) && !imagePath.StartsWith("~/") && !imagePath.StartsWith("/"))
        {
            imagePath = "~/" + imagePath;
        }
        
        // Resolve the URL to absolute path
        if (imagePath.StartsWith("~/"))
        {
            imagePath = ResolveUrl(imagePath);
        }
        
        // If no image found, use a placeholder
        if (string.IsNullOrEmpty(imagePath) || imagePath.Trim() == "")
        {
            imagePath = ResolveUrl("~/Images/products/placeholder.png");
        }
        
        return imagePath;
    }

    private void SetThumbnailImage(HtmlImage thumb, string imagePath)
    {
        if (thumb != null)
        {
            thumb.Src = imagePath;
            thumb.Alt = currentProduct.Name;
        }
    }

    protected void btnAddToCart_Click(object sender, EventArgs e)
    {
        // TODO: Implement add to cart logic
    }

    protected void btnBuyNow_Click(object sender, EventArgs e)
    {
        // TODO: Implement buy now logic
    }

    // Method để lấy ảnh sản phẩm theo màu sắc
    protected string GetProductImageByColor(string productId, string color)
    {
        try
        {
            int id = Convert.ToInt32(productId);
            string imagePath = productImageDAL.GetProductImageByColor(id, color);
            string result = !string.IsNullOrEmpty(imagePath) ? imagePath : "";
            Response.Write(string.Format("<script>console.log('GetProductImageByColor({0}, {1}) = {2}');</script>", productId, color, result));
            return result;
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>console.error('Error in GetProductImageByColor: {0}');</script>", ex.Message));
            return "";
        }
    }

    // Method để lấy ảnh thumbnail theo màu sắc
    protected string GetProductThumbnailByColor(string productId, string color, int index)
    {
        try
        {
            int id = Convert.ToInt32(productId);
            List<ProductImage> images = productImageDAL.GetProductImagesByColor(id, color);
            
            string result = "";
            if (images.Count > 0)
            {
                result = images[0].ImagePath; // Use the first image as thumbnail
            }
            
            Response.Write(string.Format("<script>console.log('GetProductThumbnailByColor({0}, {1}, {2}) = {3}');</script>", productId, color, index, result));
            return result;
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>console.error('Error in GetProductThumbnailByColor: {0}');</script>", ex.Message));
            return "";
        }
    }

    public string GetModelPath()
    {
        if (currentProduct != null && !string.IsNullOrEmpty(currentProduct.ModelPath))
        {
            return currentProduct.ModelPath;
        }
        return "";
    }
    
    // Method để sửa đường dẫn ảnh sai trong database
    private void FixImagePaths()
    {
        try
        {
            // Get all product images
            List<ProductImage> allImages = productImageDAL.GetProductImagesByProductId(currentProduct.Id);
            
            foreach (var img in allImages)
            {
                // Check if image path is wrong (starts with ~/Image/produ...)
                if (img.ImagePath.StartsWith("~/Image/produ"))
                {
                    Response.Write(string.Format("<script>console.log('Found wrong image path: {0}');</script>", img.ImagePath));
                    
                    // Try to find the correct image in Upload/Images/Products/products/
                    string fileName = System.IO.Path.GetFileName(img.ImagePath);
                    string correctPath = "Upload/Images/Products/products/" + fileName;
                    
                    // Check if the file exists
                    string physicalPath = Server.MapPath("~/" + correctPath);
                    if (System.IO.File.Exists(physicalPath))
                    {
                        Response.Write(string.Format("<script>console.log('Found correct image: {0}');</script>", correctPath));
                        
                        // Update the image path in database
                        img.ImagePath = correctPath;
                        productImageDAL.UpdateProductImage(img);
                        Response.Write(string.Format("<script>console.log('Updated image path to: {0}');</script>", correctPath));
                    }
                    else
                    {
                        Response.Write(string.Format("<script>console.log('Correct image not found: {0}');</script>", physicalPath));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>console.error('Error fixing image paths: {0}');</script>", ex.Message));
        }
    }
}