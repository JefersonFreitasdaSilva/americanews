using AmericaNews.Api.Models;
using AmericaNews.Data.Interfaces;
using AmericaNews.Data.Models;
using AmericaNews.Services.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AmericaNews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        #region Dependências

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioService _usuarioService;

        #endregion

        public UsuarioController(IUsuarioRepository usuarioRepository, IUsuarioService usuarioService)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioModel>> LogIn([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(loginRequest.Email))
                    throw new ValidationException("E-mail Inválido!");

                if (string.IsNullOrEmpty(loginRequest.Password))
                    throw new ValidationException("Senha Inválida!");

                UsuarioModel usuario = await _usuarioService.GetByCredentials(loginRequest.Email, loginRequest.Password);
                return Ok(usuario);
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
        public async Task<ActionResult<UsuarioModel>> Insert([FromBody] Usuario usuario)
        {
            try
            {
                string validationError = UsuarioValidations(usuario);

                if (!string.IsNullOrEmpty(validationError))
                    throw new ValidationException(validationError);

                var usuarioModel = new UsuarioModel()
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Senha = usuario.Senha,
                    EmailCorporativo = usuario.EmailCorporativo,
                    Endereco = usuario.Endereco,
                    NivelPermissao = (int)EnumNivelPermissao.Usuario,
                    Telefone = usuario.Telefone
                };

                _usuarioService.Insert(usuarioModel, usuario.AdminId);
                return Ok(usuarioModel);
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

        private string UsuarioValidations(Usuario usuario)
        {
            var error = string.Empty;
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (string.IsNullOrEmpty(usuario.Email) || !Regex.IsMatch(usuario.Email, emailPattern, RegexOptions.IgnoreCase) || usuario.Nome.Length > 150)
                error = "E-mail Inválido!";

            if (string.IsNullOrEmpty(usuario.Senha))
                error = "Senha Inválida!";

            if (string.IsNullOrEmpty(usuario.Nome) || usuario.Nome.Length > 100)
                error = "Nome Inválido!";

            if (!string.IsNullOrEmpty(usuario.Telefone) && usuario.Telefone.Length > 25)
                error = "Telefone Inválido!";

            if (!string.IsNullOrEmpty(usuario.Endereco) && usuario.Endereco.Length > 200)
                error = "Endereço Inválido!";

            if (string.IsNullOrEmpty(usuario.EmailCorporativo) || usuario.EmailCorporativo.Length > 100)
                error = "E-mail Corporativo Inválido!";

            if (usuario.AdminId <= 0)
                error = "O Id do Administrador é Inválido!";

            return error;
        }
    }
}
