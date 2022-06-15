using Microsoft.EntityFrameworkCore;
using ToDoBackend.DAL.Entities;

namespace ToDoBackend.DAL.Database
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
    }
}