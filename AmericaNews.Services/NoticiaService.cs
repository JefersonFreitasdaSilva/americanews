using AmericaNews.Data;
using AmericaNews.Data.Interfaces;
using AmericaNews.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AmericaNews.Services
{
    public class NoticiaService : INoticiaService
    {

        INoticiaRepository _noticiaRepository;
        IRegistroRepository _registroService;

        public NoticiaService(INoticiaRepository noticiasRepository, IRegistroRepository registroRepository)
        {
            _noticiaRepository = noticiasRepository;
            _registroService = registroRepository;
        }

        public List<NoticiaModel> GetAll()
        {
            return _noticiaRepository.GetAll();
        }

        public List<NoticiaModel> GetAllByStatus(int status)
        {
            return _noticiaRepository.GetAllByStatus(status);
        }

        public NoticiaModel? GetById(int id)
        {
            return _noticiaRepository.GetById(id);
        }

        public void Insert(NoticiaModel noticia)
        {
            try
            {
                var date = DateTime.Now;
                noticia.Data = date;

                _noticiaRepository.Insert(noticia);

                var registros = new List<RegistroModel>()
                {
                    new RegistroModel("Noticia", "Titulo", string.Empty, noticia.Titulo, date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "Subtitulo", string.Empty, noticia.Subtitulo != null ? noticia.Subtitulo : string.Empty, date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "Texto", string.Empty, noticia.Texto != null ? noticia.Texto : string.Empty, date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "qData", string.Empty, noticia.Data.ToString(), date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "Ocultar", string.Empty, noticia.Ocultar.ToString(), date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "IDUsuario", string.Empty, noticia.IDUsuario.ToString(), date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "ID_ADM_Aprovou", string.Empty, noticia.ID_ADM_Aprovou.ToString(), date, noticia.IDUsuario),
                    new RegistroModel("Noticia", "DataAprovada", string.Empty, noticia.DataAprovada.ToString(), date, noticia.IDUsuario)
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
                    throw new ValidationException("A noticia não foi encontrada!");

                var oldNoticia = noticia;

                noticia.Ocultar = Convert.ToBoolean(newStatus);
                noticia.ID_ADM_Aprovou = idAdmin;
                noticia.DataAprovada = DateTime.Now;
                _noticiaRepository.UpdateStatus(noticia);

                var registros = new List<RegistroModel>()
                {
                    new RegistroModel("Noticia", "Ocultar", oldNoticia.Ocultar.ToString(), noticia.Ocultar.ToString(), DateTime.Now, idAdmin),
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
