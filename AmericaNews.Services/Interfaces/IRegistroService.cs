namespace AmericaNews.Services.Interfaces
{
    public interface IRegistroService
    {
        public Task<List<RegistroModel>> GetAll();
        public Task<RegistroModel> GetById(int id);
        public void Insert(RegistroModel registro);
        public void InsertBatch(List<RegistroModel> registros);
    }
}
