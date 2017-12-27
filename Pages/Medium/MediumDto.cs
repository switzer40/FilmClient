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
                         string location = "")
        {
            Title = title;
            Year = year;
            MediumType = mediumType;
            Location = location;
        }
        public string Title { get; set; }
        public short Year { get; set; }
        public string MediumType { get; set; }
        public string Location { get; set; }
    }
}
