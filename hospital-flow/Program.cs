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

            // Chave secreta para JWT (use uma chave mais segura em produ��o)
            var key = Encoding.ASCII.GetBytes("sua-chave-secreta-super-segura");

            // Adicionando servi�os ao cont�iner
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configura��o da autentica��o JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Adicionando autoriza��o
            builder.Services.AddAuthorization();

            // Servi�os do banco de dados e l�gica da aplica��o
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<PacienteService>();
            builder.Services.AddSingleton<UsuarioService>();
            builder.Services.AddSingleton<AuthService>();

            var app = builder.Build();

            // Configura��o do pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Adiciona autentica��o e autoriza��o ao pipeline
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }

}
