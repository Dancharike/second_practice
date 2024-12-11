using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess.ValidationClasses
{
    public class RoleValidation : DatabaseBase
    {
        public RoleValidation(string connectionString) : base(connectionString) {}
        
        public string GetUserRoleName(int userId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT ur.role_name 
                    FROM users u
                    JOIN user_roles ur ON u.role_id = ur.role_id
                    WHERE u.user_id = @userId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.Read() ? reader.GetString("role_name") : null;
                    }
                }
            }
        }
    }
}