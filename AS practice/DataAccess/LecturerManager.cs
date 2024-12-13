using MySql.Data.MySqlClient;
using AS_practice.DataAccess.InterfacesForDataAccess;

namespace AS_practice.DataAccess
{
    public class LecturerManager : DatabaseBase, ILecturerManager
    {
        public LecturerManager(string connectionString) : base(connectionString) {}
        
        public void AddGrade(int studentId, int courseId, int grade)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                
                string query = "INSERT INTO grades (student_id, lecturer_course_id, grade_value) " +
                               "VALUES (@studentId, (SELECT lecturer_course_id FROM lecturer_courses WHERE course_id = @courseId LIMIT 1), @grade)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    command.Parameters.AddWithValue("@grade", grade);
                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void EditGrade(int studentId, int courseId, int grade)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "UPDATE grades SET grade_value = @grade " +
                               "WHERE student_id = @studentId AND lecturer_course_id = " +
                               "(SELECT lecturer_course_id FROM lecturer_courses WHERE course_id = @courseId LIMIT 1)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    command.Parameters.AddWithValue("@grade", grade);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
