using AmericaNews.Data.Interfaces;
using AmericaNews.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace AmericaNews.Services
{
    public class UsuarioService : IUsuarioService
    {
        IUsuarioRepository _usuarioRepository;
        IRegistroService _registroService;
        HashAlgorithm _criptografia;

        public UsuarioService(IUsuarioRepository usuarioRepository, IRegistroService registroService, HashAlgorithm algoritmo)
        {
            _usuarioRepository = usuarioRepository;
            _registroService = registroService;
            _criptografia = algoritmo;
        }

        public async Task<UsuarioModel> GetByCredentials(string email, string senha)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByCredentials(email);

                if (usuario == null)
                    throw new KeyNotFoundException(string.Format("O usuário com as seguintes credenciais não foi encontrado! Email: {0}  Senha {1}", email, senha));

                if (!VerificarSenha(senha, usuario.Senha))
                    throw new KeyNotFoundException(string.Format("A senha digitada está incorreta!", email, senha));

                return usuario;

            } catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar o usuario pelas credenciais: Email {0}  Senha: {1}. Detahes: {2}", email, senha, ex.Message));
                throw;
            }
        }

        public string CriptografarSenha(string senha)
        {
            var valorCodificado = Encoding.UTF8.GetBytes(senha);
            var senhaCriptografada = _criptografia.ComputeHash(valorCodificado);

            var sb = new StringBuilder();
            foreach (var caractere in senhaCriptografada)
            {
                sb.Append(caractere.ToString("X2"));
            }

            return sb.ToString();
        }

        public bool VerificarSenha(string senhaDigitada, string senhaCadastrada)
        {
            var senhaCriptografada = CriptografarSenha(senhaDigitada);
            return senhaCriptografada == senhaCadastrada;
        }

        public UsuarioModel? GetById(int id)
        {
            try
            {
                var usuario = _usuarioRepository.GetById(id);

                if (usuario == null)
                    throw new KeyNotFoundException(string.Format("O usuário de ID {0} não foi encontrado!", id));

                return usuario;

            } catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar o usuario pelo ID {0}. Detahes: {1}", id, ex.Message));
                throw;
            }
            
        }

        public bool AdminExists(int adminId)
        {
            var exists = false;
            var admin = _usuarioRepository.GetAdminById(adminId);

            if (admin != null)
                exists = true;

           return exists;
        }

        public bool UsuarioExists(int id)
        {
            var exists = false;
            var usuario = _usuarioRepository.GetById(id);

            if (usuario != null)
                exists = true;

            return exists;
        }

        public void Insert(UsuarioModel usuario, int admId)
        {
            try
            {
                var date = DateTime.Now;
                usuario.Data = date;
                usuario.Senha = CriptografarSenha(usuario.Senha);

                if (!AdminExists(admId))
                    throw new KeyNotFoundException(string.Format("O administrador que está cadastrando o usuário não é válido! Id Admin: {0}", admId));

                _usuarioRepository.Insert(usuario);

                var registros = new List<RegistroModel>()
                {
                    new RegistroModel("Usuarios", "Nome", string.Empty, usuario.Nome, date, admId),
                    new RegistroModel("Usuarios", "Telefone", string.Empty, usuario.Telefone != null ? usuario.Telefone : string.Empty, date, admId),
                    new RegistroModel("Usuarios", "Email", string.Empty, usuario.Email != null ? usuario.Email : string.Empty, date, admId),
                    new RegistroModel("Usuarios", "Senha", string.Empty, usuario.Senha, date, admId),
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
