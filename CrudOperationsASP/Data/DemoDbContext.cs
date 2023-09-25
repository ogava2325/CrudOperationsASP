using CrudOperationsASP.Data.NewFolder;
using Microsoft.EntityFrameworkCore;

namespace CrudOperationsASP.Data
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
