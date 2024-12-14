using System;
using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess.ValidationClasses
{
    public class CourseValidation : DatabaseBase
    {
        public CourseValidation(string connectionString) : base(connectionString) {}
        
        public bool CourseExists(int subjectId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM subjects WHERE subject_id = @subjectId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@subjectId", subjectId);
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }
    }
}