using FilmAPI.Common.Interfaces;
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

        Task<List<T>> GetAllAsync();
        T GetByKeyResult(string key);
        Task<OperationStatus> GetByKeyAsync(string key);
        Task<OperationStatus> UpdateAsync(T dto);
    }
}
