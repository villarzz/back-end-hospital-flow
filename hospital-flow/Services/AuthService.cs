using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.IdentityModel.Tokens;

namespace hospital_flow.Services
{

    public class AuthService
    {
        private readonly string _connectionString;
        private readonly string _jwtSecret = "zQ9sk+3xyvKZsY+U8ZVgA5NlW7Zx9XkrsT9z0yhcBkQ=";

        public AuthService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public string? AutenticarUsuario(string nomeUsuario, string senha)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT Id FROM Usuario WHERE usuario = @NomeUsuario AND Senha = @Senha";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NomeUsuario", nomeUsuario);
                    command.Parameters.AddWithValue("@Senha", senha);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = reader.GetInt32(0);
                            return GerarToken(userId, nomeUsuario);
                        }
                    }
                }
            }
            return null;
        }

        private string GerarToken(int userId, string nomeUsuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, nomeUsuario)
            }),
                Expires = DateTime.UtcNow.AddHours(2), // Token expira em 2 horas
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}