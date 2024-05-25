namespace AmericaNews.Api.Models
{
    public class Comentario
    {
        public string Texto { get; set; }
        public int IDUsuario { get; set; }
        public int IDNoticia { get; set; }
    }
}
