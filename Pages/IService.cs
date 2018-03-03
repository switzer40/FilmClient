using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmClient.Pages.Shared
{
    public interface IService<T> where T : BaseDto
    {
        Task<OperationResult<IKeyedDto>> AddAsync(T dto);        
        Task<OperationResult<int>> CountAsync();
        Task<OperationStatus> DeleteAsync(string key);
        Task<string> KeyFromAsync(T dto);
        Task<OperationResult<List<IKeyedDto>>> GetAbsolutelyAllAsync();
        Task<OperationResult<List<IKeyedDto>>> GetAllAsync(int pageIndex, int pageSize);
        OperationResult<IKeyedDto> GetByKey(string key);
        Task<OperationResult<IKeyedDto>> GetByKeyAsync(string key);
        Task<OperationResult<IKeyedDto>> GetLastEntryAsync();
        Task<OperationStatus> UpdateAsync(T dto);
        OperationResult<IKeyedDto> Add(T t);
        OperationResult<int> Count();
        OperationStatus Delete(string key);
        OperationResult<List<IKeyedDto>> GetAbsolutelyAll();
        OperationResult<List<IKeyedDto>> GetAll(int pageIndex, int pageSize);
        OperationResult<IKeyedDto> GetLastEntry();
        OperationStatus Update(T dto);
        string KeyFrom(T dto);
    }
}
