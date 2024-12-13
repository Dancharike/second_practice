using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AS_practice;
using AS_practice.DataAccess;
using AS_practice.FrontEnd;
using AS_practice.Interface;

public class MainForm : Form
{
    private IRole _role;
    private readonly UIManager _uiManager;
    private string _selectedRole;
    private DatabaseManager _database;

    public MainForm(string connectionString)
    {
        InitializeComponent();
        _database = new DatabaseManager(connectionString);
        _uiManager = new UIManager();
        Text = "Academic System";
        Width = 1920;
        Height = 1080;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.Black;
        LoadRoleSelectionStage();
    }

    private void LoadRoleSelectionStage()
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
        
        var label = _uiManager.CreateLabel(
            "Select Your Role:",
            new Font("Arial", 16, FontStyle.Bold),
            Color.White,
            new Point((panel.Width - 200) / 2, 20)
        );
        panel.Controls.Add(label);
        
        CreateRoleButton(panel, new AdminRole(), 100);
        CreateRoleButton(panel, new LecturerRole(), 250);
        CreateRoleButton(panel, new StudentRole(), 400);
    }

    private void Panel_Paint(object sender, PaintEventArgs e)
    {
        var panel = sender as Panel;
        using (Pen pen = new Pen(Color.Gray, 1))
        {
            e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
        }
    }

    private void CreateRoleButton(Panel parent, IRole role, int yOffset)
    {
        var roleButton = _uiManager.CreateButton(
            role.RoleName,
            new Point((parent.Width - 100) / 2, yOffset),
            (sender, e) =>
            {
                _selectedRole = role.RoleName;
                ShowLoginForm(parent, role);
            }
        );
        parent.Controls.Add(roleButton);
    }
    
    private void ShowLoginForm(Panel parent, IRole role)
    {
        _role = role;
        foreach (var control in parent.Controls.OfType<Button>())
        {
            control.Visible = false;
        }
        
        var oldLabel = parent.Controls.OfType<Label>().FirstOrDefault(lbl => lbl.Text == "Select Your Role:");
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
        AddLoginControls(parent, role.RoleName);
    }
    
    public void AddLoginControls(Panel parent, string role)
    {
        var descriptionLabel = _uiManager.CreateLabel(
            $"You have entered as a {role}. Please enter your credentials",
            new Font("Arial", 10),
            Color.White,
            new Point(15, 75)
        );
        parent.Controls.Add(descriptionLabel);

        // username
        var usernameLabel = _uiManager.CreateLabel("Username:", new Font("Arial", 10), Color.White, new Point(40, 150));
        parent.Controls.Add(usernameLabel);

        var usernameField = new TextBox
        {
            Width = 200,
            Location = new Point(120, 148)
        };
        parent.Controls.Add(usernameField);

        // password
        var passwordLabel = _uiManager.CreateLabel("Password:", new Font("Arial", 10), Color.White, new Point(40, 200));
        parent.Controls.Add(passwordLabel);

        var passwordField = new TextBox
        {
            Width = 200,
            Location = new Point(120, 198),
            UseSystemPasswordChar = true
        };
        parent.Controls.Add(passwordField);
        
        var loginButton = _uiManager.CreateButton(
            "Login",
            new Point((parent.Width - 100) / 2, 250),
            (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(usernameField.Text) || string.IsNullOrWhiteSpace(passwordField.Text))
                {
                    MessageBox.Show("Please enter both username and password.");
                }
                else
                {
                    bool isValid = _database.ValidateUser(usernameField.Text, passwordField.Text, _selectedRole);
                    if (isValid)
                    {
                        MessageBox.Show($"{_selectedRole} logged in.");
                        if (_selectedRole == "Admin")
                        {
                            // loadAdminPage();
                        }
                        else if (_selectedRole == "Lecturer")
                        {
                            // loadLecturerPage();
                        }
                        else if (_selectedRole == "Student")
                        {
                            // loadStudentPage();
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are trying to log in as a wrong role!");
                    }
                }
            }
        );
        parent.Controls.Add(loginButton);
        
        var backButton = _uiManager.CreateButton(
            "Back",
            new Point(245, 250),
            (sender, e) => { LoadRoleSelectionStage(); }
        );
        parent.Controls.Add(backButton);
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.SuspendLayout();
        // 
        // MainForm
        // 
        this.ClientSize = new System.Drawing.Size(286, 261);
        this.Name = "MainForm";
        this.ResumeLayout(false);
    }
}