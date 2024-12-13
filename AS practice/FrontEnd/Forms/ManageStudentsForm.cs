using System;
using System.Windows.Forms;
using AS_practice.DataAccess;

namespace AS_practice
{
    public class ManageStudentsForm : Form
    {
        private readonly DatabaseManager _dbManager;

        public ManageStudentsForm(DatabaseManager dbManager)
        {
            InitializeComponent();
            _dbManager = dbManager;
        }

        private void InitializeComponent()
        {
            Text = "Manage Students";
            Width = 600;
            Height = 400;

            var label = new Label
            {
                Text = "Student Management Module",
                Top = 50,
                Left = 100,
                AutoSize = true
            };
            Controls.Add(label);
        }
    }
}