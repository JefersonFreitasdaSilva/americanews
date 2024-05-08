using System;

public class Noticia
{
    public int ID { get; set; }
    public string Titulo { get; set; }
    public string? Subtitulo { get; set; }
    public string Texto { get; set; }
    public DateTime Data { get; set; }
    public bool? Ocultar { get; set; }
    public int IDUsuario { get; set; }
    public int? ID_ADM_Aprovou { get; set; }
    public DateTime? DataAprovada { get; set; }

    public Noticia()
    { }

    public Noticia(string titulo, string? subtitulo, string texto, DateTime data, bool? ocultar, int idUsuario, int? idAdmAprovou, DateTime? dataAprovada)
    {   
        Titulo = titulo;
        Subtitulo = subtitulo;
        Texto = texto;
        Data = data;
        Ocultar = ocultar;
        IDUsuario = idUsuario;
        ID_ADM_Aprovou = idAdmAprovou;
        DataAprovada = dataAprovada;
    }

    public override string ToString()
    {
        string admAprovouInfo = ID_ADM_Aprovou != 0 ? ID_ADM_Aprovou.ToString() : "N/A";
        string dataAprovadaInfo = DataAprovada.HasValue ? DataAprovada.Value.ToString() : "N/A";

        return $"ID: {ID}, Título: {Titulo}, Subtítulo: {Subtitulo}, Texto: {Texto}, Data: {Data}, Ocultar: {Ocultar}, IDUsuario: {IDUsuario}, ID_ADM_Aprovou: {admAprovouInfo}, DataAprovada: {dataAprovadaInfo}";
    }
}
