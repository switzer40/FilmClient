using FilmAPI.Common.DTOs;
using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Film
{
    public class FilmDto : BaseDto
    {
        public FilmDto()
        {

        }
        public FilmDto(string title, short year, short length)
        {
            _keyService = new KeyService();
            Title = title;
            Year = year;
            Length = length;
            Key = _keyService.ConstructFilmKey(title, year);
        }
        private IKeyService _keyService;
        [Required]
        public string Title { get; set; }
        [Required]
        [Range(1850, 2050)]
        public short Year { get; set; }
        [Range(10, 300)]
        public short Length { get; set; }

        public override void Copy(IBaseDto dto)
        {
            if (dto.GetType() == typeof(FilmDto))
            {
                var that = (FilmDto)dto;
                Title = that.Title;
                Year = that.Year;
                Length = that.Length;
                
            }
        }


        public override bool Equals(IBaseDto dto)
        {
            var result = false;
            if (dto.GetType() == typeof(FilmDto))
            {
                var that = (FilmDto)dto;
                result = (Title.Equals(that.Title)) &&
                         (Year.Equals(that.Year)) &&
                         (Length.Equals(that.Length));
            }
            return result;
        }
    }
}
