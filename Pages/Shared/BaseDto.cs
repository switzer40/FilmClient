using FilmAPI.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Shared
{
    public abstract class BaseDto : IBaseDto
    {
        
        public string Key { get; set; }
        public abstract void Copy(BaseDto dto);
    }
}
