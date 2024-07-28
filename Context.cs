using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp
{
    public class Context : DbContext
    {
        public Context()
        {
            Database.EnsureCreated();
        }
        public void DROPALL() => Database.EnsureDeleted();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("Data Source=EMP.db");        
        public DbSet<Employee> Employees {get; set;} = null!;
    }
}
