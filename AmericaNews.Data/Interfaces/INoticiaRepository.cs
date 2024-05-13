namespace AmericaNews.Data.Interfaces
{
    public interface INoticiaRepository
    {
        public List<NoticiaModel> GetAll();
        public List<NoticiaModel> GetAllByStatus(int status);
        public NoticiaModel? GetById(int id);
        public void Insert(NoticiaModel noticia);
        public void UpdateStatus(NoticiaModel noticia);
    }
}
