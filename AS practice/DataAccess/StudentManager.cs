using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AS_practice.DataAccess.InterfacesForDataAccess;
using AS_practice.Models;

namespace AS_practice.DataAccess
{
    public class StudentManager : DatabaseBase, IStudentManager
    {
        public StudentManager(string connectionString) : base(connectionString) {}
        
        public List<StudentGrades> GetStudentData(int? studentId)
        {
            List<StudentGrades> studentGrades = new List<StudentGrades>();

            using (var connection = GetConnection())
            {
                connection.Open();

                string gradeQuery = @"
                SELECT 
                s.student_id, 
                CONCAT(s.first_name, ' ', s.last_name) AS student_name, 
                sub.subject_name, 
                ROUND(SUM(g.grade_value * cat.weight / 100), 1) AS total_grade
                FROM grades g
                JOIN students s ON g.student_id = s.student_id
                JOIN subjects sub ON g.subject_id = sub.subject_id
                JOIN grade_categories cat ON g.category_id = cat.category_id
                WHERE s.student_id = @studentId
                GROUP BY s.student_id, sub.subject_name
                ORDER BY sub.subject_name";

                var gradeCommand = new MySqlCommand(gradeQuery, connection);
                gradeCommand.Parameters.AddWithValue("@studentId", studentId);
                var gradeReader = gradeCommand.ExecuteReader();

                while (gradeReader.Read())
                {
                    studentGrades.Add(new StudentGrades
                    {
                        StudentId = gradeReader.GetInt32("student_id"),
                        StudentName = gradeReader.GetString("student_name"),
                        SubjectName = gradeReader.GetString("subject_name"),
                        TotalGrade = gradeReader.GetDecimal("total_grade")
                    });
                }
                gradeReader.Close();
            }
            return studentGrades;
        }
    }
}