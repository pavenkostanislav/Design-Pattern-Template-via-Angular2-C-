using System.Linq;
using Grid.Tests.TestContext.Inits;
using Grid.Tests.TestContext.Models;
using Microsoft.EntityFrameworkCore;

namespace Grid.Tests.TestContext
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
        public override async System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var CurrentUser = this.Set<User>().FirstOrDefaultAsync();
            foreach (object model in this.ChangeTracker.Entries().Where(m => m.State == EntityState.Added).Select(n => n.Entity))
            {
                var logModel = model as Interfaces.ILogModel;
                if (logModel != null)
                {
                    logModel.CreatedBy = (await CurrentUser).DisplayName;
                    logModel.CreatedDate = System.DateTime.Now;
                    logModel.LastUpdatedBy = (await CurrentUser).DisplayName;
                    logModel.LastUpdatedDate = System.DateTime.Now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            var CurrentUser = this.Set<User>().FirstOrDefault();
            foreach (object model in this.ChangeTracker.Entries().Where(m => m.State == EntityState.Added).Select(n => n.Entity))
            {
                var logModel = model as Interfaces.ILogModel;
                if (logModel != null)
                {
                    logModel.CreatedBy = CurrentUser.DisplayName;
                    logModel.CreatedDate = System.DateTime.Now;
                    logModel.LastUpdatedBy = CurrentUser.DisplayName;
                    logModel.LastUpdatedDate = System.DateTime.Now;
                }
            }
            return base.SaveChanges();
        }
    }
}
