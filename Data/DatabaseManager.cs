using MySql.Data.MySqlClient;
using System;

namespace LivinParis.Data
{
    /// <summary>
    /// Manages database connections and operations.
    /// </summary>
    public class DatabaseManager
    {
        private readonly string _connectionString;
        private MySqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the DatabaseManager class.
        /// </summary>
        /// <param name="server">The database server address.</param>
        /// <param name="database">The database name.</param>
        /// <param name="username">The database username.</param>
        /// <param name="password">The database password.</param>
        public DatabaseManager(string server, string database, string username, string password)
        {
            _connectionString = $"Server={server};Database={database};Uid={username};Pwd={password};";
        }

        /// <summary>
        /// Establishes a connection to the database.
        /// </summary>
        public void Connect()
        {
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();
        }

        /// <summary>
        /// Closes the database connection.
        /// </summary>
        public void Disconnect()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        /// <summary>
        /// Executes a SQL query and returns a MySqlDataReader.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <returns>A MySqlDataReader containing the query results.</returns>
        public MySqlDataReader ExecuteReader(string query)
        {
            var command = new MySqlCommand(query, _connection);
            return command.ExecuteReader();
        }

        /// <summary>
        /// Executes a SQL command and returns the number of affected rows.
        /// </summary>
        /// <param name="query">The SQL command to execute.</param>
        /// <returns>The number of rows affected by the command.</returns>
        public int ExecuteNonQuery(string query)
        {
            var command = new MySqlCommand(query, _connection);
            return command.ExecuteNonQuery();
        }
    }
}