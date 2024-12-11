using System;
using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess.ValidationClasses
{
    public class StudentValidation : DatabaseBase
    {
        public StudentValidation(string connectionString) : base(connectionString) {}
        
        public bool StudentExists(int studentId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM students WHERE student_id = @studentId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }
    }
}