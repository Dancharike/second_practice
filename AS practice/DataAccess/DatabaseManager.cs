using System;
using System.Collections.Generic;
using AS_practice.Models;
using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public StudentManager Students { get; }
        public LecturerManager Lecturers { get; }
        public AdminManager Admins { get; }
        public CourseManager Courses { get; }
        public GradeManager Grades { get; }
        
        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
            
            Students = new StudentManager(_connectionString);
            Lecturers = new LecturerManager(_connectionString);
            Admins = new AdminManager(_connectionString);
            Courses = new CourseManager(_connectionString);
            Grades = new GradeManager(_connectionString);
        }

        /*
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public void TestConnection()
        {
            var connection = GetConnection();
            try
            {
                connection.Open();
                Console.WriteLine("Connection established");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection error: {ex.Message}");
            }
        }
        */
    }
}