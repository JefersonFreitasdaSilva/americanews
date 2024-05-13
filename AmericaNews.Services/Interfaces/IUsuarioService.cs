namespace AmericaNews.Services.Interfaces
{
    public interface IUsuarioService
    {
        public UsuarioModel? GetById(int id);
        public UsuarioModel? GetByCredentials(string email, string senha);
        public void Insert(UsuarioModel model, int admId);
    }
}
