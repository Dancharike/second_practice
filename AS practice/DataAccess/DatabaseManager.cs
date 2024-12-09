using System;
using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public void TestConnection()
        {
            var connection = GetConnection();
            try
            {
                connection.Open();
                Console.WriteLine("Connection established");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection error: {ex.Message}");
            }
        }
    }
}