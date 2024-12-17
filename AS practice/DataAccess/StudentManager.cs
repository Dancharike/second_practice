using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AS_practice.DataAccess.InterfacesForDataAccess;
using AS_practice.Models;

namespace AS_practice.DataAccess
{
    public class StudentManager : DatabaseBase, IStudentManager
    {
        public StudentManager(string connectionString) : base(connectionString) {}

        public List<StudentSubjectGrade> GetStudentGrades(int studentId)
        {
            List<StudentSubjectGrade> grades = new List<StudentSubjectGrade>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = @"
                        SELECT s.first_name, s.last_name, sub.subject_name, g.grade_value
                        FROM students s
                        JOIN student_subject_grades g ON s.student_id = g.student_id
                        JOIN subjects sub ON g.subject_id = sub.subject_id
                        WHERE s.student_id = @studentId";
            
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@studentId", studentId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            grades.Add(new StudentSubjectGrade
                            {
                                FirstName = reader.GetString("first_name"),
                                LastName = reader.GetString("last_name"),
                                SubjectName = reader.GetString("subject_name"),
                                GradeValue = reader.GetInt32("grade_value")
                            });
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
                
                string query = @"
                    SELECT 
                    g.student_id, 
                    s.first_name, 
                    s.last_name, 
                    sub.subject_name, 
                    g.grade_value
                    FROM students s
                    JOIN student_subject_grades g ON s.student_id = g.student_id
                    JOIN subjects sub ON g.subject_id = sub.subject_id
                    WHERE s.student_id = @studentId";

                var studentSubjectGradesCommand = new MySqlCommand(query, connection);
                var studentSubjectGradesReader = studentSubjectGradesCommand.ExecuteReader();
                List<StudentSubjectGrade> studentSubjectGrades = new List<StudentSubjectGrade>();
                while (studentSubjectGradesReader.Read())
                {
                    studentSubjectGrades.Add(new StudentSubjectGrade
                    {
                        FirstName = studentSubjectGradesReader.GetString("first_name"),
                        LastName = studentSubjectGradesReader.GetString("last_name"),
                        SubjectName = studentSubjectGradesReader.GetString("subject_name"),
                        GradeValue = studentSubjectGradesReader.GetInt32("grade_value")
                    });
                }
            }
            return studentData;
        }
    }
}