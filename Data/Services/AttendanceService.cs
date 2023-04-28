using EmployeeManagementSystem.Data.Models;
using MySqlConnector;
using System.Data.Common;

namespace EmployeeManagementSystem.Data.Services
{
    public class AttendanceService
    {
        private readonly MySqlConnection _con;
        private static AttendanceService _instance;
        public static AttendanceService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AttendanceService();
                }
                return _instance;
            }
        }

        public AttendanceService()
        {
            _con = EMS_Connection.Instance.Connection;
        }

        public async Task<List<Attendance>> GetAttendanceAsync()
        {
            List<Attendance> attendances = new List<Attendance>();

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT a.id, a.employee_id, a.date, a.status " +
                          "FROM attendance a " +
                          "INNER JOIN employees e ON a.employee_id = e.id " +
                          "ORDER BY a.date DESC", _con);

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Attendance attendance = new Attendance
                        {
                            Id = reader.GetInt32(0),
                            EmployeeId = reader.GetInt32(1),
                            Date = reader.GetDateTime(2),
                            Status = (AttndanceStatus)Enum.Parse(typeof(AttndanceStatus), reader.GetString(3), true)
                        };

                        attendances.Add(attendance);
                    }
                }
            }
            catch (Exception)
            {

            }

            return attendances;
        }

        public async Task<Attendance> GetAttendanceByIdAsync(int id)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT a.id, a.employee_id, a.date, a.status " +
                "FROM attendance a " +
                "INNER JOIN employees e ON a.employee_id = e.id " +
                "WHERE a.id = @id", _con);

            cmd.Parameters.AddWithValue("@id", id);

            Attendance attendance = null;

            try
            {
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        attendance = new Attendance();
                        attendance.Id = Convert.ToInt32(reader["id"]);
                        attendance.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        attendance.Date = Convert.ToDateTime(reader["date"]);
                        attendance.Status = (AttndanceStatus)Enum.Parse(typeof(AttndanceStatus), reader["status"].ToString(), true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return attendance;
        }

        public async Task<bool> AddAttendanceAsync(Attendance attendance)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO attendance (employee_id, date, status) " +
                "SELECT @employeeId, @date, @status WHERE NOT EXISTS( SELECT 1 FROM attendance " +
                "WHERE employee_id = @employeeId AND date = @date)", _con);

            cmd.Parameters.AddWithValue("@employeeId", attendance.EmployeeId);
            cmd.Parameters.AddWithValue("@date", attendance.Date.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@status", attendance.Status.ToString());

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

        public async Task<bool> ChangeAttendanceStatusAsync(Attendance attendance)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE attendance " +
                "SET employee_id = @employeeId, date = @date, status = @status " +
                "WHERE id = @id", _con);

            attendance.Status = attendance.Status == AttndanceStatus.Present ? AttndanceStatus.Absent : AttndanceStatus.Present;

            cmd.Parameters.AddWithValue("@employeeId", attendance.EmployeeId);
            cmd.Parameters.AddWithValue("@date", attendance.Date.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@status", attendance.Status.ToString());
            cmd.Parameters.AddWithValue("@id", attendance.Id);

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


        public async Task<bool> DeleteAttendanceAsync(int id)
        {
            MySqlCommand cmd = new MySqlCommand("DELETE FROM attendance WHERE id = @id", _con);
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }

}
