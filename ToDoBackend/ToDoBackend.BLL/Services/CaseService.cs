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
    public class CaseService : ICaseService
    {
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;

        public CaseService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CaseModel> GetByIdAsync(int id)
        {
            return _mapper.Map<Case, CaseModel>
                (await _unitOfWork._caseRepository.GetByIdAsync(id));
        }

        public async Task AddAsync(CaseModel model)
        {
            await _unitOfWork._caseRepository.AddAsync
                (_mapper.Map<CaseModel, Case>(model));
            await _unitOfWork.SaveAsync();
        }
        
        public async Task ChangeNameAsync(int id, string name)
        {
            Case caseForChangeName = await _unitOfWork._caseRepository.GetByIdAsync(id);
            if (caseForChangeName != null &&
                !(String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)))
            {
                caseForChangeName.Name = name;
                _unitOfWork._caseRepository.Update(caseForChangeName);
            }

            throw new ArgumentException();
        }
        
        public async Task ChangeDescriptionAsync(int id, string description)
        {
            Case caseForChangeDescription = await _unitOfWork._caseRepository.GetByIdAsync(id);
            if (caseForChangeDescription != null &&
                !(String.IsNullOrEmpty(description) || String.IsNullOrWhiteSpace(description)))
            {
                caseForChangeDescription.Description = description;
                _unitOfWork._caseRepository.Update(caseForChangeDescription);
            }

            throw new ArgumentException();
        }
        
        public async Task ChangeDeadlineAsync(int id, DateTime deadline)
        {
            Case caseForChangeDeadline = await _unitOfWork._caseRepository.GetByIdAsync(id);
            if (caseForChangeDeadline != null &&
                deadline > DateTime.Now)
            {
                caseForChangeDeadline.Deadline = deadline;
                _unitOfWork._caseRepository.Update(caseForChangeDeadline);
            }

            throw new ArgumentException();
        }
        
        public async Task ChangePriorityAsync(int id, int priority)
        {
            Case caseForChangePriority = await _unitOfWork._caseRepository.GetByIdAsync(id);
            if (caseForChangePriority != null &&
                priority >= 0 && priority <= 2)
            {
                caseForChangePriority.Priority = (Priority) priority;
                _unitOfWork._caseRepository.Update(caseForChangePriority);
            }

            throw new ArgumentException();
        }
        
        public async Task ChangeStatusAsync(int id, int status)
        {
            Case caseForChangeStatus = await _unitOfWork._caseRepository.GetByIdAsync(id);
            if (caseForChangeStatus != null &&
                status >= 0 && status <= 2)
            {
                caseForChangeStatus.Status = (Status) status;
                _unitOfWork._caseRepository.Update(caseForChangeStatus);
            }

            throw new ArgumentException();
        }
        
        public async Task SetUserAsync(int caseId, string userId)
        {
            var caseForSet = await GetByIdAsync(caseId);
            if (caseForSet != null &&
                !(String.IsNullOrEmpty(userId) || String.IsNullOrWhiteSpace(userId)))
            {
                caseForSet.UserId = userId;
                _unitOfWork._caseRepository.Update
                    (_mapper.Map<CaseModel, Case>(caseForSet));
                await _unitOfWork.SaveAsync();
            }

            throw new ArgumentException();
        }

        public async Task RemoveUserAsync(int caseId)
        {
            Case caseToRemoveUser = await _unitOfWork._caseRepository.GetByIdAsync(caseId);
            if (caseToRemoveUser != null)
            {
                caseToRemoveUser.UserId = null;
                await _unitOfWork.SaveAsync();
            }

            throw new ArgumentException();
        }

        public async Task DeleteAsync(int id)
        {
            if (await _unitOfWork._caseRepository.GetByIdAsync(id) != null)
            {
                await _unitOfWork._caseRepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveAsync();
            }

            throw new ArgumentException();
        }

        public async Task<IEnumerable<CaseModel>> GetTasksByUserAsync(string userId)
        {
            return _mapper.Map<IEnumerable<Case>, IEnumerable<CaseModel>>
            ((await _unitOfWork._caseRepository.GetAllAsync())
                .Where(c => c.UserId == userId));
        }

        public async Task<IEnumerable<CaseModel>> GetTasksByUserInProjectAsync(string userId, int projectId)
        {
            return (await GetTasksByUserAsync(userId))
                .Where(model => model.ProjectId == projectId);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
