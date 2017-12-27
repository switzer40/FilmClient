using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Person
{
    public class PersonDto : BaseDto
    {
        public PersonDto()
        {
        }

        public PersonDto(string lastName, string birthdate, string firstMidName = "")
        {
            LastName = lastName;
            BirthdateString = birthdate;
            FirstMidName = firstMidName;
            FullName = $"{FirstMidName} {LastName}";
            ShortBirthdate = DateTime.Parse(BirthdateString).ToShortDateString();
        }
        public string FirstMidName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public string BirthdateString { get; set; }
        public string FullName { get; set; }
        public string ShortBirthdate { get; set; }
    }
}
