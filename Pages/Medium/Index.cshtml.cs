using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Medium;
using FilmClient.Pages.Shared;
using FilmClient.Pages.Error;
using FilmAPI.Common.DTOs;
using FilmAPI.Common.Utilities;

namespace FilmClient.Pages.Medium
{
    public class IndexModel : BasePageModel
    {
        private readonly IMediumService _service;        
        public IndexModel(IMediumService service, IErrorService eservice) : base(eservice)
        {
            _service = service;
            _keyService = new KeyService();
            Media = new List<MediumDto>();
            _totalRows = 0;            
        }
        [BindProperty]
        public List<MediumDto> Media { get; set; }
        public async Task OnGetAsync()
        {
            await InitDataAsync();
            var res = await _service.GetAllAsync(_pageNumber, PageSize);
            if (res.Status == OperationStatus.OK)
            {
                List<IKeyedDto> media = res.Value;
                foreach (var m in media)
                {
                    KeyedMediumDto dto = (KeyedMediumDto)m;
                    var val = new MediumDto(dto.Title, dto.Year, dto.MediumType, dto.Location, dto.HasGermanSubtitles);
                    Media.Add(val);
                }
            }
           
        }

        private async Task InitDataAsync()
        {
            if (_totalRows > 0)
            {
                return; //initialize only once
            }
            var  res  = await _service.CountAsync();
            if (res.Status == OperationStatus.OK)
            {
                _totalRows = res.Value;
                CalculateRowData(_totalRows);
            }            
        }        
    }
}