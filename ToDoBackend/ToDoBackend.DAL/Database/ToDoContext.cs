using Microsoft.EntityFrameworkCore;
using ToDoBackend.DAL.Entities;

namespace ToDoBackend.DAL.Database
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        { }

        /*public ToDoContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=JACKFLASHPC;Database=ToDoDb;Trusted_Connection=True;");
        }*/

        public DbSet<Project> Projects { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
    }
}