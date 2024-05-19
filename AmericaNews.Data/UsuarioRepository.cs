using AmericaNews.Data.Interfaces;
using AmericaNews.Data.Models;
using System.Data.SqlClient;

namespace AmericaNews.Data
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private string? _connectionString { get; set; }

        public UsuarioRepository()
        {
            _connectionString = Connection.GetConnectionString();
        }

        private List<UsuarioModel> ExecuteSelectCommands(string sql)
        {
            List<UsuarioModel> usuarios = new List<UsuarioModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        UsuarioModel usuario = new UsuarioModel
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Nome = reader["Nome"].ToString(),
                            Senha = reader["Senha"].ToString(),
                            Telefone = reader["Telefone"].ToString(),
                            Email = reader["Email"].ToString(),
                            EmailCorporativo = reader["EmailCorporativo"].ToString(),
                            Endereco = reader["Endereco"].ToString(),
                            Data = Convert.ToDateTime(reader["qData"]),
                            NivelPermissao = Convert.ToInt32(reader["NivelPermissao"])
                        };

                        usuarios.Add(usuario);
                    }
                }

                connection.Close();
            }

            return usuarios;
        }

        public UsuarioModel? GetById(int id)
        {
            try
            {
                string sql = "SELECT * FROM Usuario WHERE ID = " + id;
                var usuarios = ExecuteSelectCommands(sql);

                return usuarios.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar o usuario de ID {0} no banco de dados: {1}", id, ex.Message));
                throw;
            }
        }

        public UsuarioModel? GetAdminById(int id)
        {
            try
            {
                string sql = string.Format("SELECT * FROM Usuario WHERE NivelPermissao = {0} AND ID = {1}", EnumNivelPermissao.Admin, id);
                var usuarios = ExecuteSelectCommands(sql);

                return usuarios.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar o admin de ID {0} no banco de dados: {1}", id, ex.Message));
                throw;
            }
        }

        public UsuarioModel? GetByCredentials(string email, string senha)
        {
            try
            {
                string sql = string.Format("SELECT * FROM Usuario WHERE Email = {0} AND Senha = {1}", email, senha);
                var usuarios = ExecuteSelectCommands(sql);

                return usuarios.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao buscar o usuario de email {0} e senha {1} no banco de dados: {2}", email, senha, ex.Message));
                throw;
            }
        }

        public void Insert(UsuarioModel usuario)
        {
            try
            {
                string sql = string.Format("INSERT INTO Usuario(Nome, Telefone, Email, Senha, Endereco, qData, EmailCorporativo, NivelPermissao) " +
                    "VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6})",
                    usuario.Nome, usuario.Telefone, usuario.Email, usuario.Senha, usuario.Endereco, usuario.Data, 
                    usuario.EmailCorporativo, usuario.NivelPermissao);

                Connection.ExecuteCommands(sql, _connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Ocorreu um erro ao cadastrar o usuario no banco de dados: {0}", ex.Message));
                throw;
            }
        }
    }
}
