using System;

public class RegistroModel
{ 
    public int ID { get; set; }
    public string Tabela { get; set; }
    public string Coluna { get; set; }
    public string Antes {  get; set; }
    public string Depois { get; set; }
    public int Responsavel { get; set; }
    public string ResponsavelNome { get; set; }
    public DateTime Data {  get; set; }

    public RegistroModel(string tabela,string coluna,string antes,string depois,DateTime data, int responsavel) 
    {
        Tabela=tabela;
        Coluna=coluna;
        Antes=antes;
        Depois=depois;
        Data=data;
        Responsavel=responsavel;
    }

    public RegistroModel() 
    { }

    public override string ToString()
    {
        return $"ID: {ID}, Tabela: {Tabela}, Coluna: {Coluna}, Antes: {Antes}, Depois: {Depois}, Data: {Data}";
    }
}