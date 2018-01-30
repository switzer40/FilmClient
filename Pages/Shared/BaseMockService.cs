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
        protected T _addResult;
        protected Dictionary<string, T> _getResults;
        protected IKeyService _keyService;
        public BaseMockService()
        {
            _store = new Dictionary<string, T>();
            _addResult = null;
            _getResults = new Dictionary<string, T>();
            _keyService = new KeyService();
        }
        public OperationStatus Add(T t)
        {;
            var key = KeyFrom(t);
            _store[key] = t;
            _addResult = t;
            return OperationStatus.OK;
        }

        public async Task<OperationStatus> AddAsync(T dto)
        {
            return await Task.Run(() => Add(dto));
        }
        

        public T AddResult()
        {
            return _addResult;
        }

        public int Count()
        {
            return GetAll().Count;
        }

        public async Task<int> CountAsync()
        {
            return await Task.Run(() => Count());
        }

        public OperationStatus Delete(string key)
        {
            _store.Remove(key);
            return OperationStatus.OK;
        }

        public async Task<OperationStatus> DeleteAsync(string key)
        {
            return await Task.Run(() => Delete(key));
        }

        public List<T> GetAll()
        {
            return _store.Values.ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await Task.Run(() => GetAll());
        }

        public OperationStatus GetByKey(string key)
        {
                if (_store.ContainsKey(key))
                {
                    _getResults[key] = _store[key];
                    return OperationStatus.OK;
                }
                else
                {
                    _getResults[key] = null;
                    var result = OperationStatus.NotFound;
                    result.ReasonForFailure = "Unknown entity";
                    return result;
                }
        }

        public async Task<OperationStatus> GetByKeyAsync(string key)
        {
                return await Task.Run(() => GetByKey(key));
        }

        public T GetByKeyResult(string key)
        {
                return _getResults[key];
        }

        public abstract string KeyFrom(T dto);
        

        public OperationStatus Update(T dto)
        {
            var key = dto.Key;
            if (string.IsNullOrEmpty(key))
            {
                var result = OperationStatus.BadRequest;
                result.ReasonForFailure = "Malformed key";
                return result;
            }
            if (_store.ContainsKey(key))
            {
                var storedEntity = _store[key];
                storedEntity.Copy(dto);
                return OperationStatus.OK;
            }
            else
            {
                var result = OperationStatus.NotFound;
                result.ReasonForFailure = "Unknown Entity";
                return result;
            }

        }

        public async Task<OperationStatus> UpdateAsync(T dto)
        {
            return await Task.Run(() => Update(dto));
        }
    }
}
