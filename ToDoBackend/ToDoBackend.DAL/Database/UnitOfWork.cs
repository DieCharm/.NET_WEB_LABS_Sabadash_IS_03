using System.Threading.Tasks;
using ToDoBackend.DAL.Interfaces;
using ToDoBackend.DAL.Repositories;

namespace ToDoBackend.DAL.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private ToDoContext _context;

        public UnitOfWork(ToDoContext context)
        {
            _context = context;
        }
        
        public void Dispose()
        {
            this._context.Dispose();
        }

        public CaseRepository _caseRepository
        {
            get => new CaseRepository(_context);
        }
        public ProjectRepository _projectRepository
        {
            get => new ProjectRepository(_context);
        }
        public ProjectUserRepository _projectUserRepository
        {
            get => new ProjectUserRepository(_context);
        }
        public async Task SaveAsync()
        {
            await this._context.SaveChangesAsync();
        }
    }
}