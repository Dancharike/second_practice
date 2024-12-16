using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AS_practice.DataAccess.InterfacesForDataAccess;
using AS_practice.Models;

namespace AS_practice.DataAccess
{
    public class StudentManager : DatabaseBase, IStudentManager
    {
        public StudentManager(string connectionString) : base(connectionString) {}

        public List<int> ViewGrades(int studentId)
        {
            var grades = new List<int>();

            using (var connection = GetConnection())
            {
                connection.Open();
                
                string query = "SELECT g.grade_value " +
                               "FROM grades g " +
                               "JOIN lecturer_courses lc ON g.lecturer_course_id = lc.lecturer_course_id " +
                               "WHERE g.student_id = @studentId";
                               
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

        public List<object> GetStudentData()
        {
            List<object> studentData = new List<object>();

            using (var connection = GetConnection())
            {
                connection.Open();

                string courseSubjectsQuery = "SELECT course_subject_id, course_id, subject_id FROM course_subjects";
                var courseSubjectsCommand = new MySqlCommand(courseSubjectsQuery, connection);
                var courseSubjectsReader = courseSubjectsCommand.ExecuteReader();
                List<CourseSubjects> courseSubjects = new List<CourseSubjects>();
                while (courseSubjectsReader.Read())
                {
                    courseSubjects.Add(new CourseSubjects
                    {
                        CourseSubjectId = courseSubjectsReader.GetInt32("course_subject_id"),
                        CourseId = courseSubjectsReader.GetInt32("course_id"),
                        SubjectId = courseSubjectsReader.GetInt32("subject_id")
                    });
                }
                studentData.Add(courseSubjects);
                courseSubjectsReader.Close();
            }
            return studentData;
        }
    }
}