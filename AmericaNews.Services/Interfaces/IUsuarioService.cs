namespace AmericaNews.Services.Interfaces
{
    public interface IUsuarioService
    {
        public UsuarioModel? GetById(int id);
        public Task<UsuarioModel> GetByCredentials(string email, string senha);
        public Task<UsuarioModel> Insert(UsuarioModel model, int admId);
        public bool AdminExists(int adminId);
        public bool UsuarioExists(int adminId);
    }
}
