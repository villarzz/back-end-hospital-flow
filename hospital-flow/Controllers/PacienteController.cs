using hospital_flow.Models;
using hospital_flow.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hospital_flow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PacienteController : ControllerBase
    {
        private readonly PacienteService _pacienteService;

        public PacienteController(PacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpPost("adicionar-paciente")]
        public IActionResult PostPaciente([FromBody] Paciente paciente)
        {
            if (paciente == null)
            {
                return BadRequest("Dados inválidos.");
            }

            _pacienteService.PostPaciente(paciente);
            return Ok("Paciente criado ou atualizado com sucesso.");
        }


        [HttpGet("obter-pacientes")]
        public ActionResult<List<Paciente>> GetPacientes()
        {
            try
            {
                var pacientes = _pacienteService.GetPacientes();

                if (pacientes == null || pacientes.Count == 0)
                {
                    return StatusCode(201, "Nenhum paciente encontrado.");
                }

                return Ok(pacientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}