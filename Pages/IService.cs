using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmClient.Pages.Shared
{
    public interface IService<T> where T : BaseDto
    {
        Task<IKeyedDto> AddAsync(T dto);        
        Task<int> CountAsync();
        Task DeleteAsync(string key);
        Task<List<IKeyedDto>> GetAbsolutelyAllAsync();
        Task<List<IKeyedDto>> GetAllAsync(int pageIndex, int pageSize);
         IKeyedDto GetByKey(string key);
        Task<IKeyedDto> GetByKeyAsync(string key);
        Task<IKeyedDto> GetLastEntryAsync();
        Task UpdateAsync(T dto);
       IKeyedDto Add(T t);
        int Count();
        void Delete(string key);
        List<IKeyedDto> GetAbsolutelyAll();
        List<IKeyedDto> GetAll(int pageIndex, int pageSize);
       void Update(T dto);
        string KeyFrom(T dto);
    }
}
