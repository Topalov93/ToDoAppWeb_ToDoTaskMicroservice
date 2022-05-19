using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;

namespace DAL.Data
{
    public class ToDoAppDbContext : DbContext
    {
        public ToDoAppDbContext(DbContextOptions<ToDoAppDbContext> options) : base(options)
        {
            
        }

        public ToDoAppDbContext()
        {

        }

        public DbSet<ToDoTask> ToDoTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
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
