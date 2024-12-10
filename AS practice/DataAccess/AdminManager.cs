using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess
{
    public class AdminManager
    {
        private readonly string _connectionString;

        public AdminManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public void CreateAdministrator(string username, string password)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO users (username, password, role_id) " +
                               "VALUES (@username, @password, (SELECT role_id FROM user_roles WHERE role_name = 'Admin'))";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void DeleteAdministrator(int adminId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = "DELETE FROM users WHERE user_id = @adminId AND role_id = (SELECT role_id FROM user_roles WHERE role_name = 'Admin')";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@adminId", adminId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}