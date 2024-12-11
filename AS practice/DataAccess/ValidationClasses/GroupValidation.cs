using System;
using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess.ValidationClasses
{
    public class GroupValidation : DatabaseBase
    {
        public GroupValidation(string connectionString) : base(connectionString) {}
        
        public bool GroupExists(int groupId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM student_groups WHERE group_id = @groupId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@groupId", groupId);
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }
    }
}