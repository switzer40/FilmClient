using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using Newtonsoft.Json;

namespace FilmClient.Pages.Shared
{
    public abstract class BaseService<T> : IService<T> where T : BaseDto
    {
        protected HttpClient _client;
        protected string _route;
        protected T _addResult;
        protected Dictionary<string, T> _getResults;
        protected IKeyService _keyService;
        protected IErrorService _errorService;
        public BaseService(IErrorService eservice)
        {
            _client = GetClient();
            _errorService = eservice;
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public OperationStatus Add(T t)
        {
            // Not needed
            return OperationStatus.OK;
        }

        public abstract Task<OperationStatus> AddAsync(T dto);
        

        public virtual T AddResult()
        {
            return _addResult;
        }

        public int Count()
        {
            // Not need
            return 0;
        }

        public abstract Task<int> CountAsync();
        

        public OperationStatus Delete(string key)
        {
            // Not needed
            return OperationStatus.OK;
        }

        public abstract Task<OperationStatus> DeleteAsync(string key);
        

        public List<T> GetAll()
        {
            // Not needed
            return new List<T>();
        }

        public abstract Task<List<T>> GetAllAsync();
        

        public OperationStatus GetByKey(string key)
        {
            // Not need
            return OperationStatus.OK;
        }

        public abstract Task<OperationStatus> GetByKeyAsync(string key);


        public virtual T GetByKeyResult(string key)
        {
            if (_getResults.ContainsKey(key))
            {
                return _getResults[key];
            }
            else
            {
                return null;
            }
        }

        public abstract string KeyFrom(T dto);
        

        public OperationStatus Update(T dto)
        {
            // Not needed
            return OperationStatus.OK;
        }

        public abstract Task<OperationStatus> UpdateAsync(T dto);
        protected OperationStatus StatusFromResponse(HttpResponseMessage response)
        {
            var result = OperationStatus.OK;
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    result = OperationStatus.BadRequest;
                    break;
                case HttpStatusCode.NotFound:
                    result = OperationStatus.NotFound;
                    break;
                case HttpStatusCode.OK:
                    result = OperationStatus.OK;
                    break;
                case HttpStatusCode.InternalServerError:
                    result = OperationStatus.ServerError;
                    break;
                default:
                    throw new Exception("Unexpected status code");
            }
            return result;
        }
        protected abstract IBaseDto ArgFromDto(BaseDto dto);
        protected HttpContent ContentFromDto(BaseDto dto)
        {
            var arg = ArgFromDto(dto);
            return new StringContent(JsonConvert.SerializeObject(arg), Encoding.UTF8, "application/json");
        }
    }
}
