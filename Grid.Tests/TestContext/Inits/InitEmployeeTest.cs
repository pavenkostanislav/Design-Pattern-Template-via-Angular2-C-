using Grid.Tests.TestContext.Models;
using Microsoft.EntityFrameworkCore;

namespace Grid.Tests.TestContext.Inits
{
	public static class InitEmployeeTest
    {
        public static string DisplayName = "Test name {0}";
        public static TestDbContext CreateTestEmployees(this TestDbContext context, int count)
        {
            for (var i = 0; i <= count; i++)
            {
                var user = InitEmployeeTest.DbLoad(i);
                user = context.Add(user).Entity;
            }
            context.SaveChanges();
            return context;
        }
        public static Employee Load(int id)
        {
            return new Employee
            {
                Id = id,
                Name = string.Format(InitEmployeeTest.DisplayName, id),
                DisplayName = string.Format(InitUserTest.DisplayName, id)
            };
        }
        public static Employee DbLoad(int id)
        {
            return new Employee
            {
                Name = string.Format(InitEmployeeTest.DisplayName, id),
                DisplayName = string.Format(InitUserTest.DisplayName, id)
            };
        }

        public static void AddEmployee(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee", "dbo");
                entity.HasKey(p => p.Id);
                entity.Property(e => e.Name);
                entity.Property(p => p.DisplayName);
                entity.Property(p => p.CreatedBy).HasDefaultValueSql("suser_sname()");
                entity.Property(p => p.CreatedDate).HasDefaultValueSql("sysdatetime()");
                entity.Property(p => p.LastUpdatedBy).HasDefaultValueSql("suser_sname()");
                entity.Property(p => p.LastUpdatedDate).HasDefaultValueSql("sysdatetime()");
            });
        }
    }
}
