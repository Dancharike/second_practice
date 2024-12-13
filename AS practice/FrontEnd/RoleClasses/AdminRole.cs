using System.Windows.Forms;
using AS_practice.FrontEnd;
using AS_practice.Interface;

namespace AS_practice
{
    public class AdminRole : IRole
    {
        public string RoleName => "Admin";

        public void ShowLoginForm(Panel parent, MainForm mainForm)
        {
            //var loginStage = new LoginStage(new UIManager(), this);
            //loginStage.LoadStage(mainForm, parent, this); // login stage load
            mainForm.AddLoginControls(parent, "Admin");
        }
    }
}