using hospital_flow.Services;
using Microsoft.AspNetCore.Mvc;

namespace hospital_flow.Controllers
{
    [ApiController]
    [Route("api/database")]
    public class DatabaseController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public DatabaseController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpGet("testar")]
        public IActionResult TestarConexao()
        {
            try
            {
                _databaseService.TestarConexao();
                return Ok("Conexão com SQLite bem-sucedida!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao conectar: {ex.Message}");
            }
        }
    }
}