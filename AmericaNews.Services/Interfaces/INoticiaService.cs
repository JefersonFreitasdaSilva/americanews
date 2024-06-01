namespace AmericaNews.Services.Interfaces
{
    public interface INoticiaService
    {
        public Task<List<NoticiaModel>> GetAll();
        public Task<List<NoticiaModel>> GetAllByStatus(int status);
        public Task<NoticiaModel> GetById(int id);
        public bool NoticiaExists(int id);
        public void Insert(NoticiaModel noticia);
        public void UpdateStatus(int idNoticia, int newStatus, int idAdmin);
        public Task<List<NoticiaModel>> Search(string termo, int status);
    }
}
