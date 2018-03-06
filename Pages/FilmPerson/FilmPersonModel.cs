using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Services;
using FilmClient.Pages.Shared;
using System;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonModel : BaseDto
    {
        public FilmPersonModel()
        {
        }

        public FilmPersonModel(string title,
                               short year,
                               string lastName,
                               string firstName,
                               string birthdate,
                               string role)
        {
            var keyService = new KeyService();
            Title = title;
            Year = year;
            LastName = lastName;
            Contributor = $"{firstName} {lastName}";
            ContributorBirthdate = DateTime.Parse(birthdate).ToShortDateString();
            Role = role;
            Key = keyService.ConstructFilmPersonKey(title,
                                                     year,
                                                     lastName,
                                                     birthdate,
                                                     role);

        }
        public string Title { get; set; }
        public short Year { get; set; }
        public string LastName { get; set; }
        public string ContributorBirthdate { get; set; }
        public string Contributor { get; internal set; }
        public string Role { get; internal set; }

        public override void Copy(IBaseDto dto)
        {
            if (dto.GetType() == typeof(FilmPersonModel))
            {
                var that = (FilmPersonModel)dto;
                Title = that.Title;
                Year = that.Year;
                LastName = that.LastName;
                ContributorBirthdate = that.ContributorBirthdate;
                Contributor = that.Contributor;
                Role = that.Role;
            }
        }

        public override bool Equals(IBaseDto dto)
        {
            bool result = false;
            if (dto.GetType() == typeof(FilmPersonModel))
            {
                var that = (FilmPersonModel)dto;
                result = (Title == that.Title) &&
                         (Year == that.Year) &&
                         (LastName == that.LastName) &&
                         (Contributor == that.Contributor) &&
                         (ContributorBirthdate == that.ContributorBirthdate) &&
                         (Role == that.Role);
            }
            return result;
        }
    }
}