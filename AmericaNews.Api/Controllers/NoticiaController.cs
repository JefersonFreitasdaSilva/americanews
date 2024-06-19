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
    public class NoticiaController : ControllerBase
    {
        #region Dependências

        private readonly INoticiaRepository _noticiaRepository;
        private readonly INoticiaService _noticiaService;


        public NoticiaController(INoticiaRepository noticiaRepository, INoticiaService noticiaService)
        {
            _noticiaRepository = noticiaRepository;
            _noticiaService = noticiaService;
        }

        #endregion

        [HttpGet]
        public async Task<ActionResult<List<NoticiaModel>>> GetAll()
        {
            try
            {
                List<NoticiaModel> noticias = await _noticiaService.GetAll();
                return Ok(noticias);
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
        public async Task<ActionResult<List<NoticiaModel>>> GetById(int id)
        {
            try
            {
                NoticiaModel noticias = await _noticiaService.GetById(id);
                return Ok(noticias);
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
        public async Task<ActionResult<List<NoticiaModel>>> GetAllByStatus(int status)
        {
            try
            {
                List<NoticiaModel> noticias = await _noticiaService.GetAllByStatus(status);
                return Ok(noticias);
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

        [HttpGet("search/{termo}/{status}")]
        public async Task<ActionResult<List<NoticiaModel>>> Search(string termo, int status)
        {
            try
            {
                List<NoticiaModel> noticias = await _noticiaService.Search(termo, status);
                return Ok(noticias);
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

        [HttpGet("search/{termo}")]
        public async Task<ActionResult<List<NoticiaModel>>> SearchAdmin(string termo)
        {
            try
            {
                List<NoticiaModel> noticias = await _noticiaService.Search(termo, 0);
                return Ok(noticias);
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
        public async Task<ActionResult<List<NoticiaModel>>> Insert([FromBody] Noticia noticia)
        {
            try
            {
                string validationError = NoticiaValidations(noticia);

                if (!string.IsNullOrEmpty(validationError))
                    throw new ValidationException(validationError);

                var noticiaModel = new NoticiaModel()
                {
                    Titulo = noticia.Titulo,
                    Subtitulo = noticia.Subtitulo,
                    Texto = noticia.Texto,
                    LinkIMG = noticia.LinkIMG,
                    IDUsuario = noticia.IDUsuario,
                    Status = (int)EnumStatus.Pendente,
                    ID_ADM_Aprovou = 0
            };

                _noticiaService.Insert(noticiaModel);
                return Ok(noticiaModel);
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

                _noticiaService.UpdateStatus(id, status, idAdmin);

                var mensagem = string.Format("O status da Notícia de ID {0} foi alterado para {1} pelo admin de ID {2}",
                    id, (EnumStatus)status, idAdmin);

                return Ok(new { mensagem = mensagem });
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

        private string NoticiaValidations(Noticia noticia)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(noticia.Titulo) || noticia.Titulo.Length > 255)
                error = "Título Inválido!";

            if (!string.IsNullOrEmpty(noticia.Subtitulo) && noticia.Subtitulo.Length > 255)
                error = "Subtítulo Inválido!";

            if (string.IsNullOrEmpty(noticia.Texto))
                error = "Texto Inválido!";

            if (string.IsNullOrEmpty(noticia.LinkIMG))
                error = "Link de imagem Inválido!";

            if (noticia.IDUsuario <= 0)
                error = "O Id do Usuário é Inválido!";

            return error;
        }
    }
}
