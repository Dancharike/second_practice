using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AS_practice.DataAccess;
using AS_practice.FrontEnd;

namespace AS_practice
{
    public class LoadStudentPage : Form
    {
        private readonly StudentManager _studentManager;
        private readonly UIManager _uiManager;
        private readonly List<Button> _buttons = new List<Button>();
        private readonly List<Label> _labels = new List<Label>();
        private readonly List<DataGridView> _gridViews = new List<DataGridView>();

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
            
        }
    }
}