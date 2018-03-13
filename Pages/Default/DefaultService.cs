using FilmAPI.Common.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Default
{
    public class DefaultService : IDefaultService
    {
        private const string DEFAULTPATH = "./DefaultValues.txt";
        private DefaultDto _current;
        private Dictionary<string, DefaultDto> _store;
        public DefaultService()
        {
            //LoadCurrent();
            _current = new DefaultDto();
            PopulateStore();
        }

       private void LoadCurrent()
        {
            throw new NotImplementedException();
        }

        private void PopulateStore()
        {
            _store = new Dictionary<string, DefaultDto>();
            string[] locations =
            {
                FilmConstants.Location_Left,
                FilmConstants.Location_Left1,
                FilmConstants.Location_Left2,
                FilmConstants.Location_Left3,
                FilmConstants.Location_Left4,
                FilmConstants.Location_Right,
                FilmConstants.Location_Right1,
                FilmConstants.Location_Right2,
                FilmConstants.Location_Right3,
                FilmConstants.Location_Right4,
                FilmConstants.Location_Top,
                FilmConstants.Location_Middle,
                FilmConstants.Location_Bottom,
                FilmConstants.Location_BD1,
                FilmConstants.Location_BD2,
                FilmConstants.Location_BD3,
                FilmConstants.Location_BD4,
                FilmConstants.Location_Shelf1
            };
            string[] mediaTypes =
            {
                FilmConstants.MediumType_DVD,
                FilmConstants.MediumType_BD
            };
            string[] roles =
            {
                FilmConstants.Role_Actor,
                FilmConstants.Role_Composer,
                FilmConstants.Role_Director,
                FilmConstants.Role_Writer
            };
            for (int i = 0; i < locations.Length; i++)
            {
                for (int j = 0; j < mediaTypes.Length; j++)
                {
                    for (int k = 0; k < roles.Length; k++)
                    {
                        var dto = new DefaultDto(mediaTypes[j], locations[i], roles[k]);
                        _store[dto.Key] = dto;
                    }
                }
            }
        }

        public DefaultDto GetCurrentDefaultValues()
        {
            return _current;
        }

        public void UpdateDefaultValues(string key)
        {
            if (_store.ContainsKey(key))
            {
                _current = _store[key];
            }
            else
            {
                throw new Exception("Unexpected key");
            }
        }
    }
}
