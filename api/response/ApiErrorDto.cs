using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.dto.response
{
    public class ApiErrorDto : ApiResponseDto
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public ApiErrorDto(int statusCode, string errorCode, string errorMessage)
            : base(statusCode, errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}