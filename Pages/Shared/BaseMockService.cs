using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;

namespace FilmClient.Pages.Shared
{
    public abstract class BaseMockService<T> : IService<T> where T : BaseDto
    {
        protected Dictionary<string, T> _store;
        protected IKeyService _keyService;
        protected OperationStatus OKStatus = OperationStatus.OK;
        public BaseMockService()
        {
            _store = new Dictionary<string, T>();
            _keyService = new KeyService();
        }
        public OperationResult<IKeyedDto> Add(T t)
        {
            IKeyedDto retVal = default;
            var key = KeyFrom(t);
            _store[key] = t;
            retVal = RetrieveKeyedDto(t);
            return new OperationResult<IKeyedDto>(OKStatus, retVal);
        }

        protected abstract IKeyedDto RetrieveKeyedDto(T t);


        public async Task<OperationResult<IKeyedDto>> AddAsync(T dto)
        {
            return await Task.Run(() => Add(dto));
        }

        public OperationResult<int> Count()
        {
            int count = _store.Values.Count;
            return new OperationResult<int>(OKStatus, count);
        }

        public async Task<OperationResult<int>> CountAsync()
        {
            return await Task.Run(() => Count());
        }

        public OperationStatus Delete(string key)
        {
            _store.Remove(key);
            return OKStatus;
        }

        public async Task<OperationStatus> DeleteAsync(string key)
        {
            return await Task.Run(() => Delete(key));
        }

        public OperationResult<List<IKeyedDto>> GetAbsolutelyAll()
        {
            List<IKeyedDto> retVal = new List<IKeyedDto>();
            foreach (var k in _store.Values)
            {
                var val = RetrieveKeyedDto(k);
                retVal.Add(val);
            }
            return new OperationResult<List<IKeyedDto>>(OKStatus, retVal);
        }

        public async Task<OperationResult<List<IKeyedDto>>> GetAbsolutelyAllAsync()
        {
            return await Task.Run(() => GetAbsolutelyAll());
        }

        public OperationResult<List<IKeyedDto>> GetAll(int pageIndex, int pageSize)
        {
            List<IKeyedDto> retVal = new List<IKeyedDto>();
            var list = _store.Values
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToList();
            foreach (var k in list)
            {
                var val = RetrieveKeyedDto(k);
                retVal.Add(val);
            }
            return new OperationResult<List<IKeyedDto>>(OKStatus, retVal);
        }

        public async Task<OperationResult<List<IKeyedDto>>> GetAllAsync(int pageIndex, int pageSize)
        {
            return await Task.Run(() => GetAll(pageIndex, pageSize));
        }

        public OperationResult<IKeyedDto> GetByKey(string key)
        {
            IKeyedDto retVal = (IKeyedDto)_store.Values
                .Where(k => k.Key == key).FirstOrDefault();
            return new OperationResult<IKeyedDto>(OKStatus, retVal);
        }

        public async Task<OperationResult<IKeyedDto>> GetByKeyAsync(string key)
        {
            return await Task.Run(() => GetByKey(key));
        }

        public OperationResult<IKeyedDto> GetLastEntry()
        {
            IKeyedDto retVal = (IKeyedDto)_store.Values
                .LastOrDefault();
            return new OperationResult<IKeyedDto>(OKStatus, retVal);
        }

        public async Task<OperationResult<IKeyedDto>> GetLastEntryAsync()
        {
            return await Task.Run(() => GetLastEntry());
        }

        public abstract string KeyFrom(T dto);


        public async Task<string> KeyFromAsync(T dto)
        {
            return await Task.Run(() => KeyFrom(dto));
        }

        public OperationStatus Update(T dto)
        {
            var storedEntity = RetrieveKeyedDto(_store[dto.Key]);
            SpecificCopy(storedEntity, dto);
            return OKStatus;
        }

        protected abstract void SpecificCopy(IKeyedDto target, T source);


        public async Task<OperationStatus> UpdateAsync(T dto)
        {
            return await Task.Run(() => Update(dto));
        }

        public abstract Task<PaginatedList<T>> CurrentPageAsync(int pageIndex, int pageSize);

        public abstract void SetController(string controller);        
    }
}
