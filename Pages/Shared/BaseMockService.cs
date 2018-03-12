using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;

namespace FilmClient.Pages.Shared
{
    public abstract  class BaseMockService<T> : IService<T> where T : BaseDto
    {
        protected IKeyService _keyService;
        public BaseMockService()
        {
            _keyService = new KeyService();
        }
        public abstract IKeyedDto Add(T t);
        

        public async Task<IKeyedDto> AddAsync(T dto)
        {
            return await Task.Run(() => Add(dto));
        }

        public int Count()
        {
            var list = GetAbsolutelyAll();
            return list.Count;
        }

        public async Task<int> CountAsync()
        {
            return await Task.Run(() => Count());
        }

        public abstract void Delete(string key);
        

        public async Task DeleteAsync(string key)
        {
            await Task.Run(() => Delete(key));
        }

        public abstract List<IKeyedDto> GetAbsolutelyAll();
       

        public async Task<List<IKeyedDto>> GetAbsolutelyAllAsync()
        {
            return await Task.Run(() => GetAbsolutelyAll());
        }

        public List<IKeyedDto> GetAll(int pageIndex, int pageSize)
        {
            return GetAbsolutelyAll()
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToList();
        }

        public async Task<List<IKeyedDto>> GetAllAsync(int pageIndex, int pageSize)
        {
            return await Task.Run(() => GetAll(pageIndex, pageSize));
        }

        public abstract IKeyedDto GetByKey(string key);
        

        public async Task<IKeyedDto> GetByKeyAsync(string key)
        {
            return await Task.Run(() => GetByKey(key));
        }

        public async Task<IKeyedDto> GetLastEntryAsync()
        {
            return (await GetAbsolutelyAllAsync())
                .LastOrDefault();
        }

        public abstract string KeyFrom(T dto);


        public abstract void Update(T dto);
        

        public async Task UpdateAsync(T dto)
        {
            await Task.Run(() => Update(dto));
        }
    }
}
