using System;
using System.Windows.Forms;
using AS_practice.DataAccess;

namespace AS_practice
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            string connectionString = "Server=mysql.localhost;Port=3305;Database=AS;Uid=root;Pwd=кщще;";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(connectionString));
            
        }
    }
}
