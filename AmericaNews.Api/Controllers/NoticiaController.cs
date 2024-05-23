using AmericaNews.Api.Models;
using AmericaNews.Data;
using AmericaNews.Data.Interfaces;
using AmericaNews.Data.Models;
using AmericaNews.Services;
using AmericaNews.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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
                    Status = (int)EnumStatus.Pendente
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

        private string NoticiaValidations(Noticia noticia)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(noticia.Titulo) || noticia.Titulo.Length > 255)
                error = "Título Inválido!";

            if (!string.IsNullOrEmpty(noticia.Subtitulo) && noticia.Subtitulo.Length > 255)
                error = "Subtítulo Inválido!";

            if (!string.IsNullOrEmpty(noticia.Texto))
                error = "Texto Inválido!";

            if (!string.IsNullOrEmpty(noticia.LinkIMG))
                error = "Link de imagem Inválido!";

            if (noticia.IDUsuario <= 0)
                error = "O Id do Usuário é Inválido!";

            return error;
        }
    }
}
