using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Default
{
    public interface IDefaultService
    {
        DefaultDto GetCurrentDefaultValues();
        void UpdateDefaultValues(DefaultDto dto);
    }
}
