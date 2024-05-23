namespace AmericaNews.Api.Models
{
    public class Noticia
    {
        public string Titulo { get; set; }
        public string? Subtitulo { get; set; }
        public string Texto { get; set; }
        public string LinkIMG { get; set; }
        public int IDUsuario { get; set; }
    }
}
