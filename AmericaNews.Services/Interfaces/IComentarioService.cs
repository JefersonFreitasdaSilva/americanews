namespace AmericaNews.Services.Interfaces
{
    public interface IComentarioService
    {
        public List<ComentarioModel> GetAllByNoticia(int idNoticia);
        public List<ComentarioModel> GetAllByStatus(int status);
        public ComentarioModel? GetById(int id);
        public void Insert(ComentarioModel comentario);
        public void UpdateStatus(int idComentario, int newStatus, int idAdmin);
    }
}
