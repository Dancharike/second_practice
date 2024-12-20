﻿using System;
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
                int roleSpecificId = 0;
                
                if (role == "Admin")
                {
                    var insertAdminQuery = "INSERT INTO admins (first_name, last_name) VALUES (@firstName, @lastName)";
                    var getAdminIdQuery = "SELECT LAST_INSERT_ID()";
                    using (var command = new MySqlCommand(insertAdminQuery, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", username);
                        command.Parameters.AddWithValue("@lastName", password);
                        command.ExecuteNonQuery();
                    }

                    using (var command = new MySqlCommand(getAdminIdQuery, connection))
                    {
                        roleSpecificId = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                
                if (role == "Lecturer")
                {
                    var insertLecturerQuery = "INSERT INTO lecturers (first_name, last_name) VALUES (@firstName, @lastName)";
                    var getLecturerIdQuery = "SELECT LAST_INSERT_ID()";
                    using (var command = new MySqlCommand(insertLecturerQuery, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", username);
                        command.Parameters.AddWithValue("@lastName", password);
                        command.ExecuteNonQuery();
                    }

                    using (var command = new MySqlCommand(getLecturerIdQuery, connection))
                    {
                        roleSpecificId = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                
                if (role == "Student")
                {
                    var insertStudentQuery = "INSERT INTO students (first_name, last_name) VALUES (@firstName, @lastName)";
                    var getStudentIdQuery = "SELECT LAST_INSERT_ID()";
                    using (var command = new MySqlCommand(insertStudentQuery, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", username);
                        command.Parameters.AddWithValue("@lastName", password);
                        command.ExecuteNonQuery();
                    }

                    using (var command = new MySqlCommand(getStudentIdQuery, connection))
                    {
                        roleSpecificId = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                
                var insertUserQuery = @"
            INSERT INTO users (username, password, role_id, role_specific_id) 
            VALUES (@username, @password, @roleId, @roleSpecificId)";
        
                using (var command = new MySqlCommand(insertUserQuery, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@roleId", GetRoleId(role));
                    command.Parameters.AddWithValue("@roleSpecificId", roleSpecificId);
                    command.ExecuteNonQuery();
                }
            }
        }

        private int GetRoleId(string roleName)
        {
            if (roleName == "Admin") return 1;
            if (roleName == "Lecturer") return 2;
            if (roleName == "Student") return 3;
            throw new ArgumentException("Invalid role name.");
        }
        
        public void DeleteUser(int userId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                
                string getRoleSpecificIdQuery = "SELECT role_specific_id, role_id FROM users WHERE user_id = @userId";
                int roleSpecificId = 0;
                int roleId = 0;

                using (var command = new MySqlCommand(getRoleSpecificIdQuery, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            roleSpecificId = reader.GetInt32("role_specific_id");
                            roleId = reader.GetInt32("role_id");
                        }
                        else
                        {
                            throw new ArgumentException("User not found.");
                        }
                    }
                }
                
                if (roleId == 1) // admin
                {
                    string deleteAdminQuery = "DELETE FROM admins WHERE admin_id = @roleSpecificId";
                    using (var command = new MySqlCommand(deleteAdminQuery, connection))
                    {
                        command.Parameters.AddWithValue("@roleSpecificId", roleSpecificId);
                        command.ExecuteNonQuery();
                    }
                }
                else if (roleId == 2) // lecturer
                {
                    string deleteLecturerQuery = "DELETE FROM lecturers WHERE lecturer_id = @roleSpecificId";
                    using (var command = new MySqlCommand(deleteLecturerQuery, connection))
                    {
                        command.Parameters.AddWithValue("@roleSpecificId", roleSpecificId);
                        command.ExecuteNonQuery();
                    }
                }
                else if (roleId == 3) // student
                {
                    string deleteStudentQuery = "DELETE FROM students WHERE student_id = @roleSpecificId";
                    using (var command = new MySqlCommand(deleteStudentQuery, connection))
                    {
                        command.Parameters.AddWithValue("@roleSpecificId", roleSpecificId);
                        command.ExecuteNonQuery();
                    }
                }
                
                string deleteUserQuery = "DELETE FROM users WHERE user_id = @userId";
                using (var command = new MySqlCommand(deleteUserQuery, connection))
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
        
        public void AssignGroupToCourse(int groupId, int courseId)
        {
            if (!_group.GroupExists(groupId))
            {
                throw new Exception("Group not found.");
            }

            using (var connection = GetConnection())
            {
                connection.Open();

                string query = @"
                        INSERT INTO group_courses (group_id, course_id)
                        VALUES (@groupId, @courseId)";
        
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@groupId", groupId);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    command.ExecuteNonQuery();
                }
                
                var studentsInGroup = GetStudentsByGroup(groupId);
                foreach (var student in studentsInGroup)
                {
                    AddStudentToGroup(student.StudentId, groupId);
                }
            }
        }

        private List<Student> GetStudentsByGroup(int groupId)
        {
            var students = new List<Student>();

            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT student_id FROM students WHERE group_id = @groupId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@groupId", groupId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student
                            {
                                StudentId = reader.GetInt32("student_id")
                            });
                        }
                    }
                }
            }
            return students;
        }
        
        public void AssignSubjectToLecturer(int lecturerId, int subjectId)
        {
            if (lecturerId <= 0 || subjectId <= 0)
            {
                throw new ArgumentException("Both lecturer ID and subject ID must be valid.");
            }

            using (var connection = GetConnection())
            {
                connection.Open();
                
                string query = "INSERT INTO lecturer_subjects (lecturer_id, subject_id) VALUES (@lecturerId, @subjectId)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@lecturerId", lecturerId);
                    command.Parameters.AddWithValue("@subjectId", subjectId);
                    command.ExecuteNonQuery();
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
        
        public void DeleteGroup(int groupId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM student_groups WHERE group_id = @groupId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@groupId", groupId);
                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void DeleteCourse(int courseId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM courses WHERE course_id = @courseId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@courseId", courseId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void CreateSubject(string subjectName)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO subjects (subject_name) VALUES (@subjectName)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@subjectName", subjectName);
                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void DeleteSubject(int subjectId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM subjects WHERE subject_id = @subjectId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@subjectId", subjectId);
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
                        GroupId = studentReader.IsDBNull(studentReader.GetOrdinal("group_id")) ? (int?)null : studentReader.GetInt32("group_id")
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
                
                string lecturerCourseQuery = "SELECT lecturer_course_id, lecturer_id, course_id FROM lecturer_courses";
                var lecturerCourseCommand = new MySqlCommand(lecturerCourseQuery, connection);
                var lecturerCourseReader = lecturerCourseCommand.ExecuteReader();
                List<LecturerCourse> lecturerCourses = new List<LecturerCourse>();
                while (lecturerCourseReader.Read())
                {
                    lecturerCourses.Add(new LecturerCourse
                    {
                        LecturerCourseId = lecturerCourseReader.GetInt32("lecturer_course_id"),
                        LecturerId = lecturerCourseReader.GetInt32("lecturer_id"),
                        CourseId = lecturerCourseReader.GetInt32("course_id")
                    });
                }
                adminData.Add(lecturerCourses);
                lecturerCourseReader.Close();
                
                string userQuery = "SELECT user_id, username, password, role_id, role_specific_id FROM users";
                var userCommand = new MySqlCommand(userQuery, connection);
                var userReader = userCommand.ExecuteReader();
                List<User> users = new List<User>();
                while (userReader.Read())
                {
                    users.Add(new User
                    {
                        UserId = userReader.GetInt32("user_id"),
                        Username = userReader.GetString("username"),
                        Password = userReader.GetString("password"),
                        RoleId = userReader.IsDBNull(userReader.GetOrdinal("role_id")) ? 0 : userReader.GetInt32("role_id"),
                        RoleSpecificId = userReader.IsDBNull(userReader.GetOrdinal("role_specific_id")) ? 0 : userReader.GetInt32("role_specific_id")
                    });
                }
                adminData.Add(users);
                userReader.Close();
                
                string roleQuery = "SELECT role_id, role_name FROM user_roles";
                var roleCommand = new MySqlCommand(roleQuery, connection);
                var roleReader = roleCommand.ExecuteReader();
                List<UserRoles> roles = new List<UserRoles>();
                while (roleReader.Read())
                {
                    roles.Add(new UserRoles
                    {
                        RoleId = roleReader.GetInt32("role_id"),
                        RoleName = roleReader.GetString("role_name")
                    });
                }
                adminData.Add(roles);
                roleReader.Close();
                
                string groupCourseQuery = "SELECT group_course_id, group_id, course_id FROM group_courses";
                var groupCourseCommand = new MySqlCommand(groupCourseQuery, connection);
                var groupCourseReader = groupCourseCommand.ExecuteReader();
                List<GroupCourses> groupCourses = new List<GroupCourses>();
                while (groupCourseReader.Read())
                {
                    groupCourses.Add(new GroupCourses
                    {
                        GroupCoursesId = groupCourseReader.GetInt32("group_course_id"),
                        GroupId = groupCourseReader.GetInt32("group_id"),
                        CourseId = groupCourseReader.GetInt32("course_id")
                    });
                }
                adminData.Add(groupCourses);
                groupCourseReader.Close();
                
                string subjectsQuery = "SELECT subject_id, subject_name FROM subjects";
                var subjectsCommand = new MySqlCommand(subjectsQuery, connection);
                var subjectsReader = subjectsCommand.ExecuteReader();
                List<Subjects> subjects = new List<Subjects>();
                while (subjectsReader.Read())
                {
                    subjects.Add(new Subjects
                    {
                        SubjectId = subjectsReader.GetInt32("subject_id"),
                        SubjectName = subjectsReader.GetString("subject_name")
                    });
                }
                adminData.Add(subjects);
                subjectsReader.Close();
                
                string lecturerSubjectsQuerry = @"
                    SELECT l.first_name, l.last_name, s.subject_name
                    FROM lecturer_subjects ls
                    JOIN lecturers l ON ls.lecturer_id = l.lecturer_id
                    JOIN subjects s ON ls.subject_id = s.subject_id";

                var lecturerSubjectsCommand = new MySqlCommand(lecturerSubjectsQuerry, connection);
                var lecturerSubjectsReader = lecturerSubjectsCommand.ExecuteReader();
                List<LecturerSubjects> lecturerSubjects = new List<LecturerSubjects>();
                while (lecturerSubjectsReader.Read())
                {
                    lecturerSubjects.Add(new LecturerSubjects
                    {
                        LecturerName = lecturerSubjectsReader.GetString("first_name") + " " + lecturerSubjectsReader.GetString("last_name"),
                        SubjectName = lecturerSubjectsReader.GetString("subject_name")
                    });
                }
                adminData.Add(lecturerSubjects);
                lecturerSubjectsReader.Close();
            }
            return adminData;
        }
    }
}
