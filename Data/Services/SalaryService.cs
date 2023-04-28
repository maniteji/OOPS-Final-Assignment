using MySqlConnector;
using System.Data.Common;
using EmployeeManagementSystem.Data.Models;

namespace EmployeeManagementSystem.Data.Services
{
    public class SalaryService
    {
        private readonly MySqlConnection _con;
        private static SalaryService _instance;

        public static SalaryService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SalaryService();
                }
                return _instance;
            }
        }

        public SalaryService()
        {
            _con = EMS_Connection.Instance.Connection;
        }

        public async Task<List<Salary>> GetSalariesAsync()
        {
            List<Salary> salaries = new List<Salary>();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM salary", _con);
            using (DbDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var salary = new Salary
                    {
                        Id = reader.GetInt32(0),
                        EmployeeId = reader.GetInt32(1),
                        Month = reader.GetString(2),
                        Year = reader.GetInt32(3),
                        Amount = reader.GetInt32(4)
                    };

                    salaries.Add(salary);
                }
            }
            return salaries;
        }

        public async Task<Salary> GetSalaryByIdAsync(int id)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM salary WHERE Id = @Id", _con);
            cmd.Parameters.AddWithValue("@Id", id);

            Salary salary = null;

            try
            {
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        salary = new Salary
                        {
                            Id = reader.GetInt32(0),
                            EmployeeId = reader.GetInt32(1),
                            Month = reader.GetString(2),
                            Year = reader.GetInt32(3),
                            Amount = reader.GetInt32(4)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return salary;
        }

        public async Task<bool> AddSalaryAsync(Salary salary)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO salary (employee_id, month, year, amount) " +
                "VALUES(@employee_id, @month, @year, @amount)", _con);
            cmd.Parameters.AddWithValue("@employee_id", salary.EmployeeId);
            cmd.Parameters.AddWithValue("@month", salary.Month);
            cmd.Parameters.AddWithValue("@year", salary.Year);
            cmd.Parameters.AddWithValue("@amount", salary.Amount);

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

        public async Task<bool> UpdateSalaryAsync(Salary salary)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE salary SET employee_id = @employee_id, month = @month, year = @year, " +
                "amount = @amount WHERE Id = @Id", _con);
            cmd.Parameters.AddWithValue("@employee_id", salary.EmployeeId);
            cmd.Parameters.AddWithValue("@month", salary.Month);
            cmd.Parameters.AddWithValue("@year", salary.Year);
            cmd.Parameters.AddWithValue("@amount", salary.Amount);
            cmd.Parameters.AddWithValue("@Id", salary.Id);

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

        public async Task<bool> DeleteSalaryAsync(int id)
        {
            MySqlCommand cmd = new MySqlCommand("DELETE FROM salary WHERE id = @id", _con);
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
