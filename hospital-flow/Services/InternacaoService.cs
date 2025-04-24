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

            // Gera o próximo número de atendimento automaticamente
            string queryUltimoAtendimento = "SELECT MAX(atendimento) FROM Internacao";
            int novoAtendimento = 1;

            using (var commandUltimo = new SqliteCommand(queryUltimoAtendimento, connection))
            {
                var resultado = commandUltimo.ExecuteScalar();
                if (resultado != DBNull.Value && resultado != null)
                {
                    novoAtendimento = Convert.ToInt32(resultado) + 1;
                }
            }

            // Insere uma nova internação com o novo número de atendimento
            string queryInserir = @"
            INSERT INTO Internacao 
            (DataInicio, DataFim, Atendimento, PacienteId, AcomodacaoId, StatusInternacaoId) 
            VALUES 
            (@DataInicio, @DataFim, @Atendimento, @PacienteId, @AcomodacaoId, @StatusInternacaoId)";

            using (var commandInserir = new SqliteCommand(queryInserir, connection))
            {
                commandInserir.Parameters.AddWithValue("@DataInicio", internacao.DataInicio);
                commandInserir.Parameters.AddWithValue("@DataFim", (object?)internacao.DataFim ?? DBNull.Value);
                commandInserir.Parameters.AddWithValue("@Atendimento", novoAtendimento);
                commandInserir.Parameters.AddWithValue("@PacienteId", internacao.PacienteId);
                commandInserir.Parameters.AddWithValue("@AcomodacaoId", internacao.AcomodacaoId);
                commandInserir.Parameters.AddWithValue("@StatusInternacaoId", internacao.StatusInternacaoId);

                commandInserir.ExecuteNonQuery();
            }
        }
    }


}