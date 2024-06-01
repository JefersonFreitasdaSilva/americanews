using AmericaNews.Api.Models;
using AmericaNews.Data.Interfaces;
using AmericaNews.Data.Models;
using AmericaNews.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AmericaNews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        #region Dependências

        private readonly IComentarioRepository _comentarioRepository;
        private readonly IComentarioService _comentarioService;


        public ComentarioController(IComentarioRepository comentarioRepository, IComentarioService comentarioService)
        {
            _comentarioRepository = comentarioRepository;
            _comentarioService = comentarioService;
        }

        #endregion

        [HttpGet("noticia/{noticiaId}")]
        public async Task<ActionResult<List<ComentarioModel>>> GetAllByNoticia(int noticiaId)
        {
            try
            {
                List<ComentarioModel> comentarios = await _comentarioService.GetAllByNoticia(noticiaId);
                return Ok(comentarios);
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
        public async Task<ActionResult<List<ComentarioModel>>> GetById(int id)
        {
            try
            {
                ComentarioModel comentarios = await _comentarioService.GetById(id);
                return Ok(comentarios);
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

        [HttpGet("noticiaStatus/{noticiaId}/{status}")]
        public async Task<ActionResult<List<ComentarioModel>>> GetAllByNoticiaAndStatus(int noticiaId, int status)
        {
            try
            {
                List<ComentarioModel> comentarios = await _comentarioService.GetAllByStatusAndNoticia(status, noticiaId);
                return Ok(comentarios);
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

        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<ComentarioModel>>> GetAllByStatus(int status)
        {
            try
            {
                List<ComentarioModel> comentarios = await _comentarioService.GetAllByStatus(status);
                return Ok(comentarios);
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

        [HttpPost]
        public async Task<ActionResult<List<ComentarioModel>>> Insert([FromBody] Comentario comentario)
        {
            try
            {
                string validationError = ComentarioValidations(comentario);

                if (!string.IsNullOrEmpty(validationError))
                    throw new ValidationException(validationError);

                var comentarioModel = new ComentarioModel()
                {
                    Texto = comentario.Texto,
                    IDNoticia = comentario.IDNoticia,
                    IDUsuario = comentario.IDUsuario,
                    Status = (int)EnumStatus.Aprovado
                };

                _comentarioService.Insert(comentarioModel);
                return Ok(comentarioModel);
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

        [HttpPut("atualizarStatus/{id}/{status}/{idAdmin}")]
        public async Task<ActionResult<List<ComentarioModel>>> UpdateStatus(int id, int status, int idAdmin)
        {
            try
            {
                if (id <= 0 || status <= 0 || idAdmin <= 0)
                    throw new ValidationException("Parâmetros Inválidos!");

                 _comentarioService.UpdateStatus(id, status, idAdmin);

                var mensagem = string.Format("O status do Comentário de ID {0} foi alterado para {1} pelo admin de ID {2}", 
                    id, (EnumStatus)status, idAdmin);

                return Ok(new { mensagem = mensagem } );
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

        private string ComentarioValidations(Comentario comentario)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(comentario.Texto) || comentario.Texto.Length > 255)
                error = "Texto Inválido!";

            if (comentario.IDUsuario <= 0)
                error = "O Id do Usuário é Inválido!";

            if (comentario.IDNoticia <= 0)
                error = "O Id da Notíca é Inválido!";

            return error;
        }
    }
}
