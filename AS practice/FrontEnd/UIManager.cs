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
    }
}