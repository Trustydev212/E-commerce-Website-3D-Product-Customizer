using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace App_Code
{
    public class ProductImageDAL
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public int InsertProductImage(ProductImage productImage)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO ProductImages (ProductId, Color, ImagePath, SortOrder, CreatedAt)
                    VALUES (@ProductId, @Color, @ImagePath, @SortOrder, @CreatedAt);
                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productImage.ProductId);
                    cmd.Parameters.AddWithValue("@Color", productImage.Color);
                    cmd.Parameters.AddWithValue("@ImagePath", productImage.ImagePath);
                    cmd.Parameters.AddWithValue("@SortOrder", productImage.SortOrder);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public bool UpdateProductImage(ProductImage productImage)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    UPDATE ProductImages 
                    SET Color = @Color, ImagePath = @ImagePath, SortOrder = @SortOrder
                    WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", productImage.Id);
                    cmd.Parameters.AddWithValue("@Color", productImage.Color);
                    cmd.Parameters.AddWithValue("@ImagePath", productImage.ImagePath);
                    cmd.Parameters.AddWithValue("@SortOrder", productImage.SortOrder);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteProductImage(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM ProductImages WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteProductImages(int productId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM ProductImages WHERE ProductId = @ProductId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public ProductImage GetProductImageById(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM ProductImages WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToProductImage(reader);
                        }
                    }
                }
            }
            return null;
        }

        public List<ProductImage> GetProductImagesByProductId(int productId)
        {
            List<ProductImage> images = new List<ProductImage>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM ProductImages WHERE ProductId = @ProductId ORDER BY Color, SortOrder";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            images.Add(MapReaderToProductImage(reader));
                        }
                    }
                }
            }
            return images;
        }

        public List<ProductImage> GetProductImagesByColor(int productId, string color)
        {
            List<ProductImage> images = new List<ProductImage>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM ProductImages WHERE ProductId = @ProductId AND Color = @Color ORDER BY SortOrder";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.Parameters.AddWithValue("@Color", color);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            images.Add(MapReaderToProductImage(reader));
                        }
                    }
                }
            }
            return images;
        }

        public string GetProductImageByColor(int productId, string color)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ImagePath FROM ProductImages WHERE ProductId = @ProductId AND Color = @Color ORDER BY SortOrder";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.Parameters.AddWithValue("@Color", color);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["ImagePath"].ToString();
                        }
                    }
                }
            }
            return null;
        }

        public List<ProductImage> GetAllProductImages()
        {
            List<ProductImage> images = new List<ProductImage>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM ProductImages ORDER BY ProductId, Color, SortOrder";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            images.Add(MapReaderToProductImage(reader));
                        }
                    }
                }
            }
            return images;
        }

        private ProductImage MapReaderToProductImage(SqlDataReader reader)
        {
            return new ProductImage
            {
                Id = Convert.ToInt32(reader["Id"]),
                ProductId = Convert.ToInt32(reader["ProductId"]),
                Color = reader["Color"].ToString(),
                ImagePath = reader["ImagePath"].ToString(),
                SortOrder = Convert.ToInt32(reader["SortOrder"]),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }
    }
} 