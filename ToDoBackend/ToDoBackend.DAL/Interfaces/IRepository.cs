using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoBackend.DAL.Entities;
using Task = System.Threading.Tasks.Task;

namespace ToDoBackend.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);

        Task AddAsync(TEntity entity);

        void Delete(TEntity entity);

        Task DeleteByIdAsync(int id);

        void Update(TEntity entity);

    }
}