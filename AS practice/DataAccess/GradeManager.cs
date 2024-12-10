using System.Collections.Generic;
using AS_practice.Models;
using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess
{
    public class GradeManager
    {
        private readonly string _connectionString;

        public GradeManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public void AddGrade(int studentId, int lecturerCourseId, int gradeValue)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO grades (student_id, lecturer_course_id, grade_value) VALUES (@studentId, @lecturerCourseId, @gradeValue)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    command.Parameters.AddWithValue("@lecturerCourseId", lecturerCourseId);
                    command.Parameters.AddWithValue("@gradeValue", gradeValue);
                    command.ExecuteNonQuery();
                }
            }
        }
        
        public List<Grade> GetStudentGrades(int studentId)
        {
            var grades = new List<Grade>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT g.grade_id, g.grade_value, lc.course_id
                    FROM grades g
                    JOIN lecturer_courses lc ON g.lecturer_course_id = lc.lecturer_course_id
                    WHERE g.student_id = @studentId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            grades.Add(new Grade
                            {
                                GradeId = reader.GetInt32("grade_id"),
                                GradeValue = reader.GetInt32("grade_value"),
                                LecturerCourseId = reader.GetInt32("course_id")
                            });
                        }
                    }
                }
            }

            return grades;
        }
    }
}