using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp
{
    /// <summary>
    /// Класс сотрудника
    /// </summary>
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string OTC { get; set; }
        [Required]
        public DateOnly BDate { get; set; }
        [Required]
        public bool Sex { get; set; }
        public Employee() { }
        public Employee(string FirstName, string LastName, string OTC, DateOnly BDate, bool Sex)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.OTC = OTC;
            this.BDate = BDate;
            this.Sex = Sex;
        }
        public int GetAge()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - BDate.Year;

            if (BDate > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
        //public string FullName() => $"{FirstName} {LastName} {OTC}";
    }
}
