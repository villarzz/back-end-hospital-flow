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

        public List<Paciente> GetPacientes()
        {
            var pacientes = new List<Paciente>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Paciente";

                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pacientes.Add(new Paciente
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            DataNascimento = reader.GetString(2),
                            Cpf = reader.GetString(3),
                            Convenio = reader.IsDBNull(4) ? null : reader.GetString(4) // Se for NULL, define como null na aplicação
                        });
                    }
                }
            }

            return pacientes;
        }
    }
}