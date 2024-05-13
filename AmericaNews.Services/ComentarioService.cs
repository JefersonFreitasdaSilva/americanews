using AmericaNews.Data;
using AmericaNews.Data.Interfaces;
using AmericaNews.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace AmericaNews.Services
{
    public class ComentarioService : IComentarioServices
    {
        IComentarioRepository _comentarioRepository;
        IRegistroRepository _registroService;

        public ComentarioService(IComentarioRepository comentarioRepository, IRegistroRepository registroService)
        {
            _comentarioRepository = comentarioRepository;
            _registroService = registroService;
        }

        public List<ComentarioModel> GetAllByNoticia(int idNoticia)
        {
            return _comentarioRepository.GetAllByNoticia(idNoticia);
        }

        public List<ComentarioModel> GetAllByStatus(int status)
        {
            return _comentarioRepository.GetAllByStatus(status);
        }

        public ComentarioModel? GetById(int id)
        {
            return _comentarioRepository.GetById(id);
        }

        public void Insert(ComentarioModel comentario)
        {
            try
            {
                var date = DateTime.Now;
                comentario.Data = date;

                _comentarioRepository.Insert(comentario);

                var registros = new List<RegistroModel>()
                {
                    new RegistroModel("Comentario", "Texto", string.Empty, comentario.Texto, date, comentario.IDUsuario),
                    new RegistroModel("Comentario", "Ocultar", string.Empty, comentario.Ocultar.ToString(), date, comentario.IDUsuario),
                    new RegistroModel("Comentario", "IDUsuario", string.Empty, comentario.IDUsuario.ToString(), date, comentario.IDUsuario),
                    new RegistroModel("Comentario", "IDNoticia", string.Empty, comentario.IDNoticia.ToString(), date, comentario.IDUsuario),
                    new RegistroModel("Comentario", "qData", string.Empty, comentario.Data.ToString(), date, comentario.IDUsuario),
                    new RegistroModel("Comentario", "ID_ADM_Reprovou", string.Empty, comentario.ID_ADM_Reprovou.ToString(), date, comentario.IDUsuario),
                    new RegistroModel("Comentario", "DataReprovado", string.Empty, comentario.DataReprovado.ToString(), date, comentario.IDUsuario)
                };

                _registroService.InsertBatch(registros);

            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao cadastrar a noticia: {0}", ex.Message));
                throw;
            }
        }

        public void UpdateStatus(int idComentario, int newStatus, int idAdmin)
        {
            try
            {
                var comentario = _comentarioRepository.GetById(idComentario);
                
                if (comentario == null)
                    throw new ValidationException("A noticia não foi encontrada!");

                var oldComentario = comentario;

                comentario.Ocultar = Convert.ToBoolean(newStatus);
                comentario.ID_ADM_Reprovou = idAdmin;
                comentario.DataReprovado = DateTime.Now;

                _comentarioRepository.UpdateStatus(comentario);

                var registros = new List<RegistroModel>()
                {
                    new RegistroModel("Comentario", "Ocultar", oldComentario.Ocultar.ToString(), oldComentario.Ocultar.ToString(), DateTime.Now, idAdmin),
                    new RegistroModel("Comentario", "ID_ADM_Reprovou ", string.Empty, oldComentario.ID_ADM_Reprovou.ToString(), DateTime.Now, idAdmin),
                    new RegistroModel("Comentario", "DataReprovado", string.Empty, oldComentario.DataReprovado.ToString(), DateTime.Now, idAdmin)
                };

                _registroService.InsertBatch(registros);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao atualizar o status do comentario: {0}", ex.Message));
                throw;
            }
        }
    }
}
