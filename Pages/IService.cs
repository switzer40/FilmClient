using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmClient.Pages.Shared
{
    public interface IService<T> where T : BaseDto
    {
        Task<OperationResult> AddAsync(T dto);
        
        Task<int> CountAsync();
        Task<OperationResult> DeleteAsync(string key);
        string KeyFrom(T dto);
        Task<List<T>> GetAllAsync();
        OperationResult GetByKey(string key);
        Task<OperationResult> GetByKeyAsync(string key);
        Task<OperationResult> UpdateAsync(T dto);
        OperationResult Add(T t);
        int Count();
        OperationResult Delete(string key);
        List<T> GetAll();        
        OperationResult Update(T dto);
    }
}
