using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LivinParis.Data
{
    /// <summary>
    /// Manages database connections and operations.
    /// </summary>
    public class DatabaseManager : IDisposable
    {
        private readonly string _connectionString;
        private MySqlConnection _connection;
        private MySqlCommand _currentCommand;
        private MySqlDataReader _currentReader;

        /// <summary>
        /// Initializes a new instance of the DatabaseManager class.
        /// </summary>
        /// <param name="server">The database server address.</param>
        /// <param name="database">The database name.</param>
        /// <param name="username">The database username.</param>
        /// <param name="password">The database password.</param>
        public DatabaseManager(string server, string database, string username, string password)
        {
            _connectionString = $"Server={server};Database={database};User ID={username};Password={password};";
            _connection = new MySqlConnection(_connectionString);
        }

        /// <summary>
        /// Establishes a connection to the database.
        /// </summary>
        public void Connect()
        {
            try
            {
                CloseCurrentReader();
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    _connection.Open();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur de connexion à la base de données: {ex.Message}");
            }
        }

        /// <summary>
        /// Closes the database connection.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                CloseCurrentReader();
                if (_currentCommand != null)
                {
                    _currentCommand.Dispose();
                    _currentCommand = null;
                }
                if (_connection.State != System.Data.ConnectionState.Closed)
                {
                    _connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la déconnexion: {ex.Message}");
            }
        }

        private void CloseCurrentReader()
        {
            if (_currentReader != null)
            {
                if (!_currentReader.IsClosed)
                {
                    _currentReader.Close();
                }
                _currentReader.Dispose();
                _currentReader = null;
            }
        }

        /// <summary>
        /// Executes a SQL query and returns a MySqlDataReader.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <returns>A MySqlDataReader containing the query results.</returns>
        public MySqlDataReader ExecuteReader(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                CloseCurrentReader();

                if (_currentCommand != null)
                {
                    _currentCommand.Dispose();
                    _currentCommand = null;
                }

                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    _connection.Open();
                }

                _currentCommand = new MySqlCommand(query, _connection);
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        _currentCommand.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                _currentReader = _currentCommand.ExecuteReader();
                return _currentReader;
            }
            catch (Exception ex)
            {
                CloseCurrentReader();
                if (_currentCommand != null)
                {
                    _currentCommand.Dispose();
                    _currentCommand = null;
                }
                throw new Exception($"Erreur lors de l'exécution de la requête: {ex.Message}");
            }
        }

        /// <summary>
        /// Executes a SQL command and returns the number of affected rows.
        /// </summary>
        /// <param name="query">The SQL command to execute.</param>
        /// <param name="parameters">The parameters for the command.</param>
        /// <returns>The number of rows affected by the command.</returns>
        public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                CloseCurrentReader();
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    _connection.Open();
                }

                using (var command = new MySqlCommand(query, _connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'exécution de la requête: {ex.Message}");
            }
        }

        public object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                CloseCurrentReader();
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    _connection.Open();
                }

                using (var command = new MySqlCommand(query, _connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    return command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'exécution de la requête: {ex.Message}");
            }
        }

        public void Dispose()
        {
            CloseCurrentReader();
            if (_currentCommand != null)
            {
                _currentCommand.Dispose();
                _currentCommand = null;
            }
            if (_connection != null)
            {
                if (_connection.State != System.Data.ConnectionState.Closed)
                {
                    _connection.Close();
                }
                _connection.Dispose();
            }
        }
    }
}