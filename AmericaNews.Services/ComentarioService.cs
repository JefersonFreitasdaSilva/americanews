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

        public async Task<List<ComentarioModel>> GetAllByNoticia(int idNoticia)
        {
            var comentarios = await _comentarioRepository.GetAllByNoticia(idNoticia);

            if (comentarios == null || !comentarios.Any())
                throw new KeyNotFoundException("Nenhuma comentário foi encontrado para a notícia de ID " + idNoticia);

            return comentarios;
        }

        public async Task<List<ComentarioModel>> GetAllByStatus(int status)
        {
            var comentarios = await _comentarioRepository.GetAllByStatus(status);

            if (comentarios == null || !comentarios.Any())
                throw new KeyNotFoundException("Nenhuma comentário foi encontrado com o status de ID " + status);

            return comentarios;
        }

        public async Task<ComentarioModel> GetById(int id)
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
                    throw new KeyNotFoundException("O usuário que está cadastrando o comentário não foi encontrado!");

                if (!_noticiaService.NoticiaExists(comentario.IDNoticia))
                    throw new KeyNotFoundException("A notícia não foi encontrada!");

                _comentarioRepository.Insert(comentario);

                var registros = new List<RegistroModel>()
                {
                    new RegistroModel("Comentario", "Texto", string.Empty, comentario.Texto, date, comentario.IDUsuario),
                    new RegistroModel("Comentario", "status", string.Empty, comentario.Status.ToString(), date, comentario.IDUsuario),
                    new RegistroModel("Comentario", "IDUsuario", string.Empty, comentario.IDUsuario.ToString(), date, comentario.IDUsuario),
                    new RegistroModel("Comentario", "IDNoticia", string.Empty, comentario.IDNoticia.ToString(), date, comentario.IDUsuario),
                    new RegistroModel("Comentario", "Data", string.Empty, comentario.Data.ToString(), date, comentario.IDUsuario)
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

                int oldComentarioStatus = comentario.Status;

                comentario.Status = Convert.ToInt32(newStatus);
                comentario.ID_ADM_Reprovou = idAdmin;
                comentario.DataReprovado = DateTime.Now;

                _comentarioRepository.UpdateStatus(comentario);

                var registros = new List<RegistroModel>()
                {
                    new RegistroModel("Comentario", "status", oldComentarioStatus.ToString(), comentario.Status.ToString(), DateTime.Now, idAdmin),
                    new RegistroModel("Comentario", "ID_ADM_Reprovou ", string.Empty, comentario.ID_ADM_Reprovou.ToString(), DateTime.Now, idAdmin),
                    new RegistroModel("Comentario", "DataReprovado", string.Empty, comentario.DataReprovado.ToString(), DateTime.Now, idAdmin)
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
