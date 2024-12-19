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
            
        public void AddGrade(int studentId, int subjectId, int categoryId, int gradeValue)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = @"INSERT INTO grades 
                        (student_id, subject_id, category_id, grade_value)
                        VALUES (@studentId, @subjectId, @categoryId, @gradeValue)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    command.Parameters.AddWithValue("@subjectId", subjectId);
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

        public List<object> GetLecturerData(int? lecturerId)
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
                        GroupId = studentReader.IsDBNull(studentReader.GetOrdinal("group_id")) ? (int?)null : studentReader.GetInt32("group_id")
                    });
                }
                lecturerData.Add(students);
                studentReader.Close();

                string gradeQuery = @"SELECT g.grade_id, g.student_id, s.subject_name,g.category_id, g.grade_value 
                        FROM grades g 
                        JOIN subjects s ON g.subject_id = s.subject_id
                        JOIN lecturer_subjects ls ON s.subject_id = ls.subject_id
                        JOIN grade_categories c ON g.category_id = c.category_id
                        WHERE ls.lecturer_id = @lecturerId";
                var gradeCommand = new MySqlCommand(gradeQuery, connection);
                gradeCommand.Parameters.AddWithValue("@lecturerId", lecturerId);
                var gradeReader = gradeCommand.ExecuteReader();
                List<Grade> grades = new List<Grade>();
                while (gradeReader.Read())
                {
                    grades.Add(new Grade
                    {
                        GradeId = gradeReader.IsDBNull(gradeReader.GetOrdinal("grade_id")) ? 0 : gradeReader.GetInt32("grade_id"),
                        StudentId = gradeReader.IsDBNull(gradeReader.GetOrdinal("student_id")) ? 0 : gradeReader.GetInt32("student_id"),
                        SubjectName = gradeReader.IsDBNull(gradeReader.GetOrdinal("subject_name")) ? string.Empty : gradeReader.GetString("subject_name"),
                        CategoryId = gradeReader.IsDBNull(gradeReader.GetOrdinal("category_id")) ? 0 : gradeReader.GetInt32("category_id"),
                        GradeValue = gradeReader.IsDBNull(gradeReader.GetOrdinal("grade_value")) ? 0 : gradeReader.GetInt32("grade_value")
                    });
                }
                lecturerData.Add(grades);
                gradeReader.Close();
                
                string subjectQuery = @"
                        SELECT s.subject_id, s.subject_name 
                        FROM subjects s
                        JOIN lecturer_subjects ls ON s.subject_id = ls.subject_id
                        WHERE ls.lecturer_id = @lecturerId";
                
                var subjectCommand = new MySqlCommand(subjectQuery, connection);
                subjectCommand.Parameters.AddWithValue("@lecturerId", lecturerId);
                var subjectReader = subjectCommand.ExecuteReader();
                List<Subjects> subjects = new List<Subjects>();
                while (subjectReader.Read())
                {
                    subjects.Add(new Subjects
                    {
                        SubjectId = subjectReader.IsDBNull(subjectReader.GetOrdinal("subject_id")) ? 0 : subjectReader.GetInt32("subject_id"),
                        SubjectName = subjectReader.IsDBNull(subjectReader.GetOrdinal("subject_name")) ? string.Empty : subjectReader.GetString("subject_name")
                    });
                }
                lecturerData.Add(subjects);
                subjectReader.Close();
                
                string categoryQuery = @"
                        SELECT category_id, category_name, weight
                        FROM grade_categories";
                
                var categoryCommand = new MySqlCommand(categoryQuery, connection);
                var categoryReader = categoryCommand.ExecuteReader();
                List<GradeCategories> categories = new List<GradeCategories>();
                while (categoryReader.Read())
                {
                    categories.Add(new GradeCategories
                    {
                        CategoryId = categoryReader.IsDBNull(categoryReader.GetOrdinal("category_id")) ? (int?)null : categoryReader.GetInt32("category_id"),
                        CategoryName = categoryReader.IsDBNull(categoryReader.GetOrdinal("category_name")) ? null : categoryReader.GetString("category_name"),
                        Weight = categoryReader.IsDBNull(categoryReader.GetOrdinal("weight")) ? (int?)null : categoryReader.GetInt32("weight")
                    });
                }
                lecturerData.Add(categories);
                categoryReader.Close();
            }
            return lecturerData;
        }
    }
}
