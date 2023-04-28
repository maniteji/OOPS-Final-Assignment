using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Data.Models
{

    public enum AttndanceStatus
    {
        Present,
        Absent
    }
    public class Attendance
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select the employee first.")]
        [EmployeeIdValidation(ErrorMessage = "Please select the employee first.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(AttndanceStatus), ErrorMessage = "Invalid status")]
        public AttndanceStatus Status { get; set; }
    }

	public class EmployeeIdValidationAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			int employeeId = (int)value;

			if (employeeId == 0)
			{
				return new ValidationResult("Please select the employee first.");
			}

			return ValidationResult.Success;
		}
	}
}
