using MySql.Data.MySqlClient;
using System;

namespace LivinParis.Data
{
    public class DatabaseManager
    {
        private readonly string _connectionString;
        private MySqlConnection _connection;

        public DatabaseManager(string server, string database, string username, string password)
        {
            _connectionString = $"Server={server};Port=3306;Database={database};Uid={username};Pwd={password};";
        }

        public void Connect()
        {
            try
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.Open();
                Console.WriteLine("Connexion à la base de données établie avec succès!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur de connexion: {ex.Message}");
                throw;
            }
        }

        public void Disconnect()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
                Console.WriteLine("Déconnexion de la base de données réussie.");
            }
        }

        public void ExecuteNonQuery(string query)
        {
            using (var command = new MySqlCommand(query, _connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public MySqlDataReader ExecuteReader(string query)
        {
            var command = new MySqlCommand(query, _connection);
            return command.ExecuteReader();
        }
    }
} 