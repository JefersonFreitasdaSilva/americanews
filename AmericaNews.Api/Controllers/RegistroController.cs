using AmericaNews.Data.Interfaces;
using AmericaNews.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AmericaNews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroController : ControllerBase
    {
        #region Dependências

        private readonly IRegistroRepository _registroRepository;
        private readonly IRegistroService _registroService;


        public RegistroController(IRegistroRepository registroRepository, IRegistroService registroService)
        {
            _registroRepository = registroRepository;
            _registroService = registroService;
        }

        #endregion

        [HttpGet]
        public async Task<ActionResult<List<RegistroModel>>> GetAll()
        {
            try
            {
                List<RegistroModel> regsitros = await _registroService.GetAll();
                return Ok(regsitros);
            }
            catch (ValidationException vex)
            {
                return BadRequest(new { message = vex.Message });
            }
            catch (KeyNotFoundException nex)
            {
                return NotFound(new { message = nex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro ao processar sua solicitação.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<RegistroModel>>> GetById(int id)
        {
            try
            {
                RegistroModel registro = await _registroService.GetById(id);
                return Ok(registro);
            }
            catch (ValidationException vex)
            {
                return BadRequest(new { message = vex.Message });
            }
            catch (KeyNotFoundException nex)
            {
                return NotFound(new { message = nex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro ao processar sua solicitação.", details = ex.Message });
            }
        }
    }
}
