using DAL.Models;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;
using ToDoApp.Models.Users;

namespace DAL.Data
{
    public class ToDoAppDbContext : DbContext
    {
        private const string _connectionString = "Data Source = .;Initial Catalog = ToDoAppdb;Integrated Security = True;TrustServerCertificate = False;";

        public ToDoAppDbContext()
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<ToDoList> ToDoLists { get; set; }

        public DbSet<ToDoTask> ToDoTasks { get; set; }

        public DbSet<UserToDoList> UsersToDoLists { get; set; }

        public DbSet<UserToDoTask> UsersToDoTasks { get; set; }

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
            modelBuilder.Entity<UserToDoList>()
                .HasKey(ul => new { ul.UserId, ul.ToDoListId });

            modelBuilder.Entity<UserToDoTask>()
                .HasKey(ut => new { ut.UserId, ut.ToDoTaskId });

            modelBuilder.Entity<ToDoList>()
                .HasOne(t => t.User)
                .WithMany(u => u.CreatedToDoLists)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ToDoTask>()
               .HasOne(t => t.User)
               .WithMany(u => u.CreatedToDoTasks)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ToDoTask>()
               .HasOne(t => t.ToDoList)
               .WithMany(u => u.ToDoTasks)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .Property(u => u.AddedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ToDoList>()
                .Property(l => l.AddedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ToDoTask>()
                .Property(t => t.AddedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<User>()
              .Property(u => u.EditedOn)
              .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ToDoList>()
                .Property(l => l.EditedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ToDoTask>()
                .Property(t => t.EditedOn)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
