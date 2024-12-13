using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AS_practice.Interface;

namespace AS_practice.FrontEnd
{
    public class RoleSelectionStage : IStageManager
    {
        private readonly IEnumerable<IRole> _roles;
        private readonly UIManager _ui;

        public RoleSelectionStage(IEnumerable<IRole> roles, UIManager ui)
        {
            _roles = roles;
            _ui = ui;
        }

        public void LoadStage(MainForm mainForm, Panel parentPanel, object context = null)
        {
            parentPanel.Controls.Clear();
            
            var label = _ui.CreateLabel(
                "Select Your Role:",
                new Font("Arial", 16, FontStyle.Bold),
                Color.White,
                new Point((parentPanel.Width - 200) / 2, 20)
            );
            parentPanel.Controls.Add(label);
            
            int yOffset = 100;
            foreach (var role in _roles)
            {
                var roleButton = _ui.CreateButton(
                    role.RoleName,
                    new Point((parentPanel.Width - 100) / 2, yOffset),
                    (sender, e) =>
                    {
                        context = role;
                        role.ShowLoginForm(parentPanel, mainForm);
                    }
                );
                parentPanel.Controls.Add(roleButton);
                yOffset += 150;
            }
        }
    }
}