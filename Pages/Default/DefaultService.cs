using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Default
{
    public class DefaultService : IDefaultService
    {
        private DefaultDto _current;
        public DefaultService()
        {
            _current = new DefaultDto();
        }
        public DefaultDto GetCurrentDefaultValues()
        {
            return _current;
        }

        public void UpdateDefaultValues(DefaultDto dto)
        {
            _current.Copy(dto);
        }
    }
}
