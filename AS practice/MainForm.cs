using System;
using System.Windows.Forms;
using AS_practice.DataAccess;

namespace AS_practice
{
    public class MainForm : Form
    {
        private readonly DatabaseManager _dbManager;

        public MainForm(DatabaseManager dbManager)
        {
            InitializeComponent();
            _dbManager = dbManager;
        }

        private void InitializeComponent()
        {
            Text = "Academic System";
            Width = 800;
            Height = 600;

            var btnManageStudents = new Button
            {
                Text = "Manage Students",
                Top = 50,
                Left = 100,
                Width = 200
            };
            btnManageStudents.Click += BtnManageStudents_Click;
            Controls.Add(btnManageStudents);

            var btnManageCourses = new Button
            {
                Text = "Manage Courses",
                Top = 100,
                Left = 100,
                Width = 200
            };
            btnManageCourses.Click += BtnManageCourses_Click;
            Controls.Add(btnManageCourses);
        }

        private void BtnManageStudents_Click(object sender, EventArgs e)
        {
            var studentForm = new ManageStudentsForm(_dbManager);
            studentForm.ShowDialog();
        }

        private void BtnManageCourses_Click(object sender, EventArgs e)
        {
            var courseForm = new ManageCoursesForm(_dbManager);
            courseForm.ShowDialog();
        }
    }
}