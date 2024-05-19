namespace AmericaNews.Api.Models
{
    public class Usuario
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public string EmailCorporativo { get; set; }
        public int AdminId { get; set; }
    }
}
