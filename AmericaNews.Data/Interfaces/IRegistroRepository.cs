namespace AmericaNews.Data.Interfaces
{
    public interface IRegistroRepository
    {
        public List<RegistroModel> GetAll();
        public RegistroModel? GetById(int id);
        public void Insert(RegistroModel registro);
        public void InsertBatch(List<RegistroModel> registros);
    }
}
