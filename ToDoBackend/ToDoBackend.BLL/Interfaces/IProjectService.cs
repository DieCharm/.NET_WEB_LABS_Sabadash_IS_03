using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoBackend.BLL.Models;

namespace ToDoBackend.BLL.Interfaces
{
    public interface IProjectService : ICrud<ProjectModel>
    {
        Task AddAsync(ProjectModel model, string userId);
        Task UpdateAsync(ProjectModel model);
        Task<IEnumerable<ProjectModel>> GetProjectsByUserAsync(string userId);
        Task<IEnumerable<string>> GetUserIdsByProjectAsync(int projectId);
        Task<IEnumerable<string>> GetAdminIdsByProjectAsync(int projectId);
        Task<bool> IsUserAdminAsync(int projectId, string userId);
        Task SetUserAsAdminAsync(int projectId, string userId);

        Task RemoveUserFromProjectAsync(int projectId, string userId);
    }
}