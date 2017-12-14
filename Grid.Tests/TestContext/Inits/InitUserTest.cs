using Grid.Tests.TestContext.Models;
using Microsoft.EntityFrameworkCore;

namespace Grid.Tests.TestContext.Inits
{
	public static class InitUserTest
    {
        public static string DisplayName = "Test name {0}";
        public static TestDbContext CreateTestUsers(this TestDbContext context, int count)
        {
            for (var i = 0; i <= count; i++)
            {
                var user = InitUserTest.DbLoad(i);
                user = context.Add(user).Entity;
            }
            context.SaveChanges();
            return context;
        }
        public static User Load(int id)
        {
            return new User
            {
                Id = id,
                Name = string.Format(InitUserTest.DisplayName, id),
                DisplayName = string.Format(InitUserTest.DisplayName, id),
                Login = $"login{id}"
            };
        }
        public static User DbLoad(int id)
        {
            return new User
            {
                Name = string.Format(InitUserTest.DisplayName, id),
                DisplayName = string.Format(InitUserTest.DisplayName, id),
                Login = $"login{id}"
            };
        }

        public static void AddUser(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "dbo");
                entity.HasKey(p => p.Id);
                entity.Property(e => e.Name);
                entity.Property(e => e.Login);
                entity.Property(p => p.DisplayName);
                entity.Property(p => p.CreatedBy).HasDefaultValueSql("suser_sname()");
                entity.Property(p => p.CreatedDate).HasDefaultValueSql("sysdatetime()");
                entity.Property(p => p.LastUpdatedBy).HasDefaultValueSql("suser_sname()");
                entity.Property(p => p.LastUpdatedDate).HasDefaultValueSql("sysdatetime()");
            });
        }
    }
}
