using MySqlConnector;
using System.Data.Common;
using EmployeeManagementSystem.Data.Models;

namespace EmployeeManagementSystem.Data.Services
{
    public class EmployeeService
    {
        private readonly MySqlConnection _con;
        private static EmployeeService _instance;
        public static EmployeeService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EmployeeService();
                }
                return _instance;
            }
        }

        public EmployeeService()
        {
            _con = EMS_Connection.Instance.Connection;
        }

        public async Task<List<Employee>> GetEmployeeAsync()
        {
            List<Employee> employees = new List<Employee>();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM employees", _con);
            using (DbDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var employee = new Employee
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2),
                        Phone = reader.GetString(3),
                        Address = reader.GetString(4),
                        Designation = reader.GetString(5),
                        Department = reader.GetString(6)
                    };

                    employees.Add(employee);
                }
            }
            return employees;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM employees WHERE Id = @Id", _con);
            cmd.Parameters.AddWithValue("@Id", id);

            Employee employee = null;

            try
            {
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        employee = new Employee
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Phone = reader.GetString(3),
                            Address = reader.GetString(4),
                            Designation = reader.GetString(5),
                            Department = reader.GetString(6)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return employee;
        }

        public async Task<bool> AddEmployeeAsync(Employee employee)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO employees (name, email, phone, address, department, designation) " +
                "VALUES(@name, @email, @phone, @address, @department, @designation)", _con);
            cmd.Parameters.AddWithValue("@name", employee.Name);
            cmd.Parameters.AddWithValue("@email", employee.Email);
            cmd.Parameters.AddWithValue("@phone", employee.Phone);
            cmd.Parameters.AddWithValue("@address", employee.Address);
            cmd.Parameters.AddWithValue("@department", employee.Department);
            cmd.Parameters.AddWithValue("@designation", employee.Designation);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE employees SET name = @name, designation = @designation, address = @address," +
                " department = @department, email = @email, phone = @phone WHERE Id = @Id", _con);
            cmd.Parameters.AddWithValue("@name", employee.Name);
            cmd.Parameters.AddWithValue("@email", employee.Email);
            cmd.Parameters.AddWithValue("@phone", employee.Phone);
            cmd.Parameters.AddWithValue("@address", employee.Address);
            cmd.Parameters.AddWithValue("@department", employee.Department);
            cmd.Parameters.AddWithValue("@designation", employee.Designation);
            cmd.Parameters.AddWithValue("@Id", employee.Id);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }


        public async Task<bool> DeleteEmployeeAsync(int id)
        {

            MySqlCommand cmd = new MySqlCommand("DELETE FROM salary WHERE employee_id = @id;" +
				" DELETE FROM attendance WHERE employee_id = @id;" +
                " DELETE FROM employees where id = @id", _con);
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }
}
