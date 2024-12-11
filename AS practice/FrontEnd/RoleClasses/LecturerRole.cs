using System.Windows.Forms;
using AS_practice.Interface;

namespace AS_practice
{
    public class LecturerRole : IRole
    {
        public string RoleName => "Lecturer";

        public void ShowLoginForm(Panel parent, MainForm mainForm)
        {
            mainForm.AddLoginControls(parent, "Lecturer");
        }
    }
}