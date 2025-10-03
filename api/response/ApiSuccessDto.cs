using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.dto.response
{
    public class ApiSuccessDto : ApiResponseDto
    {
        public ApiSuccessDto(int statusCode, string message, object data = null) : base(statusCode, message, data)
        {
        }
    }
}