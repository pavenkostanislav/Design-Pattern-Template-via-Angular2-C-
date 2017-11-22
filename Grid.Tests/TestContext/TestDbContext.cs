using JetBrains.Annotations;
using Grid.Test.TestContext.Inits;
using Grid.Test.TestContext.Models;
using Microsoft.EntityFrameworkCore;

namespace Grid.Test.TestContext
{
    public class TestDbContext : DbContext
    {
        public TestDbContext( DbContextOptions options) : base(options)
        {
        }

        public DbSet<TableModel> TableModelTests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employee { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddTableModel();
            modelBuilder.AddUser();
            modelBuilder.AddEmployee();
        }
    }
}
