using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmClient.Pages.Shared
{
    public interface IService<T> where T : BaseDto
    {
        Task<OperationStatus> AddAsync(T dto);
        T AddResult();
        Task<int> CountAsync();
        Task<OperationStatus> DeleteAsync(string key);
        string KeyFrom(T dto);
        Task<List<T>> GetAllAsync();
        T GetByKeyResult(string key);
        Task<OperationStatus> GetByKeyAsync(string key);
        Task<OperationStatus> UpdateAsync(T dto);
        OperationStatus Add(T t);
        int Count();
        OperationStatus Delete(string key);
        List<T> GetAll();
        OperationStatus GetByKey(string key);
        OperationStatus Update(T dto);
    }
}
