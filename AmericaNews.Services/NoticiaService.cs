using AmericaNews.Data.Interfaces;
using AmericaNews.Services.Interfaces;

namespace AmericaNews.Services
{
    public class NoticiaService : INoticiaService
    {

        INoticiaRepository _noticiaRepository;
        IRegistroRepository _registroService;
        IUsuarioService _usuarioService;

        public NoticiaService(INoticiaRepository noticiasRepository, IRegistroRepository registroRepository, IUsuarioService usuarioService)
        {
            _noticiaRepository = noticiasRepository;
            _registroService = registroRepository;
            _usuarioService = usuarioService;
        }

        public async Task<List<NoticiaModel>> GetAll()
        {
            var noticias = await _noticiaRepository.GetAll();

            if (noticias == null || !noticias.Any())
                throw new KeyNotFoundException("Nenhuma notícia foi encontrada!");

            return noticias;
        }

        public async Task<List<NoticiaModel>> GetAllByStatus(int status)
        {
            var noticias = await _noticiaRepository.GetAllByStatus(status);

            if (noticias == null || !noticias.Any())
                throw new KeyNotFoundException(string.Format("Nenhuma notícia com o status {0} foi encontrada!", status));

            return noticias;
        }

        public async Task<List<NoticiaModel>> Search(string termo, int status)
        {
            var noticias = await _noticiaRepository.Search(termo, status);

            if (noticias == null || !noticias.Any())
                throw new KeyNotFoundException(string.Format("Nenhuma notícia de status ID {0} com a palavra {1} no título foi encontrada!", status, termo));

            return noticias;
        }

        public async Task<NoticiaModel> GetById(int id)
        {
            try
            {
                var noticia = _noticiaRepository.GetById(id);

                if (noticia == null)
                    throw new KeyNotFoundException(string.Format("A Notícia de ID {0} não foi encontrada!", id));

                return noticia;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar a noticia de ID {0}. Detalhes: {1}", id, ex.Message));
                throw;
            }      
        }

        public bool NoticiaExists(int id)
        {
            var exists = false;
            var noticia = _noticiaRepository.GetById(id);

            if (noticia != null)
                exists = true;

            return exists;
        }

        public void Insert(NoticiaModel noticia)
        {
            try
            {
                var date = DateTime.Now;
                noticia.Data = date;

                if (!_usuarioService.UsuarioExists(noticia.IDUsuario))
                    throw new KeyNotFoundException("O usuário que está cadastrando a notícia não foi encontrado!");

                _noticiaRepository.Insert(noticia);

                var registros = new List<RegistroModel>()
                {
                    new RegistroModel("Noticia", "Titulo", string.Empty, noticia.Titulo, date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "Subtitulo", string.Empty, noticia.Subtitulo != null ? noticia.Subtitulo : string.Empty, date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "Texto", string.Empty, noticia.Texto != null ? noticia.Texto : string.Empty, date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "Data", string.Empty, noticia.Data.ToString(), date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "LinkIMG", string.Empty, noticia.LinkIMG, date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "status", string.Empty, noticia.Status.ToString(), date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "IDUsuario", string.Empty, noticia.IDUsuario.ToString(), date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "ID_ADM_Aprovou", string.Empty, "null", date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "DataAprovada", string.Empty, "null", date, noticia.IDUsuario)
                };

                _registroService.InsertBatch(registros);

            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao cadastrar a noticia: {0}", ex.Message));
                throw;
            }
        }

        public void UpdateStatus(int idNoticia, int newStatus, int idAdmin)
        {
            try
            {
                var noticia = _noticiaRepository.GetById(idNoticia);

                if (noticia == null)
                    throw new KeyNotFoundException(string.Format("A notícia de ID {0} não foi encontrada!", idNoticia.ToString()));

                if (!_usuarioService.AdminExists(idAdmin))
                    throw new KeyNotFoundException(string.Format("O administrador que está modificando o status da notícia não é válido! Id Admin: {0}", idAdmin.ToString()));

                int oldStatus = noticia.Status;

                noticia.Status = newStatus;
                noticia.ID_ADM_Aprovou = idAdmin;
                noticia.DataAprovada = DateTime.Now;
                _noticiaRepository.UpdateStatus(noticia);

                var registros = new List<RegistroModel>()
                {
                    new RegistroModel("Noticia", "Status", oldStatus.ToString(), noticia.Status.ToString(), DateTime.Now, idAdmin),
                    new RegistroModel("Noticia", "ID_ADM_Aprovou", string.Empty, noticia.ID_ADM_Aprovou.ToString(), DateTime.Now, idAdmin),
                    new RegistroModel("Noticia", "DataAprovada", string.Empty, noticia.DataAprovada.ToString(), DateTime.Now, idAdmin)
                };

                _registroService.InsertBatch(registros);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao atualizar o status da noticia: {0}", ex.Message));
                throw;
            }
        }
    }
}
