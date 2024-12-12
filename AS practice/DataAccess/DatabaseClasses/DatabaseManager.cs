using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AS_practice.DataAccess.InterfacesForDataAccess;
using AS_practice.Models;
using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess
{
    public class DatabaseManager
    {
        private readonly string _connectionString;
        
        public IStudentManager Students { get; }
        public ILecturerManager Lecturers { get; }
        public IAdminManager Admins { get; }
        
        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
            
            Students = new StudentManager(_connectionString);
            Lecturers = new LecturerManager(_connectionString);
            Admins = new AdminManager(_connectionString);
        }
        
        public bool ValidateUser(string username, string password, string selectedRole)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                SELECT u.role_id, ur.role_name
                FROM users u
                JOIN user_roles ur ON u.role_id = ur.role_id
                WHERE u.username = @username AND u.password = @password";
                
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string dbRole = reader.GetString("role_name");
                            if(dbRole.Equals(selectedRole, StringComparison.OrdinalIgnoreCase))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}