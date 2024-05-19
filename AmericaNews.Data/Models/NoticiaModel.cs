using System;

public class NoticiaModel
{
    public int ID { get; set; }
    public string Titulo { get; set; }
    public string? Subtitulo { get; set; }
    public string Texto { get; set; }
    public string LinkIMG { get; set; }
    public DateTime Data { get; set; }
    public int Status { get; set; }
    public int IDUsuario { get; set; }
    public int? ID_ADM_Aprovou { get; set; }
    public DateTime? DataAprovada { get; set; }

    public NoticiaModel()
    { }

    public NoticiaModel(string titulo, string? subtitulo, string texto, DateTime data, int status, int idUsuario, int? idAdmAprovou, DateTime? dataAprovada)
    {   
        Titulo = titulo;
        Subtitulo = subtitulo;
        Texto = texto;
        Data = data;
        Status = status;
        IDUsuario = idUsuario;
        ID_ADM_Aprovou = idAdmAprovou;
        DataAprovada = dataAprovada;
    }

    public override string ToString()
    {
        string admAprovouInfo = ID_ADM_Aprovou != 0 ? ID_ADM_Aprovou.ToString() : "N/A";
        string dataAprovadaInfo = DataAprovada.HasValue ? DataAprovada.Value.ToString() : "N/A";

        return $"ID: {ID}, Título: {Titulo}, Subtítulo: {Subtitulo}, Texto: {Texto}, Data: {Data}, Status: {Status}, IDUsuario: {IDUsuario}, ID_ADM_Aprovou: {admAprovouInfo}, DataAprovada: {dataAprovadaInfo}";
    }
}
