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
        public BaseMockService()
        {
            _store = new Dictionary<string, T>();            
            _keyService = new KeyService();
        }
        public OperationResult Add(T t)
        {;
            var key = KeyFrom(t);
            _store[key] = t;
            OperationResult result = new OperationResult();
            result.ResultValue.Add(RetrieveKeyedDtoFrom(t));
            return result;
        }

        protected abstract IKeyedDto RetrieveKeyedDtoFrom(T t);
        
        public async Task<OperationResult> AddAsync(T dto)
        {
            return await Task.Run(() => Add(dto));
        }
       
        public int Count()
        {
            return (_store.Values).Count;
        }

        public async Task<int> CountAsync()
        {
            return await Task.Run(() => Count());
        }

        public OperationResult Delete(string key)
        {
            _store.Remove(key);
            return new OperationResult();
        }

        public async Task<OperationResult> DeleteAsync(string key)
        {
            return await Task.Run(() => Delete(key));
        }

        public List<T> GetAll(int pageIndex, int pageSize)
        {
            return (_store.Values
                .Skip(pageIndex * pageSize)
                .Take(pageSize)).ToList();
        }

        public async Task<List<T>> GetAllAsync(int pageIndex, int pageSize)
        {
            return await Task.Run(() => GetAll(pageIndex, pageSize));
        }

        public OperationResult GetByKey(string key)
        {
            var status = OperationStatus.OK;
            var retVal = new List<IKeyedDto>();
            if (_store.ContainsKey(key))
            {                
                retVal.Add(RetrieveKeyedDtoFrom(_store[key]));
            }
            else
            {
                status = OperationStatus.NotFound;
                status.ReasonForFailure = "Unknown entity";
            }
            return new OperationResult(status, retVal);
                
        }
        
        public async Task<OperationResult> GetByKeyAsync(string key)
        {
                return await Task.Run(() => GetByKey(key));
        }

        public abstract string KeyFrom(T dto);
        

        public OperationResult Update(T dto)
        {
            var key = dto.Key;           
            var status = OperationStatus.OK;
            var val = new List<IKeyedDto>();
             if (string.IsNullOrEmpty(key))
            {
                status = OperationStatus.BadRequest;
                status.ReasonForFailure = "Malformed key";
                val = null;
            }
            if (_store.ContainsKey(key))
            {
                var storedEntity = _store[key];
                storedEntity.Copy(dto);                
                val.Add((IKeyedDto)storedEntity);                
            }
            else
            {
                status = OperationStatus.NotFound;
                status.ReasonForFailure = "Unknown Entity";
                val = null;
            }
            return new OperationResult(status, val);
        }

        public async Task<OperationResult> UpdateAsync(T dto)
        {
            return await Task.Run(() => Update(dto));
        }

        public Task<T> GetLastEntryAsync()
        {
            throw new NotImplementedException();
        }
    }
}
