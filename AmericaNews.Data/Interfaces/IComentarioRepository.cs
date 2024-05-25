namespace AmericaNews.Data.Interfaces
{
    public interface IComentarioRepository
    {
        public Task<List<ComentarioModel>> GetAllByNoticia(int idNoticia);
        public Task<List<ComentarioModel>> GetAllByStatus(int status);
        public Task<ComentarioModel?> GetById(int id);
        public void Insert(ComentarioModel comentario);
        public void UpdateStatus(ComentarioModel comentario);
    }
}
