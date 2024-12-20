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
    private readonly List<Label> _labels = new List<Label>();
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
    private DataGridView _lecturerSubjectsGridView;
    
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
        
        CreateButton("Add User", new Point(20, 400), AddUserButtonClick);
        CreateButton("Delete User", new Point(110, 400), DeleteUserButtonClick);
        CreateButton("Create Group", new Point(20, 440), CreateGroupButtonClick);
        CreateButton("Delete Group", new Point(110, 440), DeleteGroupButtonClick);
        CreateButton("Create Course", new Point(20, 480), CreateCourseButtonClick);
        CreateButton("Delete Course", new Point(110, 480), DeleteCourseButtonClick);
        CreateButton("Create Subject", new Point(20, 520), CreateSubjectButtonClick);
        CreateButton("Delete Subject", new Point(110, 520), DeleteSubjectButtonClick);
        CreateButton("Assign Student to Group", new Point(20, 560), AssignStudentToGroupButtonClick);
        CreateButton("Assign Group to Course", new Point(20, 600), AssignGroupToCourseButtonClick);
        CreateButton("Assign Subject to Lecturer", new Point(20, 640), AssignSubjectToLecturerButtonClick);
        CreateButton("Assign Lecturer to Course", new Point(20, 680), AssignLecturerToCourseButtonClick);
        
        CreateLabel("Users Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(500, 10));
        CreateDataGridView(new Point(500, 30), ref _usersGridView); // 1
        CreateLabel("Lecturers Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(1150, 10));
        CreateDataGridView(new Point(1150, 30), ref _lecturersGridView); // 2
        CreateLabel("Students Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(500, 210));
        CreateDataGridView(new Point(500, 230), ref _studentsGridView); // 3
        CreateLabel("Roles Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(1150, 210));
        CreateDataGridView(new Point(1150, 230), ref _rolesGridView); // 4
        CreateLabel("Courses Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(500, 410));
        CreateDataGridView(new Point(500, 430), ref _coursesGridView); // 5
        CreateLabel("Groups Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(1150, 410));
        CreateDataGridView(new Point(1150, 430), ref _groupsGridView); // 6
        CreateLabel("Groups Assigned to Courses Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(500, 610));
        CreateDataGridView(new Point(500, 630), ref _groupCoursesGridView); // 7
        CreateLabel("Lecturers Assigned to Courses Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(1150, 610));
        CreateDataGridView(new Point(1150, 630), ref _lecturerCoursesGridView); // 8
        CreateLabel("Lecturer Subjects Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(500, 810));
        CreateDataGridView(new Point(500, 830), ref _lecturerSubjectsGridView); // 9
        CreateLabel("Subjects Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(1150, 810));
        CreateDataGridView(new Point(1150, 830), ref _subjectsGridView); // 10
        
        Controls.AddRange(_buttons.ToArray());
        Controls.AddRange(_labels.ToArray());
        Controls.AddRange(_gridViews.ToArray());
    }

    private void CreateButton(string text, Point location, EventHandler clickEvent)
    {
        var button = _uiManager.CreateButton(text, location, clickEvent);
        _buttons.Add(button);
    }

    private void CreateLabel(string text, Font font, Color color, Point location)
    {
        var label = _uiManager.CreateLabel(text, font, color, location);
        _labels.Add(label);
    }

    private void CreateDataGridView(Point location, ref DataGridView gridView)
    {
        gridView = new DataGridView
        {
            Location = location,
            Size = new Size(600, 170),
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
            //_adminsGridView.DataSource = (List<Admin>)adminData[7]; // there is no any reason for what admin would need to see full list of admins
            _groupCoursesGridView.DataSource = (List<GroupCourses>)adminData[7];
            _subjectsGridView.DataSource = (List<Subjects>)adminData[8];
            _lecturerSubjectsGridView.DataSource = (List<LecturerSubjects>)adminData[9];
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
        string role = UIManager.ShowPrompt("Enter RoleName:", "Add User");
        
        _adminManager.AddUser(username, password, role);
        LoadData();
        MessageBox.Show("User added successfully.");
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
        
        _adminManager.CreateGroup(groupName);
        LoadData();
        MessageBox.Show("Group created successfully.");
    }
    
    private void DeleteGroupButtonClick(object sender, EventArgs e)
    {
        string groupIdStr = UIManager.ShowPrompt("Enter Group ID:", "Delete Group");
        if (int.TryParse(groupIdStr, out int groupId))
        {
            _adminManager.DeleteGroup(groupId);
            LoadData();
            MessageBox.Show("Group deleted successfully.");
        }
    }

    private void CreateCourseButtonClick(object sender, EventArgs e)
    {
        string courseName = UIManager.ShowPrompt("Enter course name:", "Create Course");
        
        _adminManager.CreateCourse(courseName);
        LoadData();
        MessageBox.Show("Course created successfully.");
    }
    
    private void DeleteCourseButtonClick(object sender, EventArgs e)
    {
        string courseIdStr = UIManager.ShowPrompt("Enter Course ID:", "Delete Course");
        if (int.TryParse(courseIdStr, out int courseId))
        {
            _adminManager.DeleteCourse(courseId);
            LoadData();
            MessageBox.Show("Course deleted successfully.");
        }
    }
    
    private void CreateSubjectButtonClick(object sender, EventArgs e)
    {
        string subjectName = UIManager.ShowPrompt("Enter Subject Name:", "Create Subject");
        if (!string.IsNullOrEmpty(subjectName))
        {
            _adminManager.CreateSubject(subjectName);
            LoadData();
            MessageBox.Show("Subject created successfully.");
        }
    }

    private void DeleteSubjectButtonClick(object sender, EventArgs e)
    {
        string subjectIdStr = UIManager.ShowPrompt("Enter Subject ID:", "Delete Subject");
        if (int.TryParse(subjectIdStr, out int subjectId))
        {
            _adminManager.DeleteSubject(subjectId);
            LoadData();
            MessageBox.Show("Subject deleted successfully.");
        }
    }
    
    private void AssignStudentToGroupButtonClick(object sender, EventArgs e)
    {
        string studentIdStr = UIManager.ShowPrompt("Enter student ID:", "Assign Student to Group");
        string groupIdStr = UIManager.ShowPrompt("Enter group ID:", "Assign Student to Group");

        if (int.TryParse(studentIdStr, out int studentId) && int.TryParse(groupIdStr, out int groupId))
        {
            _adminManager.AddStudentToGroup(studentId, groupId);
            LoadData();
            MessageBox.Show("Student assigned to group successfully.");
        }
        else
        {
            MessageBox.Show("Invalid input.");
        }
    }

    private void AssignGroupToCourseButtonClick(object sender, EventArgs e)
    {
        string courseIdStr = UIManager.ShowPrompt("Enter course ID:", "Assign Group to Course");
        string groupIdStr = UIManager.ShowPrompt("Enter group ID:", "Assign Group to Course");

        if (int.TryParse(courseIdStr, out int studentId) && int.TryParse(groupIdStr, out int groupId))
        {
            _adminManager.AssignGroupToCourse(groupId, studentId);
            LoadData();
            MessageBox.Show("Group assigned to course successfully.");
        }
        else
        {
            MessageBox.Show("Invalid input.");
        }
    }
    
    private void AssignSubjectToLecturerButtonClick(object sender, EventArgs e)
    {
        string lecturerIdStr = UIManager.ShowPrompt("Enter lecturer ID:", "Assign Subject to Lecturer");
        string subjectIdStr = UIManager.ShowPrompt("Enter subject ID:", "Assign Subject to Lecturer");

        if (int.TryParse(lecturerIdStr, out int lecturerId) && int.TryParse(subjectIdStr, out int subjectId))
        {
            try
            {
                _adminManager.AssignSubjectToLecturer(lecturerId, subjectId);
                LoadData();
                MessageBox.Show("Subject successfully assigned to the lecturer.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        else
        {
            MessageBox.Show("Invalid input. Please enter valid IDs.");
        }
    }
    
    private void AssignLecturerToCourseButtonClick(object sender, EventArgs e)
    {
        string lecturerIdStr = UIManager.ShowPrompt("Enter lecturer ID:", "Assign Lecturer to Course");
        string courseIdStr = UIManager.ShowPrompt("Enter course ID:", "Assign Lecturer to Course");

        if (int.TryParse(lecturerIdStr, out int lecturerId) && int.TryParse(courseIdStr, out int courseId))
        {
            _adminManager.AssignLecturerToCourse(lecturerId, courseId);
            LoadData();
            MessageBox.Show("Lecturer assigned to course successfully.");
        }
        else
        {
            MessageBox.Show("Invalid input.");
        }
    }
}
