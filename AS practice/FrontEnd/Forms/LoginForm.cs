/*
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AS_practice.DataAccess;
using AS_practice.Interface;
using Org.BouncyCastle.Asn1.Crmf;

namespace AS_practice.Forms
{
    public class LoginForm : Form
    {
        private IRole _role;
        private string _selectedRole;
        private readonly DatabaseManager _database;

        public LoginForm(DatabaseManager database)
        {
            _database = database;
        }
        
        public void LoadLoginStage()
        {
            Controls.Clear();
            var panel = new Panel
            {
                Size = new Size(Width - 1500, Height - 500),
                Location = new Point((Width - (Width - 1500)) / 2, (Height - (Height - 500)) / 2),
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

            CreateRoleButton(panel, new AdminRole(), 100);
            CreateRoleButton(panel, new LecturerRole(), 250);
            CreateRoleButton(panel, new StudentRole(), 400);
        }
        
        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            using (Pen pen = new Pen(Color.Gray, 5))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }
        
        private void CreateRoleButton(Panel parent, IRole role, int yOffset)
        {
            var roleButton = new Button
            {
                Text = role.RoleName,
                AutoSize = true,
                Location = new Point((parent.Width - 100) / 2, yOffset),
                ForeColor = Color.White
            };
            roleButton.Click += (sender, e) =>
            {
                _selectedRole = role.RoleName;
                ShowLoginForm(parent, role);
            };
            parent.Controls.Add(roleButton);
        }
        
        private void ShowLoginForm(Panel parent, IRole role)
        {
            _role = role;
            foreach (var control in parent.Controls.OfType<Button>())
            {
                control.Visible = false;
            }

            role.ShowLoginForm(parent, this);

            var oldLabel = parent.Controls.OfType<Label>()
                .FirstOrDefault(lbl => lbl.Text == "Select Your Role:");

            if (oldLabel != null)
            {
                oldLabel.Visible = false;
            }

            var dataLabel = new Label
            {
                Text = "Data Entering:",
                AutoSize = true,
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point((parent.Width - 200) / 2, 20)
            };
            parent.Controls.Add(dataLabel);
        }
        
        public void AddLoginControls(Panel parent, string role)
        {
            var descriptionLabel = new Label
            {
                Text = $"You have selected the {role} role. Please enter your credentials to log in.",
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point(50, 75)
            };
            parent.Controls.Add(descriptionLabel);
            
            var usernameLabel = new Label
            {
                Text = "Username:",
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point(50, 150)
            };
            parent.Controls.Add(usernameLabel);

            var usernameField = new TextBox
            {
                Width = 200,
                Location = new Point(120, 150)
            };
            parent.Controls.Add(usernameField);

            var passwordLabel = new Label
            {
                Text = "Password:",
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point(50, 200)
            };
            parent.Controls.Add(passwordLabel);

            var passwordField = new TextBox
            {
                Width = 200,
                Location = new Point(120, 200),
                UseSystemPasswordChar = true
            };
            parent.Controls.Add(passwordField);
            
            var loginButton = new Button
            {
                Text = "Login",
                AutoSize = true,
                Location = new Point((parent.Width - 100) / 2, 250),
                ForeColor = Color.White
            };
            parent.Controls.Add(loginButton);

            var backButton = new Button
            {
                Text = "Back",
                AutoSize = true,
                Location = new Point(245, 250),
                ForeColor = Color.White
            };
            parent.Controls.Add(backButton);

            backButton.Click += (sender, e) =>
            {
                LoadLoginStage();
            };

            loginButton.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(usernameField.Text) || string.IsNullOrWhiteSpace(passwordField.Text))
                {
                    MessageBox.Show("Please enter both username and password.");
                }
                else
                {
                    bool isValid = _database.ValidateUser(usernameField.Text, passwordField.Text,_selectedRole);
                    if (isValid)
                    {
                        MessageBox.Show($"{role} logged in.");
                        if (_selectedRole == "Admin")
                        {
                            // goto the admin page
                        }
                        else if (_selectedRole == "Lecturer")
                        {
                            // goto the lecturer page
                            
                        }
                        else if (_selectedRole == "Student")
                        {
                            // goto the student page
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid login or password.");
                    }
                }
            };
        }
    }
}
*/