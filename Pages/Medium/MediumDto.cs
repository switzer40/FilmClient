using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Medium
{
    public class MediumDto : BaseDto
    {
        public MediumDto()
        {

        }
        public MediumDto(string title,
                         short year,
                         string mediumType,
                         string location = "",
                         bool subTitles = true)
        {
            _keyService = new KeyService();
            Title = title;
            Year = year;
            MediumType = mediumType;
            Location = location;
            GermanSubtitles = subTitles;
            Key = _keyService.ConstructMediumKey(title, year, mediumType);
        }
        private readonly IKeyService _keyService;
        public string Title { get; set; }
        public short Year { get; set; }
        public string MediumType { get; set; }
        public string Location { get; set; }
        public bool GermanSubtitles { get; set; }

        public override void Copy(BaseDto dto)
        {
            if (dto.GetType() == typeof(MediumDto))
            {
                var that = (MediumDto)dto;
                Title = that.Title;
                Year = that.Year;
                MediumType = that.MediumType;
                GermanSubtitles = that.GermanSubtitles;
                Key = that.Key;
            }
        }
    }
}
