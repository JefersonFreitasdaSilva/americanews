namespace AmericaNews.Data.Interfaces
{
    public interface IComentarioRepository
    {
        public List<ComentarioModel> GetAllByNoticia(int idNoticia);
        public List<ComentarioModel> GetAllByStatus(int status);
        public ComentarioModel? GetById(int id);
        public void Insert(ComentarioModel comentario);
        public void UpdateStatus(ComentarioModel comentario);
    }
}
