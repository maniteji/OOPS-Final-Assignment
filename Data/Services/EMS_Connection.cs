using MySqlConnector;

namespace EmployeeManagementSystem.Data.Services
{
    public class EMS_Connection
    {
        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            UserID = "root",
            Password = "mysql",
            Database = "employee_management_system",
        };

        private MySqlConnection _connection;
        private static EMS_Connection _instance;

        public static EMS_Connection Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EMS_Connection();
                }
                return _instance;
            }
        }

        private EMS_Connection()
        {
        }
        
        public MySqlConnectionStringBuilder Builder
        {
            get
            {
                if (builder == null)
                {
                    builder = new MySqlConnectionStringBuilder
                    {
                        Server = "localhost",
                        UserID = "root",
                        Password = "mysql",
                        Database = "employee_management_system",
                    };
                }
                return builder;
            }
        }

        public MySqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new MySqlConnection(builder.ConnectionString);
                    _connection.Open();
                }
                return _connection;
            }
        }
    }
}