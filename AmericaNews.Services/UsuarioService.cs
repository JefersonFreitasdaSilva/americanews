using AmericaNews.Data;
using AmericaNews.Data.Interfaces;
using AmericaNews.Services.Interfaces;
using System.Data;

namespace AmericaNews.Services
{
    public class UsuarioService : IUsuarioService
    {
        IUsuarioRepository _usuarioRepository;
        IRegistroService _registroService;

        public UsuarioService(IUsuarioRepository usuarioRepository, IRegistroService registroService)
        {
            _usuarioRepository = usuarioRepository;
            _registroService = registroService;
        }

        public UsuarioModel? GetByCredentials(string email, string senha)
        {
            return _usuarioRepository.GetByCredentials(email, senha);
        }

        public UsuarioModel? GetById(int id)
        {
            return _usuarioRepository.GetById(id);
        }

        public void Insert(UsuarioModel usuario, int admId)
        {
            try
            {
                var date = DateTime.Now;
                usuario.Data = date;

                _usuarioRepository.Insert(usuario);

                var registros = new List<RegistroModel>()
                {
                    new RegistroModel("Usuarios", "Nome", string.Empty, usuario.Nome, date, admId),
                    new RegistroModel("Usuarios", "Telefone", string.Empty, usuario.Telefone != null ? usuario.Telefone : string.Empty, date, admId),
                    new RegistroModel("Usuarios", "Email", string.Empty, usuario.Email != null ? usuario.Email : string.Empty, date, admId),
                    new RegistroModel("Usuarios", "Endereco", string.Empty, usuario.Endereco != null ? usuario.Endereco : string.Empty, date, admId),
                    new RegistroModel("Usuarios", "qData", string.Empty, date.ToString(), date, admId),
                    new RegistroModel("Usuarios", "qData", string.Empty, date.ToString(), date, admId),
                    new RegistroModel("Usuarios", "EmailCorporativo", string.Empty, usuario.EmailCorporativo, date, admId),
                    new RegistroModel("Usuarios", "NivelPermissao", string.Empty, usuario.NivelPermissao.ToString(), date, admId)
                };

                _registroService.InsertBatch(registros);

            } catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao cadastrar o usuario: {0}", ex.Message));
                throw;
            }
        }
    }
}
