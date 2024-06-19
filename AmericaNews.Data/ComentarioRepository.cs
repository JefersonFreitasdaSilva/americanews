using AmericaNews.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace AmericaNews.Data
{
    public class ComentarioRepository : IComentarioRepository
    {
        private string? _connectionString { get; set; }

        public ComentarioRepository()
        {
            _connectionString = Connection.GetConnectionString();
        }

        private List<ComentarioModel> ExecuteSelectCommands(string sql)
        {
            List<ComentarioModel> comentarios = new List<ComentarioModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ComentarioModel comentario = new ComentarioModel
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Texto = reader["Texto"].ToString(),
                            NomeAutor = reader["NomeAutor"].ToString(),
                            Data = Convert.ToDateTime(reader["Data"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            IDUsuario = Convert.ToInt32(reader["IDUsuario"]),
                            IDNoticia = Convert.ToInt32(reader["IDNoticia"]),
                            ID_ADM_Reprovou = reader["ID_ADM_Reprovou"] != DBNull.Value ? Convert.ToInt32(reader["ID_ADM_Reprovou"]) : (int?)null,
                            DataReprovado = reader["DataReprovado"] != DBNull.Value ? Convert.ToDateTime(reader["DataReprovado"]) : (DateTime?)null
                        };

                        comentarios.Add(comentario);
                    }

                    connection.Close();
                }
            }

            return comentarios;
        }

        public Task<List<ComentarioModel>> GetAllByNoticia(int idNoticia)
        {
            try
            {
                string sql = @"SELECT c.*, u.Nome as NomeAutor FROM Comentario AS c
                                INNER JOIN Usuario AS u ON c.IDUsuario = u.ID
                                WHERE c.IDNoticia = " + idNoticia;
                var result = ExecuteSelectCommands(sql);

                Task<List<ComentarioModel>> comentarios = Task.FromResult(result);

                return comentarios;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar todos os comentarios da noticia de ID {0} no banco de dados: {1}", idNoticia, ex.Message));
                throw;
            }
        }

        public Task<List<ComentarioModel>> GetAllByStatusAndNoticia(int status, int idNoticia)
        {
            try
            {
                string sql = string.Format(@"SELECT c.*, u.Nome as NomeAutor FROM Comentario AS c
                                            INNER JOIN Usuario AS u ON c.IDUsuario = u.ID
                                            WHERE c.IDNoticia = {0} AND c.Status = {1}", idNoticia, status);
                var result = ExecuteSelectCommands(sql);

                Task<List<ComentarioModel>> comentarios = Task.FromResult(result);

                return comentarios;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar todos os comentarios da noticia de ID {0} e status de ID {1} no banco de dados: {1}", idNoticia, status, ex.Message));
                throw;
            }
        }

        public Task<List<ComentarioModel>> GetAllByStatus(int status)
        {
            try
            {
                string sql = @"SELECT c.*, u.Nome as NomeAutor FROM Comentario AS c 
                                INNER JOIN Usuario AS u ON c.IDUsuario = u.ID
                                WHERE c.Status = " + status;
                var result = ExecuteSelectCommands(sql);

                Task<List<ComentarioModel>> comentarios = Task.FromResult(result);

                return comentarios;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar todos os comentarios do status {0} no banco de dados: {1}", status, ex.Message));
                throw;
            }
        }

        public ComentarioModel? GetById(int id)
        {
            try
            {
                string sql = @"SELECT c.*, u.Nome as NomeAutor FROM Comentario AS c
                                INNER JOIN Usuario AS u ON c.IDUsuario = u.ID
                                WHERE c.ID = " + id;
                var comentario = ExecuteSelectCommands(sql);

                return comentario.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar o comentario de ID {0} no banco de dados: {1}", id, ex.Message));
                throw;
            }
        }

        public void Insert(ComentarioModel comentario)
        {
            try
            {
                string sql = string.Format("INSERT INTO Comentario(Texto, status, IDUsuario, IDNoticia, Data) " +
                    "VALUES('{0}', {1}, {2}, {3}, '{4}')",
                    comentario.Texto, comentario.Status, comentario.IDUsuario, comentario.IDNoticia, comentario.Data);

                Connection.ExecuteCommands(sql, _connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao cadastrar o comentario no banco de dados: {0}", ex.Message));
                throw;
            }
        }

        public void UpdateStatus(ComentarioModel comentario)
        {
            try
            {
                string sql = string.Format("UPDATE Comentario SET status = {0}, ID_ADM_Reprovou  = {1}, DataReprovado = '{2}'" +
                                           " WHERE ID = {3}",
                    comentario.Status, comentario.ID_ADM_Reprovou, comentario.DataReprovado, comentario.ID);

                Connection.ExecuteCommands(sql, _connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao cadastrar a noticia no banco de dados: {0}", ex.Message));
                throw;
            }
        }
    }
}
