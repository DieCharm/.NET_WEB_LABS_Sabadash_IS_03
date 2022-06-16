using System;
using System.Threading.Tasks;

namespace ToDoBackend.BLL.Interfaces
{
    public interface ICrud<TModel> : IDisposable 
        where TModel : class
    {
        Task<TModel> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}