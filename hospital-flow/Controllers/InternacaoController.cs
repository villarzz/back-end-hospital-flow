using hospital_flow.Models;
using hospital_flow.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hospital_flow.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InternacoesController : ControllerBase
    {
        private readonly InternacaoService _internacaoService;

        public InternacoesController(InternacaoService internacaoService)
        {
            _internacaoService = internacaoService;
        }

        [HttpPost("criar-internacao")]
        public IActionResult PostInternacao([FromBody] Internacao internacao)
        {
            if (internacao == null)
            {
                return BadRequest("Dados inválidos.");
            }

            if (!DateTime.TryParseExact(internacao.DataInicio, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out _))
            {
                return BadRequest("Data de início deve estar no formato dd/MM/yyyy.");
            }

            if (!string.IsNullOrEmpty(internacao.DataFim))
            {
                if (!DateTime.TryParseExact(internacao.DataFim, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out _))
                {
                    return BadRequest("Data de fim deve estar no formato dd/MM/yyyy.");
                }
            }

            _internacaoService.PostInternacao(internacao);
            return Ok("Internação criada com sucesso.");
        }

        [HttpPut("atualizar-internacao")]
        public IActionResult PutInternacao([FromBody] Internacao internacao)
        {
            if (internacao == null || internacao.Id == 0)
            {
                return BadRequest("Dados inválidos.");
            }

            _internacaoService.PutInternacao(internacao);
            return Ok("Internação atualizada com sucesso.");
        }

        [HttpGet("obter-internacoes")]
        public IActionResult ObterInternacoes(
            [FromQuery] string? atendimento,
            [FromQuery] string? nomePaciente,
            [FromQuery] string? convenio,
            [FromQuery] string? statusInternacao)
        {
            var internacoes = _internacaoService.ObterInternacoes(atendimento, nomePaciente, convenio, statusInternacao);
            return Ok(internacoes);
        }
    }
}
