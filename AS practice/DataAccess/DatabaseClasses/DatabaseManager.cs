using System;
using System.Collections.Generic;
using AS_practice.DataAccess.InterfacesForDataAccess;
using AS_practice.Models;
using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess
{
    public class DatabaseManager
    {
        private readonly string _connectionString;
        
        public IStudentManager Students { get; }
        public LecturerManager Lecturers { get; }
        public IAdminManager Admins { get; }
        
        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
            
            Students = new StudentManager(_connectionString);
            Lecturers = new LecturerManager(_connectionString);
            Admins = new AdminManager(_connectionString);
        }
    }
}