using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AS_practice.DataAccess;
using AS_practice.FrontEnd;
using AS_practice.Models;

namespace AS_practice
{
    public class LoadLecturerPage : Form
    {
        private readonly LecturerManager _lecturerManager;
        private readonly UIManager _uiManager;
        private readonly List<Button> _buttons = new List<Button>();
        private readonly List<Label> _labels = new List<Label>();
        private readonly List<DataGridView> _gridViews = new List<DataGridView>();
        private DataGridView _studentGridView;
        private DataGridView _gradeGridView;
        private DataGridView _subjectsGridView;

        public LoadLecturerPage(LecturerManager lecturerManager)
        {
            _lecturerManager = lecturerManager;
            _uiManager = new UIManager();
            InitializeComponents();
            LoadData();
        }

        private void InitializeComponents()
        {
            Text = "Lecturer Panel";
            Size = new Size(1920, 1080);
            BackColor = Color.Black;
            
            CreateButton("Add Grade", new Point(20, 400), AddGradeButtonClick);
            CreateButton("Edit Grade", new Point(20, 440), EditGradeButtonClick);

            CreateLabel("Students Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(500, 10));
            CreateDataGridView(new Point(500, 30), ref _studentGridView);
            CreateLabel("Grades Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(1150, 10));
            CreateDataGridView(new Point(1150, 30), ref _gradeGridView);
            CreateLabel("Subjects Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(500, 210));
            CreateDataGridView(new Point(500, 230), ref _subjectsGridView);
            
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
                var lecturerData = _lecturerManager.GetLecturerData();
                
                _studentGridView.DataSource = (List<Student>)lecturerData[0];
                _gradeGridView.DataSource = (List<Grade>)lecturerData[1];
                _subjectsGridView.DataSource = (List<Subjects>)lecturerData[2];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AddGradeButtonClick(object sender, EventArgs e)
        {
            string studentIdStr = UIManager.ShowPrompt("Enter Student ID:", "Add Grade");
            string subjectIdStr = UIManager.ShowPrompt("Enter Subject ID:", "Add Grade");
            string gradeValueStr = UIManager.ShowPrompt("Enter Grade Value:", "Add Grade");

            if (int.TryParse(studentIdStr, out int studentId) &&
                int.TryParse(subjectIdStr, out int subjectId) &&
                int.TryParse(gradeValueStr, out int grade))
            {
                _lecturerManager.AddGrade(studentId, subjectId, grade);
                LoadData();
                MessageBox.Show("Grade added successfully.");
            }
            else
            {
                MessageBox.Show("Invalid input.");
            }
        }

        private void EditGradeButtonClick(object sender, EventArgs e)
        {
            string studentIdStr = UIManager.ShowPrompt("Enter Student ID:", "Edit Grade");
            string subjectIdStr = UIManager.ShowPrompt("Enter Subject ID:", "Edit Grade");
            string newGradeValueStr = UIManager.ShowPrompt("Enter New Grade Value:", "Edit Grade");

            if (int.TryParse(studentIdStr, out int studentId) &&
                int.TryParse(subjectIdStr, out int subjectId) &&
                int.TryParse(newGradeValueStr, out int grade))
            {
                _lecturerManager.EditGrade(studentId, subjectId, grade);
                LoadData();
                MessageBox.Show("Grade updated successfully.");
            }
            else
            {
                MessageBox.Show("Invalid input.");
            }
        }
    }
}