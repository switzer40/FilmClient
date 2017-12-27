using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonDtoByLastNameAndBirthdate
    {
        private string _lastName;
        private string _birthdate;
        public FilmPersonDtoByLastNameAndBirthdate(string lastName, string birthdate)
        {
            _lastName = lastName;
            _birthdate = birthdate;
        }
        public Func<FilmPersonDto, bool> Predicate => (fp) => fp.LastName == _lastName && fp.Birthdate == _birthdate;
    }
}
