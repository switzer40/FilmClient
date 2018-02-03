using FilmAPI.Common.Constants;
using FilmAPI.Common.Interfaces;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Default
{
    public class DefaultDto : BaseDto
    {
        public DefaultDto()
        {
            MediumType = FilmConstants.MediumType_DVD;
            Location = FilmConstants.Location_Left;
            Role = FilmConstants.Role_Actor;
        }

        public DefaultDto(string mediumType = FilmConstants.MediumType_DVD,
                          string location = FilmConstants.Location_Left,
                          string role = FilmConstants.Role_Actor)
        {
            MediumType = mediumType;
            Location = location ;
            Role = role;
            Key = ConstructKey(this);
        }
        public string MediumType { get; set; }
        public string Location { get; set; }
        public string Role { get; set; }
        public void Copy(DefaultDto other)
        {
            MediumType = other.MediumType;
            Location = other.Location;
            Role = other.Role;
            Key = ConstructKey(this);
        }

        public static string ConstructKey(DefaultDto dto)
        {
            return $"{dto.MediumType}*{dto.Location}*{dto.Role}";
        }

        public override void Copy(IBaseDto dto)
        {
            if (dto.GetType() == typeof(DefaultDto))
            {
                var that = (DefaultDto)dto;
                Copy(that);
            }
        }

        public override bool Equals(IBaseDto dto)
        {
            bool result = false;
            if (dto.GetType() == typeof(DefaultDto))
            {
                var that = (DefaultDto)dto;
                result = (MediumType.Equals(that.MediumType)) &&
                         (Location.Equals(that.Location)) &&
                         (Role.Equals(that.Role));
            }
            return result;
        }
    }
}
