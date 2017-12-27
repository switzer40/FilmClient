using System;
using System.Linq.Expressions;

namespace FilmClient.Pages.FilmPerson
{
    public class FilmPersonDtoByTitleAndYear
    {
        private string _title;
        private short _year;
        public FilmPersonDtoByTitleAndYear(string title, short year)
        {
            _title = title;
            _year = year;
        }
        public Func<FilmPersonDto, bool> Predicate => (fp) => fp.Title == _title && fp.Year == _year;
    }
}
