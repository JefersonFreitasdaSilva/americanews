namespace AmericaNews.Data.Interfaces
{
    public interface IUsuarioRepository
    {
        public UsuarioModel? GetById(int id);
        public UsuarioModel? GetByCredentials(string email, string senha);
        public void Insert(UsuarioModel usuario);
    }
}
