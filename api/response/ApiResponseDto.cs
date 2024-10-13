using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.dto.response
{
    public class ApiResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ApiResponseDto(int statusCode, string message, object data = null)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
    }
}