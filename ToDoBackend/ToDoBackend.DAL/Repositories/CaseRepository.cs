using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoBackend.DAL.Database;
using ToDoBackend.DAL.Entities;
using ToDoBackend.DAL.Interfaces;

namespace ToDoBackend.DAL.Repositories
{
    public class CaseRepository : IRepository<Case>
    {
        private ToDoContext _context;

        public CaseRepository(ToDoContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Case>> GetAllAsync()
        {
            return await _context.Cases.ToListAsync();
        }

        public async Task<Case> GetByIdAsync(int id)
        {
            return await _context.Cases.FindAsync(id);
        }

        public async Task AddAsync(Case entity)
        {
            await _context.Cases.AddAsync(entity);
        }

        public void Delete(Case entity)
        {
            _context.Cases.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            _context.Cases.Remove(await _context.Cases.FindAsync(id));
        }

        public void Update(Case entity)
        {
            _context.Cases.Update(entity);
        }
    }
}