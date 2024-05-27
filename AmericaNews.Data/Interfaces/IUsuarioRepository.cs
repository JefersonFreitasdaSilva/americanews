namespace AmericaNews.Data.Interfaces
{
    public interface IUsuarioRepository
    {
        public UsuarioModel? GetById(int id);
        public UsuarioModel? GetAdminById(int id);
        public Task<UsuarioModel?> GetByCredentials(string email);
        public void Insert(UsuarioModel usuario);
        public bool EmailExists(string email);
    }
}
