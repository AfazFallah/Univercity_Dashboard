using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Univercity_Dashboard
{
    public abstract class User
    {
        #region Prop
        [Key]
        public int UserId { get; set; }
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string Name { get; set; }
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string Family { get; set; }
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(11)]
        public string Phonenumber { get; set; }
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string Password { get; set; }
        [Required]
        public Role RoleId { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsActiv { get; set; }
        #endregion

        #region Ctor
        public User() { }
        public User(int UserId, string Name, string Family, string Phonenumber, string Password, Role RoleId)
        {
            this.UserId = UserId;
            this.Name = Name;
            this.Family = Family;
            this.Phonenumber = Phonenumber;
            this.Password = Password;
            this.RoleId = RoleId;
            Birthdate = DateTime.Now.AddYears(-25);
            RegisterDate = DateTime.Now;
            IsActiv = true;
        }
        public User(string Name, string Family, string Phonenumber, string Password, Role RoleId)
        {
            this.Name = Name;
            this.Family = Family;
            this.Phonenumber = Phonenumber;
            this.Password = Password;
            this.RoleId = RoleId;
            Birthdate = DateTime.Now.AddYears(-25);
            RegisterDate = DateTime.Now;
            IsActiv = true;
        } 
        #endregion
    }
}
