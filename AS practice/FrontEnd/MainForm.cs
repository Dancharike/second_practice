using System;
using System.Drawing; 
using System.Windows.Forms;
using AS_practice.DataAccess;

namespace AS_practice
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Text = "Academic System";
            Width = 1920;
            Height = 1080;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.Black; 
            
            LoadLoginStage();
        }

        private void LoadLoginStage()
        {
            Controls.Clear();
            
            var panel = new Panel
            {
                Size = new Size(Width - 400, Height - 300),
                Location = new Point(200, 150),
                BackColor = Color.Gray, 
                BorderStyle = BorderStyle.FixedSingle 
            };
            Controls.Add(panel);
            
            var label = new Label
            {
                Text = "Select Your Role:",
                AutoSize = true,
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point((panel.Width - 200) / 2, 20)
            };
            panel.Controls.Add(label);
            
            CreateRoleSection(panel, "Admin", 100);
            CreateRoleSection(panel, "Lecturer", 250);
            CreateRoleSection(panel, "Student", 400);
        }

        private void CreateRoleSection(Panel parent, string role, int yOffset)
        {
            var usernameLabel = new Label
            {
                Text = "Username:",
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point((parent.Width - 200) / 2, yOffset)
            };
            parent.Controls.Add(usernameLabel);
            
            var usernameField = new TextBox
            {
                Width = 200,
                Location = new Point((parent.Width - 200) / 2, yOffset + 25)
            };
            parent.Controls.Add(usernameField);
            
            var passwordLabel = new Label
            {
                Text = "Password:",
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point((parent.Width - 200) / 2, yOffset + 60)
            };
            parent.Controls.Add(passwordLabel);
            
            var passwordField = new TextBox
            {
                Width = 200,
                Location = new Point((parent.Width - 200) / 2, yOffset + 85),
                UseSystemPasswordChar = true 
            };
            parent.Controls.Add(passwordField);
            
            var roleButton = new Button
            {
                Text = role,
                AutoSize = true,
                Location = new Point((parent.Width - 100) / 2, yOffset + 130)
            };
            
            if (role == "Admin")
                roleButton.Click += (sender, e) => LoadAdminStage();
            else if (role == "Lecturer")
                roleButton.Click += (sender, e) => LoadLecturerStage();
            else if (role == "Student")
                roleButton.Click += (sender, e) => LoadStudentStage();

            parent.Controls.Add(roleButton);
        }

        private void LoadAdminStage()
        {
            Controls.Clear();
            var label = new Label
            {
                Text = "Welcome, Admin!",
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point((Width - 100) / 2, (Height - 30) / 2)
            };
            Controls.Add(label);

            var backButton = new Button
            {
                Text = "Back",
                AutoSize = true,
                Location = new Point((Width - 100) / 2, (Height + 30) / 2)
            };
            backButton.Click += (sender, e) => LoadLoginStage();
            Controls.Add(backButton);
        }

        private void LoadLecturerStage()
        {
            Controls.Clear();
            var label = new Label
            {
                Text = "Welcome, Lecturer!",
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point((Width - 100) / 2, (Height - 30) / 2)
            };
            Controls.Add(label);

            var backButton = new Button
            {
                Text = "Back",
                AutoSize = true,
                Location = new Point((Width - 100) / 2, (Height + 30) / 2)
            };
            backButton.Click += (sender, e) => LoadLoginStage();
            Controls.Add(backButton);
        }

        private void LoadStudentStage()
        {
            Controls.Clear();
            var label = new Label
            {
                Text = "Welcome, Student!",
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point((Width - 100) / 2, (Height - 30) / 2)
            };
            Controls.Add(label);

            var backButton = new Button
            {
                Text = "Back",
                AutoSize = true,
                Location = new Point((Width - 100) / 2, (Height + 30) / 2)
            };
            backButton.Click += (sender, e) => LoadLoginStage();
            Controls.Add(backButton);
        }
    }
}
