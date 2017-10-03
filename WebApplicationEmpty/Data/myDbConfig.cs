using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KPMA.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KPMA.Data
{
    public class MyDbConfig : IDbContextConfig
    {
        public void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Chat>().Property(p => p.Rating);
            builder.Entity<Chat>().Property(p => p.Message);
            builder.Entity<Chat>().Property(p => p.RatedUsers);
            builder.Entity<Chat>().Property(p => p.AuthorId);
            //builder.Entity<Chat>().Property(p => p.DisplayName).HasComputedColumnSql("[Message]");
            builder.Entity<Chat>().Property(p => p.CreatedBy).HasDefaultValueSql("suser_sname()");
            builder.Entity<Chat>().Property(p => p.CreatedDate).HasDefaultValueSql("sysdatetime()");
            builder.Entity<Chat>().Property(p => p.LastUpdatedBy).HasDefaultValueSql("suser_sname()");
            builder.Entity<Chat>().Property(p => p.LastUpdatedDate).HasDefaultValueSql("sysdatetime()");
            builder.Entity<Chat>().HasOne(p => p.Author).WithMany().HasForeignKey("AuthorId").OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}