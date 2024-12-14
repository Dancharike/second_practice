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

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = @"
                    INSERT INTO grades (student_id, lecturer_course_id, grade_value)
                    VALUES (@studentId, @courseId, NULL)";

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
                
                /*
                // this should belong to LecturerManager
                string gradeQuery = "SELECT grade_id, student_id, lecturer_course_id, grade_value FROM grades";
                var gradeCommand = new MySqlCommand(gradeQuery, connection);
                var gradeReader = gradeCommand.ExecuteReader();
                List<Grade> grades = new List<Grade>();
                while (gradeReader.Read())
                {
                    grades.Add(new Grade
                    {
                        GradeId = gradeReader.GetInt32("grade_id"),
                        StudentId = gradeReader.GetInt32("student_id"),
                        LecturerCourseId = gradeReader.GetInt32("lecturer_course_id"),
                        GradeValue = gradeReader.GetInt32("grade_value")
                    });
                }
                adminData.Add(grades);
                gradeReader.Close();
                */
                
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
                
                /*
                string adminQuery = "SELECT admin_id, first_name, last_name FROM admins";
                var adminCommand = new MySqlCommand(adminQuery, connection);
                var adminReader = adminCommand.ExecuteReader();
                List<Admin> admins = new List<Admin>();
                while (adminReader.Read())
                {
                    admins.Add(new Admin
                    {
                        AdminId = adminReader.GetInt32("admin_id"),
                        FirstName = adminReader.GetString("first_name"),
                        LastName = adminReader.GetString("last_name")
                    });
                }
                adminData.Add(admins);
                adminReader.Close();
                */
                
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
                
                string courseSubjectsQuery = "SELECT course_subject_id, course_id, subject_id FROM course_subjects";
                var courseSubjectsCommand = new MySqlCommand(courseSubjectsQuery, connection);
                var courseSubjectsReader = courseSubjectsCommand.ExecuteReader();
                List<CourseSubjects> courseSubjects = new List<CourseSubjects>();
                while (courseSubjectsReader.Read())
                {
                    courseSubjects.Add(new CourseSubjects
                    {
                        CourseSubjectId = courseSubjectsReader.GetInt32("course_subject_id"),
                        CourseId = courseSubjectsReader.GetInt32("course_id"),
                        SubjectId = courseSubjectsReader.GetInt32("subject_id")
                    });
                }
                adminData.Add(courseSubjects);
                courseSubjectsReader.Close();
            }
            return adminData;
        }
    }
}
