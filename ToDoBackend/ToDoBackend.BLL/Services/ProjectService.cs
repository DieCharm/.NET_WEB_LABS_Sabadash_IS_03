using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ToDoBackend.BLL.Interfaces;
using ToDoBackend.BLL.Models;
using ToDoBackend.DAL.Entities;
using ToDoBackend.DAL.Interfaces;

namespace ToDoBackend.BLL.Services
{
    public class ProjectService : IProjectService
    {
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;

        public ProjectService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectModel> GetByIdAsync(int id)
        {
            return _mapper.Map<Project, ProjectModel>
                (await _unitOfWork._projectRepository.GetByIdAsync(id));
        }
        
        public async Task AddAsync(ProjectModel model, string userId)
        {
            Project newProject = _mapper.Map<ProjectModel, Project>(model);
            await _unitOfWork._projectRepository.AddAsync(newProject);
            await _unitOfWork.SaveAsync();
            await _unitOfWork._projectUserRepository.AddAsync
                (_mapper.Map<ProjectUserModel, ProjectUser>
                    (new ProjectUserModel()
                    {
                        ProjectId = newProject.Id,
                        UserId = userId,
                        IsAdmin = true
                    }));
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ProjectModel model)
        {
            _unitOfWork._projectRepository.Update
                (_mapper.Map<ProjectModel, Project>(model));
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (await _unitOfWork._projectRepository.GetByIdAsync(id) != null)
            {
                await _unitOfWork._projectRepository
                    .DeleteByIdAsync(id);
                await _unitOfWork._projectUserRepository
                    .DeleteByConditionAsync((projectUser => projectUser.ProjectId == id));
                await _unitOfWork._caseRepository
                    .DeleteByConditionAsync(c => c.ProjectId == id);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task<IEnumerable<ProjectModel>> GetProjectsByUserAsync(string userId)
        {
            IEnumerable<int> indexes = (await _unitOfWork._projectUserRepository.GetAllAsync())
                .Where(projectUser => projectUser.UserId == userId)
                .Select(projectUser => projectUser.ProjectId);
            List<ProjectModel> result = new List<ProjectModel>();
            foreach (var index in indexes)
            {
                result.Add(_mapper.Map<Project, ProjectModel>
                    (await _unitOfWork._projectRepository.GetByIdAsync(index)));
            }

            return result;
        }

        public async Task<IEnumerable<string>> GetUserIdsByProjectAsync(int projectId)
        {
            return (await _unitOfWork._projectUserRepository.GetAllAsync())
                .Where(projectUser => projectUser.ProjectId == projectId)
                .Select(projectUser => projectUser.UserId);
        }

        public async Task<IEnumerable<string>> GetAdminIdsByProjectAsync(int projectId)
        {
            return (await _unitOfWork._projectUserRepository.GetAllAsync())
                .Where(projectUser => projectUser.ProjectId == projectId && projectUser.IsAdmin)
                .Select(projectUser => projectUser.UserId);
        }

        public async Task<bool> IsUserAdminAsync(int projectId, string userId)
        {
            var userProject = (await _unitOfWork._projectUserRepository.GetAllAsync())
                .FirstOrDefault(projectUser => projectUser.UserId == userId && projectUser.ProjectId == projectId);
            if (userProject != null)
            {
                return userProject.IsAdmin;
            }
            throw new ArgumentException();
        }

        public async Task SetUserAsAdminAsync(int projectId, string userId)
        {
            var userProject = (await _unitOfWork._projectUserRepository.GetAllAsync())
                .FirstOrDefault(projectUser => projectUser.UserId == userId && projectUser.ProjectId == projectId);
            if (userProject != null)
            {
                userProject.IsAdmin = true;
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task AddUserToProjectAsync(int projectId, string userId)
        {
            await _unitOfWork._projectUserRepository.AddAsync(
                new ProjectUser()
                {
                    ProjectId = projectId,
                    UserId = userId
                });
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveUserFromProjectAsync(int projectId, string userId)
        {
            if (await _unitOfWork._projectUserRepository.DeleteByConditionAsync
                    (projectUser => projectUser.ProjectId == projectId && projectUser.UserId == userId))
            {
                foreach (var caseToRemoveUser in (await _unitOfWork._caseRepository.GetAllAsync()))
                {
                    if (caseToRemoveUser.UserId == userId && caseToRemoveUser.ProjectId == projectId)
                    {
                        caseToRemoveUser.UserId = null;
                        _unitOfWork._caseRepository.Update(caseToRemoveUser);
                        await _unitOfWork.SaveAsync();
                    }
                }
            
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
