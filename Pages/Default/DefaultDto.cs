using FilmAPI.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Default
{
    public class DefaultDto
    {
        public DefaultDto()
        {
            MediumType = FilmConstants.MediumType_DVD;
            Location = FilmConstants.Location_Left;
            Role = FilmConstants.Role_Actor;
        }
        public string MediumType { get; set; }
        public string Location { get; set; }
        public string Role { get; set; }
        public void Copy(DefaultDto other)
        {
            MediumType = other.MediumType;
            Location = other.Location;
            Role = other.Role;
        }
    }
}
