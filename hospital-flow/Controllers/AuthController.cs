using hospital_flow.Services;
using Microsoft.AspNetCore.Mvc;

namespace hospital_flow.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NomeUsuario) || string.IsNullOrWhiteSpace(request.Senha))
            {
                return BadRequest("Usuário e senha são obrigatórios.");
            }

            var token = _authService.AutenticarUsuario(request.NomeUsuario, request.Senha);

            if (token == null)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            return Ok(new { Token = token });
        }
    }

    public class LoginRequest
    {
        public string NomeUsuario { get; set; }
        public string Senha { get; set; }
    }

}
