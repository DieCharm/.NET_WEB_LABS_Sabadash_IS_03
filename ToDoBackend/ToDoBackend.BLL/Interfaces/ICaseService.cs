using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoBackend.BLL.Models;

namespace ToDoBackend.BLL.Interfaces
{
    public interface ICaseService : ICrud<CaseModel>
    {
        Task AddAsync(CaseModel model);
        Task<IEnumerable<CaseModel>> GetTasksByUserAsync(string userId);
        Task<IEnumerable<CaseModel>> GetTasksByUserInProjectAsync(string userId, int projectId);
        Task ChangeNameAsync(int id, string name);
        Task ChangeDescriptionAsync(int id, string description);
        Task ChangeDeadlineAsync(int id, DateTime deadline);
        Task ChangePriorityAsync(int id, int priority);
        Task ChangeStatusAsync(int id, int status);
        
        Task SetUserAsync(int caseId, string userId);
        Task RemoveUserAsync(int caseId);
    }
}