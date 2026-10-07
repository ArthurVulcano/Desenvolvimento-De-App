using System;
using System.IO;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace Connection
{
    public class Connection
    {
        private readonly MySqlConnection connection;
        private readonly string connectionString;

        // jsonFileName: nome do arquivo JSON que contém a connection string (padrão: appsettings.json)
        public Connection(string jsonFileName = "appsettings.json")
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(jsonFileName, optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            // Procura a connection string em ConnectionStrings:Default
            connectionString = configuration.GetConnectionString("Default")
                               ?? configuration["ConnectionStrings:Default"]
                               ?? configuration["ConnectionString"];

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException($"Connection string not found in '{jsonFileName}'. Use 'ConnectionStrings:Default' or 'ConnectionString'.");

            connection = new MySqlConnection(connectionString);
        }

        public MySqlConnection GetConnection() => connection;
    }
}
