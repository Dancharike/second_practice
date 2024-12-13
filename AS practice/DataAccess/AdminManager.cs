using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AS_practice.DataAccess.InterfacesForDataAccess;
using AS_practice.DataAccess.ValidationClasses;
using AS_practice.Models;

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
        
        public List<object> GetAdminData()
        {
            List<object> adminData = new List<object>();

            using (var connection = GetConnection())
            {
                connection.Open();
                
                string studentQuery = "SELECT student_id, first_name, last_name, group_id FROM students";
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
                        GroupId = studentReader.GetInt32("group_id")
                    });
                }
                adminData.Add(students);
                studentReader.Close();
                
                string groupQuery = "SELECT group_id, group_name FROM student_groups";
                var groupCommand = new MySqlCommand(groupQuery, connection);
                var groupReader = groupCommand.ExecuteReader();
                List<StudentGroup> groups = new List<StudentGroup>();
                while (groupReader.Read())
                {
                    groups.Add(new StudentGroup
                    {
                        GroupId = groupReader.GetInt32("group_id"),
                        GroupName = groupReader.GetString("group_name")
                    });
                }
                adminData.Add(groups);
                groupReader.Close();
                
                string courseQuery = "SELECT course_id, course_name FROM courses";
                var courseCommand = new MySqlCommand(courseQuery, connection);
                var courseReader = courseCommand.ExecuteReader();
                List<Course> courses = new List<Course>();
                while (courseReader.Read())
                {
                    courses.Add(new Course
                    {
                        CourseId = courseReader.GetInt32("course_id"),
                        CourseName = courseReader.GetString("course_name")
                    });
                }
                adminData.Add(courses);
                courseReader.Close();
                
                string lecturerQuery = "SELECT lecturer_id, first_name, last_name FROM lecturers";
                var lecturerCommand = new MySqlCommand(lecturerQuery, connection);
                var lecturerReader = lecturerCommand.ExecuteReader();
                List<Lecturer> lecturers = new List<Lecturer>();
                while (lecturerReader.Read())
                {
                    lecturers.Add(new Lecturer
                    {
                        LecturerId = lecturerReader.GetInt32("lecturer_id"),
                        FirstName = lecturerReader.GetString("first_name"),
                        LastName = lecturerReader.GetString("last_name")
                    });
                }
                adminData.Add(lecturers);
                lecturerReader.Close();
            }

            return adminData;
        }
        
        public void DisplayAdminData(List<object> adminData)
        {
            var students = (List<Student>)adminData[0];
            var groups = (List<StudentGroup>)adminData[1];
            var courses = (List<Course>)adminData[2];
            var lecturers = (List<Lecturer>)adminData[3];
        }
    }
}
