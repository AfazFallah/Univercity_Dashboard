using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Univercity_Dashboard
{
    public class Employee : User
    {
        public string Department { get; set; }
        public float Salary { get; set; }


        public Employee() { }
        public Employee(string Department, float Salary, string Name, string Family, string Phonenumber, string Password, Role RoleId) : base(Name, Family, Phonenumber, Password, RoleId)
        {
            this.Department = Department;
            this.Salary = Salary;
        }
    }
}
