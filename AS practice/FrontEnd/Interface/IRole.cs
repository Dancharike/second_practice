using System.Windows.Forms;

namespace AS_practice.Interface
{
    public interface IRole
    {
        void ShowLoginForm(Panel parent, MainForm mainForm);
        string RoleName { get; }
    }
}