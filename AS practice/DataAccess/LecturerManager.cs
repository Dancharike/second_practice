using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AS_practice.DataAccess.InterfacesForDataAccess;
using AS_practice.Models;

namespace AS_practice.DataAccess
{
    public class LecturerManager : DatabaseBase, ILecturerManager
    {
        public LecturerManager(string connectionString) : base(connectionString) {}

        public void AddGrade(int studentId, int lecturerCourseId, int categoryId, int gradeValue)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = @"INSERT INTO grades 
                        (student_id, lecturer_course_id, category_id, grade_value)
                        VALUES (@studentId, @lecturerCourseId, @categoryId, @gradeValue)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    command.Parameters.AddWithValue("@lecturerCourseId", lecturerCourseId);
                    command.Parameters.AddWithValue("@categoryId", categoryId);
                    command.Parameters.AddWithValue("@gradeValue", gradeValue);
                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void EditGrade(int gradeId, int gradeValue)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = @"UPDATE grades 
                         SET grade_value = @gradeValue WHERE grade_id = @gradeId";
                         
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@gradeId", gradeId);
                    command.Parameters.AddWithValue("@gradeValue", gradeValue);
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
                
                string subjectQuery = "SELECT subject_id, subject_name FROM subjects";
                var subjectCommand = new MySqlCommand(subjectQuery, connection);
                var subjectReader = subjectCommand.ExecuteReader();
                List<Subjects> subjects = new List<Subjects>();
                while (subjectReader.Read())
                {
                    subjects.Add(new Subjects
                    {
                        SubjectId = subjectReader.GetInt32("subject_id"),
                        SubjectName = subjectReader.GetString("subject_name")
                    });
                }
                lecturerData.Add(subjects);
                subjectReader.Close();
                
                string categoryQuerry = @"
                        SELECT category_id, category_name, weight
                        FROM grade_categories";
                
                var categoryCommand = new MySqlCommand(categoryQuerry, connection);
                var categoryReader = categoryCommand.ExecuteReader();
                List<GradeCategories> categories = new List<GradeCategories>();
                while (categoryReader.Read())
                {
                    categories.Add(new GradeCategories
                    {
                        CategoryId = categoryReader.IsDBNull(categoryReader.GetOrdinal("category_id")) 
                            ? (int?)null 
                            : categoryReader.GetInt32("category_id"),
                        CategoryName = categoryReader.IsDBNull(categoryReader.GetOrdinal("category_name")) 
                            ? null 
                            : categoryReader.GetString("category_name"),
                        Weight = categoryReader.IsDBNull(categoryReader.GetOrdinal("weight")) 
                            ? (int?)null 
                            : categoryReader.GetInt32("weight")
                    });
                }
                lecturerData.Add(categories);
                categoryReader.Close();
            }
            return lecturerData;
        }
    }
}
