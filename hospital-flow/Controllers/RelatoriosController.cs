using hospital_flow.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hospital_flow.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly RelatoriosService _relatoriosService;

        public RelatoriosController(RelatoriosService relatoriosService)
        {
            _relatoriosService = relatoriosService;
        }

        [HttpGet("relatorio-internacoes")]
        public IActionResult GerarRelatorioInternacoes(
            [FromQuery] string? atendimento,
            [FromQuery] string? nomePaciente,
            [FromQuery] string? convenio,
            [FromQuery] string? statusInternacao)
        {
            var arquivo = _relatoriosService.GerarRelatorioInternacoes(atendimento, nomePaciente, convenio, statusInternacao);

            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "RelatorioInternacoes.xlsx";

            return File(arquivo, contentType, fileName);
        }
    }
}
