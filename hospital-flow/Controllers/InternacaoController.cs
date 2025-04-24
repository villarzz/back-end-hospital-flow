using hospital_flow.Models;
using hospital_flow.Services;
using Microsoft.AspNetCore.Mvc;

namespace hospital_flow.Controllers
{
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

            _internacaoService.PostInternacao(internacao);
            return Ok("Internação criada com sucesso.");
        }
    }


}
