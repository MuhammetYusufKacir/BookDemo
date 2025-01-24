using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookDemo.Core.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; }


        public ApiResponse(bool success, T data, string message, int statusCode)
        {
            Success = success;
            Data = data;
            Message = message;
            StatusCode = statusCode;
            Errors = new List<string>();
        }

    }
}

