using MySql.Data.MySqlClient;

namespace AS_practice.DataAccess
{
    public abstract class DatabaseBase
    {
        protected readonly string ConnectionString;

        protected DatabaseBase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}