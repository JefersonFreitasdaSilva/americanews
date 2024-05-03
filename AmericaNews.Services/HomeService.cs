using AmericaNews.Data;

namespace AmericaNews.Services
{
    public class HomeService
    {
        public string GetTitulo()
        {
            var repositorio = new HomeRepository();
            var titulo = repositorio.GetTitulo();

            return titulo;
        }
    }
}
