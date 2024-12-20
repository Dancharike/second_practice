using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AS_practice.DataAccess;
using AS_practice.FrontEnd;

namespace AS_practice
{
    public class LoadStudentPage : Form
    {
        private readonly StudentManager _studentManager;
        private readonly int? _studentId;
        private readonly UIManager _uiManager;
        private readonly List<Label> _labels = new List<Label>();
        private readonly List<DataGridView> _gridViews = new List<DataGridView>();
        private DataGridView _studentCourseDataGridView;

        public LoadStudentPage(StudentManager studentManager, int? studentId)
        {
            _studentManager = studentManager;
            _studentId = studentId;
            _uiManager = new UIManager();
            InitializeComponents();
            LoadData();
        }

        private void InitializeComponents()
        {
            Text = "Student Panel";
            Size = new Size(1920, 1080);
            BackColor = Color.Black;
            
            CreateLabel("Student Grades Table", new Font("Arial", 12, FontStyle.Bold), Color.White, new Point(500, 310));
            CreateDataGridView(new Point(500, 330), ref _studentCourseDataGridView);
            
            Controls.AddRange(_labels.ToArray());
            Controls.AddRange(_gridViews.ToArray());
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
                var studentGrades = _studentManager.GetStudentData(_studentId);

                if (studentGrades.Count > 0)
                {
                    var dataTable = new DataTable();
                    dataTable.Columns.Add("Student Name");
                    dataTable.Columns.Add("Subject Name");
                    dataTable.Columns.Add("Total Grade");

                    foreach (var grade in studentGrades)
                    {
                        dataTable.Rows.Add(grade.StudentName, grade.SubjectName, grade.TotalGrade);
                    }

                    _studentCourseDataGridView.DataSource = dataTable;
                }
                else
                {
                    MessageBox.Show("No intel.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in loading data: {ex.Message}");
            }
        }
    }
}