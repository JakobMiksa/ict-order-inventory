using ICT.Model;
using Microsoft.Data.SqlClient;

namespace ICT.DataAccess;

public class ProductAccess
{
    private readonly string _connectionString;

    public ProductAccess(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Product> GetAll()
    {
        List<Product> products = new List<Product>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT Id, Name, Price, StockQuantity FROM Product ORDER BY Name", connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product product = new Product();
                    product.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    product.Name = reader.GetString(reader.GetOrdinal("Name"));
                    product.Price = reader.GetDecimal(reader.GetOrdinal("Price"));
                    product.StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity"));
                    products.Add(product);
                }
            }
        }

        return products;
    }

    public bool AddStock(int productId, int amount)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("UPDATE Product SET StockQuantity = StockQuantity + @amount WHERE Id = @productId", connection);
            command.Parameters.AddWithValue("amount", amount);
            command.Parameters.AddWithValue("productId", productId);
            return command.ExecuteNonQuery() == 1;
        }
    }
}
