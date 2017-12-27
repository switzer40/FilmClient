using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FilmClient.Pages.Shared
{
    public abstract class BaseService<T> : IService<T> where T : BaseDto
    {
        protected HttpClient _client;
        protected string _route;
        protected IKeyService _keyService;
        protected IErrorService _errorService;

        public BaseService(IErrorService eservice)
        {
            _client = GetClient();
            _errorService = eservice;
            _keyService = new KeyService();
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
        public abstract Task<OperationStatus> AddAsync(T dto);


        public abstract T AddResult();


        public abstract Task<OperationStatus> DeleteAsync(string key);


        public abstract Task<List<T>> GetAllAsync();


        public abstract Task<OperationStatus> GetByKeyAsync(string key);


        public abstract T GetByKeyResult(string key);

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

        public async Task<int> CountAsync()
        {
            var entities = await GetAllAsync();
            return entities.Count();
        }
    }
}
