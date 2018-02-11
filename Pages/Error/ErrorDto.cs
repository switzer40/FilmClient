using FilmAPI.Common.Interfaces;
using FilmAPI.Common.Utilities;
using FilmClient.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Error
{
    public class ErrorDto : BaseDto
    {
        public OperationStatus Status { get; set; }
        public ErrorDto(OperationStatus status)
        {
            Status = status;
        }
        public override void Copy(IBaseDto dto)
        {            
            if (dto.GetType() == typeof(ErrorDto))
            {
                var that = (ErrorDto)dto;
                Status = that.Status;
            }
        }
   
        public override bool Equals(IBaseDto dto)
        {
            bool result = false;
            if (dto.GetType() == typeof(ErrorDto))
            {
                var that = (ErrorDto)dto;
                result = (Status.Equals(that.Status));
            }
            return result;
        }
    }
}
