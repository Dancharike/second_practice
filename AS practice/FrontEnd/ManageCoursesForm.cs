using System;
using System.Windows.Forms;
using AS_practice.DataAccess;

namespace AS_practice
{
    public class ManageCoursesForm : Form
    {
        private readonly DatabaseManager _dbManager;

        public ManageCoursesForm(DatabaseManager dbManager)
        {
            InitializeComponent();
            _dbManager = dbManager;
        }

        private void InitializeComponent()
        {
            Text = "Manage Courses";
            Width = 600;
            Height = 400;

            var label = new Label
            {
                Text = "Course Management Module",
                Top = 50,
                Left = 100,
                AutoSize = true
            };
            Controls.Add(label);
        }
    }
}