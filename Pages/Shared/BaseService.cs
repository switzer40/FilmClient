using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;
using Newtonsoft.Json;

namespace FilmClient.Pages.Shared
{
    public abstract class BaseService<T> : IService<T> where T : BaseDto
    {
        protected HttpClient _client;
        protected string _route;
        protected string _controller;
        protected string _action;        
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

        protected string ComputeRoute(string arg = "")
        {
            var result = (string.IsNullOrEmpty(arg)) ? $"/api/{_controller}/{_action}" : $"/api/{_controller}/{_action}/{arg}";
            return result;
        }
        public OperationResult Add(T t)
        {
            // Not needed
            return new OperationResult(OperationStatus.OK);
        }

        public abstract Task<OperationResult> AddAsync(T dto);
   
        public int Count()
        {
            // Not need
            return 0;
        }

        public abstract Task<int> CountAsync();
        

        public OperationResult Delete(string key)
        {
            // Not needed
            return new OperationResult(OperationStatus.OK);
        }

        public abstract Task<OperationResult> DeleteAsync(string key);
        

        public List<T> GetAll()
        {
            // Not needed
            return new List<T>();
        }

        public abstract Task<List<T>> GetAllAsync();
        

        public OperationResult GetByKey(string key)
        {
            // Not need
            return new OperationResult(OperationStatus.OK);
        }

        public abstract Task<OperationResult> GetByKeyAsync(string key);

        public abstract string KeyFrom(T dto);
        

        public OperationResult Update(T dto)
        {
            // Not needed
            return new OperationResult(OperationStatus.OK);
        }

        public abstract Task<OperationResult> UpdateAsync(T dto);

        protected async Task<OperationResult> ResultFromResponseAsync(HttpResponseMessage response)
        {
            OperationResult result = new OperationResult(OperationStatus.OK);
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    result = new OperationResult(OperationStatus.BadRequest);
                    break;
                case HttpStatusCode.NotFound:
                    result = new OperationResult(OperationStatus.NotFound);
                    break;
                case HttpStatusCode.OK:
                    result = new OperationResult(OperationStatus.OK);
                    break;
                case HttpStatusCode.InternalServerError:
                    result = new OperationResult(OperationStatus.ServerError);
                    break;
                default:
                    throw new Exception("Unexpected status code");
            };
            result.ResultValue = await ExtractListFromAsync(response);            
            return result;
        }

        protected abstract Task<List<IKeyedDto>> ExtractListFromAsync(HttpResponseMessage response);
        

        protected abstract IBaseDto ArgFromDto(BaseDto dto);
        protected HttpContent ContentFromDto(BaseDto dto)
        {
            var arg = ArgFromDto(dto);
            return new StringContent(JsonConvert.SerializeObject(arg), Encoding.UTF8, "application/json");
        }
    }
}
