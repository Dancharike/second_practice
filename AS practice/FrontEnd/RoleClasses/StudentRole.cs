using System.Windows.Forms;
using AS_practice.FrontEnd;
using AS_practice.Interface;

namespace AS_practice
{
    public class StudentRole : IRole
    {
        public string RoleName => "Student";

        public void ShowLoginForm(Panel parent, MainForm mainForm)
        {
            //var loginStage = new LoginStage(new UIManager(), this);
            //loginStage.LoadStage(mainForm, parent, this);
            mainForm.AddLoginControls(parent);
        }
    }
}