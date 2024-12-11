using System.Windows.Forms;
using AS_practice.Interface;

namespace AS_practice
{
    public class StudentRole : IRole
    {
        public string RoleName => "Student";

        public void ShowLoginForm(Panel parent, MainForm mainForm)
        {
            mainForm.AddLoginControls(parent, "Student");
        }
    }
}