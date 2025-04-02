using Microsoft.Data.Sqlite;

namespace hospital_flow.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void TestarConexao()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine("Conexão com SQLite bem-sucedida!");
            }
        }
    }
}