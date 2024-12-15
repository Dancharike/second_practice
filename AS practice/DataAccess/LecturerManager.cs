using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AS_practice.DataAccess.InterfacesForDataAccess;
using AS_practice.Models;

namespace AS_practice.DataAccess
{
    public class LecturerManager : DatabaseBase, ILecturerManager
    {
        public LecturerManager(string connectionString) : base(connectionString)
        {
        }

        public void AddGrade(int studentId, int subjectId, int grade)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = @"INSERT INTO subject_grades (student_id, subject_id, grade_value) 
                         VALUES (@studentId, @subjectId, @grade)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    command.Parameters.AddWithValue("@subjectId", subjectId);
                    command.Parameters.AddWithValue("@grade", grade);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EditGrade(int studentId, int subjectId, int grade)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = @"UPDATE subject_grades 
                         SET grade_value = @grade 
                         WHERE student_id = @studentId AND subject_id = @subjectId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    command.Parameters.AddWithValue("@subjectId", subjectId);
                    command.Parameters.AddWithValue("@grade", grade);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<object> GetLecturerData()
        {
            List<object> lecturerData = new List<object>();

            using (var connection = GetConnection())
            {
                connection.Open();
                
                string studentQuery = @"
                    SELECT student_id, first_name, last_name, group_id 
                    FROM students";
                var studentCommand = new MySqlCommand(studentQuery, connection);
                var studentReader = studentCommand.ExecuteReader();
                List<Student> students = new List<Student>();
                while (studentReader.Read())
                {
                    students.Add(new Student
                    {
                        StudentId = studentReader.GetInt32("student_id"),
                        FirstName = studentReader.GetString("first_name"),
                        LastName = studentReader.GetString("last_name"),
                        GroupId = studentReader.IsDBNull(3) ? (int?)null : studentReader.GetInt32("group_id")
                    });
                }
                lecturerData.Add(students);
                studentReader.Close();
                /*
                string gradeQuery = @"
                    SELECT s.first_name, s.last_name, sb.subject_name, sg.grade_value 
                    FROM subject_grades sg
                    JOIN students s ON sg.student_id = s.student_id
                    JOIN subjects sb ON sg.subject_id = sb.subject_id";
                var gradeCommand = new MySqlCommand(gradeQuery, connection);
                var gradeReader = gradeCommand.ExecuteReader();
                List<Grades> grades = new List<Grades>();
                while (gradeReader.Read())
                {
                    grades.Add(new Grades
                    {
                        FirstName = gradeReader.GetString("first_name"),
                        LastName = gradeReader.GetString("last_name"),
                        SubjectName = gradeReader.GetString("subject_name"),
                        GradeValue = gradeReader.IsDBNull(3) ? (int?)null : gradeReader.GetInt32("grade_value")
                    });
                }
                lecturerData.Add(grades);
                gradeReader.Close();
                */
                string gradeQuery = "SELECT grade_id, student_id, lecturer_course_id, grade_value FROM grades";
                var gradeCommand = new MySqlCommand(gradeQuery, connection);
                var gradeReader = gradeCommand.ExecuteReader();
                List<Grade> grades = new List<Grade>();
                while (gradeReader.Read())
                {
                    grades.Add(new Grade
                    {
                        GradeId = gradeReader.GetInt32("grade_id"),
                        StudentId = gradeReader.GetInt32("student_id"),
                        LecturerCourseId = gradeReader.GetInt32("lecturer_course_id"),
                        GradeValue = gradeReader.GetInt32("grade_value")
                    });
                }
                lecturerData.Add(grades);
                gradeReader.Close();
                
                string subjectsQuery = @"
                    SELECT subject_id, subject_name 
                    FROM subjects";
                var subjectsCommand = new MySqlCommand(subjectsQuery, connection);
                var subjectsReader = subjectsCommand.ExecuteReader();
                List<Subjects> subjects = new List<Subjects>();
                while (subjectsReader.Read())
                {
                    subjects.Add(new Subjects
                    {
                        SubjectId = subjectsReader.GetInt32("subject_id"),
                        SubjectName = subjectsReader.GetString("subject_name")
                    });
                }
                lecturerData.Add(subjects);
                subjectsReader.Close();
            }
            return lecturerData;
        }
    }
}
