using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Person
{
    public interface IPersonService : IService<PersonDto>
    {
        Task<PersonDto> GetByLastNameAndBirthdateAsync(string lastName, string birthdate);
    }
}
