using System;
using System.Threading.Tasks;
using ToDoBackend.DAL.Repositories;

namespace ToDoBackend.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        CaseRepository _caseRepository { get; }
        ProjectRepository _projectRepository { get;  }
        ProjectUserRepository _projectUserRepository { get; }
        Task SaveAsync();
    }
}