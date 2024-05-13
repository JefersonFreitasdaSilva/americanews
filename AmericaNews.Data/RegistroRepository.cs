﻿using AmericaNews.Data.Interfaces;
using System.Data.SqlClient;

namespace AmericaNews.Data
{
    public class RegistroRepository : IRegistroRepository
    {
        private string? _connectionString { get; set; }

        public RegistroRepository()
        {
            _connectionString = Connection.GetConnectionString();
        }

        private List<RegistroModel> ExecuteSelectCommands(string sql)
        {
            List<RegistroModel> registros = new List<RegistroModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        RegistroModel registro = new RegistroModel
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Tabela = reader["Tabela"].ToString(),
                            Coluna = reader["Coluna"].ToString(),
                            Antes = reader["antes"].ToString(),
                            Depois = reader["depois"].ToString(),
                            Responsavel = Convert.ToInt32(reader["Responsavel"]),
                            Data = Convert.ToDateTime(reader["Dataq"])
                        };

                        registros.Add(registro);
                    }

                    connection.Close();
                }
            }

            return registros;
        }

        public List<RegistroModel> GetAll()
        {
            try
            {
                string sql = "SELECT * FROM Registro";
                var registros = ExecuteSelectCommands(sql);

                return registros;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar todos os registros no banco de dados: {0}", ex.Message));
                throw;
            }
        }

        public RegistroModel? GetById(int id)
        {
            try
            {
                string sql = "SELECT * FROM Registro WHERE ID = " + id;
                var registros = ExecuteSelectCommands(sql);

                return registros.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar o registro de ID {0} no banco de dados: {1}", id, ex.Message));
                throw;
            }
        }

        public void Insert(RegistroModel registro)
        {
            try
            {
                string sql = string.Format("INSERT INTO Registro(Tabela, Coluna, antes, depois, Responsavel, Dataq) " +
                    "VALUES({0}, {1}, {2}, {3}, {4}, {5})",
                    registro.Tabela, registro.Coluna, registro.Antes, registro.Depois, registro.Responsavel, registro.Data);

                Connection.ExecuteCommands(sql, _connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao cadastrar o registro no banco de dados: {0}", ex.Message));
                throw;
            }
        }

        public void InsertBatch(List<RegistroModel> registros)
        {
            try
            {
                string sql = "INSERT INTO Registro(Tabela, Coluna, antes, depois, Responsavel, Dataq) VALUES";

                foreach (var registro in registros)
                {
                    sql += string.Format(@"
                                        ({0}, {1}, {2}, {3}, {4}, {5})",
                    registro.Tabela, registro.Coluna, registro.Antes, registro.Depois, registro.Responsavel, registro.Data);
                }

                Connection.ExecuteCommands(sql, _connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao cadastrar os registros no banco de dados: {0}", ex.Message));
                throw;
            }
        }
    }
}
