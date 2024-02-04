using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Univercity_Dashboard
{
    class UnivercityContext : DbContext
    {
        public UnivercityContext(string name) : base(name) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Master> Masters { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
