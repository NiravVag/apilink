using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerComponent
{
    public class LoggerDbContext : DbContext
    {
        public virtual DbSet<EventLog> EventLog { get; set; }
        public virtual DbSet<RestApiLog> RestApiLog { get; set; }
        private readonly string _ConnectionString;


        public LoggerDbContext(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventLog>()
            .Property(f => f.Id)
            .UseIdentityColumn();

            modelBuilder.Entity<RestApiLog>()
                 .Property(f => f.Id)
                 .UseIdentityColumn();

            base.OnModelCreating(modelBuilder);
        }
    }
}
