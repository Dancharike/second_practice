using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AS_practice.DataAccess;
using AS_practice.FrontEnd;
using AS_practice.Models;

public class LoadAdminPage : Form
{
    private readonly AdminManager _adminManager;
    private DataGridView _studentsGridView;
    private DataGridView _groupsGridView;
    private DataGridView _coursesGridView;
    private DataGridView _lecturersGridView;
    
    private Button _addUserButton;
    private Button _deleteUserButton;
    private Button _createGroupButton;
    private Button _createCourseButton;
    private Button _assignStudentToGroupButton;
    private Button _assignStudentToCourseButton;
    private Button _assignSubjectsToGroupButton;
    private Button _assignLecturerToCourseButton;

    public LoadAdminPage(AdminManager adminManager)
    {
        _adminManager = adminManager;
        InitializeComponents();
        LoadData();
    }

    private void InitializeComponents()
    {
        Text = "Admin Panel";
        Size = new Size(1920, 1080);
        BackColor = Color.Black;
        
        _addUserButton = new Button
        {
            Text = "Add User",
            Location = new Point(20, 20),
            Size = new Size(150, 30),
            ForeColor = Color.White
        };
        _addUserButton.Click += AddUserButton_Click;
        
        _deleteUserButton = new Button
        {
            Text = "Delete User",
            Location = new Point(20, 60),
            Size = new Size(150, 30),
            ForeColor = Color.White
        };
        _deleteUserButton.Click += DeleteUserButton_Click;
        
        _createGroupButton = new Button
        {
            Text = "Create Group",
            Location = new Point(20, 100),
            Size = new Size(150, 30),
            ForeColor = Color.White
        };
        _createGroupButton.Click += CreateGroupButton_Click;
        
        _createCourseButton = new Button
        {
            Text = "Create Course",
            Location = new Point(20, 140),
            Size = new Size(150, 30),
            ForeColor = Color.White
        };
        _createCourseButton.Click += CreateCourseButton_Click;
        
        _assignStudentToGroupButton = new Button
        {
            Text = "Assign Student to Group",
            Location = new Point(20, 180),
            Size = new Size(200, 30),
            ForeColor = Color.White
        };
        _assignStudentToGroupButton.Click += AssignStudentToGroupButton_Click;
        
        _assignStudentToCourseButton = new Button
        {
            Text = "Assign Student to Course",
            Location = new Point(20, 220),
            Size = new Size(200, 30),
            ForeColor = Color.White
        };
        _assignStudentToCourseButton.Click += AssignStudentToCourseButton_Click;
        
        _assignSubjectsToGroupButton = new Button
        {
            Text = "Assign Subjects to Group",
            Location = new Point(20, 260),
            Size = new Size(200, 30),
            ForeColor = Color.White
        };
        _assignSubjectsToGroupButton.Click += AssignSubjectsToGroupButton_Click;
        
        _assignLecturerToCourseButton = new Button
        {
            Text = "Assign Lecturer to Course",
            Location = new Point(20, 300),
            Size = new Size(200, 30),
            ForeColor = Color.White
        };
        _assignLecturerToCourseButton.Click += AssignLecturerToCourseButton_Click;
        
        _studentsGridView = new DataGridView
        {
            Location = new Point(600, 20),
            Size = new Size(800, 250),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        
        _groupsGridView = new DataGridView
        {
            Location = new Point(600, 800),
            Size = new Size(800, 250),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        _coursesGridView = new DataGridView
        {
            Location = new Point(600, 540),
            Size = new Size(800, 250),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        _lecturersGridView = new DataGridView
        {
            Location = new Point(600, 280),
            Size = new Size(800, 250),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        
        Controls.Add(_addUserButton);
        Controls.Add(_deleteUserButton);
        Controls.Add(_createGroupButton);
        Controls.Add(_createCourseButton);
        Controls.Add(_assignStudentToGroupButton);
        Controls.Add(_assignStudentToCourseButton);
        Controls.Add(_assignSubjectsToGroupButton);
        Controls.Add(_assignLecturerToCourseButton);
        Controls.Add(_studentsGridView);
        Controls.Add(_groupsGridView);
        Controls.Add(_coursesGridView);
        Controls.Add(_lecturersGridView);
    }
    
    private void LoadData()
    {
        try
        {
            var adminData = _adminManager.GetAdminData();
            
            var students = (List<Student>)adminData[0];
            _studentsGridView.DataSource = students;
            
            var groups = (List<StudentGroup>)adminData[1];
            _groupsGridView.DataSource = groups;
            
            var courses = (List<Course>)adminData[2];
            _coursesGridView.DataSource = courses;
            
            var lecturers = (List<Lecturer>)adminData[3];
            _lecturersGridView.DataSource = lecturers;
            MessageBox.Show("User data loaded successfully.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading data: {ex.Message}\nStack Trace: {ex.StackTrace}");
        }
    }
    
    private void AddUserButton_Click(object sender, EventArgs e)
    {
        string username = UIManager.ShowPrompt("Enter username:", "Add User");
        string password = UIManager.ShowPrompt("Enter password:", "Add User");
        string role = UIManager.ShowPrompt("Enter role (Admin/Lecturer/Student):", "Add User");

        try
        {
            _adminManager.AddUser(username, password, role);
            MessageBox.Show("User added successfully.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }

    private void DeleteUserButton_Click(object sender, EventArgs e)
    {
        string userIdStr = UIManager.ShowPrompt("Enter user ID to delete:", "Delete User");
        if (int.TryParse(userIdStr, out int userId))
        {
            try
            {
                _adminManager.DeleteUser(userId);
                MessageBox.Show("User deleted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        else
        {
            MessageBox.Show("Invalid user ID.");
        }
    }

    private void CreateGroupButton_Click(object sender, EventArgs e)
    {
        string groupName = UIManager.ShowPrompt("Enter group name:", "Create Group");

        try
        {
            _adminManager.CreateGroup(groupName);
            MessageBox.Show("Group created successfully.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }

    private void CreateCourseButton_Click(object sender, EventArgs e)
    {
        string courseName = UIManager.ShowPrompt("Enter course name:", "Create Course");

        try
        {
            _adminManager.CreateCourse(courseName);
            MessageBox.Show("Course created successfully.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }

    private void AssignStudentToGroupButton_Click(object sender, EventArgs e)
    {
        string studentIdStr = UIManager.ShowPrompt("Enter student ID:", "Assign Student to Group");
        string groupIdStr = UIManager.ShowPrompt("Enter group ID:", "Assign Student to Group");

        if (int.TryParse(studentIdStr, out int studentId) && int.TryParse(groupIdStr, out int groupId))
        {
            try
            {
                _adminManager.AddStudentToGroup(studentId, groupId);
                MessageBox.Show("Student assigned to group successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        else
        {
            MessageBox.Show("Invalid input.");
        }
    }

    private void AssignStudentToCourseButton_Click(object sender, EventArgs e)
    {
        string studentIdStr = UIManager.ShowPrompt("Enter student ID:", "Assign Student to Course");
        string courseIdStr = UIManager.ShowPrompt("Enter course ID:", "Assign Student to Course");

        if (int.TryParse(studentIdStr, out int studentId) && int.TryParse(courseIdStr, out int courseId))
        {
            try
            {
                _adminManager.AddStudentToCourse(studentId, courseId);
                MessageBox.Show("Student assigned to course successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        else
        {
            MessageBox.Show("Invalid input.");
        }
    }

    private void AssignSubjectsToGroupButton_Click(object sender, EventArgs e)
    {
        string groupIdStr = UIManager.ShowPrompt("Enter group ID:", "Assign Subjects to Group");
        string subjectIdsStr = UIManager.ShowPrompt("Enter subject IDs (comma-separated):", "Assign Subjects to Group");

        if (int.TryParse(groupIdStr, out int groupId))
        {
            try
            {
                var subjectIds = new List<int>();
                foreach (var id in subjectIdsStr.Split(','))
                {
                    if (int.TryParse(id.Trim(), out int subjectId))
                    {
                        subjectIds.Add(subjectId);
                    }
                }

                _adminManager.AssignSubjectsToGroup(groupId, subjectIds);
                MessageBox.Show("Subjects assigned to group successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        else
        {
            MessageBox.Show("Invalid group ID.");
        }
    }

    private void AssignLecturerToCourseButton_Click(object sender, EventArgs e)
    {
        string lecturerIdStr = UIManager.ShowPrompt("Enter lecturer ID:", "Assign Lecturer to Course");
        string courseIdStr = UIManager.ShowPrompt("Enter course ID:", "Assign Lecturer to Course");

        if (int.TryParse(lecturerIdStr, out int lecturerId) && int.TryParse(courseIdStr, out int courseId))
        {
            try
            {
                _adminManager.AssignLecturerToCourse(lecturerId, courseId);
                MessageBox.Show("Lecturer assigned to course successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        else
        {
            MessageBox.Show("Invalid input.");
        }
    }
}
