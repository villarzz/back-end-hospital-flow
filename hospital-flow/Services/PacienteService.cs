using hospital_flow.Models;
using Microsoft.Data.Sqlite;
using System.Data.Entity;

namespace hospital_flow.Services
{
    public class PacienteService
    {
        private readonly string _connectionString;

        public PacienteService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void PostPaciente(Paciente paciente)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                // Verifica se já existe um paciente com o mesmo CPF
                string queryVerifica = "SELECT Id FROM Paciente WHERE Cpf = @Cpf";

                int? pacienteId = null;

                using (var commandVerifica = new SqliteCommand(queryVerifica, connection))
                {
                    commandVerifica.Parameters.AddWithValue("@Cpf", paciente.Cpf);
                    using (var reader = commandVerifica.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pacienteId = reader.GetInt32(0); // Obtém o ID do paciente existente
                        }
                    }
                }

                if (pacienteId.HasValue)
                {
                    // Atualiza o paciente existente
                    string queryAtualiza = @"UPDATE Paciente 
                                     SET Nome = @Nome, 
                                         DataNascimento = @DataNascimento, 
                                         Convenio = @Convenio 
                                     WHERE Id = @Id";

                    using (var commandAtualiza = new SqliteCommand(queryAtualiza, connection))
                    {
                        commandAtualiza.Parameters.AddWithValue("@Id", pacienteId.Value);
                        commandAtualiza.Parameters.AddWithValue("@Nome", paciente.Nome);
                        commandAtualiza.Parameters.AddWithValue("@DataNascimento", paciente.DataNascimento);
                        commandAtualiza.Parameters.AddWithValue("@Convenio", (object?)paciente.Convenio ?? DBNull.Value);

                        commandAtualiza.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Insere um novo paciente
                    string queryInserir = "INSERT INTO Paciente (Nome, DataNascimento, Cpf, Convenio) VALUES (@Nome, @DataNascimento, @Cpf, @Convenio)";

                    using (var commandInserir = new SqliteCommand(queryInserir, connection))
                    {
                        commandInserir.Parameters.AddWithValue("@Nome", paciente.Nome);
                        commandInserir.Parameters.AddWithValue("@DataNascimento", paciente.DataNascimento);
                        commandInserir.Parameters.AddWithValue("@Cpf", paciente.Cpf);
                        commandInserir.Parameters.AddWithValue("@Convenio", (object?)paciente.Convenio ?? DBNull.Value);

                        commandInserir.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<PacienteFiltro> GetPacientes(string? nomePaciente, string? cpf, string? dataNascimento)
        {
            var pacientes = new List<PacienteFiltro>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var sql = @"SELECT DISTINCT p.*
                    FROM Paciente p
                    LEFT JOIN INTERNACAO i ON i.PACIENTEID = p.Id
                    WHERE 1 = 1";

                var parameters = new List<SqliteParameter>();

                if (!string.IsNullOrEmpty(nomePaciente))
                {
                    sql += " AND p.NOME LIKE @NomePaciente";
                    parameters.Add(new SqliteParameter("@NomePaciente", $"%{nomePaciente}%"));
                }

                if (!string.IsNullOrEmpty(cpf))
                {
                    sql += " AND p.Cpf LIKE @Cpf";
                    parameters.Add(new SqliteParameter("@Cpf", $"%{cpf}%"));
                }

                if (!string.IsNullOrEmpty(dataNascimento))
                {
                    sql += " AND p.DataNascimento = @DataNascimento";
                    parameters.Add(new SqliteParameter("@DataNascimento", dataNascimento));
                }

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pacientes.Add(new PacienteFiltro
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                DataNascimento = reader.GetString(reader.GetOrdinal("DataNascimento")),
                                Cpf = reader.GetString(reader.GetOrdinal("Cpf")),
                                Convenio = reader.IsDBNull(reader.GetOrdinal("Convenio"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("Convenio"))
                            });
                        }
                    }
                }
            }

            return pacientes;
        }
    }
}