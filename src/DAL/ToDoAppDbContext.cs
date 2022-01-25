using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;

namespace DAL.Data
{
    public class ToDoAppDbContext : DbContext
    {
        private const string _connectionString = "Data Source = .;Initial Catalog = ToDoAppdbWeb_ToDoTaskMicroservice;Integrated Security = True;TrustServerCertificate = False;";

        public ToDoAppDbContext()
        {

        }

        public DbSet<ToDoTask> ToDoTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(_connectionString);

                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoTask>()
                .Property(t => t.AddedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ToDoTask>()
                .Property(t => t.EditedOn)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
