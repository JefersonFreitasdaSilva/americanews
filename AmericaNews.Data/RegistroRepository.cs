using AmericaNews.Data.Interfaces;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

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
                            Data = Convert.ToDateTime(reader["Data"])
                        };

                        registros.Add(registro);
                    }

                    connection.Close();
                }
            }

            return registros;
        }

        public Task<List<RegistroModel>> GetAll()
        {
            try
            {
                string sql = "SELECT * FROM Registro";
                var result = ExecuteSelectCommands(sql);

                Task<List<RegistroModel>> registros = Task.FromResult(result);

                return registros;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar todos os registros no banco de dados: {0}", ex.Message));
                throw;
            }
        }

        public Task<RegistroModel?> GetById(int id)
        {
            try
            {
                string sql = "SELECT * FROM Registro WHERE ID = " + id;
                var result = ExecuteSelectCommands(sql);

                Task<RegistroModel?> registro = Task.FromResult(result.FirstOrDefault());

                return registro;
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
                    "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
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
                string sql = "INSERT INTO Registro(Tabela, Coluna, antes, depois, Responsavel, Data) VALUES";

                for (int i = 0; i < registros.Count(); i++)
                {
                    var registro = registros[i];

                    if (i == registros.Count() - 1)
                    {
                        sql += string.Format(@"
                                        ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                                registro.Tabela, registro.Coluna, registro.Antes, registro.Depois, registro.Responsavel, registro.Data);
                    }
                    else
                    {
                        sql += string.Format(@"
                                        ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}'),",
                                registro.Tabela, registro.Coluna, registro.Antes, registro.Depois, registro.Responsavel, registro.Data);
                    }
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
