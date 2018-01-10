using FilmAPI.Common.Services;
using System;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonModel
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
        public string Key { get; internal set; }
    }
}