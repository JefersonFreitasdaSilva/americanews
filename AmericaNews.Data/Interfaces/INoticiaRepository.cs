namespace AmericaNews.Data.Interfaces
{
    public interface INoticiaRepository
    {
        public Task<List<NoticiaModel>> GetAll();
        public Task<List<NoticiaModel>> GetAllByStatus(int status);
        public NoticiaModel? GetById(int id);
        public void Insert(NoticiaModel noticia);
        public void UpdateStatus(NoticiaModel noticia);
        public Task<List<NoticiaModel>> Search(string termo, int status);
    }
}
