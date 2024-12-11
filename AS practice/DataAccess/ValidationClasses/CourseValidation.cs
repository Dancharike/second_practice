using System;
using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess.ValidationClasses
{
    public class CourseValidation : DatabaseBase
    {
        public CourseValidation(string connectionString) : base(connectionString) {}
        
        public bool CourseExists(int courseId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM courses WHERE course_id = @courseId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@courseId", courseId);
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }
    }
}