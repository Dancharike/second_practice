using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AS_practice.DataAccess.InterfacesForDataAccess;

namespace AS_practice.DataAccess
{
    public class StudentManager : DatabaseBase, IStudentManager
    {
        public StudentManager(string connectionString) : base(connectionString) { }

        public List<int> ViewGrades(int studentId)
        {
            var grades = new List<int>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT grade_value FROM grades WHERE student_id = @studentId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            grades.Add(reader.GetInt32("grade_value"));
                        }
                    }
                }
            }
            return grades;
        }
    }
}