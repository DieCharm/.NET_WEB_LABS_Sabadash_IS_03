using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoBackend.DAL.Database;
using ToDoBackend.DAL.Entities;
using ToDoBackend.DAL.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace ToDoBackend.DAL.Repositories
{
    public class ProjectUserRepository : IRepository<ProjectUser>
    {
        private ToDoContext _context;

        public ProjectUserRepository(ToDoContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<ProjectUser>> GetAllAsync()
        {
            return await _context.ProjectUsers.ToListAsync();
        }

        public async Task<ProjectUser> GetByIdAsync(int id)
        {
            return await _context.ProjectUsers.FindAsync(id);
        }

        public async Task AddAsync(ProjectUser entity)
        {
            await _context.ProjectUsers.AddAsync(entity);
        }

        public void Delete(ProjectUser entity)
        {
            _context.ProjectUsers.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            _context.ProjectUsers.Remove(await _context.ProjectUsers.FindAsync(id));
        }

        public void Update(ProjectUser entity)
        {
            _context.ProjectUsers.Update(entity);
        }
        
        public async Task<bool> DeleteByConditionAsync(Func<ProjectUser, bool> condition)
        {
            int firstLength = _context.ProjectUsers.Count();
            (await _context.ProjectUsers.ToListAsync())
                .RemoveAll(item => condition(item));
            return _context.ProjectUsers.Count() != firstLength;
        }
    }
}