using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using System.Threading.Tasks;

namespace FilmClient.Pages.Person
{
    public interface IPersonService : IService<PersonDto>
    {
        Task<OperationResult<PersonDto>> GetByLastNameAndBirthdateAsync(string lastName, string birthdate);
    }
}
