using hospital_flow.Models;
using hospital_flow.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hospital_flow.Controllers 
{
    [ApiController]
    [Route("api/usuarios")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("adicionar")]
        public IActionResult CriarOuAtualizarUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null || string.IsNullOrWhiteSpace(usuario.NomeUsuario) || string.IsNullOrWhiteSpace(usuario.Senha))
            {
                return BadRequest("Nome de usuário e senha são obrigatórios.");
            }

            _usuarioService.CriarOuAtualizarUsuario(usuario);
            return Ok("Usuário criado ou atualizado com sucesso.");
        }

        [HttpGet("obter-usuarios")]
        public IActionResult GetUsuarios()
        {
            var usuarios = _usuarioService.GetUsuarios();
            return Ok(usuarios);
        }

        [HttpDelete("deletar/{id}")]
        public IActionResult ExcluirUsuario(int id)
        {
            try
            {
                _usuarioService.ExcluirUsuario(id);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Usuário não encontrado.");
            }
        }
    }

}
