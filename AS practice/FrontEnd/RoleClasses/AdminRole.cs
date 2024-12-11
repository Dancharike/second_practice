using System.Windows.Forms;
using AS_practice.Interface;

namespace AS_practice
{
    public class AdminRole : IRole
    {
        public string RoleName => "Admin";

        public void ShowLoginForm(Panel parent, MainForm mainForm)
        {
            mainForm.AddLoginControls(parent, "Admin");
        }
    }
}