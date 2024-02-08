using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Univercity_Dashboard
{
    public class Master : User
    {
        #region Prop
        public string Degree { get; set; }
        public float Salary { get; set; }
        #endregion

        #region Reel
        [ForeignKey("CourseId")]
        public virtual IEnumerable<Course> Courses { get; set; }
        #endregion

        #region Ctor
        public Master() { }
        public Master(string Degree, float Salary, string Name, string Family, string Phonenumber, string Password, Role RoleId) : base(Name, Family, Phonenumber, Password, RoleId)
        {
            this.Degree = Degree;
            this.Salary = Salary;
        } 
        #endregion
    }
}