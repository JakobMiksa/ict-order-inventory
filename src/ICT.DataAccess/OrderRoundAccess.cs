using ICT.Model;
using Microsoft.Data.SqlClient;

namespace ICT.DataAccess;

public class OrderRoundAccess
{
    private readonly string _connectionString;

    public OrderRoundAccess(string connectionString)
    {
        _connectionString = connectionString;
    }

    public OrderRound? GetOpen()
    {
        OrderRound? round = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT Id, Start, Deadline FROM OrderRound WHERE Start <= @now AND Deadline > @now", connection);
            command.Parameters.AddWithValue("now", DateTime.Now);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    round = new OrderRound();
                    round.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    round.Start = reader.GetDateTime(reader.GetOrdinal("Start"));
                    round.Deadline = reader.GetDateTime(reader.GetOrdinal("Deadline"));
                }
            }
        }

        return round;
    }
}
