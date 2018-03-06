using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Error;

namespace FilmClient.Pages.Shared
{
    public abstract class BaseService<T> : IService<T> where T : BaseDto
    {
        protected HttpClient _client;
        protected string _controller;
        protected string _action;
        protected string _route;
        protected IKeyService _keyService;
        protected IErrorService _errorService;
        public BaseService(IErrorService eservice)
        {
            _client = GetClient();
            _errorService = eservice;
            _keyService = new KeyService();
        }
        public void SetController(string controller)
        {
            _controller = controller;
        }
        protected HttpClient GetClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
        protected OperationStatus OKStatus = OperationStatus.OK;
        public OperationResult<IKeyedDto> Add(T t)
        {
            // Not neded here
            return new OperationResult<IKeyedDto>(OKStatus);
        }

        public abstract Task<OperationResult<IKeyedDto>> AddAsync(T dto);


        public OperationResult<int> Count()
        {
            // Not neded here
            return new OperationResult<int>(OKStatus, 0);
        }

        public abstract Task<OperationResult<int>> CountAsync();


        public OperationStatus Delete(string key)
        {
            // Not neded here
            return OKStatus;
        }

        public abstract Task<OperationStatus> DeleteAsync(string key);


        public OperationResult<List<IKeyedDto>> GetAbsolutelyAll()
        {
            // Not neded here
            var list = new List<IKeyedDto>();
            return new OperationResult<List<IKeyedDto>>(OKStatus, list);
        }

        public abstract Task<OperationResult<List<IKeyedDto>>> GetAbsolutelyAllAsync();


        public OperationResult<List<IKeyedDto>> GetAll(int pageIndex, int pageSize)
        {
            // Not neded here
            return new OperationResult<List<IKeyedDto>>(OKStatus);
        }

        public abstract Task<OperationResult<List<IKeyedDto>>> GetAllAsync(int pageIndex, int pageSize);


        public OperationResult<IKeyedDto> GetByKey(string key)
        {
            // Not neded here
            return new OperationResult<IKeyedDto>(OKStatus);
        }

        public abstract Task<OperationResult<IKeyedDto>> GetByKeyAsync(string key);

        public async Task<string> KeyFromAsync(T dto)
        {
            return await Task.Run(() => KeyFrom(dto));
        }

        public abstract string KeyFrom(T dto);


        public OperationStatus Update(T dto)
        {
            throw new NotImplementedException();
        }

        public Task<OperationStatus> UpdateAsync(T dto)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<IKeyedDto>> GetLastEntryAsync()
        {
            return await Task.Run(() => GetLastEntry());
        }

        public abstract OperationResult<IKeyedDto> GetLastEntry();

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
        protected void ComputeRoute(string arg = "")
        {
            if (string.IsNullOrEmpty(arg))
            {
                _route = $"api/{_controller}/{_action}";
            }
            else
            {
                _route = $"api/{_controller}/{_action}/{arg}";
            }
        }
        protected abstract StringContent ContentFromDto(BaseDto dto);
        protected async Task<string> StringResponseForAddAsync(BaseDto dto)
        {
            _action = "Add";
            ComputeRoute();
            var jsonContent = ContentFromDto(dto);
            var response = await _client.PostAsync(_route, jsonContent);
            return await response.Content.ReadAsStringAsync();
        }
        protected async Task<string> StringResponseForCountAsync()
        {
            _action = "Count";
            ComputeRoute();
            var response = await _client.GetAsync(_route);
            return await response.Content.ReadAsStringAsync();
        }
        protected async Task<string> StringResponseForGetAllAsync(int pageIndex, int pageSize)
        {
            _action = "GetAll";
            ComputeRoute();
            var route = _route + $"?pageIndex={pageIndex}&pageSize={pageSize}";
            var response = await _client.GetAsync(route);
            return await response.Content.ReadAsStringAsync();
        }
        protected async Task<string> StringResponseForGetByKeyAsync(string key)
        {
            _action = "GetByKey"; ;
            ComputeRoute(key);
            var response = await _client.GetAsync(_route);
            return await response.Content.ReadAsStringAsync();
        }
        protected async Task<string> StringResponseForGetAbsolutelyAllAsync()
        {
            _action = "GetAbsolutelyAll";
            ComputeRoute();
            var response = await _client.GetAsync(_route);
            return await response.Content.ReadAsStringAsync();
        }
        protected async Task<string> StringResponseForDeleteAsync()
        {
            _action = "Delete";
            ComputeRoute();
            var response = await _client.DeleteAsync(_route);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
