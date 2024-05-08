using System;

public class Usuario
{
    public int ID { get; set; }
    public string Nome { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public string? Endereco { get; set; }
    public DateTime Data { get; set; }
    public string EmailCorporativo { get; set; }
    public int NivelPermissao { get; set; }

    public Usuario()
    { }

    public Usuario(string nome, string? telefone, string? email, string? endereco, DateTime data, string emailCorporativo, int nivelPermissao)
    {
        Nome = nome;
        Telefone = telefone;
        Email = email;
        Endereco = endereco;
        Data = data;
        EmailCorporativo = emailCorporativo;
        NivelPermissao = nivelPermissao;
    }

    public override string ToString()
    {
        return $"ID: {ID}, Nome: {Nome}, Telefone: {Telefone ?? "N/A"}, Email: {Email ?? "N/A"}, Endereco: {Endereco ?? "N/A"}, Data: {Data}, Email Corporativo: {EmailCorporativo}, Nivel de Permissao: {NivelPermissao}";
    }
}
