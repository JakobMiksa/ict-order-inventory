using System.Transactions;
using ICT.Model;
using Microsoft.Data.SqlClient;

namespace ICT.DataAccess;

public class OrderAccess
{
    private readonly string _connectionString;

    public OrderAccess(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int Create(Order order)
    {
        int orderId;

        TransactionOptions options = new TransactionOptions();
        options.IsolationLevel = IsolationLevel.ReadCommitted;

        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand insertOrder = connection.CreateCommand())
                {
                    insertOrder.CommandText = "INSERT INTO Orders (MemberId, OrderRoundId) OUTPUT INSERTED.Id VALUES (@memberId, @orderRoundId)";
                    insertOrder.Parameters.AddWithValue("memberId", order.MemberId);
                    insertOrder.Parameters.AddWithValue("orderRoundId", order.OrderRoundId);
                    orderId = (int)insertOrder.ExecuteScalar();
                }

                foreach (OrderLine line in order.Lines)
                {
                    using (SqlCommand insertLine = connection.CreateCommand())
                    {
                        insertLine.CommandText = "INSERT INTO OrderLine (OrderId, ProductId, Quantity, UnitPrice) SELECT @orderId, Id, @quantity, Price FROM Product WHERE Id = @productId";
                        insertLine.Parameters.AddWithValue("orderId", orderId);
                        insertLine.Parameters.AddWithValue("productId", line.ProductId);
                        insertLine.Parameters.AddWithValue("quantity", line.Quantity);
                        if (insertLine.ExecuteNonQuery() == 0)
                        {
                            throw new InvalidOperationException("En af varerne findes ikke.");
                        }
                    }

                    using (SqlCommand updateStock = connection.CreateCommand())
                    {
                        updateStock.CommandText = "UPDATE Product SET StockQuantity = StockQuantity - @quantity WHERE Id = @productId";
                        updateStock.Parameters.AddWithValue("productId", line.ProductId);
                        updateStock.Parameters.AddWithValue("quantity", line.Quantity);
                        updateStock.ExecuteNonQuery();
                    }
                }
            }

            scope.Complete();
        }

        return orderId;
    }
}
