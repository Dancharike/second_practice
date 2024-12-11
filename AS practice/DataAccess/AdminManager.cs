using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AS_practice.DataAccess.InterfacesForDataAccess;
using AS_practice.DataAccess.ValidationClasses;

namespace AS_practice.DataAccess
{
    public class AdminManager : DatabaseBase, IAdminManager
    {
        private readonly GroupValidation _group;
        private readonly CourseValidation _course;
        private readonly StudentValidation _student;
        private readonly RoleValidation _role;

        public AdminManager(string connectionString) : base(connectionString)
        {
            _group = new GroupValidation(connectionString);
            _course = new CourseValidation(connectionString);
            _student = new StudentValidation(connectionString);
            _role = new RoleValidation(connectionString);
        }

        public void AddUser(string username, string password, string role)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = @"
                    INSERT INTO users (username, password, role_id)
                    VALUES (@username, @password, (SELECT role_id FROM user_roles WHERE role_name = @role))";
                
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@role", role);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int userId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string roleName = _role.GetUserRoleName(userId);
                
                if (string.IsNullOrEmpty(roleName))
                {
                    throw new Exception("User not found.");
                }

                if (roleName == "Admin")
                {
                    throw new InvalidOperationException("Cannot delete an administrator.");
                }

                string query = "DELETE FROM users WHERE user_id = @userId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void CreateGroup(string groupName)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO student_groups (group_name) VALUES (@groupName)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@groupName", groupName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void CreateCourse(string courseName)
        {
            using (var connection = GetConnection())
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

        public void AddStudentToGroup(int studentId, int groupId)
        {
            if (!_group.GroupExists(groupId))
            {
                throw new Exception("Group not found.");
            }

            if (!_student.StudentExists(studentId))
            {
                throw new Exception("Student not found.");
            }

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "UPDATE students SET group_id = @groupId WHERE student_id = @studentId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@groupId", groupId);
                    command.Parameters.AddWithValue("@studentId", studentId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddStudentToCourse(int studentId, int courseId)
        {
            if (!_course.CourseExists(courseId))
            {
                throw new Exception("Course not found.");
            }

            if (!_student.StudentExists(studentId))
            {
                throw new Exception("Student not found.");
            }

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO grades (student_id, lecturer_course_id, grade_value) VALUES (@studentId, @courseId, NULL)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@studentId", studentId);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void AssignSubjectsToGroup(int groupId, List<int> subjectIds)
        {
            if (subjectIds == null || subjectIds.Count == 0 || subjectIds.Count > 7)
            {
                throw new ArgumentException("You must provide between 1 and 7 subject IDs.");
            }

            if (!_group.GroupExists(groupId))
            {
                throw new Exception("Group not found.");
            }

            using (var connection = GetConnection())
            {
                connection.Open();

                foreach (var subjectId in subjectIds)
                {
                    if (!_course.CourseExists(subjectId))
                    {
                        throw new Exception($"Subject with ID {subjectId} not found.");
                    }

                    string query = "INSERT INTO group_courses (group_id, course_id) VALUES (@groupId, @subjectId)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@groupId", groupId);
                        command.Parameters.AddWithValue("@subjectId", subjectId);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        
        public void AssignLecturerToCourse(int lecturerId, int courseId)
        {
            using (var connection = GetConnection())
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
