using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class PaymentDAL
{
    private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    public int InsertPayment(Payment payment)
    {
        int newId = 0;
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Payments (OrderId, PaymentMethod, Amount, Status, TransactionId, PaymentData, ProcessedAt, CreatedAt)
                                 VALUES (@OrderId, @PaymentMethod, @Amount, @Status, @TransactionId, @PaymentData, @ProcessedAt, @CreatedAt);
                                 SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderId", payment.OrderId);
                    cmd.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod ?? "");
                    cmd.Parameters.AddWithValue("@Amount", payment.Amount);
                    cmd.Parameters.AddWithValue("@Status", payment.Status ?? "Pending");
                    cmd.Parameters.AddWithValue("@TransactionId", payment.TransactionId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PaymentData", payment.PaymentData ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ProcessedAt", payment.ProcessedAt ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedAt", payment.CreatedAt);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        newId = Convert.ToInt32(result);
                }
            }
        }
        catch (Exception ex)
        {
            // Log error if needed
        }
        return newId;
    }
} 