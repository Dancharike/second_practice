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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string connectionString = "Server=mysql.localhost;Port=3305;Database=AS;Uid=root;Pwd=кщще;";
            var dbManager = new DatabaseManager(connectionString);

            Application.Run(new MainForm(dbManager));
        }
    }
}