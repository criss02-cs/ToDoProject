using Microsoft.EntityFrameworkCore;
using ToDoProject.Models.Entities;

namespace ToDoProject.Server
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoItem>().HasOne(t => t.User)
                .WithMany(t => t.ToDoItems).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>().HasMany(x => x.ToDoItems)
                .WithOne(x => x.User);
        }
    }
}
