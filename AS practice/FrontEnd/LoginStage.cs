using System.Drawing;
using System.Windows.Forms;
using AS_practice.Interface;

namespace AS_practice.FrontEnd
{
    public class LoginStage : IStageManager
    {
        private readonly UIManager _ui;
        private readonly IRole _role;
        
        public LoginStage(UIManager uiManager, IRole role)
        {
            _ui = uiManager;
            _role = role;
        }
        
        public void LoadStage(MainForm mainForm, Panel parentPanel, object context = null)
        {
            parentPanel.Controls.Clear();

            // adds description when user chooses one of the roles
            var descriptionLabel = _ui.CreateLabel($"You have selected the {_role.RoleName} role. Please enter your credentials to log in.", 
                new Font("Arial", 12, FontStyle.Regular), 
                Color.White, 
                new Point(50, 75));
            parentPanel.Controls.Add(descriptionLabel);

            // add input fields
            var usernameLabel = _ui.CreateLabel("Username:", new Font("Arial", 12), Color.White, new Point(50, 150));
            var usernameField = new TextBox { Width = 200, Location = new Point(120, 150) };
            parentPanel.Controls.Add(usernameLabel);
            parentPanel.Controls.Add(usernameField);

            var passwordLabel = _ui.CreateLabel("Password:", new Font("Arial", 12), Color.White, new Point(50, 200));
            var passwordField = new TextBox { Width = 200, Location = new Point(120, 200), UseSystemPasswordChar = true };
            parentPanel.Controls.Add(passwordLabel);
            parentPanel.Controls.Add(passwordField);
            
            // buttons
            var loginButton = _ui.CreateButton("Login", new Point((parentPanel.Width - 100) / 2, 250), 
                (sender, e) =>
                {
                    if (string.IsNullOrWhiteSpace(usernameField.Text) || string.IsNullOrWhiteSpace(passwordField.Text))
                    {
                        MessageBox.Show("Please enter both username and password.");
                    }
                    else
                    {
                        MessageBox.Show($"{_role.RoleName} logged in.");
                        if (_role.RoleName == "Admin")
                        {
                            // goto the admin page
                        }
                        else if (_role.RoleName == "Lecturer")
                        {
                            // goto the lecturer page
                        
                        }
                        else if (_role.RoleName == "Student")
                        {
                            // goto the student page
                        }
                    }
                });
            
            var backButton = _ui.CreateButton("Back", new Point(245, 250), 
                (sender, e) => mainForm.Controls.Clear());

            parentPanel.Controls.Add(loginButton);
            parentPanel.Controls.Add(backButton);
        }
    }
}