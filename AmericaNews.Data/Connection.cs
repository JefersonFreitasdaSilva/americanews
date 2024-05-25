using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;


namespace AmericaNews.Data
{
    public static class Connection
    {
        public static string? GetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            if (configuration.GetConnectionString("HOMOLOG") == null)
                Console.WriteLine("Não foi possível encontrar a string de conexão!");

            return configuration.GetConnectionString("HOMOLOG");
        }

        public static void ExecuteCommands(string sql, string? connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
