using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Grid.Test.TestContext.Models;

namespace Grid.Test.TestContext.Inits

{
	public static class InitTableModel
    {
        public static TestDbContext CreateTableModels(this TestDbContext context, int count)
        {
            for (var i = 0; i <= count; i++)
            {
                var model = InitTableModel.DbLoad(i, count);
                model = context.Add(model).Entity;
            }
            context.SaveChanges();
            return context;
        }
        public static System.Collections.Generic.List<TableModel> CreateTableModels(int count = 5)
        {
            var listTableModelTest = new System.Collections.Generic.List<TableModel>();
            for (var i = 0; i <= count; i++)
            {
                var tableTest = SimplyLoad(i);
                listTableModelTest.Add(tableTest);
            }
            return listTableModelTest;
        }

        public static TableModel SimplyLoad(int i)
        {
            var tm = new TableModel
            {
                Id = i,
                Test = $"table test text {i.ToString()}",
                DisplayName = $"table test text {i.ToString()}",
                Date = DateTime.Now.AddDays(i),
                UserId = Tools.TestTools.rInt(i > 0 ? i : 1),
                EmployeeId = Tools.TestTools.rInt(i > 0 ? i : 1),
                IsBool = true,
                Decimal = (300.5m * i)
            };
            return tm;
        }

        public static TableModel DbLoad(int i, int count)
        {
            return new TableModel
            {
                Test = $"table test text {i.ToString()}",
                DisplayName = $"table test text {i.ToString()}",
                Date = DateTime.Now.AddDays(i),
                UserId = Tools.TestTools.rInt(i > 0 ? i : 1),
                EmployeeId = Tools.TestTools.rInt(i > 0 ? i : 1),
                IsBool = true,
                Decimal = (300.5m * i)
            };
        }

        public static void AddTableModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TableModel>(entity =>
            {
                entity.ToTable("TableModelTest", "dbo");
                entity.HasKey(p => p.Id);
                entity.Property(e => e.Test);
                entity.Property(e => e.Date);
                entity.Property(e => e.DateNull).IsRequired(false);
                entity.Property(e => e.UserId);
                entity.Property(e => e.UserNullId).IsRequired(false);
                entity.Property(e => e.IsBool);
                entity.Property(e => e.IsBoolNull).IsRequired(false);
                entity.Property(e => e.Decimal);
                entity.Property(e => e.DecimalNull).IsRequired(false);
                entity.HasOne(p => p.User).WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.UserNull).WithMany().HasForeignKey("UserNullId").OnDelete(DeleteBehavior.Restrict).IsRequired(false); 
                entity.Property(p => p.DisplayName);
                entity.Property(e => e.DecimalNull).HasColumnName("DecimalNull").IsRequired(false);
                entity.Property(p => p.CreatedBy).HasDefaultValueSql("suser_sname()");
                entity.Property(p => p.CreatedDate).HasDefaultValueSql("sysdatetime()");
                entity.Property(p => p.LastUpdatedBy).HasDefaultValueSql("suser_sname()");
                entity.Property(p => p.LastUpdatedDate).HasDefaultValueSql("sysdatetime()");
            });
        }
    }
}
