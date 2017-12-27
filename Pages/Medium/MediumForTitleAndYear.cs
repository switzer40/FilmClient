using System;

namespace FilmClient.Pages.Medium
{
    public class MediumForTitleAndYear
    {
        private string _title;
        private short _year;

        public MediumForTitleAndYear(string title, short year)
        {
            _title = title;
            _year = year;
        }
        public Func<MediumDto, bool> Predicate => (m) => m.Title == _title && m.Year == _year;
    }
}