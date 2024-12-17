using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AS_practice.DataAccess;
using AS_practice.FrontEnd;
using AS_practice.Models;

namespace AS_practice
{
    public class LoadStudentPage : Form
    {
        private readonly StudentManager _studentManager;
        private readonly UIManager _uiManager;
        private readonly List<Button> _buttons = new List<Button>();
        private readonly List<Label> _labels = new List<Label>();
        private readonly List<DataGridView> _gridViews = new List<DataGridView>();
        private DataGridView _coursesSubjectsGridView;
        private DataGridView studentSubjectGradesGridView;

        public LoadStudentPage(StudentManager studentManager)
        {
            _studentManager = studentManager;
            _uiManager = new UIManager();
            InitializeComponents();
            LoadData();
        }
        
        private void InitializeComponents()
        {
            Text = "Student Panel";
            Size = new Size(1920, 1080);
            BackColor = Color.Black;
            
            CreateLabel("Course Subjects Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(500, 10));
            CreateDataGridView(new Point(500, 30), ref _coursesSubjectsGridView);
            //CreateLabel("Student Grades Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(1150, 10));
            //CreateDataGridView(new Point(1150, 10), ref studentSubjectGradesGridView);
            
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
                var studentData = _studentManager.GetStudentData();
                //var finalScores = _studentManager.GetStudentFinalScores(studentId);
                
                _coursesSubjectsGridView.DataSource = (List<CourseSubjects>)studentData[0];
                //studentSubjectGradesGridView.DataSource = (List<StudentSubjectGrade>)studentData[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
        }
    }
}