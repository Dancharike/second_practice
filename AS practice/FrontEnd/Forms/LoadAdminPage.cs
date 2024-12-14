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
    private readonly UIManager _uiManager;
    private readonly List<Button> _buttons = new List<Button>();
    private readonly List<DataGridView> _gridViews = new List<DataGridView>();
    private DataGridView _studentsGridView;
    private DataGridView _lecturersGridView;
    private DataGridView _coursesGridView;
    private DataGridView _groupsGridView;
    private DataGridView _lecturerCoursesGridView;
    private DataGridView _usersGridView;
    private DataGridView _rolesGridView;
    private DataGridView _groupCoursesGridView;
    private DataGridView _subjectsGridView;
    private DataGridView _coursesSubjectsGridView;
    
    public LoadAdminPage(AdminManager adminManager)
    {
        _adminManager = adminManager;
        _uiManager = new UIManager();
        InitializeComponents();
        LoadData();
    }

    private void InitializeComponents()
    {
        Text = "Admin Panel";
        Size = new Size(1920, 1080);
        BackColor = Color.Black;

        CreateButton("Add User", new Point(20, 20), AddUserButtonClick);
        CreateButton("Delete User", new Point(20, 60), DeleteUserButtonClick);
        CreateButton("Create Group", new Point(20, 100), CreateGroupButtonClick);
        CreateButton("Create Course", new Point(20, 140), CreateCourseButtonClick);
        CreateButton("Assign Student to Group", new Point(20, 180), AssignStudentToGroupButtonClick);
        CreateButton("Assign Student to Course", new Point(20, 220), AssignStudentToCourseButtonClick);
        CreateButton("Assign Subjects to Group", new Point(20, 260), AssignSubjectsToGroupButtonClick);
        CreateButton("Assign Lecturer to Course", new Point(20, 300), AssignLecturerToCourseButtonClick);

        CreateDataGridView(new Point(500, 0), ref _usersGridView); // 1
        CreateDataGridView(new Point(1150, 0), ref _lecturersGridView); // 2
        CreateDataGridView(new Point(500, 200), ref _studentsGridView); // 3
        CreateDataGridView(new Point(1150, 200), ref _rolesGridView); // 4
        CreateDataGridView(new Point(500, 400), ref _coursesGridView); // 5
        CreateDataGridView(new Point(1150, 400), ref _groupsGridView); // 6
        CreateDataGridView(new Point(500, 600), ref _groupCoursesGridView); // 7
        CreateDataGridView(new Point(1150, 600), ref _lecturerCoursesGridView); // 8
        CreateDataGridView(new Point(500, 800), ref _coursesSubjectsGridView); // 9
        CreateDataGridView(new Point(1150, 800), ref _subjectsGridView); // 10
        
        Controls.AddRange(_buttons.ToArray());
        Controls.AddRange(_gridViews.ToArray());
    }

    private void CreateButton(string text, Point location, EventHandler clickEvent)
    {
        var button = _uiManager.CreateButton(text, location, clickEvent);
        _buttons.Add(button);
    }

    private void CreateDataGridView(Point location, ref DataGridView gridView)
    {
        gridView = new DataGridView
        {
            Location = location,
            Size = new Size(600, 200),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        _gridViews.Add(gridView);
    }

    private void LoadData()
    {
        try
        {
            var adminData = _adminManager.GetAdminData();

            _studentsGridView.DataSource = (List<Student>)adminData[0];
            _groupsGridView.DataSource = (List<StudentGroup>)adminData[1];
            _coursesGridView.DataSource = (List<Course>)adminData[2];
            _lecturersGridView.DataSource = (List<Lecturer>)adminData[3];
            _lecturerCoursesGridView.DataSource = (List<LecturerCourse>)adminData[4];
            //_gradesGridView.DataSource = (List<Grade>)adminData[5]; // admin do not need to see grades for no reason
            _usersGridView.DataSource = (List<User>)adminData[5];
            _rolesGridView.DataSource = (List<UserRoles>)adminData[6];
            //_adminsGridView.DataSource = (List<Admin>)adminData[7]; // there is no any reason for what admin would need to see full list of admins, they can not even be deleted
            _groupCoursesGridView.DataSource = (List<GroupCourses>)adminData[7];
            _subjectsGridView.DataSource = (List<Subjects>)adminData[8];
            _coursesSubjectsGridView.DataSource = (List<CourseSubjects>)adminData[9];

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading data: {ex.Message}\nStack Trace: {ex.StackTrace}");
        }
    }
    
    private void AddUserButtonClick(object sender, EventArgs e)
    {
        string username = UIManager.ShowPrompt("Enter username:", "Add User");
        string password = UIManager.ShowPrompt("Enter password:", "Add User");
        string role = UIManager.ShowPrompt("Enter role (Admin/Lecturer/Student):", "Add User");
        int roleSpecificId = 0;

        if (role == "Admin")
        {
            roleSpecificId = int.Parse(UIManager.ShowPrompt("Enter Admin ID:", "Add User"));
        }
        else if (role == "Lecturer")
        {
            roleSpecificId = int.Parse(UIManager.ShowPrompt("Enter Lecturer ID:", "Add User"));
        }
        else if (role == "Student")
        {
            roleSpecificId = int.Parse(UIManager.ShowPrompt("Enter Student ID:", "Add User"));
        }

        try
        {
            _adminManager.AddUser(username, password, role, roleSpecificId);
            MessageBox.Show("User added successfully.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }

    private void DeleteUserButtonClick(object sender, EventArgs e)
    {
        string userIdStr = UIManager.ShowPrompt("Enter user ID to delete:", "Delete User");
        if (int.TryParse(userIdStr, out int userId))
        {
            _adminManager.DeleteUser(userId);
            LoadData();
            MessageBox.Show("User deleted successfully.");
        }
        else
        {
            MessageBox.Show("Invalid user ID.");
        }
    }

    private void CreateGroupButtonClick(object sender, EventArgs e)
    {
        string groupName = UIManager.ShowPrompt("Enter group name:", "Create Group");

        try
        {
            _adminManager.CreateGroup(groupName);
            LoadData();
            MessageBox.Show("Group created successfully.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }

    private void CreateCourseButtonClick(object sender, EventArgs e)
    {
        string courseName = UIManager.ShowPrompt("Enter course name:", "Create Course");

        try
        {
            _adminManager.CreateCourse(courseName);
            LoadData();
            MessageBox.Show("Course created successfully.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }

    private void AssignStudentToGroupButtonClick(object sender, EventArgs e)
    {
        string studentIdStr = UIManager.ShowPrompt("Enter student ID:", "Assign Student to Group");
        string groupIdStr = UIManager.ShowPrompt("Enter group ID:", "Assign Student to Group");

        if (int.TryParse(studentIdStr, out int studentId) && int.TryParse(groupIdStr, out int groupId))
        {
            try
            {
                _adminManager.AddStudentToGroup(studentId, groupId);
                LoadData();
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

    private void AssignStudentToCourseButtonClick(object sender, EventArgs e)
    {
        string studentIdStr = UIManager.ShowPrompt("Enter student ID:", "Assign Student to Course");
        string courseIdStr = UIManager.ShowPrompt("Enter course ID:", "Assign Student to Course");

        if (int.TryParse(studentIdStr, out int studentId) && int.TryParse(courseIdStr, out int courseId))
        {
            try
            {
                _adminManager.AddStudentToCourse(studentId, courseId);
                LoadData();
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

    private void AssignSubjectsToGroupButtonClick(object sender, EventArgs e)
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
                LoadData();
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

    private void AssignLecturerToCourseButtonClick(object sender, EventArgs e)
    {
        string lecturerIdStr = UIManager.ShowPrompt("Enter lecturer ID:", "Assign Lecturer to Course");
        string courseIdStr = UIManager.ShowPrompt("Enter course ID:", "Assign Lecturer to Course");

        if (int.TryParse(lecturerIdStr, out int lecturerId) && int.TryParse(courseIdStr, out int courseId))
        {
            try
            {
                _adminManager.AssignLecturerToCourse(lecturerId, courseId);
                LoadData();
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
