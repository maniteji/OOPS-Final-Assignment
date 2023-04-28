using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Data.Models
{
	public class Salary
	{
		public int Id { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Please select an employee first.")]
		public int EmployeeId { get; set; }

		[Required(ErrorMessage = "Month is required")]
		[StringLength(255, ErrorMessage = "Month cannot be longer than 255 characters")]
		[RegularExpression(@"^(January|February|March|April|May|June|July|August|September|October|November|December)$", ErrorMessage = "Invalid month name")]
		public string Month { get; set; }

		[Required(ErrorMessage = "Year is required")]
		[RegularExpression(@"^\d{4}$", ErrorMessage = "Invalid year")]
		public int Year { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than zero")]
		[Required(ErrorMessage = "Amount is required")]
		public int Amount { get; set; }
	}
}
