using AmericaNews.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AmericaNews.Data
{
    public class HomeRepository
    {
        public string GetTitulo()
        {
            // TODO: Substitua as credenciais abaixo pelas suas credenciais pessoais para testar localmente
            string connectionString = "Data Source=DESKTOP-Q0ODSD0\\SQLEXPRESS;Initial Catalog=AMERICANEWS;Integrated Security=True";

            string primeiroTitulo = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Abrir a conexão
                    connection.Open();

                    // Comando SQL para selecionar todos os campos da tabela Noticia
                    string sql = "SELECT * FROM Noticia";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Executar o comando e lidar com os resultados
                        SqlDataReader reader = command.ExecuteReader();

                        // Lista para armazenar as notícias
                        List<NoticiaModel> noticias = new List<NoticiaModel>();

                        // Ler os resultados e criar objetos Noticia
                        while (reader.Read())
                        {
                            NoticiaModel noticia = new NoticiaModel
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Titulo = reader["Titulo"].ToString(),
                                Subtitulo = reader["Subtitulo"].ToString(),
                                Texto = reader["Texto"].ToString(),
                                Data = Convert.ToDateTime(reader["Data"]),
                                Ocultar = Convert.ToBoolean(reader["Ocultar"]),
                                IDUsuario = Convert.ToInt32(reader["IDUsuario"]),
                                ID_ADM_Aprovou = reader["ID_ADM_Aprovou"] != DBNull.Value ? Convert.ToInt32(reader["ID_ADM_Aprovou"]) : (int?)null,
                                DataAprovada = reader["DataAprovada"] != DBNull.Value ? Convert.ToDateTime(reader["DataAprovada"]) : (DateTime?)null
                            };

                            noticias.Add(noticia);
                        }

                        // Obter o título do primeiro item
                        if (noticias.Count > 0)
                        {
                            primeiroTitulo = noticias[0].Titulo;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Lidar com exceções de conexão
                    Console.WriteLine("Erro de conexão: " + ex.Message);
                }
            }

            return primeiroTitulo;
        }
    }
}