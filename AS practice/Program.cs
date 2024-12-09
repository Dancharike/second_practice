using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AS_practice.DataAccess;

namespace AS_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=mysql.localhost;Port=3305;Database=AS;Uid=root;Pwd=кщще;";
            var dbManager = new DatabaseManager(connectionString);
            
            dbManager.TestConnection();
            Console.WriteLine("System is prepared for work");
        }
    }
}