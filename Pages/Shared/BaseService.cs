using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmAPI.Common.Utilities;
using Newtonsoft.Json;
using System.Text;

namespace FilmClient.Pages.Shared
{
    public abstract class BaseService<T> : IService<T> where T : BaseDto
    {
        protected HttpClient _client;
        protected string _controller;
        protected string _action;
        protected string _route;
        protected IKeyService _keyService;
        protected OperationStatus OKStatus = OperationStatus.OK;
        public BaseService()
        {
            _client = GetClient();
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
        protected void ComputeRoute(string arg = "")
        {
            _route = (string.IsNullOrEmpty(arg)) ?
                     $"api/{_controller}/{_action}" :
                     $"api/{_controller}/{_action}/{arg}";            
        }
        public IKeyedDto Add(T t)
        {
            // Not needed
            throw new NotImplementedException();
        }

        public abstract Task<IKeyedDto> AddAsync(T dto);
        

        public int Count()
        {
            // Not needed
            throw new NotImplementedException();
        }

        public abstract Task<int> CountAsync();
        

        public void Delete(string key)
        {
            // Not needed
            throw new NotImplementedException();
        }

        public abstract Task DeleteAsync(string key);
        

        public List<IKeyedDto> GetAbsolutelyAll()
        {
                // Not needed
                throw new NotImplementedException();
        }

        public abstract Task<List<IKeyedDto>> GetAbsolutelyAllAsync();
        

        public List<IKeyedDto> GetAll(int pageIndex, int pageSize)
        {
            // Not needed
            throw new NotImplementedException();
        }

        public abstract Task<List<IKeyedDto>> GetAllAsync(int pageIndex, int pageSize);
            
        public IKeyedDto GetByKey(string key)
        {
            // Not needed
            throw new NotImplementedException();
        }

        public abstract Task<IKeyedDto> GetByKeyAsync(string key);


        public abstract Task<IKeyedDto> GetLastEntryAsync();


        public abstract string KeyFrom(T dto);
        

        public void Update(T dto)
        {
            throw new NotImplementedException();
        }

        public abstract Task UpdateAsync(T dto);
        protected async Task<string> ResponseForAddAsync(BaseDto dto)
        {
            IBaseDto b = RecoverBaseDto(dto);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            _action = "Add";
            ComputeRoute();
            var response = await _client.PostAsync(_route, jsonContent);
            return await response.Content.ReadAsStringAsync();
            
        }
        protected async Task<string> ResponseForCountAsync()
        {
            _action = "Count";
            ComputeRoute();
            var response = await _client.GetAsync(_route);
            var stringResponse = await response.Content.ReadAsStringAsync();
            return stringResponse;
        }
        protected async Task<string> ResponseForDeleteAsync(string key)
        {
            _action = "Delete";
            ComputeRoute(key);
            var response = await _client.DeleteAsync(_route);
            return await response.Content.ReadAsStringAsync();
           
        }
        protected async Task<string> ResponseForGetAbsolutelyAllAsync()
        {
            _action = "GetAbsolutelyAll";
            ComputeRoute();
            var response = await _client.GetAsync(_route);
            return await response.Content.ReadAsStringAsync();
        }
        protected async Task<string> ResponseForGetAllAsync(int pageIndex, int pageSize)
        {
            _action = "GetAll";
            ComputeRoute();
            var queryString = $"?pageIndex={pageIndex}&pageSize={pageSize}";
            var route = _route + queryString;
            var response = await _client.GetAsync(route);
            return await response.Content.ReadAsStringAsync();            
        }
        protected async Task<string> ResponseForGetByKey(string key)
        {
            _action = "GetByKey";
            ComputeRoute(key);
            var response = await _client.GetAsync(_route);
            return await response.Content.ReadAsStringAsync();
        }
        protected async Task<string> ResponseForLastEntryAsync()
        {
            _action = "GetLastEntry";
            ComputeRoute();
            var response = await _client.GetAsync(_route);
            return await response.Content.ReadAsStringAsync();
        }
        protected string BuildExplanation(OperationStatus s)
        {
            return $"Status code: {s.Name}; ReasonForFailure; {s.ReasonForFailure}";
        }
        protected async Task<string> ResponseForUpdateAsync(BaseDto dto)
        {
            IBaseDto b = RecoverBaseDto(dto);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            _action = "Edit";
            ComputeRoute();
            var response = await _client.PostAsync(_route, jsonContent);
            return await response.Content.ReadAsStringAsync(); ;
        }
        protected abstract IBaseDto RecoverBaseDto(BaseDto dto);
        protected  void HandleError(OperationStatus s)
        {
            throw new Exception(BuildExplanation(s));
        }
        protected abstract IKeyedDto ResultFromResponse(string response);
        protected abstract List<IKeyedDto> ListResultFromResponse(string response);
        protected int IntResultFromResponse(string response)
        {
            int result = 0;
            try
            {
                var res = JsonConvert.DeserializeObject<OperationResult<int>>(response);
                var status = res.Status;
                if (status == OKStatus)
                {
                    result = res.Value;
                }
                else
                {
                    HandleError(status);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        protected OperationStatus StatusResultFromResponse(string response)
        {
            OperationStatus result = OKStatus;
            try
            {
                var s = JsonConvert.DeserializeObject<OperationStatus>(response);
                if (s != OKStatus)
                {
                    HandleError(s);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        protected void VoidResultFromResponse(string response)
        {
            try
            {
                var s = JsonConvert.DeserializeObject<OperationStatus>(response);
                if (s != OKStatus)
                {
                    HandleError(s);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }
    }
}
