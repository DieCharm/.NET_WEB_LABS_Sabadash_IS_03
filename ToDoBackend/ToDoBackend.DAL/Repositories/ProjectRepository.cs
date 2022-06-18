using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoBackend.DAL.Database;
using ToDoBackend.DAL.Entities;
using ToDoBackend.DAL.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace ToDoBackend.DAL.Repositories
{
    public class ProjectRepository : IRepository<Project>
    {
        private ToDoContext _context;

        public ProjectRepository(ToDoContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project> GetByIdAsync(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task AddAsync(Project entity)
        {
            await _context.Projects.AddAsync(entity);
        }

        public void Delete(Project entity)
        {
            _context.Projects.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            Delete(await _context.Projects.FindAsync(id));
        }

        public void Update(Project entity)
        {
            _context.Projects.Update(entity);
        }
    }
}