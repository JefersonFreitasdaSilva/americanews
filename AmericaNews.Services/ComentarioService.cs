using AmericaNews.Data;
using AmericaNews.Data.Interfaces;
using AmericaNews.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace AmericaNews.Services
{
    public class ComentarioService : IComentarioService
    {
        IComentarioRepository _comentarioRepository;
        IRegistroRepository _registroService;
        IUsuarioService _usuarioService;
        INoticiaService _noticiaService;

        public ComentarioService(IComentarioRepository comentarioRepository, IRegistroRepository registroService, IUsuarioService usuarioService, INoticiaService noticiaService)
        {
            _comentarioRepository = comentarioRepository;
            _registroService = registroService;
            _usuarioService = usuarioService;
            _noticiaService = noticiaService;
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
            try { 
                var comentario = _comentarioRepository.GetById(id);

                if (comentario == null)
                    throw new KeyNotFoundException(string.Format("O comentário de ID {0} não foi encontrado!", id));

                return comentario;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar o comentário de ID {0}. Detalhes: {1}", id, ex.Message));
                throw;
            }
        }

        public void Insert(ComentarioModel comentario)
        {
            try
            {
                var date = DateTime.Now;
                comentario.Data = date;

                if (!_usuarioService.UsuarioExists(comentario.IDUsuario))
                    throw new KeyNotFoundException(string.Format("O usuário que está cadastrando o comentário não foi encontrado! Id Usuário: {0}", comentario.IDUsuario));

                if (!_noticiaService.NoticiaExists(comentario.IDNoticia))
                    throw new KeyNotFoundException(string.Format("A notícia não foi encontrada! Id Notícia: {0}", comentario.IDNoticia));

                _comentarioRepository.Insert(comentario);

                var registros = new List<RegistroModel>()
                {
                    new RegistroModel("Comentario", "Texto", string.Empty, comentario.Texto, date, comentario.IDUsuario),
                    new RegistroModel("Comentario", "status", string.Empty, comentario.Status.ToString(), date, comentario.IDUsuario),
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
                    throw new KeyNotFoundException(string.Format("O comentário de ID {0} não foi encontrado!", idComentario));

                if (!_usuarioService.AdminExists(idAdmin))
                    throw new KeyNotFoundException(string.Format("O administrador que está modificando o status do comentário não é válido! Id Admin: {0}", idAdmin));

                var oldComentario = comentario;

                comentario.Status = Convert.ToInt32(newStatus);
                comentario.ID_ADM_Reprovou = idAdmin;
                comentario.DataReprovado = DateTime.Now;

                _comentarioRepository.UpdateStatus(comentario);

                var registros = new List<RegistroModel>()
                {
                    new RegistroModel("Comentario", "status", oldComentario.Status.ToString(), oldComentario.Status.ToString(), DateTime.Now, idAdmin),
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
