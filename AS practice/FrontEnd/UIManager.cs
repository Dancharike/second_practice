using System;
using System.Drawing;
using System.Windows.Forms;

namespace AS_practice.FrontEnd
{
    public class UIManager
    {
        public Panel CreatePanel(Size size, Point location, Color color)
        {
            return new Panel()
            {
                Size = size,
                Location = location,
                BackColor = color,
                BorderStyle = BorderStyle.None,
            };
        }

        public Label CreateLabel(string text, Font font, Color color, Point location)
        {
            return new Label()
            {
                Text = text,
                AutoSize = true,
                Font = font,
                ForeColor = color,
                Location = location
            };
        }

        public Button CreateButton(string text, Point location, EventHandler clickEvent)
        {
            var button = new Button()
            {
                Text = text,
                AutoSize = true,
                Location = location,
                ForeColor = Color.White
            };
            button.Click += clickEvent;
            return button;
        }

        public static string ShowPrompt(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                Text = caption
            };
    
            Label label = new Label() { Left = 50, Top = 20, Text = text };
            TextBox inputBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button okButton = new Button() { Text = "OK", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };

            prompt.Controls.Add(label);
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(okButton);
            prompt.AcceptButton = okButton;

            return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : string.Empty;
        }
    }
}