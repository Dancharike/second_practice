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
        
        public (bool isValid, string role, int? roleSpecificId) ValidateUser(string username, string password)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                        SELECT ur.role_name, u.role_specific_id
                        FROM users u
                        JOIN user_roles ur ON u.role_id = ur.role_id
                        WHERE u.username = @username AND u.password = @password";
            
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
            
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var role = reader.GetString("role_name");
                            var roleSpecificId = reader.IsDBNull(reader.GetOrdinal("role_specific_id")) 
                                ? (int?)null
                                : reader.GetInt32("role_specific_id");

                            return (true, role, roleSpecificId);
                        }
                    }
                }
            }
            return (false, null, null);
        }
    }
}