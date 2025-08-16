using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class PostDAL
{
    private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    public List<Post> GetPublishedPosts()
    {
        List<Post> posts = new List<Post>();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT Id, Title, Summary, Content, ImagePath, Category, 
                           PublishedDate, CreatedDate, Author, ViewCount, IsPublished, IsActive 
                           FROM Posts 
                           WHERE IsPublished = 1 AND IsActive = 1 
                           ORDER BY PublishedDate DESC";
            
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Post post = new Post
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            ImagePath = reader["ImagePath"].ToString(),
                            Category = reader["Category"].ToString(),
                            PublishedDate = Convert.ToDateTime(reader["PublishedDate"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            Author = reader["Author"].ToString(),
                            ViewCount = Convert.ToInt32(reader["ViewCount"]),
                            IsPublished = Convert.ToBoolean(reader["IsPublished"]),
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        };
                        posts.Add(post);
                    }
                }
            }
        }
        return posts;
    }

    public int GetPostCount(string searchKeyword = "", bool publishedOnly = true)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT COUNT(*) FROM Posts WHERE IsActive = 1";
            
            if (publishedOnly)
                query += " AND IsPublished = 1";
            if (!string.IsNullOrEmpty(searchKeyword))
                query += " AND (Title LIKE @search OR Summary LIKE @search OR Content LIKE @search)";
            
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + searchKeyword + "%");
                }
                
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }

    public List<Post> GetPosts(int page, int pageSize, string searchKeyword = "", string sortBy = "newest", bool publishedOnly = true)
    {
        List<Post> posts = new List<Post>();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT Id, Title, Summary, Content, ImagePath, Category, 
                           PublishedDate, CreatedDate, Author, ViewCount, IsPublished, IsActive 
                           FROM Posts 
                           WHERE IsActive = 1";
            
            if (publishedOnly)
                query += " AND IsPublished = 1";
            if (!string.IsNullOrEmpty(searchKeyword))
                query += " AND (Title LIKE @search OR Summary LIKE @search OR Content LIKE @search)";
            
            // Add sorting
            switch (sortBy.ToLower())
            {
                case "oldest":
                    query += " ORDER BY PublishedDate ASC";
                    break;
                case "popular":
                    query += " ORDER BY ViewCount DESC";
                    break;
                default:
                    query += " ORDER BY PublishedDate DESC";
                    break;
            }
            
            query += " OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";
            
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + searchKeyword + "%");
                }
                
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Post post = new Post
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            ImagePath = reader["ImagePath"].ToString(),
                            Category = reader["Category"].ToString(),
                            PublishedDate = Convert.ToDateTime(reader["PublishedDate"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            Author = reader["Author"].ToString(),
                            ViewCount = Convert.ToInt32(reader["ViewCount"]),
                            IsPublished = Convert.ToBoolean(reader["IsPublished"]),
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        };
                        posts.Add(post);
                    }
                }
            }
        }
        return posts;
    }

    public List<Post> GetRecentPosts(int count, bool publishedOnly = true)
    {
        List<Post> posts = new List<Post>();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT TOP (@count) Id, Title, Summary, Content, ImagePath, Category, 
                           PublishedDate, CreatedDate, Author, ViewCount, IsPublished, IsActive 
                           FROM Posts 
                           WHERE IsActive = 1";
            
            if (publishedOnly)
                query += " AND IsPublished = 1";
            
            query += " ORDER BY PublishedDate DESC";
            
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@count", count);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Post post = new Post
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            ImagePath = reader["ImagePath"].ToString(),
                            Category = reader["Category"].ToString(),
                            PublishedDate = Convert.ToDateTime(reader["PublishedDate"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            Author = reader["Author"].ToString(),
                            ViewCount = Convert.ToInt32(reader["ViewCount"]),
                            IsPublished = Convert.ToBoolean(reader["IsPublished"]),
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        };
                        posts.Add(post);
                    }
                }
            }
        }
        return posts;
    }

    public List<Post> GetPopularPosts(int count, bool publishedOnly = true)
    {
        List<Post> posts = new List<Post>();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT TOP (@count) Id, Title, Summary, Content, ImagePath, Category, 
                           PublishedDate, CreatedDate, Author, ViewCount, IsPublished, IsActive 
                           FROM Posts 
                           WHERE IsActive = 1";
            
            if (publishedOnly)
                query += " AND IsPublished = 1";
            
            query += " ORDER BY ViewCount DESC, PublishedDate DESC";
            
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@count", count);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Post post = new Post
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            ImagePath = reader["ImagePath"].ToString(),
                            Category = reader["Category"].ToString(),
                            PublishedDate = Convert.ToDateTime(reader["PublishedDate"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            Author = reader["Author"].ToString(),
                            ViewCount = Convert.ToInt32(reader["ViewCount"]),
                            IsPublished = Convert.ToBoolean(reader["IsPublished"]),
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        };
                        posts.Add(post);
                    }
                }
            }
        }
        return posts;
    }

    public List<Post> GetRelatedPosts(int currentPostId, string category, int count)
    {
        List<Post> posts = new List<Post>();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT TOP (@count) Id, Title, Summary, Content, ImagePath, Category, 
                           PublishedDate, CreatedDate, Author, ViewCount, IsPublished, IsActive 
                           FROM Posts 
                           WHERE IsActive = 1 AND IsPublished = 1 AND Id != @currentPostId";
            
            if (!string.IsNullOrEmpty(category))
                query += " AND Category = @category";
            
            query += " ORDER BY PublishedDate DESC";
            
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@count", count);
                cmd.Parameters.AddWithValue("@currentPostId", currentPostId);
                if (!string.IsNullOrEmpty(category))
                {
                    cmd.Parameters.AddWithValue("@category", category);
                }
                
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Post post = new Post
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            ImagePath = reader["ImagePath"].ToString(),
                            Category = reader["Category"].ToString(),
                            PublishedDate = Convert.ToDateTime(reader["PublishedDate"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            Author = reader["Author"].ToString(),
                            ViewCount = Convert.ToInt32(reader["ViewCount"]),
                            IsPublished = Convert.ToBoolean(reader["IsPublished"]),
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        };
                        posts.Add(post);
                    }
                }
            }
        }
        return posts;
    }

    public Post GetPostById(int id)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT Id, Title, Summary, Content, ImagePath, Category, 
                           PublishedDate, CreatedDate, Author, ViewCount, IsPublished, IsActive 
                           FROM Posts 
                           WHERE Id = @id AND IsActive = 1";
            
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Post
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            ImagePath = reader["ImagePath"].ToString(),
                            Category = reader["Category"].ToString(),
                            PublishedDate = Convert.ToDateTime(reader["PublishedDate"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            Author = reader["Author"].ToString(),
                            ViewCount = Convert.ToInt32(reader["ViewCount"]),
                            IsPublished = Convert.ToBoolean(reader["IsPublished"]),
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        };
                    }
                }
            }
        }
        return null;
    }

    public bool IncrementViewCount(int postId)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Posts SET ViewCount = ViewCount + 1 WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", postId);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        catch
        {
            return false;
        }
    }

    public int InsertPost(Post post)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"INSERT INTO Posts (Title, Summary, Content, ImagePath, Category, 
                           Author, IsPublished, IsActive, PublishedDate, CreatedDate, UpdatedAt) 
                           VALUES (@Title, @Summary, @Content, @ImagePath, @Category, 
                           @Author, @IsPublished, @IsActive, @PublishedDate, @CreatedDate, @UpdatedAt);
                           SELECT SCOPE_IDENTITY();";
            
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Title", post.Title);
                cmd.Parameters.AddWithValue("@Summary", post.Summary ?? "");
                cmd.Parameters.AddWithValue("@Content", post.Content);
                cmd.Parameters.AddWithValue("@ImagePath", post.ImagePath ?? "");
                cmd.Parameters.AddWithValue("@Category", post.Category ?? "");
                cmd.Parameters.AddWithValue("@Author", post.Author);
                cmd.Parameters.AddWithValue("@IsPublished", post.IsPublished);
                cmd.Parameters.AddWithValue("@IsActive", post.IsActive);
                cmd.Parameters.AddWithValue("@PublishedDate", post.PublishedDate);
                cmd.Parameters.AddWithValue("@CreatedDate", post.CreatedDate);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }
    }

    public bool UpdatePost(Post post)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"UPDATE Posts SET Title = @Title, Summary = @Summary, Content = @Content, 
                           ImagePath = @ImagePath, Category = @Category, Author = @Author, 
                           IsPublished = @IsPublished, IsActive = @IsActive, UpdatedAt = @UpdatedAt 
                           WHERE Id = @Id";
            
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", post.Id);
                cmd.Parameters.AddWithValue("@Title", post.Title);
                cmd.Parameters.AddWithValue("@Summary", post.Summary ?? "");
                cmd.Parameters.AddWithValue("@Content", post.Content);
                cmd.Parameters.AddWithValue("@ImagePath", post.ImagePath ?? "");
                cmd.Parameters.AddWithValue("@Category", post.Category ?? "");
                cmd.Parameters.AddWithValue("@Author", post.Author);
                cmd.Parameters.AddWithValue("@IsPublished", post.IsPublished);
                cmd.Parameters.AddWithValue("@IsActive", post.IsActive);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }

    public bool DeletePost(int id)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "UPDATE Posts SET IsActive = 0 WHERE Id = @id";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
} 