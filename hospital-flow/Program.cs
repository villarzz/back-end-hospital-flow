using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using hospital_flow.Services;

namespace hospital_flow
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Chave secreta para JWT (use uma chave mais segura em produção)
            var key = Encoding.ASCII.GetBytes("sua-chave-secreta-super-segura");

            // Adicionando serviços ao contêiner
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 🟡 Configuração do CORS para produção (permitir apenas uma origem específica)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("https://seu-dominio.com") // Substitua pelo domínio do seu front-end
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Configuração da autenticação JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;  // Impede uso de HTTP (só HTTPS)
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Adicionando autorização
            builder.Services.AddAuthorization();

            // Serviços do banco de dados e lógica da aplicação
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<PacienteService>();
            builder.Services.AddSingleton<UsuarioService>();
            builder.Services.AddSingleton<AuthService>();

            var app = builder.Build();

            // Configuração do pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // 🟡 Ativando o CORS restrito no pipeline para produção
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
