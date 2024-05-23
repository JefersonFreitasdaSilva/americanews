using AmericaNews.Data.Interfaces;
using System.Data.SqlClient;

namespace AmericaNews.Data
{
    public class NoticiaRepository : INoticiaRepository
    {
        private string? _connectionString { get; set; }

        public NoticiaRepository()
        {
            _connectionString = Connection.GetConnectionString();
        }

        private List<NoticiaModel> ExecuteSelectCommands(string sql)
        {
            List<NoticiaModel> noticias = new List<NoticiaModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        NoticiaModel noticia = new NoticiaModel
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Titulo = reader["Titulo"].ToString(),
                            LinkIMG = reader["LinkIMG"].ToString(),
                            Subtitulo = reader["Subtitulo"].ToString(),
                            Texto = reader["Texto"].ToString(),
                            Data = Convert.ToDateTime(reader["Data"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            IDUsuario = Convert.ToInt32(reader["IDUsuario"]),
                            ID_ADM_Aprovou = reader["ID_ADM_Aprovou"] != DBNull.Value ? Convert.ToInt32(reader["ID_ADM_Aprovou"]) : (int?)null,
                            DataAprovada = reader["DataAprovada"] != DBNull.Value ? Convert.ToDateTime(reader["DataAprovada"]) : (DateTime?)null
                        };

                        noticias.Add(noticia);
                    }

                    connection.Close();
                }
            }

            return noticias;
        }

        public Task<List<NoticiaModel>> GetAll()
        {
            try
            {
                string sql = "SELECT * FROM Noticia";
                var result = ExecuteSelectCommands(sql);

                Task<List<NoticiaModel>> noticias = Task.FromResult(result);

                return noticias;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar todas as notícias no banco de dados: {0}", ex.Message));
                throw;
            }
        }

        public Task<List<NoticiaModel>> GetAllByStatus(int status)
        {
            try
            {  
                string sql = "SELECT * FROM Noticia WHERE Ocultar = " + status;
                var result = ExecuteSelectCommands(sql);

                Task<List<NoticiaModel>> noticias = Task.FromResult(result);

                return noticias;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar todas as notícias do status {0} no banco de dados: {1}", status, ex.Message));
                throw;
            }
        }

        public Task<NoticiaModel?> GetById(int id)
        {
            try
            {
                string sql = "SELECT * FROM Noticia WHERE ID = " + id;
                var result = ExecuteSelectCommands(sql);

                Task<NoticiaModel?> noticia = Task.FromResult(result.FirstOrDefault());

                return noticia;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar a notícia de ID {0} no banco de dados: {1}", id, ex.Message));
                throw;
            }
        }

        public void Insert(NoticiaModel noticia)
        {
            try
            {
                string sql = string.Format("INSERT INTO Noticia(Titulo, Subtitulo, Texto, qData, Status, IDUsuario, ID_ADM_Aprovou, DataAprovada) " +
                    "VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                    noticia.Titulo, noticia.Subtitulo, noticia.Texto, noticia.Data, noticia.Status,
                    noticia.IDUsuario, noticia.ID_ADM_Aprovou, noticia.DataAprovada);

                Connection.ExecuteCommands(sql, _connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao cadastrar a noticia no banco de dados: {0}", ex.Message));
                throw;
            }
        }

        public void UpdateStatus(NoticiaModel noticia)
        {
            try
            {
                string sql = string.Format("UPDATE Noticia SET Status = {0}, ID_ADM_Aprovou = {1}, DataAprovada = {2}" +
                                           "WHERE ID = {3}",
                    noticia.Status, noticia.ID_ADM_Aprovou, noticia.DataAprovada, noticia.ID);

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
