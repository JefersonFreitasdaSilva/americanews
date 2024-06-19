using System;

public class ComentarioModel
{
    public int ID { get; set; }
    public string Texto { get; set; }
    public string NomeAutor { get; set; }
    public int Status { get; set; }
    public int IDUsuario { get; set; }
    public int IDNoticia { get; set; }
    public DateTime Data { get; set; }
    public int? ID_ADM_Reprovou { get; set; }
    public DateTime? DataReprovado { get; set; }

    public ComentarioModel()
    { }

    public ComentarioModel(string texto, int status, int idUsuario, int idNoticia, DateTime data, int? idAdmReprovou, DateTime? dataReprovado)
    {
        Texto = texto;
        Status = status;
        IDUsuario = idUsuario;
        IDNoticia = idNoticia;
        Data = data;
        ID_ADM_Reprovou = idAdmReprovou;
        DataReprovado = dataReprovado;
    }

    public override string ToString()
    {
        string admReprovouInfo = ID_ADM_Reprovou != 0 ? ID_ADM_Reprovou.ToString() : "N/A";
        string dataReprovadoInfo = DataReprovado.HasValue ? DataReprovado.Value.ToString() : "N/A";

        return $"ID: {ID}, Texto: {Texto}, Status: {Status}, IDUsuario: {IDUsuario}, IDNoticia: {IDNoticia}, Data: {Data}, ID_ADM_Reprovou: {admReprovouInfo}, DataReprovado: {dataReprovadoInfo}";
    }
}
