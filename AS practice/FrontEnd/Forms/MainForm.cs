using System.Drawing;
using System.Windows.Forms;
using AS_practice;
using AS_practice.DataAccess;
using AS_practice.FrontEnd;

public class MainForm : Form
{
    private readonly UIManager _uiManager;
    private readonly DatabaseManager _database;
    private readonly AdminManager _admin;
    private readonly LecturerManager _lecturer;
    private readonly StudentManager _student;

    public MainForm(string connectionString)
    {
        InitializeComponent();
        _database = new DatabaseManager(connectionString);
        _uiManager = new UIManager();
        _admin = new AdminManager(connectionString);
        _lecturer = new LecturerManager(connectionString);
        _student = new StudentManager(connectionString);
        Text = "Academic System";
        Width = 1920;
        Height = 1080;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.Black;
        ShowLoginForm();
    }
    
    private void Panel_Paint(object sender, PaintEventArgs e)
    {
        var panel = sender as Panel;
        using (Pen pen = new Pen(Color.White, 1))
        {
            e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
        }
    }
    
    private void ShowLoginForm()
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

        var dataLabel = new Label
        {
            Text = "Data Entering:",
            AutoSize = true,
            Font = new Font("Arial", 16, FontStyle.Bold),
            ForeColor = Color.White,
            Location = new Point((panel.Width - 200) / 2, 20)
        };
        panel.Controls.Add(dataLabel);

        AddLoginControls(panel);
    }
    
    public void AddLoginControls(Panel parent)
    {
        var descriptionLabel = _uiManager.CreateLabel($"Please enter your credentials", new Font("Arial", 10), Color.White, new Point(15, 75));
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
        
        var loginButton = _uiManager.CreateButton("Login", new Point((parent.Width - 100) / 2, 250), (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(usernameField.Text) || string.IsNullOrWhiteSpace(passwordField.Text))
                {
                    MessageBox.Show("Please enter both username and password.");
                }
                else
                {
                    var (isValid, role, roleSpecificId) = _database.ValidateUser(usernameField.Text, passwordField.Text);
                    if (isValid)
                    {
                        MessageBox.Show($"{role} logged in.");
                        switch (role)
                        {
                            case "Admin":
                                LoadAdminPage adminPage = new LoadAdminPage(_admin);
                                adminPage.Show();
                                break;
                            case "Lecturer":
                                LoadLecturerPage lecturerPage = new LoadLecturerPage(_lecturer, roleSpecificId);
                                lecturerPage.Show();
                                break;
                            case "Student":
                                LoadStudentPage studentPage = new LoadStudentPage(_student, roleSpecificId);
                                studentPage.Show();
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid credentials!");
                    }
                }
            }
        );
        parent.Controls.Add(loginButton);
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