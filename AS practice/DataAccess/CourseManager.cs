using System.Collections.Generic;
using AS_practice.Models;
using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess
{
    public class CourseManager
    {
        private readonly string _connectionString;

        public CourseManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public List<Course> GetAllCourses()
        {
            var courses = new List<Course>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM courses";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courses.Add(new Course
                        {
                            CourseId = reader.GetInt32("course_id"),
                            CourseName = reader.GetString("course_name")
                        });
                    }
                }
            }

            return courses;
        }
        
        public void AddCourse(string courseName)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO courses (course_name) VALUES (@courseName)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@courseName", courseName);
                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void AssignCourseToGroup(int courseId, int groupId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO group_courses (course_id, group_id) VALUES (@courseId, @groupId)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@courseId", courseId);
                    command.Parameters.AddWithValue("@groupId", groupId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}