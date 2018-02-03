using FilmAPI.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Shared
{
    public interface IMapper
    {
        IKeyedDto Map(BaseDto dto);
        BaseDto Mapback(IKeyedDto k);
    }
}
