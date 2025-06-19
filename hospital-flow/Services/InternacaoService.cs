using Microsoft.Data.Sqlite;
using hospital_flow.Models;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Office2010.Excel;

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

    public void PutInternacao(InternacaoVielModel internacao)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            string query = @"
                UPDATE Internacao 
                SET DataInicio = @DataInicio,
                    DataFim = @DataFim,
                    PacienteId = @PacienteId,
                    AcomodacaoId = @AcomodacaoId,
                    StatusInternacaoId = @StatusInternacaoId
                WHERE Id = @Id";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DataInicio", internacao.DataInicio);
                command.Parameters.AddWithValue("@DataFim", (object?)internacao.DataFim ?? DBNull.Value);
                command.Parameters.AddWithValue("@PacienteId", internacao.PacienteId);
                command.Parameters.AddWithValue("@AcomodacaoId", internacao.AcomodacaoId);
                command.Parameters.AddWithValue("@StatusInternacaoId", internacao.StatusInternacaoId);
                command.Parameters.AddWithValue("@Id", internacao.Id);

                command.ExecuteNonQuery();
            }
        }
    }

    public List<InternacaoFiltro> ObterInternacoes(string? atendimento, string? nomePaciente, string? convenio, string? statusInternacao)
    {
        var internacoes = new List<InternacaoFiltro>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var sql = @"SELECT 
                            i.*, 
                            p.Nome AS NomePaciente, 
                        p.Convenio, 
                        a.DESCRICAO as DescricaoAcomodacao, 
                        s.DESCRICAO as StatusInternacaoDescricao
                    FROM INTERNACAO i
                    LEFT JOIN ACOMODACAO a ON i.ACOMODACAOID = a.id
                    LEFT JOIN STATUSINTERNACAO s ON i.STATUSINTERNACAOID = s.id
                    LEFT JOIN Paciente p ON i.PACIENTEID = p.id
                    WHERE 1 = 1";

            var parameters = new List<SqliteParameter>();

            if (!string.IsNullOrEmpty(atendimento))
            {
                sql += " AND i.Atendimento = @Atendimento";
                parameters.Add(new SqliteParameter("@Atendimento", atendimento));
            }

            if (!string.IsNullOrEmpty(nomePaciente))
            {
                sql += " AND p.NOME LIKE @NomePaciente";
                parameters.Add(new SqliteParameter("@NomePaciente", $"%{nomePaciente}%"));
            }

            if (!string.IsNullOrEmpty(convenio))
            {
                sql += " AND p.Convenio LIKE @Convenio";
                parameters.Add(new SqliteParameter("@Convenio", $"%{convenio}%"));
            }

            if (!string.IsNullOrEmpty(statusInternacao))
            {
                sql += " AND i.STATUSINTERNACAOID = @Status";
                parameters.Add(new SqliteParameter("@Status", statusInternacao));
            }

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddRange(parameters.ToArray());

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        internacoes.Add(new InternacaoFiltro
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Atendimento = Convert.ToInt32(reader["Atendimento"]),
                            DataInicio = reader["DataInicio"]?.ToString(),
                            DataFim = reader["DataFim"]?.ToString(),
                            StatusInternacaoId = Convert.ToInt32(reader["STATUSINTERNACAOID"]),
                            PacienteId = Convert.ToInt32(reader["PACIENTEID"]),
                            NomePaciente = reader["NomePaciente"]?.ToString(),
                            Convenio = reader["Convenio"]?.ToString(),
                            AcomodacaoId = Convert.ToInt32(reader["ACOMODACAOID"]),
                            AcomodacaoDescricao = reader["DescricaoAcomodacao"]?.ToString(),
                            StatusInternacaoDescricao = reader["StatusInternacaoDescricao"]?.ToString()
                        });
                    }
                }
            }
        }
        return internacoes;
    }

    public void DeletarInternacao(int id)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            string query = "DELETE FROM Internacao WHERE Id = @Id";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}