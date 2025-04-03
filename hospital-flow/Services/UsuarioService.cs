using hospital_flow.Models;
using Microsoft.Data.Sqlite;

namespace hospital_flow.Services
{
    public class UsuarioService
    {
        private readonly string _connectionString;

        public UsuarioService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void CriarOuAtualizarUsuario(Usuario usuario)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                // Verifica se o usuário já existe pelo nome de usuário
                string queryVerifica = "SELECT Id FROM Usuario WHERE USUARIO = @NomeUsuario";

                int? usuarioId = null;

                using (var commandVerifica = new SqliteCommand(queryVerifica, connection))
                {
                    commandVerifica.Parameters.AddWithValue("@NomeUsuario", usuario.NomeUsuario);
                    using (var reader = commandVerifica.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuarioId = reader.GetInt32(0);
                        }
                    }
                }

                if (usuarioId.HasValue)
                {
                    // Atualiza o usuário existente
                    string queryAtualiza = "UPDATE Usuario SET Senha = @Senha WHERE Id = @Id";

                    using (var commandAtualiza = new SqliteCommand(queryAtualiza, connection))
                    {
                        commandAtualiza.Parameters.AddWithValue("@Id", usuarioId.Value);
                        commandAtualiza.Parameters.AddWithValue("@Senha", usuario.Senha);

                        commandAtualiza.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Insere um novo usuário
                    string queryInserir = "INSERT INTO Usuario (Usuario, Senha) VALUES (@NomeUsuario, @Senha)";

                    using (var commandInserir = new SqliteCommand(queryInserir, connection))
                    {
                        commandInserir.Parameters.AddWithValue("@NomeUsuario", usuario.NomeUsuario);
                        commandInserir.Parameters.AddWithValue("@Senha", usuario.Senha);

                        commandInserir.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<Usuario> GetUsuarios()
        {
            var usuarios = new List<Usuario>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Usuario";

                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            Id = reader.GetInt32(0),
                            NomeUsuario = reader.GetString(1),
                            Senha = reader.GetString(2),
                        });
                    }
                }
            }

            return usuarios;
        }

        public void ExcluirUsuario(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Usuario WHERE Id = @Id";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new KeyNotFoundException("Usuário não encontrado.");
                    }
                }
            }
        }
    }
}