using hospital_flow.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class PacienteController : ControllerBase
{
    private readonly PacienteService _pacienteService;

    public PacienteController(PacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    [HttpPost("adicionar-paciente")]
    public IActionResult CriarOuAtualizarPaciente([FromBody] Paciente paciente)
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
                return StatusCode(201,  "Nenhum paciente encontrado.");
            }

            return Ok(pacientes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }
}