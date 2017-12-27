using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public short Year { get; set; }
        [Required]
        public string Contributor { get; set; }
        [Required]
        public string ContributorBirthdate { get; set; }
        [Required]
        public string Role { get; set; }
        public string Key { get; set; }
    }
}
