using Microsoft.Data.SqlClient;

const string connectionSting = @"Server=BST\SQLEXPRESS;Database=MinionsDB;Trusted_Connection=True;Trust Server Certificate=true";

SqlConnection connection = new SqlConnection(connectionSting);
await connection.OpenAsync();

SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync();

try
{
    Console.Write("Ender Villain Id: ");
    string? id = Console.ReadLine();

    string vilainQuery = @"SELECT Name FROM Villains WHERE Id = @Id";
    string commandQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

    SqlCommand vilainCommand = new SqlCommand(vilainQuery, connection, transaction);
    vilainCommand.Parameters.AddWithValue("@Id", id);

    SqlCommand command = new SqlCommand(commandQuery, connection, transaction);
    command.Parameters.AddWithValue("@Id", id);

    using (connection)
    {

        string? vilainName = (string?)await vilainCommand.ExecuteScalarAsync();

        Console.WriteLine($"Villain: {vilainName}");

        SqlDataReader reader = await command.ExecuteReaderAsync();

        string? minionName = string.Empty;
        int? minionAge = 0;

        int counter = 1;

        while (reader.Read())
        {
            minionName = reader["Name"].ToString();
            minionAge = int.Parse(reader["Age"].ToString()!);
            Console.WriteLine($"{counter}. {minionName} {minionAge}");

            counter++;
        }
    }
}
catch (Exception e)
{
    await transaction.RollbackAsync();
    Console.WriteLine($"Error occured, transaction rolled back!. Error: {e.Message}");
}

