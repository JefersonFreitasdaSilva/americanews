using System;

public class Registro
{ 
    public int ID { get; set; }
    public string Tabela { get; set; }
    public string Coluna { get; set; }
    public string Antes {  get; set; }
    public string Depois { get; set; }
    public DateTime Data {  get; set; }

    public Registro(string tabela,string coluna,string antes,string depois,DateTime data) 
    {
        Tabela=tabela;
        Coluna=coluna;
        Antes=antes;
        Depois=depois;
        Data = data;
    }

    public Registro() 
    { }

    public override string ToString()
    {
        return $"ID: {ID}, Tabela: {Tabela}, Coluna: {Coluna}, Antes: {Antes}, Depois: {Depois}, Data: {Data}";
    }
}