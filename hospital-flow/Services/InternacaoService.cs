using Microsoft.Data.Sqlite;
using hospital_flow.Models;

public class InternacaoService
{
    private readonly string _connectionString;

    public InternacaoService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public void PostInternacao(Internacao internacao)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            string query = @"
            INSERT INTO Internacao 
            (DataInicio, DataFim, PacienteId, AcomodacaoId, StatusInternacaoId) 
            VALUES 
            (@DataInicio, @DataFim, @PacienteId, @AcomodacaoId, @StatusInternacaoId)";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DataInicio", internacao.DataInicio);
                command.Parameters.AddWithValue("@DataFim", (object?)internacao.DataFim ?? DBNull.Value);
                command.Parameters.AddWithValue("@PacienteId", internacao.PacienteId);
                command.Parameters.AddWithValue("@AcomodacaoId", internacao.AcomodacaoId);
                command.Parameters.AddWithValue("@StatusInternacaoId", internacao.StatusInternacaoId);

                command.ExecuteNonQuery();
            }
        }
    }

}