using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;

namespace FilmClient.Pages.Shared
{
    public class BaseMockService<T> : IService<T> where T : BaseDto
    {
        public OperationResult<IKeyedDto> Add(T t)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IKeyedDto>> AddAsync(T dto)
        {
            throw new NotImplementedException();
        }

        public OperationResult<int> Count()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<int>> CountAsync()
        {
            throw new NotImplementedException();
        }

        public OperationStatus Delete(string key)
        {
            throw new NotImplementedException();
        }

        public Task<OperationStatus> DeleteAsync(string key)
        {
            throw new NotImplementedException();
        }

        public OperationResult<List<IKeyedDto>> GetAbsolutelyAll()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<IKeyedDto>>> GetAbsolutelyAllAsync()
        {
            throw new NotImplementedException();
        }

        public OperationResult<List<IKeyedDto>> GetAll(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<IKeyedDto>>> GetAllAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public OperationResult<IKeyedDto> GetByKey(string key)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IKeyedDto>> GetByKeyAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IKeyedDto>> GetLastEntryAsync()
        {
            throw new NotImplementedException();
        }

        public string KeyFrom(T dto)
        {
            throw new NotImplementedException();
        }

        public Task<string> KeyFromAsync(T dto)
        {
            throw new NotImplementedException();
        }

        public void SetController(string controller)
        {
            throw new NotImplementedException();
        }

        public OperationStatus Update(T dto)
        {
            throw new NotImplementedException();
        }

        public Task<OperationStatus> UpdateAsync(T dto)
        {
            throw new NotImplementedException();
        }
    }
}
