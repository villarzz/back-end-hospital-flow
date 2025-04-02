using Microsoft.Data.Sqlite;

public class DatabaseService
{
    private readonly string _connectionString = "Data Source=./hospital-flow-DB.db;";

    public void TestarConexao()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            Console.WriteLine("Conexão com SQLite bem-sucedida!");
        }
    }
}
