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
                Size = new Size(Width - 1500, Height - 500),
                Location = new Point((Width - (Width - 1500)) / 2, (Height - (Height - 500)) / 2), // centers panel in the middle of the screen 
                BackColor = Color.Black,
                BorderStyle = BorderStyle.None
            };
            panel.Paint += Panel_Paint; 
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

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            using (Pen pen = new Pen(Color.Gray, 5)) 
            {
                e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }

        private void CreateRoleSection(Panel parent, string role, int yOffset)
        {
            var usernameLabel = new Label
            {
                Text = "Username:",
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point(50, yOffset)
            };
            parent.Controls.Add(usernameLabel);
            
            var usernameField = new TextBox
            {
                Width = 200,
                Location = new Point(120, yOffset)
            };
            parent.Controls.Add(usernameField);
            
            var passwordLabel = new Label
            {
                Text = "Password:",
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point(50, yOffset + 60)
            };
            parent.Controls.Add(passwordLabel);
            
            var passwordField = new TextBox
            {
                Width = 200,
                Location = new Point(120, yOffset + 60),
                UseSystemPasswordChar = true
            };
            parent.Controls.Add(passwordField);
            
            var roleButton = new Button
            {
                Text = role,
                AutoSize = true,
                Location = new Point((parent.Width - 100) / 2, yOffset + 100),
                ForeColor = Color.White // Белый цвет текста на кнопке
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
                Location = new Point((Width - 100) / 2, (Height + 30) / 2),
                ForeColor = Color.White // Белый цвет текста на кнопке
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
                Location = new Point((Width - 100) / 2, (Height + 30) / 2),
                ForeColor = Color.White // Белый цвет текста на кнопке
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
                Location = new Point((Width - 100) / 2, (Height + 30) / 2),
                ForeColor = Color.White
            };
            backButton.Click += (sender, e) => LoadLoginStage();
            Controls.Add(backButton);
        }
    }
}
