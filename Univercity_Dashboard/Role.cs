using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Univercity_Dashboard
{
    public class Role
    {
        #region Prop
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoleId { get; set; }
        [Required]
        [Column("Title", TypeName = "varchar")]
        [MaxLength(20)]
        public string RoleTitle { get; set; }
        [Required]
        [Column("Name", TypeName = "varchar")]
        [MaxLength(20)]
        [Index("Key-Name", IsUnique = true)]
        public string RoleName { get; set; }
        #endregion

        #region Ctor
        public Role() { }
        public Role(int RoleId, string RoleTitle, string RoleName)
        {
            this.RoleId = RoleId;
            this.RoleTitle = RoleTitle;
            this.RoleName = RoleName;
        } 
        #endregion
    }
}
