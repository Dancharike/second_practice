using System.Collections.Generic;
using AS_practice.Models;
using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess
{
    public class StudentManager
    {
        private readonly string _connectionString;

        public StudentManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM students";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            StudentId = reader.GetInt32("student_id"),
                            FirstName = reader.GetString("first_name"),
                            LastName = reader.GetString("last_name"),
                            GroupId = reader.GetInt32("group_id")
                        });
                    }
                }
            }

            return students;
        }
        
        public void AddStudent(string firstName, string lastName, int groupId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO students (first_name, last_name, group_id) VALUES (@firstName, @lastName, @groupId)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@lastName", lastName);
                    command.Parameters.AddWithValue("@groupId", groupId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}