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
            _totalRows = 0;            
        }
        [BindProperty]
        public PaginatedList<MediumDto> Media { get; set; }
        public async Task OnGetAsync(int? pageIndex = 0)
        {
            await InitDataAsync();
            var items = new List<MediumDto>();
            var res = await _service.GetAllAsync(pageIndex.Value, PageSize);
            if (res.Status == OperationStatus.OK)
            {
                var rawList = res.Value;
                foreach (var k in rawList)
                {
                    var m = (KeyedMediumDto)k;
                    var dto = new MediumDto(m.Title,
                                            m.Year,
                                            m.MediumType,
                                            m.Location,
                                            m.HasGermanSubtitles);
                    items.Add(dto);
                }
            }
            Media = new PaginatedList<MediumDto>(items, _totalRows, pageIndex.Value, PageSize);
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