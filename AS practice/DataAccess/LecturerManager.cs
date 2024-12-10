using System.Collections.Generic;
using AS_practice.Models;
using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess
{
    public class LecturerManager
    {
        private readonly string _connectionString;

        public LecturerManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public List<Lecturer> GetAllLecturers()
        {
            var lecturers = new List<Lecturer>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM lecturers";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lecturers.Add(new Lecturer
                        {
                            LecturerId = reader.GetInt32("lecturer_id"),
                            FirstName = reader.GetString("first_name"),
                            LastName = reader.GetString("last_name")
                        });
                    }
                }
            }

            return lecturers;
        }
        
        public void AddLecturer(string firstName, string lastName)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO lecturers (first_name, last_name) VALUES (@firstName, @lastName)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@lastName", lastName);
                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void AssignLecturerToCourse(int lecturerId, int courseId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO lecturer_courses (lecturer_id, course_id) VALUES (@lecturerId, @courseId)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@lecturerId", lecturerId);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}