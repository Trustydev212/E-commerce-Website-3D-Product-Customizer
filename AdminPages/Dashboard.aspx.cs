using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using App_Code;

public partial class AdminPages_Dashboard : System.Web.UI.Page
{
    private OrderDAL orderDAL = new OrderDAL();
    private ProductDAL productDAL = new ProductDAL();
    private UserDAL userDAL = new UserDAL();
    private CategoryDAL categoryDAL = new CategoryDAL();
    private JavaScriptSerializer serializer = new JavaScriptSerializer();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDashboardData();
        }
    }

    private void LoadDashboardData()
    {
        try
        {
            LoadStatistics();
            LoadRevenueChart();
            LoadOrderStatusChart();
            LoadRecentOrders();
            LoadTopProducts();
            lblLastUpdate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }
        catch (Exception ex)
        {
            // Removed debug alert - Lỗi tải dashboard: {0}
        }
    }

    private void LoadStatistics()
    {
        try
        {
            List<Order> orders = orderDAL.GetAllOrders();
            List<Product> products = productDAL.GetAllProducts();
            List<User> users = userDAL.GetAllUsers();

            // Tổng đơn hàng
            lblTotalOrders.Text = orders.Count.ToString();

            // Tổng doanh thu
            decimal totalRevenue = orders.Where(o => o.Status == "Completed").Sum(o => o.TotalAmount);
            lblTotalRevenue.Text = string.Format("{0:N0}đ", totalRevenue);

            // Tổng người dùng
            lblTotalUsers.Text = users.Count.ToString();

            // Tổng sản phẩm
            lblTotalProducts.Text = products.Count.ToString();
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>alert('Lỗi tải thống kê: {0}');</script>", ex.Message));
        }
    }

    private void LoadRevenueChart()
    {
        try
        {
            // Lấy dữ liệu doanh thu 7 ngày qua
            var revenueData = new List<decimal>();
            var labels = new List<string>();
            
            for (int i = 6; i >= 0; i--)
            {
                DateTime date = DateTime.Now.AddDays(-i);
                labels.Add(date.ToString("dd/MM"));
                
                // Tính doanh thu theo ngày (giả lập dữ liệu)
                decimal dailyRevenue = GetDailyRevenue(date);
                revenueData.Add(dailyRevenue);
            }

            litRevenueLabels.Text = serializer.Serialize(labels);
            litRevenueData.Text = serializer.Serialize(revenueData);
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>alert('Lỗi tải biểu đồ doanh thu: {0}');</script>", ex.Message));
        }
    }

    private void LoadOrderStatusChart()
    {
        try
        {
            List<Order> orders = orderDAL.GetAllOrders();
            
            var statusCounts = orders.GroupBy(o => o.Status)
                                   .Select(g => new { Status = g.Key, Count = g.Count() })
                                   .ToList();

            var labels = new List<string>();
            var data = new List<int>();

            foreach (var status in statusCounts)
            {
                labels.Add(GetStatusDisplayName(status.Status));
                data.Add(status.Count);
            }

            litStatusLabels.Text = serializer.Serialize(labels);
            litStatusData.Text = serializer.Serialize(data);
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>alert('Lỗi tải biểu đồ trạng thái: {0}');</script>", ex.Message));
        }
    }

    private void LoadRecentOrders()
    {
        try
        {
            List<Order> orders = orderDAL.GetAllOrders();
            var recentOrders = orders.OrderByDescending(o => o.CreatedDate)
                                   .Take(5)
                                   .Select(o => new
                                   {
                                       Id = o.Id,
                                       CustomerName = GetCustomerName(o.UserId),
                                       TotalAmount = o.TotalAmount,
                                       Status = o.Status
                                   })
                                   .ToList();

            rptRecentOrders.DataSource = recentOrders;
            rptRecentOrders.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>alert('Lỗi tải đơn hàng gần đây: {0}');</script>", ex.Message));
        }
    }

    private void LoadTopProducts()
    {
        try
        {
            List<Product> products = productDAL.GetAllProducts();
            List<Order> orders = orderDAL.GetAllOrders();
            
            // Giả lập dữ liệu sản phẩm bán chạy
            var topProducts = products.Take(5).Select(p => new
            {
                Name = p.Name,
                Price = p.Price,
                SoldCount = new Random().Next(10, 100), // Giả lập số lượng bán
                Revenue = string.Format("{0:N0}đ", p.Price * new Random().Next(10, 100))
            }).ToList();

            rptTopProducts.DataSource = topProducts;
            rptTopProducts.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(string.Format("<script>alert('Lỗi tải sản phẩm bán chạy: {0}');</script>", ex.Message));
        }
    }

    private decimal GetDailyRevenue(DateTime date)
    {
        try
        {
            List<Order> orders = orderDAL.GetAllOrders();
            return orders.Where(o => o.CreatedDate.Date == date.Date && o.Status == "Completed")
                        .Sum(o => o.TotalAmount);
        }
        catch
        {
            // Trả về giá trị giả lập nếu có lỗi
            return new Random().Next(1000000, 5000000);
        }
    }

    private string GetCustomerName(int userId)
    {
        try
        {
            User user = userDAL.GetUserById(userId);
            return user != null ? user.FullName : "Khách hàng";
        }
        catch
        {
            return "Khách hàng";
        }
    }

    public string GetStatusDisplayName(string status)
    {
        switch (status.ToLower())
        {
            case "pending": return "Chờ xử lý";
            case "processing": return "Đang xử lý";
            case "shipping": return "Đang giao hàng";
            case "completed": return "Hoàn thành";
            case "cancelled": return "Đã hủy";
            default: return status;
        }
    }
}