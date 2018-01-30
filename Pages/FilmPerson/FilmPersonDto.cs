using FilmAPI.Common.Interfaces;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonDto : BaseDto
    {
        public FilmPersonDto()
        {
        }
        public FilmPersonDto(string title,
                             short year,
                             string lastName,
                             string birthdate,
                             string role)
        {
            Title = title;
            Year = year;
            LastName = lastName;
            Birthdate = birthdate;
            Role = role;
        }
        [Required]
        public string Title { get; set; }
        [Required]
        public short Year { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Birthdate { get; set; }
        [Required]
        public string Role { get; set; }

        public override void Copy(IBaseDto dto)
        {
            if (dto.GetType() == typeof(FilmPersonDto))
            {
                var that = (FilmPersonDto)dto;
                Title = that.Title;
                Year = that.Year;
                LastName = that.LastName;
                Birthdate = that.Birthdate;
                Role = that.Role;
            }
        }

        public override bool Equals(IBaseDto dto)
        {
            var result = false;
            if (dto.GetType() == typeof(FilmPersonDto))
            {
                var that = (FilmPersonDto)dto;
                result = (Title.Equals(that.Title)) &&
                         (Year.Equals(that.Year)) &&
                         (LastName.Equals(that.LastName)) &&
                         (Birthdate.Equals(that.Birthdate)) &&
                         (Role.Equals(that.Role));
            }
            return result;
        }
    }
}
