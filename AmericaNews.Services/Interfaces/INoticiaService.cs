namespace AmericaNews.Services.Interfaces
{
    public interface INoticiaService
    {
        public List<NoticiaModel> GetAll();
        public List<NoticiaModel> GetAllByStatus(int status);
        public NoticiaModel? GetById(int id);
        public bool NoticiaExists(int id);
        public void Insert(NoticiaModel noticia);
        public void UpdateStatus(int idNoticia, int newStatus, int idAdmin);
    }
}
