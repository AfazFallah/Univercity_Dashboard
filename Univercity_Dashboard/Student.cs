using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Univercity_Dashboard
{
    public class Student : User
    {
        #region Prop
        public int StudentCode { get; set; }
        public static int Code { get; set; } = 100;
        public string Degree { get; set; }
        #endregion

        #region Reel
        public virtual ICollection<Course> Courses { get; set; }
        #endregion

        #region Ctor
        public Student() { }
        public Student(string Degree, string Name, string Family, string Phonenumber, string Password, Role RoleId) : base(Name, Family, Phonenumber, Password, RoleId)
        {
            Code++;
            StudentCode = Code;
            this.Degree = Degree;
            Courses = new List<Course>();
        } 
        #endregion
    }
}
